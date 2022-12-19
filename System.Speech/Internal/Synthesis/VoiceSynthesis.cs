using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Internal.ObjectTokens;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Text;
using System.Threading;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000105 RID: 261
	internal sealed class VoiceSynthesis : IDisposable
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x0001C11C File Offset: 0x0001B11C
		internal VoiceSynthesis(WeakReference speechSynthesizer)
		{
			this._asyncWorker = new AsyncSerializedWorker(new WaitCallback(this.ProcessPostData), null);
			this._asyncWorkerUI = new AsyncSerializedWorker(null, AsyncOperationManager.CreateOperation(null));
			this._eventStateChanged = new WaitCallback(this.OnStateChanged);
			this._signalWorkerCallback = new WaitCallback(this.SignalWorkerThread);
			this._speechSyntesizer = speechSynthesizer;
			this._resourceLoader = new ResourceLoader();
			this._site = new EngineSite(this._resourceLoader);
			this._evtPendingSpeak.Reset();
			this._waveOut = new AudioDeviceOut(SAPICategories.DefaultDeviceOut(), this._asyncWorker);
			if (VoiceSynthesis._allVoices == null)
			{
				VoiceSynthesis._allVoices = VoiceSynthesis.BuildInstalledVoices(this);
				if (VoiceSynthesis._allVoices.Count == 0)
				{
					VoiceSynthesis._allVoices = null;
					throw new PlatformNotSupportedException(SR.Get(SRID.SynthesizerVoiceFailed, new object[0]));
				}
			}
			this._installedVoices = new List<InstalledVoice>(VoiceSynthesis._allVoices.Count);
			foreach (InstalledVoice installedVoice in VoiceSynthesis._allVoices)
			{
				this._installedVoices.Add(new InstalledVoice(this, installedVoice.VoiceInfo));
			}
			this._site.VoiceRate = (this._defaultRate = (int)VoiceSynthesis.GetDefaultRate());
			this._workerThread = new Thread(new ThreadStart(this.ThreadProc));
			this._workerThread.IsBackground = true;
			this._workerThread.Start();
			this.SetInterest(this._ttsEvents);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001C380 File Offset: 0x0001B380
		~VoiceSynthesis()
		{
			this.Dispose(false);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001C3B0 File Offset: 0x0001B3B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001C3F8 File Offset: 0x0001B3F8
		internal void Speak(Prompt prompt)
		{
			bool done = false;
			EventHandler<StateChangedEventArgs> eventHandler = delegate(object sender, StateChangedEventArgs args)
			{
				if (prompt.IsCompleted && args.State == SynthesizerState.Ready)
				{
					done = true;
					this._workerWaitHandle.Set();
				}
			};
			try
			{
				this._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Combine(this._stateChanged, eventHandler);
				this._asyncWorkerUI.AsyncMode = false;
				this._asyncWorkerUI.WorkItemPending += this._signalWorkerCallback;
				this.QueuePrompt(prompt);
				while (!done && !this._isDisposed)
				{
					this._workerWaitHandle.WaitOne();
					this._asyncWorkerUI.ConsumeQueue();
				}
				if (prompt._exception != null)
				{
					throw prompt._exception;
				}
			}
			finally
			{
				this._asyncWorkerUI.AsyncMode = true;
				this._asyncWorkerUI.WorkItemPending -= this._signalWorkerCallback;
				this._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Remove(this._stateChanged, eventHandler);
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001C4F8 File Offset: 0x0001B4F8
		internal void SpeakAsync(Prompt prompt)
		{
			this.QueuePrompt(prompt);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001C504 File Offset: 0x0001B504
		internal void OnSpeakStarted(SpeakStartedEventArgs e)
		{
			if (this._speakStarted != null)
			{
				this._asyncWorkerUI.PostOperation(this._speakStarted, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001C544 File Offset: 0x0001B544
		internal void FireSpeakCompleted(object sender, SpeakCompletedEventArgs e)
		{
			if (this._speakCompleted != null && !e.Prompt._syncSpeak)
			{
				this._speakCompleted.Invoke(sender, e);
			}
			e.Prompt.Synthesizer = null;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001C574 File Offset: 0x0001B574
		internal void OnSpeakCompleted(SpeakCompletedEventArgs e)
		{
			e.Prompt.IsCompleted = true;
			this._asyncWorkerUI.PostOperation(new EventHandler<SpeakCompletedEventArgs>(this.FireSpeakCompleted), new object[]
			{
				this._speechSyntesizer.Target,
				e
			});
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001C5C0 File Offset: 0x0001B5C0
		internal void OnSpeakProgress(SpeakProgressEventArgs e)
		{
			if (this._speakProgress != null)
			{
				string text = string.Empty;
				if (e.Prompt._media == SynthesisMediaType.Ssml)
				{
					int characterCount = e.CharacterCount;
					text = this.RemoveEscapeString(e.Prompt._text, e.CharacterPosition, characterCount, out characterCount);
					e.CharacterCount = characterCount;
				}
				else
				{
					text = e.Prompt._text.Substring(e.CharacterPosition, e.CharacterCount);
				}
				e.Text = text;
				this._asyncWorkerUI.PostOperation(this._speakProgress, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001C668 File Offset: 0x0001B668
		private string RemoveEscapeString(string text, int start, int length, out int newLength)
		{
			newLength = length;
			int num = text.LastIndexOf('>', start);
			int num2 = num;
			StringBuilder stringBuilder = new StringBuilder(text.Substring(0, num2));
			do
			{
				int num3 = -1;
				int num4 = int.MaxValue;
				for (int i = 0; i < this.xmlEscapeStrings.Length; i++)
				{
					int num5;
					if ((num5 = text.IndexOf(this.xmlEscapeStrings[i], num2, 4)) >= 0 && num4 > num5)
					{
						num4 = num5;
						num3 = i;
					}
				}
				if (num3 < 0)
				{
					num4 = text.Length;
				}
				else if (num4 >= num)
				{
					newLength += this.xmlEscapeStrings[num3].Length - 1;
				}
				else
				{
					num4 += this.xmlEscapeStrings[num3].Length;
					num3 = -1;
				}
				int num6 = num4 - num2;
				stringBuilder.Append(text.Substring(num2, num6));
				if (num3 >= 0)
				{
					stringBuilder.Append(this.xmlEscapeChars[num3]);
					int length2 = this.xmlEscapeStrings[num3].Length;
					num4 += length2;
				}
				num2 = num4;
			}
			while (start + length > stringBuilder.Length);
			return stringBuilder.ToString().Substring(start, length);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001C774 File Offset: 0x0001B774
		internal void OnBookmarkReached(BookmarkReachedEventArgs e)
		{
			if (this._bookmarkReached != null)
			{
				this._asyncWorkerUI.PostOperation(this._bookmarkReached, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001C7B4 File Offset: 0x0001B7B4
		internal void OnVoiceChange(VoiceChangeEventArgs e)
		{
			if (this._voiceChange != null)
			{
				this._asyncWorkerUI.PostOperation(this._voiceChange, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001C7F4 File Offset: 0x0001B7F4
		internal void OnPhonemeReached(PhonemeReachedEventArgs e)
		{
			if (this._phonemeReached != null)
			{
				this._asyncWorkerUI.PostOperation(this._phonemeReached, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001C834 File Offset: 0x0001B834
		private void OnVisemeReached(VisemeReachedEventArgs e)
		{
			if (this._visemeReached != null)
			{
				this._asyncWorkerUI.PostOperation(this._visemeReached, new object[]
				{
					this._speechSyntesizer.Target,
					e
				});
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001C874 File Offset: 0x0001B874
		private void OnStateChanged(object o)
		{
			lock (this._thisObjectLock)
			{
				StateChangedEventArgs stateChangedEventArgs = (StateChangedEventArgs)o;
				if (this._stateChanged != null)
				{
					this._asyncWorkerUI.PostOperation(this._stateChanged, new object[]
					{
						this._speechSyntesizer.Target,
						stateChangedEventArgs
					});
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001C8E4 File Offset: 0x0001B8E4
		internal void AddEvent<T>(TtsEventId ttsEvent, ref EventHandler<T> internalEventHandler, EventHandler<T> eventHandler) where T : PromptEventArgs
		{
			lock (this._thisObjectLock)
			{
				Helpers.ThrowIfNull(eventHandler, "eventHandler");
				bool flag = internalEventHandler == null;
				internalEventHandler = (EventHandler<T>)Delegate.Combine(internalEventHandler, eventHandler);
				if (flag)
				{
					this._ttsEvents |= 1 << (int)ttsEvent;
					this.SetInterest(this._ttsEvents);
				}
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001C95C File Offset: 0x0001B95C
		internal void RemoveEvent<T>(TtsEventId ttsEvent, ref EventHandler<T> internalEventHandler, EventHandler<T> eventHandler) where T : EventArgs
		{
			lock (this._thisObjectLock)
			{
				Helpers.ThrowIfNull(eventHandler, "eventHandler");
				internalEventHandler = (EventHandler<T>)Delegate.Remove(internalEventHandler, eventHandler);
				if (internalEventHandler == null)
				{
					this._ttsEvents &= ~(1 << (int)ttsEvent);
					this.SetInterest(this._ttsEvents);
				}
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001C9D0 File Offset: 0x0001B9D0
		internal void SetOutput(Stream stream, SpeechAudioFormatInfo formatInfo, bool headerInfo)
		{
			lock (this._pendingSpeakQueue)
			{
				if (this.State == SynthesizerState.Speaking)
				{
					throw new InvalidOperationException(SR.Get(SRID.SynthesizerSetOutputSpeaking, new object[0]));
				}
				if (this.State == SynthesizerState.Paused)
				{
					throw new InvalidOperationException(SR.Get(SRID.SynthesizerSyncSetOutputWhilePaused, new object[0]));
				}
				lock (this._processingSpeakLock)
				{
					if (stream == null)
					{
						this._waveOut = new AudioDeviceOut(SAPICategories.DefaultDeviceOut(), this._asyncWorker);
					}
					else
					{
						this._waveOut = new AudioFileOut(stream, formatInfo, headerInfo, this._asyncWorker);
					}
				}
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001CA94 File Offset: 0x0001BA94
		internal void Abort()
		{
			lock (this._pendingSpeakQueue)
			{
				lock (this._site)
				{
					if (this._currentPrompt != null)
					{
						this._site.Abort();
						this._waveOut.Abort();
					}
				}
				lock (this._processingSpeakLock)
				{
					VoiceSynthesis.Parameters[] array = this._pendingSpeakQueue.ToArray();
					foreach (VoiceSynthesis.Parameters parameters in array)
					{
						VoiceSynthesis.ParametersSpeak parametersSpeak = parameters._parameter as VoiceSynthesis.ParametersSpeak;
						if (parametersSpeak != null)
						{
							parametersSpeak._prompt._exception = new OperationCanceledException(SR.Get(SRID.PromptAsyncOperationCancelled, new object[0]));
						}
					}
					this._evtPendingSpeak.Set();
				}
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001CB90 File Offset: 0x0001BB90
		internal void Abort(Prompt prompt)
		{
			lock (this._pendingSpeakQueue)
			{
				bool flag = false;
				foreach (VoiceSynthesis.Parameters parameters in this._pendingSpeakQueue)
				{
					VoiceSynthesis.ParametersSpeak parametersSpeak = parameters._parameter as VoiceSynthesis.ParametersSpeak;
					if (parametersSpeak._prompt == prompt)
					{
						parametersSpeak._prompt._exception = new OperationCanceledException(SR.Get(SRID.PromptAsyncOperationCancelled, new object[0]));
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					lock (this._site)
					{
						if (this._currentPrompt == prompt)
						{
							this._site.Abort();
							this._waveOut.Abort();
						}
					}
					lock (this._processingSpeakLock)
					{
					}
				}
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001CCA4 File Offset: 0x0001BCA4
		internal void Pause()
		{
			lock (this._waveOut)
			{
				if (this._waveOut != null)
				{
					this._waveOut.Pause();
				}
				lock (this._pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count > 0 && this.State == SynthesizerState.Ready)
					{
						this.OnStateChanged(SynthesizerState.Speaking);
					}
					this.OnStateChanged(SynthesizerState.Paused);
				}
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001CD30 File Offset: 0x0001BD30
		internal void Resume()
		{
			lock (this._waveOut)
			{
				if (this._waveOut != null)
				{
					this._waveOut.Resume();
				}
				lock (this._pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count > 0 || this._currentPrompt != null)
					{
						this.OnStateChanged(SynthesizerState.Speaking);
					}
					else
					{
						if (this.State == SynthesizerState.Paused)
						{
							this.OnStateChanged(SynthesizerState.Speaking);
						}
						this.OnStateChanged(SynthesizerState.Ready);
					}
				}
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001CDD0 File Offset: 0x0001BDD0
		internal void AddLexicon(Uri uri, string mediaType)
		{
			LexiconEntry lexiconEntry = new LexiconEntry(uri, mediaType);
			lock (this._processingSpeakLock)
			{
				foreach (LexiconEntry lexiconEntry2 in this._lexicons)
				{
					if (lexiconEntry2._uri.Equals(uri))
					{
						throw new InvalidOperationException(SR.Get(SRID.DuplicatedEntry, new object[0]));
					}
				}
				this._lexicons.Add(lexiconEntry);
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001CE74 File Offset: 0x0001BE74
		internal void RemoveLexicon(Uri uri)
		{
			lock (this._processingSpeakLock)
			{
				foreach (LexiconEntry lexiconEntry in this._lexicons)
				{
					if (lexiconEntry._uri.Equals(uri))
					{
						this._lexicons.Remove(lexiconEntry);
						return;
					}
				}
				throw new InvalidOperationException(SR.Get(SRID.FileNotFound, new object[] { uri.ToString() }));
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001CF20 File Offset: 0x0001BF20
		internal TTSVoice GetEngine(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = ((this._currentVoice != null) ? this._currentVoice : this.GetVoice(switchContext));
			return this.GetEngineWithVoice(ttsvoice, null, name, culture, gender, age, variant, switchContext);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001CF58 File Offset: 0x0001BF58
		internal ReadOnlyCollection<InstalledVoice> GetInstalledVoices(CultureInfo culture)
		{
			if (culture == null || culture == CultureInfo.InvariantCulture)
			{
				return new ReadOnlyCollection<InstalledVoice>(this._installedVoices);
			}
			Collection<InstalledVoice> collection = new Collection<InstalledVoice>();
			foreach (InstalledVoice installedVoice in this._installedVoices)
			{
				if (culture.Equals(installedVoice.VoiceInfo.Culture))
				{
					collection.Add(installedVoice);
				}
			}
			return new ReadOnlyCollection<InstalledVoice>(collection);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001CFE4 File Offset: 0x0001BFE4
		internal Prompt Prompt
		{
			get
			{
				Prompt currentPrompt;
				lock (this._pendingSpeakQueue)
				{
					currentPrompt = this._currentPrompt;
				}
				return currentPrompt;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001D020 File Offset: 0x0001C020
		internal SynthesizerState State
		{
			get
			{
				return this._synthesizerState;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001D04A File Offset: 0x0001C04A
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0001D028 File Offset: 0x0001C028
		internal int Rate
		{
			get
			{
				return this._site.VoiceRate;
			}
			set
			{
				EngineSite site = this._site;
				this._defaultRate = value;
				site.VoiceRate = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0001D065 File Offset: 0x0001C065
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0001D057 File Offset: 0x0001C057
		internal int Volume
		{
			get
			{
				return this._site.VoiceVolume;
			}
			set
			{
				this._site.VoiceVolume = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0001D074 File Offset: 0x0001C074
		internal TTSVoice Voice
		{
			set
			{
				lock (this._defaultVoiceLock)
				{
					if (this._currentVoice == this._defaultVoice && value == null)
					{
						this._defaultVoiceInitialized = false;
					}
					this._currentVoice = value;
				}
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001D0C8 File Offset: 0x0001C0C8
		internal TTSVoice CurrentVoice(bool switchContext)
		{
			TTSVoice currentVoice;
			lock (this._defaultVoiceLock)
			{
				if (this._currentVoice == null)
				{
					this.GetVoice(switchContext);
				}
				currentVoice = this._currentVoice;
			}
			return currentVoice;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001D114 File Offset: 0x0001C114
		private void ThreadProc()
		{
			do
			{
				this._evtPendingSpeak.WaitOne();
				VoiceSynthesis.Parameters parameters;
				lock (this._pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count > 0)
					{
						parameters = this._pendingSpeakQueue.Dequeue();
						VoiceSynthesis.ParametersSpeak parametersSpeak = parameters._parameter as VoiceSynthesis.ParametersSpeak;
						if (parametersSpeak != null)
						{
							lock (this._site)
							{
								if (this._currentPrompt == null && this.State != SynthesizerState.Paused)
								{
									this.OnStateChanged(SynthesizerState.Speaking);
								}
								this._currentPrompt = parametersSpeak._prompt;
								this._waveOut.IsAborted = false;
								goto IL_96;
							}
						}
						this._currentPrompt = null;
					}
					else
					{
						parameters = null;
					}
					IL_96:;
				}
				if (parameters != null)
				{
					switch (parameters._action)
					{
					case VoiceSynthesis.Action.GetVoice:
						try
						{
							try
							{
								this._pendingVoice = null;
								this._pendingException = null;
								this._pendingVoice = this.GetProxyEngine((VoiceInfo)parameters._parameter);
							}
							catch (Exception ex)
							{
								this._pendingException = ex;
							}
							goto IL_23F;
						}
						finally
						{
							this._evtPendingGetProxy.Set();
						}
						break;
					case VoiceSynthesis.Action.SpeakText:
						break;
					default:
						goto IL_23F;
					}
					VoiceSynthesis.ParametersSpeak parametersSpeak2 = (VoiceSynthesis.ParametersSpeak)parameters._parameter;
					try
					{
						this.InjectEvent(TtsEventId.StartInputStream, parametersSpeak2._prompt, parametersSpeak2._prompt._exception, null);
						if (parametersSpeak2._prompt._exception == null)
						{
							List<LexiconEntry> list = new List<LexiconEntry>();
							TTSVoice ttsvoice = ((this._currentVoice != null) ? this._currentVoice : this.GetVoice(false));
							SpeakInfo speakInfo = new SpeakInfo(this, ttsvoice);
							if (parametersSpeak2._textToSpeak != null)
							{
								if (!parametersSpeak2._isXml)
								{
									TextFragment textFragment = new TextFragment(new FragmentState
									{
										Action = TtsEngineAction.Speak,
										Prosody = new Prosody()
									}, string.Copy(parametersSpeak2._textToSpeak));
									speakInfo.AddText(ttsvoice, textFragment);
								}
								else
								{
									TextFragmentEngine textFragmentEngine = new TextFragmentEngine(speakInfo, parametersSpeak2._textToSpeak, false, this._resourceLoader, list);
									SsmlParser.Parse(parametersSpeak2._textToSpeak, textFragmentEngine, speakInfo.Voice);
								}
							}
							else
							{
								speakInfo.AddAudio(new AudioData(parametersSpeak2._audioFile, this._resourceLoader));
							}
							list.AddRange(this._lexicons);
							this.SpeakText(speakInfo, parametersSpeak2._prompt, list);
						}
						this.ChangeStateToReady(parametersSpeak2._prompt, parametersSpeak2._prompt._exception);
					}
					catch (Exception ex2)
					{
						this.ChangeStateToReady(parametersSpeak2._prompt, ex2);
					}
				}
				IL_23F:
				lock (this._pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count == 0)
					{
						this._evtPendingSpeak.Reset();
					}
				}
			}
			while (!this._fExitWorkerThread);
			this._synthesizerState = SynthesizerState.Ready;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001D438 File Offset: 0x0001C438
		private void AddSpeakParameters(VoiceSynthesis.Parameters param)
		{
			lock (this._pendingSpeakQueue)
			{
				this._pendingSpeakQueue.Enqueue(param);
				if (this._pendingSpeakQueue.Count == 1)
				{
					this._evtPendingSpeak.Set();
				}
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001D494 File Offset: 0x0001C494
		private void SpeakText(SpeakInfo speakInfo, Prompt prompt, List<LexiconEntry> lexicons)
		{
			VoiceInfo voiceInfo = null;
			SpeechSeg speechSeg;
			while ((speechSeg = speakInfo.RemoveFirst()) != null)
			{
				TTSVoice voice = speechSeg.Voice;
				if (voice != null && (voiceInfo == null || !voiceInfo.Equals(voice.VoiceInfo)))
				{
					voiceInfo = voice.VoiceInfo;
					this.InjectEvent(TtsEventId.VoiceChange, prompt, null, voiceInfo);
				}
				lock (this._processingSpeakLock)
				{
					if (speechSeg.IsText)
					{
						lock (this._site)
						{
							if (this._waveOut.IsAborted)
							{
								this._waveOut.IsAborted = false;
								throw new OperationCanceledException(SR.Get(SRID.PromptAsyncOperationCancelled, new object[0]));
							}
							this._site.InitRun(this._waveOut, this._defaultRate, prompt);
							this._waveOut.Begin(voice.WaveFormat(this._waveOut.WaveFormat));
						}
						try
						{
							voice.UpdateLexicons(lexicons);
							this._site.SetEventsInterest(this._ttsInterest);
							byte[] array = voice.WaveFormat(this._waveOut.WaveFormat);
							ITtsEngineProxy ttsEngine = voice.TtsEngine;
							if ((this._ttsInterest & 64) != 0 && ttsEngine.EngineAlphabet != AlphabetType.Ipa)
							{
								this._site.EventMapper = new PhonemeEventMapper(this._site, PhonemeEventMapper.PhonemeConversion.SapiToIpa, ttsEngine.AlphabetConverter);
							}
							else
							{
								this._site.EventMapper = null;
							}
							this._site.LastException = null;
							ttsEngine.Speak(speechSeg.FragmentList, array);
							goto IL_189;
						}
						finally
						{
							this._waveOut.WaitUntilDone();
							this._waveOut.End();
						}
					}
					this._waveOut.PlayWaveFile(speechSeg.Audio);
					speechSeg.Audio.Dispose();
					IL_189:
					lock (this._site)
					{
						this._currentPrompt = null;
						if (this._waveOut.IsAborted || this._site.LastException != null)
						{
							this._waveOut.IsAborted = false;
							if (this._site.LastException != null)
							{
								Exception lastException = this._site.LastException;
								this._site.LastException = null;
								throw lastException;
							}
							throw new OperationCanceledException(SR.Get(SRID.PromptAsyncOperationCancelled, new object[0]));
						}
					}
				}
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001D72C File Offset: 0x0001C72C
		private static uint GetDefaultRate()
		{
			uint num = 0U;
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Speech\\Voices"))
			{
				if (objectTokenCategory != null)
				{
					objectTokenCategory.TryGetDWORD("DefaultTTSRate", ref num);
				}
			}
			return num;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001D774 File Offset: 0x0001C774
		private void InjectEvent(TtsEventId evtId, Prompt prompt, Exception exception, VoiceInfo voiceInfo)
		{
			if (evtId == TtsEventId.EndInputStream)
			{
				if (this._site.EventMapper != null)
				{
					this._site.EventMapper.FlushEvent();
				}
				prompt._exception = exception;
			}
			int num = 1 << (int)evtId;
			if ((num & this._ttsInterest) != 0)
			{
				TTSEvent ttsevent = new TTSEvent(evtId, prompt, exception, voiceInfo);
				this._asyncWorker.Post(ttsevent);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001D7D4 File Offset: 0x0001C7D4
		private void OnStateChanged(SynthesizerState state)
		{
			if (this._synthesizerState != state)
			{
				SynthesizerState synthesizerState = this._synthesizerState;
				this._synthesizerState = state;
				if (this._eventStateChanged != null)
				{
					this._asyncWorker.PostOperation(this._eventStateChanged, new object[]
					{
						new StateChangedEventArgs(state, synthesizerState)
					});
				}
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001D824 File Offset: 0x0001C824
		private void ChangeStateToReady(Prompt prompt, Exception exception)
		{
			lock (this._waveOut)
			{
				lock (this._pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count == 0)
					{
						this._currentPrompt = null;
						if (this.State != SynthesizerState.Paused)
						{
							SynthesizerState synthesizerState = this._synthesizerState;
							this._synthesizerState = SynthesizerState.Ready;
							this.InjectEvent(TtsEventId.EndInputStream, prompt, exception, null);
							if (this._eventStateChanged != null)
							{
								this._asyncWorker.PostOperation(this._eventStateChanged, new object[]
								{
									new StateChangedEventArgs(this._synthesizerState, synthesizerState)
								});
							}
						}
						else
						{
							this.InjectEvent(TtsEventId.EndInputStream, prompt, exception, null);
						}
					}
					else
					{
						this.InjectEvent(TtsEventId.EndInputStream, prompt, exception, null);
					}
				}
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001D8F8 File Offset: 0x0001C8F8
		private TTSVoice GetVoice(VoiceInfo voiceInfo, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			lock (this._voiceDictionary)
			{
				if (!this._voiceDictionary.TryGetValue(voiceInfo, ref ttsvoice))
				{
					if (switchContext)
					{
						this.ExecuteOnBackgroundThread(VoiceSynthesis.Action.GetVoice, voiceInfo);
						ttsvoice = ((this._pendingException == null) ? this._pendingVoice : null);
					}
					else
					{
						ttsvoice = this.GetProxyEngine(voiceInfo);
					}
				}
			}
			return ttsvoice;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001D968 File Offset: 0x0001C968
		private void ExecuteOnBackgroundThread(VoiceSynthesis.Action action, object parameter)
		{
			lock (this._pendingSpeakQueue)
			{
				this._evtPendingGetProxy.Reset();
				this._pendingSpeakQueue.Enqueue(new VoiceSynthesis.Parameters(action, parameter));
				if (this._pendingSpeakQueue.Count == 1)
				{
					this._evtPendingSpeak.Set();
				}
			}
			this._evtPendingGetProxy.WaitOne();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001D9E0 File Offset: 0x0001C9E0
		private TTSVoice GetEngineWithVoice(TTSVoice defaultVoice, VoiceInfo defaultVoiceId, string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			lock (this._enabledVoicesLock)
			{
				if (!string.IsNullOrEmpty(name))
				{
					ttsvoice = this.MatchVoice(name, variant, switchContext);
				}
				if (ttsvoice == null)
				{
					InstalledVoice installedVoice = null;
					if (defaultVoice != null || defaultVoiceId != null)
					{
						installedVoice = InstalledVoice.Find(this._installedVoices, (defaultVoice != null) ? defaultVoice.VoiceInfo : defaultVoiceId);
						if (installedVoice != null && installedVoice.Enabled && variant == 1)
						{
							VoiceInfo voiceInfo = installedVoice.VoiceInfo;
							if (installedVoice.Enabled && voiceInfo.Culture.Equals(culture) && (gender == VoiceGender.NotSet || gender == VoiceGender.Neutral || gender == voiceInfo.Gender) && (age == VoiceAge.NotSet || age == voiceInfo.Age))
							{
								ttsvoice = defaultVoice;
							}
						}
					}
					while (ttsvoice == null && this._installedVoices.Count > 0)
					{
						if (installedVoice == null)
						{
							installedVoice = InstalledVoice.FirstEnabled(this._installedVoices, CultureInfo.CurrentUICulture);
						}
						if (installedVoice == null)
						{
							break;
						}
						ttsvoice = this.MatchVoice(culture, gender, age, variant, switchContext, ref installedVoice);
					}
				}
				if (ttsvoice == null)
				{
					if (defaultVoice == null)
					{
						throw new InvalidOperationException(SR.Get(SRID.SynthesizerVoiceFailed, new object[0]));
					}
					ttsvoice = defaultVoice;
				}
			}
			return ttsvoice;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001DB00 File Offset: 0x0001CB00
		private TTSVoice MatchVoice(string name, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			VoiceInfo voiceInfo = null;
			int num = variant;
			foreach (InstalledVoice installedVoice in this._installedVoices)
			{
				int num2;
				if (installedVoice.Enabled && (num2 = name.IndexOf(installedVoice.VoiceInfo.Name, 4)) >= 0)
				{
					int num3 = num2 + installedVoice.VoiceInfo.Name.Length;
					if ((num2 == 0 || name.get_Chars(num2 - 1) == ' ') && (num3 == name.Length || name.get_Chars(num3) == ' '))
					{
						voiceInfo = installedVoice.VoiceInfo;
						if (num-- == 1)
						{
							break;
						}
					}
				}
			}
			if (voiceInfo != null)
			{
				ttsvoice = this.GetVoice(voiceInfo, switchContext);
			}
			return ttsvoice;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001DBCC File Offset: 0x0001CBCC
		private TTSVoice MatchVoice(CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool switchContext, ref InstalledVoice viDefault)
		{
			TTSVoice ttsvoice = null;
			List<InstalledVoice> list = new List<InstalledVoice>(this._installedVoices);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!list[i].Enabled)
				{
					list.RemoveAt(i);
				}
			}
			while (ttsvoice == null && list.Count > 0)
			{
				InstalledVoice installedVoice = VoiceSynthesis.MatchVoice(viDefault, culture, gender, age, variant, list);
				if (installedVoice != null)
				{
					ttsvoice = this.GetVoice(installedVoice.VoiceInfo, switchContext);
					if (ttsvoice != null)
					{
						break;
					}
					list.Remove(installedVoice);
					installedVoice.SetEnabledFlag(false, switchContext);
					if (installedVoice == viDefault)
					{
						viDefault = null;
						break;
					}
					break;
				}
			}
			return ttsvoice;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001DC60 File Offset: 0x0001CC60
		private static InstalledVoice MatchVoice(InstalledVoice defaultTokenInfo, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, List<InstalledVoice> tokensInfo)
		{
			InstalledVoice installedVoice = defaultTokenInfo;
			int num = VoiceSynthesis.CalcMatchValue(culture, gender, age, installedVoice.VoiceInfo);
			int num2 = -1;
			for (int i = 0; i < tokensInfo.Count; i++)
			{
				InstalledVoice installedVoice2 = tokensInfo[i];
				if (installedVoice2.Enabled)
				{
					int num3 = VoiceSynthesis.CalcMatchValue(culture, gender, age, installedVoice2.VoiceInfo);
					if (installedVoice2.Equals(defaultTokenInfo))
					{
						num2 = i;
					}
					if (num3 > num)
					{
						installedVoice = installedVoice2;
						num = num3;
					}
					if (num3 == 7 && (variant == 1 || num2 >= 0))
					{
						break;
					}
				}
			}
			if (variant > 1)
			{
				tokensInfo[num2] = tokensInfo[0];
				tokensInfo[0] = defaultTokenInfo;
				int num4 = variant;
				do
				{
					foreach (InstalledVoice installedVoice3 in tokensInfo)
					{
						if (installedVoice3.Enabled && VoiceSynthesis.CalcMatchValue(culture, gender, age, installedVoice3.VoiceInfo) == num)
						{
							variant--;
							installedVoice = installedVoice3;
						}
						if (variant == 0)
						{
							break;
						}
					}
					if (variant > 0)
					{
						variant = num4 % (num4 - variant);
					}
				}
				while (variant > 0);
			}
			return installedVoice;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001DD7C File Offset: 0x0001CD7C
		private static int CalcMatchValue(CultureInfo culture, VoiceGender gender, VoiceAge age, VoiceInfo voiceInfo)
		{
			int num;
			if (voiceInfo != null)
			{
				num = 0;
				CultureInfo culture2 = voiceInfo.Culture;
				if (culture != null && Helpers.CompareInvariantCulture(culture2, culture))
				{
					if (culture.Equals(culture2))
					{
						num |= 4;
					}
					if (gender == VoiceGender.NotSet || voiceInfo.Gender == gender)
					{
						num |= 2;
					}
					if (age == VoiceAge.NotSet || voiceInfo.Age == age)
					{
						num |= 1;
					}
				}
			}
			else
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001DDD4 File Offset: 0x0001CDD4
		private TTSVoice GetProxyEngine(VoiceInfo voiceInfo)
		{
			ITtsEngineProxy ttsEngineProxy = this.GetSsmlEngine(voiceInfo);
			if (ttsEngineProxy == null)
			{
				ttsEngineProxy = this.GetComEngine(voiceInfo);
			}
			TTSVoice ttsvoice = null;
			if (ttsEngineProxy != null)
			{
				ttsvoice = new TTSVoice(ttsEngineProxy, voiceInfo);
				this._voiceDictionary.Add(voiceInfo, ttsvoice);
			}
			return ttsvoice;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001DE10 File Offset: 0x0001CE10
		private ITtsEngineProxy GetSsmlEngine(VoiceInfo voiceInfo)
		{
			ITtsEngineProxy ttsEngineProxy = null;
			try
			{
				Assembly assembly;
				if (!string.IsNullOrEmpty(voiceInfo.AssemblyName) && (assembly = Assembly.Load(voiceInfo.AssemblyName)) != null)
				{
					Type[] types = assembly.GetTypes();
					TtsEngineSsml ttsEngineSsml = null;
					foreach (Type type in types)
					{
						if (type.IsSubclassOf(typeof(TtsEngineSsml)))
						{
							string[] array2 = new string[] { voiceInfo.Clsid };
							ttsEngineSsml = assembly.CreateInstance(type.ToString(), false, 0, null, array2, CultureInfo.CurrentUICulture, null) as TtsEngineSsml;
							break;
						}
					}
					if (ttsEngineSsml != null)
					{
						ttsEngineProxy = new TtsProxySsml(ttsEngineSsml, this._site, voiceInfo.Culture.LCID);
					}
				}
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			return ttsEngineProxy;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001DF00 File Offset: 0x0001CF00
		private ITtsEngineProxy GetComEngine(VoiceInfo voiceInfo)
		{
			ITtsEngineProxy ttsEngineProxy = null;
			try
			{
				ObjectToken objectToken = ObjectToken.Open(null, voiceInfo.RegistryKeyPath, false);
				if (objectToken != null)
				{
					object obj = objectToken.CreateObjectFromToken<object>("CLSID");
					if (obj != null)
					{
						ITtsEngineSsml ttsEngineSsml = obj as ITtsEngineSsml;
						if (ttsEngineSsml != null)
						{
							ttsEngineProxy = new TtsProxyCom(ttsEngineSsml, this.ComEngineSite, voiceInfo.Culture.LCID);
						}
						else
						{
							ITtsEngine ttsEngine = obj as ITtsEngine;
							if (ttsEngine != null)
							{
								ttsEngineProxy = new TtsProxySapi(ttsEngine, this.ComEngineSite, voiceInfo.Culture.LCID);
							}
						}
					}
				}
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			catch (COMException)
			{
			}
			catch (FormatException)
			{
			}
			return ttsEngineProxy;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001DFCC File Offset: 0x0001CFCC
		private TTSVoice GetVoice(bool switchContext)
		{
			lock (this._defaultVoiceLock)
			{
				if (!this._defaultVoiceInitialized)
				{
					this._defaultVoice = null;
					ObjectToken objectToken = SAPICategories.DefaultToken("Voices");
					if (objectToken != null)
					{
						VoiceGender voiceGender = VoiceGender.NotSet;
						VoiceAge voiceAge = VoiceAge.NotSet;
						SsmlParserHelpers.TryConvertGender(objectToken.Gender.ToLowerInvariant(), out voiceGender);
						SsmlParserHelpers.TryConvertAge(objectToken.Age.ToLowerInvariant(), out voiceAge);
						this._defaultVoice = this.GetEngineWithVoice(null, new VoiceInfo(objectToken), objectToken.TokenName(), objectToken.Culture, voiceGender, voiceAge, 1, switchContext);
						objectToken = null;
					}
					if (this._defaultVoice == null)
					{
						VoiceInfo voiceInfo = ((objectToken != null) ? new VoiceInfo(objectToken) : null);
						this._defaultVoice = this.GetEngineWithVoice(null, voiceInfo, null, CultureInfo.CurrentUICulture, VoiceGender.NotSet, VoiceAge.NotSet, 1, switchContext);
					}
					this._defaultVoiceInitialized = true;
					this._currentVoice = this._defaultVoice;
				}
			}
			return this._defaultVoice;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E0B8 File Offset: 0x0001D0B8
		private static List<InstalledVoice> BuildInstalledVoices(VoiceSynthesis voiceSynthesizer)
		{
			List<InstalledVoice> list = new List<InstalledVoice>();
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices"))
			{
				if (objectTokenCategory != null)
				{
					foreach (ObjectToken objectToken in objectTokenCategory.FindMatchingTokens(null, null))
					{
						if (objectToken != null && objectToken.Attributes != null)
						{
							list.Add(new InstalledVoice(voiceSynthesizer, new VoiceInfo(objectToken)));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001E14C File Offset: 0x0001D14C
		private void SignalWorkerThread(object ignored)
		{
			if (!this._asyncWorkerUI.AsyncMode)
			{
				this._workerWaitHandle.Set();
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001E168 File Offset: 0x0001D168
		private void ProcessPostData(object arg)
		{
			TTSEvent ttsevent = arg as TTSEvent;
			if (ttsevent == null)
			{
				return;
			}
			lock (this._thisObjectLock)
			{
				if (!this._isDisposed)
				{
					this.DispatchEvent(ttsevent);
				}
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001E1B8 File Offset: 0x0001D1B8
		private void DispatchEvent(TTSEvent ttsEvent)
		{
			Prompt prompt = ttsEvent.Prompt;
			TtsEventId id = ttsEvent.Id;
			prompt._exception = ttsEvent.Exception;
			switch (id)
			{
			case TtsEventId.StartInputStream:
				this.OnSpeakStarted(new SpeakStartedEventArgs(prompt));
				return;
			case TtsEventId.EndInputStream:
				this.OnSpeakCompleted(new SpeakCompletedEventArgs(prompt));
				return;
			case TtsEventId.VoiceChange:
			{
				VoiceInfo voice = ttsEvent.Voice;
				this.OnVoiceChange(new VoiceChangeEventArgs(prompt, voice));
				return;
			}
			case TtsEventId.Bookmark:
				this.OnBookmarkReached(new BookmarkReachedEventArgs(prompt, ttsEvent.Bookmark, ttsEvent.AudioPosition));
				return;
			case TtsEventId.WordBoundary:
				this.OnSpeakProgress(new SpeakProgressEventArgs(prompt, ttsEvent.AudioPosition, (int)ttsEvent.LParam, (int)ttsEvent.WParam));
				return;
			case TtsEventId.Phoneme:
				this.OnPhonemeReached(new PhonemeReachedEventArgs(prompt, ttsEvent.Phoneme, ttsEvent.AudioPosition, ttsEvent.PhonemeDuration, ttsEvent.PhonemeEmphasis, ttsEvent.NextPhoneme));
				return;
			case TtsEventId.SentenceBoundary:
				return;
			case TtsEventId.Viseme:
				this.OnVisemeReached(new VisemeReachedEventArgs(prompt, (int)ttsEvent.LParam & 65535, ttsEvent.AudioPosition, TimeSpan.FromMilliseconds(ttsEvent.WParam >> 16), (SynthesizerEmphasis)((uint)(int)ttsEvent.LParam >> 16), (int)(ttsEvent.WParam & 65535U)));
				return;
			default:
				throw new InvalidOperationException(SR.Get(SRID.SynthesizerUnknownEvent, new object[0]));
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001E30C File Offset: 0x0001D30C
		private void Dispose(bool disposing)
		{
			if (!this._isDisposed)
			{
				lock (this._thisObjectLock)
				{
					this._fExitWorkerThread = true;
					this.Abort();
					int num = 0;
					while (num < 20 && this.State != SynthesizerState.Ready)
					{
						Thread.Sleep(100);
						num++;
					}
					if (disposing)
					{
						this._evtPendingSpeak.Set();
						this._workerThread.Join();
						foreach (KeyValuePair<VoiceInfo, TTSVoice> keyValuePair in this._voiceDictionary)
						{
							if (keyValuePair.Value != null)
							{
								keyValuePair.Value.TtsEngine.ReleaseInterface();
							}
						}
						this._voiceDictionary.Clear();
						this._evtPendingSpeak.Close();
						this._evtPendingGetProxy.Close();
						this._workerWaitHandle.Close();
					}
					if (this._iSite != IntPtr.Zero)
					{
						Marshal.Release(this._iSite);
					}
					this._isDisposed = true;
				}
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001E438 File Offset: 0x0001D438
		private void QueuePrompt(Prompt prompt)
		{
			switch (prompt._media)
			{
			case SynthesisMediaType.Text:
				this.Speak(prompt._text, prompt, false);
				return;
			case SynthesisMediaType.Ssml:
				this.Speak(prompt._text, prompt, true);
				return;
			case SynthesisMediaType.WaveAudio:
				this.SpeakStream(prompt._audio, prompt);
				return;
			default:
				throw new ArgumentException(SR.Get(SRID.SynthesizerUnknownMediaType, new object[0]));
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001E4A1 File Offset: 0x0001D4A1
		private void Speak(string textToSpeak, Prompt prompt, bool fIsXml)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("VoiceSynthesis");
			}
			this.AddSpeakParameters(new VoiceSynthesis.Parameters(VoiceSynthesis.Action.SpeakText, new VoiceSynthesis.ParametersSpeak(textToSpeak, prompt, fIsXml, null)));
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001E4D6 File Offset: 0x0001D4D6
		private void SpeakStream(Uri audio, Prompt prompt)
		{
			this.AddSpeakParameters(new VoiceSynthesis.Parameters(VoiceSynthesis.Action.SpeakText, new VoiceSynthesis.ParametersSpeak(null, prompt, false, audio)));
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001E4F0 File Offset: 0x0001D4F0
		private void SetInterest(int ttsInterest)
		{
			this._ttsInterest = ttsInterest;
			lock (this._pendingSpeakQueue)
			{
				this._site.SetEventsInterest(this._ttsInterest);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001E53C File Offset: 0x0001D53C
		private IntPtr ComEngineSite
		{
			get
			{
				if (this._iSite == IntPtr.Zero)
				{
					this._siteSapi = new EngineSiteSapi(this._site, this._resourceLoader);
					this._iSite = Marshal.GetComInterfaceForObject(this._siteSapi, typeof(ISpEngineSite));
				}
				return this._iSite;
			}
		}

		// Token: 0x040004D9 RID: 1241
		private const string defaultVoiceRate = "DefaultTTSRate";

		// Token: 0x040004DA RID: 1242
		private const bool _pexml = false;

		// Token: 0x040004DB RID: 1243
		internal EventHandler<StateChangedEventArgs> _stateChanged;

		// Token: 0x040004DC RID: 1244
		internal EventHandler<SpeakStartedEventArgs> _speakStarted;

		// Token: 0x040004DD RID: 1245
		internal EventHandler<SpeakCompletedEventArgs> _speakCompleted;

		// Token: 0x040004DE RID: 1246
		internal EventHandler<SpeakProgressEventArgs> _speakProgress;

		// Token: 0x040004DF RID: 1247
		internal EventHandler<BookmarkReachedEventArgs> _bookmarkReached;

		// Token: 0x040004E0 RID: 1248
		internal EventHandler<VoiceChangeEventArgs> _voiceChange;

		// Token: 0x040004E1 RID: 1249
		internal EventHandler<PhonemeReachedEventArgs> _phonemeReached;

		// Token: 0x040004E2 RID: 1250
		internal EventHandler<VisemeReachedEventArgs> _visemeReached;

		// Token: 0x040004E3 RID: 1251
		private WaitCallback _eventStateChanged;

		// Token: 0x040004E4 RID: 1252
		private WaitCallback _signalWorkerCallback;

		// Token: 0x040004E5 RID: 1253
		private readonly ResourceLoader _resourceLoader;

		// Token: 0x040004E6 RID: 1254
		private readonly EngineSite _site;

		// Token: 0x040004E7 RID: 1255
		private EngineSiteSapi _siteSapi;

		// Token: 0x040004E8 RID: 1256
		private IntPtr _iSite;

		// Token: 0x040004E9 RID: 1257
		private int _ttsInterest;

		// Token: 0x040004EA RID: 1258
		private ManualResetEvent _evtPendingSpeak = new ManualResetEvent(false);

		// Token: 0x040004EB RID: 1259
		private ManualResetEvent _evtPendingGetProxy = new ManualResetEvent(false);

		// Token: 0x040004EC RID: 1260
		private Exception _pendingException;

		// Token: 0x040004ED RID: 1261
		private Queue<VoiceSynthesis.Parameters> _pendingSpeakQueue = new Queue<VoiceSynthesis.Parameters>();

		// Token: 0x040004EE RID: 1262
		private TTSVoice _pendingVoice;

		// Token: 0x040004EF RID: 1263
		private Thread _workerThread;

		// Token: 0x040004F0 RID: 1264
		private bool _fExitWorkerThread;

		// Token: 0x040004F1 RID: 1265
		private object _processingSpeakLock = new object();

		// Token: 0x040004F2 RID: 1266
		private Dictionary<VoiceInfo, TTSVoice> _voiceDictionary = new Dictionary<VoiceInfo, TTSVoice>();

		// Token: 0x040004F3 RID: 1267
		private List<InstalledVoice> _installedVoices;

		// Token: 0x040004F4 RID: 1268
		private static List<InstalledVoice> _allVoices;

		// Token: 0x040004F5 RID: 1269
		private object _enabledVoicesLock = new object();

		// Token: 0x040004F6 RID: 1270
		private TTSVoice _defaultVoice;

		// Token: 0x040004F7 RID: 1271
		private TTSVoice _currentVoice;

		// Token: 0x040004F8 RID: 1272
		private bool _defaultVoiceInitialized;

		// Token: 0x040004F9 RID: 1273
		private object _defaultVoiceLock = new object();

		// Token: 0x040004FA RID: 1274
		private AudioBase _waveOut;

		// Token: 0x040004FB RID: 1275
		private int _defaultRate;

		// Token: 0x040004FC RID: 1276
		private bool _isDisposed;

		// Token: 0x040004FD RID: 1277
		private List<LexiconEntry> _lexicons = new List<LexiconEntry>();

		// Token: 0x040004FE RID: 1278
		private SynthesizerState _synthesizerState;

		// Token: 0x040004FF RID: 1279
		private Prompt _currentPrompt;

		// Token: 0x04000500 RID: 1280
		private AsyncSerializedWorker _asyncWorker;

		// Token: 0x04000501 RID: 1281
		private AsyncSerializedWorker _asyncWorkerUI;

		// Token: 0x04000502 RID: 1282
		private int _ttsEvents = 6;

		// Token: 0x04000503 RID: 1283
		private object _thisObjectLock = new object();

		// Token: 0x04000504 RID: 1284
		private AutoResetEvent _workerWaitHandle = new AutoResetEvent(false);

		// Token: 0x04000505 RID: 1285
		private WeakReference _speechSyntesizer;

		// Token: 0x04000506 RID: 1286
		private readonly string[] xmlEscapeStrings = new string[] { "&quot;", "&apos;", "&amp;", "&lt;", "&gt;" };

		// Token: 0x04000507 RID: 1287
		private readonly char[] xmlEscapeChars = new char[] { '"', '\'', '&', '<', '>' };

		// Token: 0x02000106 RID: 262
		private enum Action
		{
			// Token: 0x04000509 RID: 1289
			GetVoice,
			// Token: 0x0400050A RID: 1290
			SpeakText
		}

		// Token: 0x02000107 RID: 263
		private class Parameters
		{
			// Token: 0x06000683 RID: 1667 RVA: 0x0001E593 File Offset: 0x0001D593
			internal Parameters(VoiceSynthesis.Action action, object parameter)
			{
				this._action = action;
				this._parameter = parameter;
			}

			// Token: 0x0400050B RID: 1291
			internal VoiceSynthesis.Action _action;

			// Token: 0x0400050C RID: 1292
			internal object _parameter;
		}

		// Token: 0x02000108 RID: 264
		private class ParametersSpeak
		{
			// Token: 0x06000684 RID: 1668 RVA: 0x0001E5A9 File Offset: 0x0001D5A9
			internal ParametersSpeak(string textToSpeak, Prompt prompt, bool isXml, Uri audioFile)
			{
				this._textToSpeak = textToSpeak;
				this._prompt = prompt;
				this._isXml = isXml;
				this._audioFile = audioFile;
			}

			// Token: 0x0400050D RID: 1293
			internal string _textToSpeak;

			// Token: 0x0400050E RID: 1294
			internal Prompt _prompt;

			// Token: 0x0400050F RID: 1295
			internal bool _isXml;

			// Token: 0x04000510 RID: 1296
			internal Uri _audioFile;
		}
	}
}
