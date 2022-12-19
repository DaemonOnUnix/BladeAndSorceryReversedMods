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
	// Token: 0x020000CD RID: 205
	internal sealed class VoiceSynthesis : IDisposable
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x0001CF68 File Offset: 0x0001B168
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

		// Token: 0x06000713 RID: 1811 RVA: 0x0001D1C8 File Offset: 0x0001B3C8
		~VoiceSynthesis()
		{
			this.Dispose(false);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001D208 File Offset: 0x0001B408
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

		// Token: 0x06000716 RID: 1814 RVA: 0x0001D308 File Offset: 0x0001B508
		internal void SpeakAsync(Prompt prompt)
		{
			this.QueuePrompt(prompt);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001D311 File Offset: 0x0001B511
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

		// Token: 0x06000718 RID: 1816 RVA: 0x0001D344 File Offset: 0x0001B544
		internal void FireSpeakCompleted(object sender, SpeakCompletedEventArgs e)
		{
			if (this._speakCompleted != null && !e.Prompt._syncSpeak)
			{
				this._speakCompleted(sender, e);
			}
			e.Prompt.Synthesizer = null;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001D374 File Offset: 0x0001B574
		internal void OnSpeakCompleted(SpeakCompletedEventArgs e)
		{
			e.Prompt.IsCompleted = true;
			this._asyncWorkerUI.PostOperation(new EventHandler<SpeakCompletedEventArgs>(this.FireSpeakCompleted), new object[]
			{
				this._speechSyntesizer.Target,
				e
			});
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001D3B4 File Offset: 0x0001B5B4
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

		// Token: 0x0600071B RID: 1819 RVA: 0x0001D458 File Offset: 0x0001B658
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
					if ((num5 = text.IndexOf(this.xmlEscapeStrings[i], num2, StringComparison.Ordinal)) >= 0 && num4 > num5)
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

		// Token: 0x0600071C RID: 1820 RVA: 0x0001D563 File Offset: 0x0001B763
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

		// Token: 0x0600071D RID: 1821 RVA: 0x0001D596 File Offset: 0x0001B796
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

		// Token: 0x0600071E RID: 1822 RVA: 0x0001D5C9 File Offset: 0x0001B7C9
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

		// Token: 0x0600071F RID: 1823 RVA: 0x0001D5FC File Offset: 0x0001B7FC
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

		// Token: 0x06000720 RID: 1824 RVA: 0x0001D630 File Offset: 0x0001B830
		private void OnStateChanged(object o)
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
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

		// Token: 0x06000721 RID: 1825 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
		internal void AddEvent<T>(TtsEventId ttsEvent, ref EventHandler<T> internalEventHandler, EventHandler<T> eventHandler) where T : PromptEventArgs
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
			{
				Helpers.ThrowIfNull(eventHandler, "eventHandler");
				bool flag2 = internalEventHandler == null;
				internalEventHandler = (EventHandler<T>)Delegate.Combine(internalEventHandler, eventHandler);
				if (flag2)
				{
					this._ttsEvents |= 1 << (int)ttsEvent;
					this.SetInterest(this._ttsEvents);
				}
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001D720 File Offset: 0x0001B920
		internal void RemoveEvent<T>(TtsEventId ttsEvent, ref EventHandler<T> internalEventHandler, EventHandler<T> eventHandler) where T : EventArgs
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
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

		// Token: 0x06000723 RID: 1827 RVA: 0x0001D798 File Offset: 0x0001B998
		internal void SetOutput(Stream stream, SpeechAudioFormatInfo formatInfo, bool headerInfo)
		{
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
			{
				if (this.State == SynthesizerState.Speaking)
				{
					throw new InvalidOperationException(SR.Get(SRID.SynthesizerSetOutputSpeaking, new object[0]));
				}
				if (this.State == SynthesizerState.Paused)
				{
					throw new InvalidOperationException(SR.Get(SRID.SynthesizerSyncSetOutputWhilePaused, new object[0]));
				}
				object processingSpeakLock = this._processingSpeakLock;
				lock (processingSpeakLock)
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

		// Token: 0x06000724 RID: 1828 RVA: 0x0001D868 File Offset: 0x0001BA68
		internal void Abort()
		{
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
			{
				EngineSite site = this._site;
				lock (site)
				{
					if (this._currentPrompt != null)
					{
						this._site.Abort();
						this._waveOut.Abort();
					}
				}
				object processingSpeakLock = this._processingSpeakLock;
				lock (processingSpeakLock)
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

		// Token: 0x06000725 RID: 1829 RVA: 0x0001D980 File Offset: 0x0001BB80
		internal void Abort(Prompt prompt)
		{
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
			{
				bool flag2 = false;
				foreach (VoiceSynthesis.Parameters parameters in this._pendingSpeakQueue)
				{
					VoiceSynthesis.ParametersSpeak parametersSpeak = parameters._parameter as VoiceSynthesis.ParametersSpeak;
					if (parametersSpeak._prompt == prompt)
					{
						parametersSpeak._prompt._exception = new OperationCanceledException(SR.Get(SRID.PromptAsyncOperationCancelled, new object[0]));
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					EngineSite site = this._site;
					lock (site)
					{
						if (this._currentPrompt == prompt)
						{
							this._site.Abort();
							this._waveOut.Abort();
						}
					}
					object processingSpeakLock = this._processingSpeakLock;
					lock (processingSpeakLock)
					{
					}
				}
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001DAB4 File Offset: 0x0001BCB4
		internal void Pause()
		{
			AudioBase waveOut = this._waveOut;
			lock (waveOut)
			{
				if (this._waveOut != null)
				{
					this._waveOut.Pause();
				}
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
				lock (pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count > 0 && this.State == SynthesizerState.Ready)
					{
						this.OnStateChanged(SynthesizerState.Speaking);
					}
					this.OnStateChanged(SynthesizerState.Paused);
				}
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001DB4C File Offset: 0x0001BD4C
		internal void Resume()
		{
			AudioBase waveOut = this._waveOut;
			lock (waveOut)
			{
				if (this._waveOut != null)
				{
					this._waveOut.Resume();
				}
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
				lock (pendingSpeakQueue)
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

		// Token: 0x06000728 RID: 1832 RVA: 0x0001DBF8 File Offset: 0x0001BDF8
		internal void AddLexicon(Uri uri, string mediaType)
		{
			LexiconEntry lexiconEntry = new LexiconEntry(uri, mediaType);
			object processingSpeakLock = this._processingSpeakLock;
			lock (processingSpeakLock)
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

		// Token: 0x06000729 RID: 1833 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		internal void RemoveLexicon(Uri uri)
		{
			object processingSpeakLock = this._processingSpeakLock;
			lock (processingSpeakLock)
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

		// Token: 0x0600072A RID: 1834 RVA: 0x0001DD54 File Offset: 0x0001BF54
		internal TTSVoice GetEngine(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = ((this._currentVoice != null) ? this._currentVoice : this.GetVoice(switchContext));
			return this.GetEngineWithVoice(ttsvoice, null, name, culture, gender, age, variant, switchContext);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001DD8C File Offset: 0x0001BF8C
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

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001DE18 File Offset: 0x0001C018
		internal Prompt Prompt
		{
			get
			{
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
				Prompt currentPrompt;
				lock (pendingSpeakQueue)
				{
					currentPrompt = this._currentPrompt;
				}
				return currentPrompt;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001DE5C File Offset: 0x0001C05C
		internal SynthesizerState State
		{
			get
			{
				return this._synthesizerState;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001DE86 File Offset: 0x0001C086
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001DE64 File Offset: 0x0001C064
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001DEA1 File Offset: 0x0001C0A1
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001DE93 File Offset: 0x0001C093
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

		// Token: 0x17000174 RID: 372
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001DEB0 File Offset: 0x0001C0B0
		internal TTSVoice Voice
		{
			set
			{
				object defaultVoiceLock = this._defaultVoiceLock;
				lock (defaultVoiceLock)
				{
					if (this._currentVoice == this._defaultVoice && value == null)
					{
						this._defaultVoiceInitialized = false;
					}
					this._currentVoice = value;
				}
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001DF0C File Offset: 0x0001C10C
		internal TTSVoice CurrentVoice(bool switchContext)
		{
			object defaultVoiceLock = this._defaultVoiceLock;
			TTSVoice currentVoice;
			lock (defaultVoiceLock)
			{
				if (this._currentVoice == null)
				{
					this.GetVoice(switchContext);
				}
				currentVoice = this._currentVoice;
			}
			return currentVoice;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001DF60 File Offset: 0x0001C160
		private void ThreadProc()
		{
			do
			{
				this._evtPendingSpeak.WaitOne();
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
				VoiceSynthesis.Parameters parameters;
				lock (pendingSpeakQueue)
				{
					if (this._pendingSpeakQueue.Count > 0)
					{
						parameters = this._pendingSpeakQueue.Dequeue();
						VoiceSynthesis.ParametersSpeak parametersSpeak = parameters._parameter as VoiceSynthesis.ParametersSpeak;
						if (parametersSpeak != null)
						{
							EngineSite site = this._site;
							lock (site)
							{
								if (this._currentPrompt == null && this.State != SynthesizerState.Paused)
								{
									this.OnStateChanged(SynthesizerState.Speaking);
								}
								this._currentPrompt = parametersSpeak._prompt;
								this._waveOut.IsAborted = false;
								goto IL_AF;
							}
						}
						this._currentPrompt = null;
					}
					else
					{
						parameters = null;
					}
				}
				IL_AF:
				if (parameters != null)
				{
					VoiceSynthesis.Action action = parameters._action;
					if (action != VoiceSynthesis.Action.GetVoice)
					{
						if (action != VoiceSynthesis.Action.SpeakText)
						{
							goto IL_25A;
						}
					}
					else
					{
						try
						{
							this._pendingVoice = null;
							this._pendingException = null;
							this._pendingVoice = this.GetProxyEngine((VoiceInfo)parameters._parameter);
							goto IL_25A;
						}
						catch (Exception ex)
						{
							this._pendingException = ex;
							goto IL_25A;
						}
						finally
						{
							this._evtPendingGetProxy.Set();
						}
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
				IL_25A:
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue2 = this._pendingSpeakQueue;
				lock (pendingSpeakQueue2)
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

		// Token: 0x06000735 RID: 1845 RVA: 0x0001E2A8 File Offset: 0x0001C4A8
		private void AddSpeakParameters(VoiceSynthesis.Parameters param)
		{
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
			{
				this._pendingSpeakQueue.Enqueue(param);
				if (this._pendingSpeakQueue.Count == 1)
				{
					this._evtPendingSpeak.Set();
				}
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001E308 File Offset: 0x0001C508
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
				object processingSpeakLock = this._processingSpeakLock;
				lock (processingSpeakLock)
				{
					if (speechSeg.IsText)
					{
						EngineSite site = this._site;
						lock (site)
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
							goto IL_19A;
						}
						finally
						{
							this._waveOut.WaitUntilDone();
							this._waveOut.End();
						}
					}
					this._waveOut.PlayWaveFile(speechSeg.Audio);
					speechSeg.Audio.Dispose();
					IL_19A:
					EngineSite site2 = this._site;
					lock (site2)
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

		// Token: 0x06000737 RID: 1847 RVA: 0x0001E5BC File Offset: 0x0001C7BC
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

		// Token: 0x06000738 RID: 1848 RVA: 0x0001E604 File Offset: 0x0001C804
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

		// Token: 0x06000739 RID: 1849 RVA: 0x0001E664 File Offset: 0x0001C864
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

		// Token: 0x0600073A RID: 1850 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
		private void ChangeStateToReady(Prompt prompt, Exception exception)
		{
			AudioBase waveOut = this._waveOut;
			lock (waveOut)
			{
				Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
				lock (pendingSpeakQueue)
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

		// Token: 0x0600073B RID: 1851 RVA: 0x0001E794 File Offset: 0x0001C994
		private TTSVoice GetVoice(VoiceInfo voiceInfo, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			Dictionary<VoiceInfo, TTSVoice> voiceDictionary = this._voiceDictionary;
			lock (voiceDictionary)
			{
				if (!this._voiceDictionary.TryGetValue(voiceInfo, out ttsvoice))
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

		// Token: 0x0600073C RID: 1852 RVA: 0x0001E808 File Offset: 0x0001CA08
		private void ExecuteOnBackgroundThread(VoiceSynthesis.Action action, object parameter)
		{
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
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

		// Token: 0x0600073D RID: 1853 RVA: 0x0001E888 File Offset: 0x0001CA88
		private TTSVoice GetEngineWithVoice(TTSVoice defaultVoice, VoiceInfo defaultVoiceId, string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			object enabledVoicesLock = this._enabledVoicesLock;
			lock (enabledVoicesLock)
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

		// Token: 0x0600073E RID: 1854 RVA: 0x0001E9B4 File Offset: 0x0001CBB4
		private TTSVoice MatchVoice(string name, int variant, bool switchContext)
		{
			TTSVoice ttsvoice = null;
			VoiceInfo voiceInfo = null;
			int num = variant;
			foreach (InstalledVoice installedVoice in this._installedVoices)
			{
				int num2;
				if (installedVoice.Enabled && (num2 = name.IndexOf(installedVoice.VoiceInfo.Name, StringComparison.Ordinal)) >= 0)
				{
					int num3 = num2 + installedVoice.VoiceInfo.Name.Length;
					if ((num2 == 0 || name[num2 - 1] == ' ') && (num3 == name.Length || name[num3] == ' '))
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

		// Token: 0x0600073F RID: 1855 RVA: 0x0001EA8C File Offset: 0x0001CC8C
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

		// Token: 0x06000740 RID: 1856 RVA: 0x0001EB20 File Offset: 0x0001CD20
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

		// Token: 0x06000741 RID: 1857 RVA: 0x0001EC3C File Offset: 0x0001CE3C
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

		// Token: 0x06000742 RID: 1858 RVA: 0x0001EC94 File Offset: 0x0001CE94
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

		// Token: 0x06000743 RID: 1859 RVA: 0x0001ECD0 File Offset: 0x0001CED0
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
							ttsEngineSsml = assembly.CreateInstance(type.ToString(), false, BindingFlags.Default, null, array2, CultureInfo.CurrentUICulture, null) as TtsEngineSsml;
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

		// Token: 0x06000744 RID: 1860 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
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

		// Token: 0x06000745 RID: 1861 RVA: 0x0001EE8C File Offset: 0x0001D08C
		private TTSVoice GetVoice(bool switchContext)
		{
			object defaultVoiceLock = this._defaultVoiceLock;
			lock (defaultVoiceLock)
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

		// Token: 0x06000746 RID: 1862 RVA: 0x0001EF80 File Offset: 0x0001D180
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

		// Token: 0x06000747 RID: 1863 RVA: 0x0001F014 File Offset: 0x0001D214
		private void SignalWorkerThread(object ignored)
		{
			if (!this._asyncWorkerUI.AsyncMode)
			{
				this._workerWaitHandle.Set();
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001F030 File Offset: 0x0001D230
		private void ProcessPostData(object arg)
		{
			TTSEvent ttsevent = arg as TTSEvent;
			if (ttsevent == null)
			{
				return;
			}
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
			{
				if (!this._isDisposed)
				{
					this.DispatchEvent(ttsevent);
				}
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001F084 File Offset: 0x0001D284
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

		// Token: 0x0600074A RID: 1866 RVA: 0x0001F1D4 File Offset: 0x0001D3D4
		private void Dispose(bool disposing)
		{
			if (!this._isDisposed)
			{
				object thisObjectLock = this._thisObjectLock;
				lock (thisObjectLock)
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

		// Token: 0x0600074B RID: 1867 RVA: 0x0001F308 File Offset: 0x0001D508
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

		// Token: 0x0600074C RID: 1868 RVA: 0x0001F371 File Offset: 0x0001D571
		private void Speak(string textToSpeak, Prompt prompt, bool fIsXml)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("VoiceSynthesis");
			}
			this.AddSpeakParameters(new VoiceSynthesis.Parameters(VoiceSynthesis.Action.SpeakText, new VoiceSynthesis.ParametersSpeak(textToSpeak, prompt, fIsXml, null)));
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001F3A6 File Offset: 0x0001D5A6
		private void SpeakStream(Uri audio, Prompt prompt)
		{
			this.AddSpeakParameters(new VoiceSynthesis.Parameters(VoiceSynthesis.Action.SpeakText, new VoiceSynthesis.ParametersSpeak(null, prompt, false, audio)));
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
		private void SetInterest(int ttsInterest)
		{
			this._ttsInterest = ttsInterest;
			Queue<VoiceSynthesis.Parameters> pendingSpeakQueue = this._pendingSpeakQueue;
			lock (pendingSpeakQueue)
			{
				this._site.SetEventsInterest(this._ttsInterest);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001F414 File Offset: 0x0001D614
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

		// Token: 0x04000550 RID: 1360
		internal EventHandler<StateChangedEventArgs> _stateChanged;

		// Token: 0x04000551 RID: 1361
		internal EventHandler<SpeakStartedEventArgs> _speakStarted;

		// Token: 0x04000552 RID: 1362
		internal EventHandler<SpeakCompletedEventArgs> _speakCompleted;

		// Token: 0x04000553 RID: 1363
		internal EventHandler<SpeakProgressEventArgs> _speakProgress;

		// Token: 0x04000554 RID: 1364
		internal EventHandler<BookmarkReachedEventArgs> _bookmarkReached;

		// Token: 0x04000555 RID: 1365
		internal EventHandler<VoiceChangeEventArgs> _voiceChange;

		// Token: 0x04000556 RID: 1366
		internal EventHandler<PhonemeReachedEventArgs> _phonemeReached;

		// Token: 0x04000557 RID: 1367
		internal EventHandler<VisemeReachedEventArgs> _visemeReached;

		// Token: 0x04000558 RID: 1368
		private WaitCallback _eventStateChanged;

		// Token: 0x04000559 RID: 1369
		private WaitCallback _signalWorkerCallback;

		// Token: 0x0400055A RID: 1370
		private readonly ResourceLoader _resourceLoader;

		// Token: 0x0400055B RID: 1371
		private readonly EngineSite _site;

		// Token: 0x0400055C RID: 1372
		private EngineSiteSapi _siteSapi;

		// Token: 0x0400055D RID: 1373
		private IntPtr _iSite;

		// Token: 0x0400055E RID: 1374
		private int _ttsInterest;

		// Token: 0x0400055F RID: 1375
		private ManualResetEvent _evtPendingSpeak = new ManualResetEvent(false);

		// Token: 0x04000560 RID: 1376
		private ManualResetEvent _evtPendingGetProxy = new ManualResetEvent(false);

		// Token: 0x04000561 RID: 1377
		private Exception _pendingException;

		// Token: 0x04000562 RID: 1378
		private Queue<VoiceSynthesis.Parameters> _pendingSpeakQueue = new Queue<VoiceSynthesis.Parameters>();

		// Token: 0x04000563 RID: 1379
		private TTSVoice _pendingVoice;

		// Token: 0x04000564 RID: 1380
		private Thread _workerThread;

		// Token: 0x04000565 RID: 1381
		private bool _fExitWorkerThread;

		// Token: 0x04000566 RID: 1382
		private object _processingSpeakLock = new object();

		// Token: 0x04000567 RID: 1383
		private Dictionary<VoiceInfo, TTSVoice> _voiceDictionary = new Dictionary<VoiceInfo, TTSVoice>();

		// Token: 0x04000568 RID: 1384
		private List<InstalledVoice> _installedVoices;

		// Token: 0x04000569 RID: 1385
		private static List<InstalledVoice> _allVoices;

		// Token: 0x0400056A RID: 1386
		private object _enabledVoicesLock = new object();

		// Token: 0x0400056B RID: 1387
		private TTSVoice _defaultVoice;

		// Token: 0x0400056C RID: 1388
		private TTSVoice _currentVoice;

		// Token: 0x0400056D RID: 1389
		private bool _defaultVoiceInitialized;

		// Token: 0x0400056E RID: 1390
		private object _defaultVoiceLock = new object();

		// Token: 0x0400056F RID: 1391
		private AudioBase _waveOut;

		// Token: 0x04000570 RID: 1392
		private int _defaultRate;

		// Token: 0x04000571 RID: 1393
		private bool _isDisposed;

		// Token: 0x04000572 RID: 1394
		private List<LexiconEntry> _lexicons = new List<LexiconEntry>();

		// Token: 0x04000573 RID: 1395
		private SynthesizerState _synthesizerState;

		// Token: 0x04000574 RID: 1396
		private Prompt _currentPrompt;

		// Token: 0x04000575 RID: 1397
		private const string defaultVoiceRate = "DefaultTTSRate";

		// Token: 0x04000576 RID: 1398
		private AsyncSerializedWorker _asyncWorker;

		// Token: 0x04000577 RID: 1399
		private AsyncSerializedWorker _asyncWorkerUI;

		// Token: 0x04000578 RID: 1400
		private const bool _pexml = false;

		// Token: 0x04000579 RID: 1401
		private int _ttsEvents = 6;

		// Token: 0x0400057A RID: 1402
		private object _thisObjectLock = new object();

		// Token: 0x0400057B RID: 1403
		private AutoResetEvent _workerWaitHandle = new AutoResetEvent(false);

		// Token: 0x0400057C RID: 1404
		private WeakReference _speechSyntesizer;

		// Token: 0x0400057D RID: 1405
		private readonly string[] xmlEscapeStrings = new string[] { "&quot;", "&apos;", "&amp;", "&lt;", "&gt;" };

		// Token: 0x0400057E RID: 1406
		private readonly char[] xmlEscapeChars = new char[] { '"', '\'', '&', '<', '>' };

		// Token: 0x0200019D RID: 413
		private enum Action
		{
			// Token: 0x04000953 RID: 2387
			GetVoice,
			// Token: 0x04000954 RID: 2388
			SpeakText
		}

		// Token: 0x0200019E RID: 414
		private class Parameters
		{
			// Token: 0x06000BA6 RID: 2982 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
			internal Parameters(VoiceSynthesis.Action action, object parameter)
			{
				this._action = action;
				this._parameter = parameter;
			}

			// Token: 0x04000955 RID: 2389
			internal VoiceSynthesis.Action _action;

			// Token: 0x04000956 RID: 2390
			internal object _parameter;
		}

		// Token: 0x0200019F RID: 415
		private class ParametersSpeak
		{
			// Token: 0x06000BA7 RID: 2983 RVA: 0x0002DDEA File Offset: 0x0002BFEA
			internal ParametersSpeak(string textToSpeak, Prompt prompt, bool isXml, Uri audioFile)
			{
				this._textToSpeak = textToSpeak;
				this._prompt = prompt;
				this._isXml = isXml;
				this._audioFile = audioFile;
			}

			// Token: 0x04000957 RID: 2391
			internal string _textToSpeak;

			// Token: 0x04000958 RID: 2392
			internal Prompt _prompt;

			// Token: 0x04000959 RID: 2393
			internal bool _isXml;

			// Token: 0x0400095A RID: 2394
			internal Uri _audioFile;
		}
	}
}
