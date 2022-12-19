using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.SapiInterop;
using System.Text;

namespace System.Speech.Recognition
{
	// Token: 0x0200004F RID: 79
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public sealed class RecognitionResult : RecognizedPhrase, ISerializable
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00006804 File Offset: 0x00004A04
		internal RecognitionResult(IRecognizerInternal recognizer, ISpRecoResult recoResult, byte[] sapiResultBlob, int maxAlternates)
		{
			this.Initialize(recognizer, recoResult, sapiResultBlob, maxAlternates);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006822 File Offset: 0x00004A22
		internal RecognitionResult()
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006838 File Offset: 0x00004A38
		private RecognitionResult(SerializationInfo info, StreamingContext context)
		{
			Type type = base.GetType();
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(type, context);
			bool flag = context.State == StreamingContextStates.CrossAppDomain;
			foreach (MemberInfo memberInfo in serializableMembers)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				if (!flag || (memberInfo.Name != "_recognizer" && memberInfo.Name != "_grammar" && memberInfo.Name != "_ruleList" && memberInfo.Name != "_audio" && memberInfo.Name != "_audio"))
				{
					fieldInfo.SetValue(this, info.GetValue(fieldInfo.Name, fieldInfo.FieldType));
				}
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000691C File Offset: 0x00004B1C
		public RecognizedAudio GetAudioForWordRange(RecognizedWordUnit firstWord, RecognizedWordUnit lastWord)
		{
			Helpers.ThrowIfNull(firstWord, "firstWord");
			Helpers.ThrowIfNull(lastWord, "lastWord");
			return this.Audio.GetRange(firstWord._audioPosition, lastWord._audioPosition + lastWord._audioDuration - firstWord._audioPosition);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000696C File Offset: 0x00004B6C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Helpers.ThrowIfNull(info, "info");
			bool flag = context.State == StreamingContextStates.CrossAppDomain;
			if (!flag)
			{
				foreach (RecognizedPhrase recognizedPhrase in this.Alternates)
				{
					try
					{
						string smlContent = recognizedPhrase.SmlContent;
						RecognizedAudio audio = this.Audio;
						if (recognizedPhrase.Text == null || recognizedPhrase.Homophones == null || recognizedPhrase.Semantics == null || (smlContent == null && smlContent != null) || (audio == null && audio != null))
						{
							throw new SerializationException();
						}
					}
					catch (NotSupportedException)
					{
					}
				}
			}
			Type type = base.GetType();
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(type, context);
			foreach (MemberInfo memberInfo in serializableMembers)
			{
				if (!flag || (memberInfo.Name != "_recognizer" && memberInfo.Name != "_grammar" && memberInfo.Name != "_ruleList" && memberInfo.Name != "_audio" && memberInfo.Name != "_audio"))
				{
					info.AddValue(memberInfo.Name, ((FieldInfo)memberInfo).GetValue(this));
				}
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006AD4 File Offset: 0x00004CD4
		internal bool SetTextFeedback(string text, bool isSuccessfulAction)
		{
			if (this._sapiRecoResult == null)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			try
			{
				this._sapiRecoResult.SetTextFeedback(text, isSuccessfulAction);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147200893)
				{
					throw new NotSupportedException(SR.Get(SRID.SapiErrorNotSupportedForInprocRecognizer, new object[0]));
				}
				return false;
			}
			return true;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00006B48 File Offset: 0x00004D48
		public RecognizedAudio Audio
		{
			get
			{
				if (this._audio == null && this._header.ulRetainedOffset > 0U)
				{
					int num = this._sapiAudioBlob.Length;
					GCHandle gchandle = GCHandle.Alloc(this._sapiAudioBlob, GCHandleType.Pinned);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						SPWAVEFORMATEX spwaveformatex = (SPWAVEFORMATEX)Marshal.PtrToStructure(intPtr, typeof(SPWAVEFORMATEX));
						IntPtr intPtr2 = new IntPtr((long)intPtr + (long)((ulong)spwaveformatex.cbUsed));
						byte[] array = new byte[(long)num - (long)((ulong)spwaveformatex.cbUsed)];
						Marshal.Copy(intPtr2, array, 0, num - (int)spwaveformatex.cbUsed);
						byte[] array2 = new byte[(int)spwaveformatex.cbSize];
						if (spwaveformatex.cbSize > 0)
						{
							IntPtr intPtr3 = new IntPtr((long)intPtr + 38L);
							Marshal.Copy(intPtr3, array2, 0, (int)spwaveformatex.cbSize);
						}
						SpeechAudioFormatInfo speechAudioFormatInfo = new SpeechAudioFormatInfo((EncodingFormat)spwaveformatex.wFormatTag, (int)spwaveformatex.nSamplesPerSec, (int)((short)spwaveformatex.wBitsPerSample), (int)((short)spwaveformatex.nChannels), (int)spwaveformatex.nAvgBytesPerSec, (int)((short)spwaveformatex.nBlockAlign), array2);
						DateTime dateTime;
						if (this._header.times.dwTickCount == 0U)
						{
							dateTime = this._startTime - this.AudioDuration;
						}
						else
						{
							dateTime = DateTime.FromFileTime((long)(((ulong)this._header.times.ftStreamTime.dwHighDateTime << 32) + (ulong)this._header.times.ftStreamTime.dwLowDateTime));
						}
						this._audio = new RecognizedAudio(array, speechAudioFormatInfo, dateTime, this.AudioPosition, this.AudioDuration);
					}
					finally
					{
						gchandle.Free();
					}
				}
				return this._audio;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006CF8 File Offset: 0x00004EF8
		public ReadOnlyCollection<RecognizedPhrase> Alternates
		{
			get
			{
				return new ReadOnlyCollection<RecognizedPhrase>(this.GetAlternates());
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006D08 File Offset: 0x00004F08
		internal string ConvertPronunciation(string pronunciation, int langId)
		{
			if (this._alphabetConverter == null)
			{
				this._alphabetConverter = new AlphabetConverter(langId);
			}
			else
			{
				this._alphabetConverter.SetLanguageId(langId);
			}
			char[] array = this._alphabetConverter.SapiToIpa(pronunciation.ToCharArray());
			if (array != null)
			{
				pronunciation = new string(array);
			}
			else
			{
				Trace.TraceError("Cannot convert the pronunciation to IPA alphabet.");
			}
			return pronunciation;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006D62 File Offset: 0x00004F62
		internal IRecognizerInternal Recognizer
		{
			get
			{
				if (this._recognizer == null)
				{
					throw new NotSupportedException(SR.Get(SRID.CantGetPropertyFromSerializedInfo, new object[] { "Recognizer" }));
				}
				return this._recognizer;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006D8D File Offset: 0x00004F8D
		internal TimeSpan AudioPosition
		{
			get
			{
				if (this._audioPosition == null)
				{
					this._audioPosition = new TimeSpan?(new TimeSpan((long)this._header.times.ullStart));
				}
				return this._audioPosition.Value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006DC7 File Offset: 0x00004FC7
		internal TimeSpan AudioDuration
		{
			get
			{
				if (this._audioDuration == null)
				{
					this._audioDuration = new TimeSpan?(new TimeSpan((long)this._header.times.ullLength));
				}
				return this._audioDuration.Value;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006E04 File Offset: 0x00005004
		private void Initialize(IRecognizerInternal recognizer, ISpRecoResult recoResult, byte[] sapiResultBlob, int maxAlternates)
		{
			this._recognizer = recognizer;
			this._maxAlternates = maxAlternates;
			try
			{
				this._sapiRecoResult = recoResult as ISpRecoResult2;
			}
			catch (COMException)
			{
				this._sapiRecoResult = null;
			}
			GCHandle gchandle = GCHandle.Alloc(sapiResultBlob, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				int num = Marshal.ReadInt32(intPtr, 4);
				if (num == Marshal.SizeOf(typeof(SPRESULTHEADER_Sapi51)))
				{
					SPRESULTHEADER_Sapi51 spresultheader_Sapi = (SPRESULTHEADER_Sapi51)Marshal.PtrToStructure(intPtr, typeof(SPRESULTHEADER_Sapi51));
					this._header = new SPRESULTHEADER(spresultheader_Sapi);
					this._isSapi53Header = false;
				}
				else
				{
					this._header = (SPRESULTHEADER)Marshal.PtrToStructure(intPtr, typeof(SPRESULTHEADER));
					this._isSapi53Header = true;
				}
				this._header.Validate();
				IntPtr intPtr2 = new IntPtr((long)intPtr + (long)this._header.ulPhraseOffset);
				SPSERIALIZEDPHRASE phraseHeader = RecognizedPhrase.GetPhraseHeader(intPtr2, this._header.ulPhraseDataSize, this._isSapi53Header);
				bool flag = (this._header.fAlphabet & 1U) > 0U;
				base.InitializeFromSerializedBuffer(this, phraseHeader, intPtr2, (int)this._header.ulPhraseDataSize, this._isSapi53Header, flag);
				if (recoResult != null)
				{
					this.ExtractDictationAlternates(recoResult, maxAlternates);
					recoResult.Discard(255U);
				}
			}
			finally
			{
				gchandle.Free();
			}
			this._sapiAudioBlob = new byte[this._header.ulRetainedDataSize];
			Array.Copy(sapiResultBlob, (int)this._header.ulRetainedOffset, this._sapiAudioBlob, 0, (int)this._header.ulRetainedDataSize);
			this._sapiAlternatesBlob = new byte[this._header.ulPhraseAltDataSize];
			Array.Copy(sapiResultBlob, (int)this._header.ulPhraseAltOffset, this._sapiAlternatesBlob, 0, (int)this._header.ulPhraseAltDataSize);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006FD0 File Offset: 0x000051D0
		private Collection<RecognizedPhrase> ExtractAlternates(int numberOfAlternates, bool isSapi53Header)
		{
			Collection<RecognizedPhrase> collection = new Collection<RecognizedPhrase>();
			if (numberOfAlternates > 0)
			{
				GCHandle gchandle = GCHandle.Alloc(this._sapiAlternatesBlob, GCHandleType.Pinned);
				try
				{
					IntPtr intPtr = gchandle.AddrOfPinnedObject();
					int num = Marshal.SizeOf(typeof(SPSERIALIZEDPHRASEALT));
					int num2 = 0;
					for (int i = 0; i < numberOfAlternates; i++)
					{
						IntPtr intPtr2 = new IntPtr((long)intPtr + (long)num2);
						SPSERIALIZEDPHRASEALT spserializedphrasealt = (SPSERIALIZEDPHRASEALT)Marshal.PtrToStructure(intPtr2, typeof(SPSERIALIZEDPHRASEALT));
						num2 += num;
						if (isSapi53Header)
						{
							num2 += (int)((ulong)(spserializedphrasealt.cbAltExtra + 7U) & 18446744073709551608UL);
						}
						else
						{
							num2 += (int)spserializedphrasealt.cbAltExtra;
						}
						IntPtr intPtr3 = new IntPtr((long)intPtr + (long)num2);
						SPSERIALIZEDPHRASE phraseHeader = RecognizedPhrase.GetPhraseHeader(intPtr3, this._header.ulPhraseAltDataSize - (uint)num2, this._isSapi53Header);
						int ulSerializedSize = (int)phraseHeader.ulSerializedSize;
						RecognizedPhrase recognizedPhrase = new RecognizedPhrase();
						bool flag = (this._header.fAlphabet & 2U) > 0U;
						recognizedPhrase.InitializeFromSerializedBuffer(this, phraseHeader, intPtr3, ulSerializedSize, isSapi53Header, flag);
						if (isSapi53Header)
						{
							num2 += (ulSerializedSize + 7) & -8;
						}
						else
						{
							num2 += ulSerializedSize;
						}
						collection.Add(recognizedPhrase);
					}
				}
				finally
				{
					gchandle.Free();
				}
			}
			return collection;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007124 File Offset: 0x00005324
		private void ExtractDictationAlternates(ISpRecoResult recoResult, int maxAlternates)
		{
			if (recoResult != null && base.Grammar is DictationGrammar)
			{
				this._alternates = new Collection<RecognizedPhrase>();
				IntPtr[] array = new IntPtr[maxAlternates];
				try
				{
					recoResult.GetAlternates(0, -1, maxAlternates, array, out maxAlternates);
				}
				catch (COMException)
				{
					maxAlternates = 0;
				}
				uint num = 0U;
				while ((ulong)num < (ulong)((long)maxAlternates))
				{
					ISpPhraseAlt spPhraseAlt = (ISpPhraseAlt)Marshal.GetObjectForIUnknown(array[(int)num]);
					try
					{
						IntPtr intPtr;
						spPhraseAlt.GetSerializedPhrase(out intPtr);
						try
						{
							RecognizedPhrase recognizedPhrase = new RecognizedPhrase();
							SPSERIALIZEDPHRASE phraseHeader = RecognizedPhrase.GetPhraseHeader(intPtr, uint.MaxValue, this._isSapi53Header);
							bool flag = (this._header.fAlphabet & 1U) > 0U;
							recognizedPhrase.InitializeFromSerializedBuffer(this, phraseHeader, intPtr, (int)phraseHeader.ulSerializedSize, this._isSapi53Header, flag);
							this._alternates.Add(recognizedPhrase);
						}
						finally
						{
							Marshal.FreeCoTaskMem(intPtr);
						}
					}
					finally
					{
						Marshal.Release(array[(int)num]);
					}
					num += 1U;
				}
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007220 File Offset: 0x00005420
		private Collection<RecognizedPhrase> GetAlternates()
		{
			if (this._alternates == null)
			{
				this._alternates = this.ExtractAlternates((int)this._header.ulNumPhraseAlts, this._isSapi53Header);
				if (this._alternates.Count == 0 && this._maxAlternates > 0)
				{
					RecognizedPhrase recognizedPhrase = new RecognizedPhrase();
					GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, GCHandleType.Pinned);
					try
					{
						recognizedPhrase.InitializeFromSerializedBuffer(this, this._serializedPhrase, gchandle.AddrOfPinnedObject(), this._phraseBuffer.Length, this._isSapi53Header, this._hasIPAPronunciation);
					}
					finally
					{
						gchandle.Free();
					}
					this._alternates.Add(recognizedPhrase);
				}
			}
			return this._alternates;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000072D4 File Offset: 0x000054D4
		internal string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("Recognized text: '");
			stringBuilder.Append(base.Text);
			stringBuilder.Append("'");
			if (base.Semantics.Value != null)
			{
				stringBuilder.Append(" - Semantic Value  = ");
				stringBuilder.Append(base.Semantics.Value.ToString());
			}
			if (base.Semantics.Count > 0)
			{
				stringBuilder.Append(" - Semantic children count = ");
				stringBuilder.Append(base.Semantics.Count.ToString(CultureInfo.InvariantCulture));
			}
			if (this.Alternates.Count > 1)
			{
				stringBuilder.Append(" - Alternate word count = ");
				stringBuilder.Append(this.Alternates.Count.ToString(CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040002F5 RID: 757
		[NonSerialized]
		private IRecognizerInternal _recognizer;

		// Token: 0x040002F6 RID: 758
		[NonSerialized]
		private int _maxAlternates;

		// Token: 0x040002F7 RID: 759
		[NonSerialized]
		private AlphabetConverter _alphabetConverter;

		// Token: 0x040002F8 RID: 760
		private byte[] _sapiAudioBlob;

		// Token: 0x040002F9 RID: 761
		private byte[] _sapiAlternatesBlob;

		// Token: 0x040002FA RID: 762
		private Collection<RecognizedPhrase> _alternates;

		// Token: 0x040002FB RID: 763
		private SPRESULTHEADER _header;

		// Token: 0x040002FC RID: 764
		private RecognizedAudio _audio;

		// Token: 0x040002FD RID: 765
		private DateTime _startTime = DateTime.Now;

		// Token: 0x040002FE RID: 766
		[NonSerialized]
		private ISpRecoResult2 _sapiRecoResult;

		// Token: 0x040002FF RID: 767
		private TimeSpan? _audioPosition;

		// Token: 0x04000300 RID: 768
		private TimeSpan? _audioDuration;
	}
}
