using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B2 RID: 178
	internal class EngineSite : ITtsEngineSite, ITtsEventSink
	{
		// Token: 0x060005FE RID: 1534 RVA: 0x00017E78 File Offset: 0x00016078
		internal EngineSite(ResourceLoader resourceLoader)
		{
			this._resourceLoader = resourceLoader;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00017E97 File Offset: 0x00016097
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x00017E9F File Offset: 0x0001609F
		internal TtsEventMapper EventMapper
		{
			get
			{
				return this._eventMapper;
			}
			set
			{
				this._eventMapper = value;
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00017EA8 File Offset: 0x000160A8
		public void AddEvents([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] SpeechEventInfo[] events, int ulCount)
		{
			try
			{
				foreach (SpeechEventInfo speechEventInfo in events)
				{
					int num = 1 << (int)speechEventInfo.EventId;
					if (speechEventInfo.EventId == 2 && this._eventMapper != null)
					{
						this._eventMapper.FlushEvent();
					}
					if ((num & this._eventInterest) != 0)
					{
						TTSEvent ttsevent = this.CreateTtsEvent(speechEventInfo);
						if (this._eventMapper == null)
						{
							this.AddEvent(ttsevent);
						}
						else
						{
							this._eventMapper.AddEvent(ttsevent);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this._exception = ex;
				this._actions |= SPVESACTIONS.SPVES_ABORT;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00017F58 File Offset: 0x00016158
		public int Write(IntPtr pBuff, int cb)
		{
			try
			{
				this._audio.Play(pBuff, cb);
			}
			catch (Exception ex)
			{
				this._exception = ex;
				this._actions |= SPVESACTIONS.SPVES_ABORT;
			}
			return cb;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00017FA0 File Offset: 0x000161A0
		public SkipInfo GetSkipInfo()
		{
			return new SkipInfo(1, 1);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void CompleteSkip(int ulNumSkipped)
		{
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00017FA9 File Offset: 0x000161A9
		public int EventInterest
		{
			get
			{
				return this._eventInterest;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00017FB1 File Offset: 0x000161B1
		public int Actions
		{
			get
			{
				return (int)this._actions;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00017FB9 File Offset: 0x000161B9
		public int Rate
		{
			get
			{
				this._actions &= ~SPVESACTIONS.SPVES_RATE;
				return this._defaultRate;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00017FD0 File Offset: 0x000161D0
		public int Volume
		{
			get
			{
				this._actions &= ~SPVESACTIONS.SPVES_VOLUME;
				return this._volume;
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00017FE8 File Offset: 0x000161E8
		public Stream LoadResource(Uri uri, string mediaType)
		{
			try
			{
				string text;
				Uri uri2;
				string text2;
				using (Stream stream = this._resourceLoader.LoadFile(uri, out text, out uri2, out text2))
				{
					int num = (int)stream.Length;
					MemoryStream memoryStream = new MemoryStream(num);
					byte[] array = new byte[num];
					stream.Read(array, 0, array.Length);
					this._resourceLoader.UnloadFile(text2);
					memoryStream.Write(array, 0, num);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
			catch (Exception ex)
			{
				this._exception = ex;
				this._actions |= SPVESACTIONS.SPVES_ABORT;
			}
			return null;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001809C File Offset: 0x0001629C
		public void AddEvent(TTSEvent evt)
		{
			this._audio.InjectEvent(evt);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void FlushEvent()
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000180AA File Offset: 0x000162AA
		internal void SetEventsInterest(int eventInterest)
		{
			this._eventInterest = eventInterest;
			if (this._eventMapper != null)
			{
				this._eventMapper.FlushEvent();
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000180DD File Offset: 0x000162DD
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x000180C6 File Offset: 0x000162C6
		internal int VoiceRate
		{
			get
			{
				return this._defaultRate;
			}
			set
			{
				this._defaultRate = value;
				this._actions |= SPVESACTIONS.SPVES_RATE;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x000180FC File Offset: 0x000162FC
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x000180E5 File Offset: 0x000162E5
		internal int VoiceVolume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
				this._actions |= SPVESACTIONS.SPVES_VOLUME;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001810D File Offset: 0x0001630D
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00018104 File Offset: 0x00016304
		internal Exception LastException
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00018115 File Offset: 0x00016315
		internal void Abort()
		{
			this._actions = SPVESACTIONS.SPVES_ABORT;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001811E File Offset: 0x0001631E
		internal void InitRun(AudioBase audioDevice, int defaultRate, Prompt prompt)
		{
			this._audio = audioDevice;
			this._prompt = prompt;
			this._defaultRate = defaultRate;
			this._actions = SPVESACTIONS.SPVES_RATE | SPVESACTIONS.SPVES_VOLUME;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00018140 File Offset: 0x00016340
		private TTSEvent CreateTtsEvent(SpeechEventInfo sapiEvent)
		{
			TtsEventId eventId = (TtsEventId)sapiEvent.EventId;
			TTSEvent ttsevent;
			if (eventId != TtsEventId.Bookmark)
			{
				if (eventId == TtsEventId.Phoneme)
				{
					ttsevent = TTSEvent.CreatePhonemeEvent(((char)((int)sapiEvent.Param2 & 65535)).ToString() ?? "", ((char)(sapiEvent.Param1 & 65535)).ToString() ?? "", TimeSpan.FromMilliseconds((double)(sapiEvent.Param1 >> 16)), (SynthesizerEmphasis)((uint)(int)sapiEvent.Param2 >> 16), this._prompt, this._audio.Duration);
				}
				else
				{
					ttsevent = new TTSEvent((TtsEventId)sapiEvent.EventId, this._prompt, null, null, this._audio.Duration, this._audio.Position, null, (uint)sapiEvent.Param1, sapiEvent.Param2);
				}
			}
			else
			{
				string text = Marshal.PtrToStringUni(sapiEvent.Param2);
				ttsevent = new TTSEvent((TtsEventId)sapiEvent.EventId, this._prompt, null, null, this._audio.Duration, this._audio.Position, text, (uint)sapiEvent.Param1, sapiEvent.Param2);
			}
			return ttsevent;
		}

		// Token: 0x0400049A RID: 1178
		private int _eventInterest;

		// Token: 0x0400049B RID: 1179
		private SPVESACTIONS _actions = SPVESACTIONS.SPVES_RATE | SPVESACTIONS.SPVES_VOLUME;

		// Token: 0x0400049C RID: 1180
		private AudioBase _audio;

		// Token: 0x0400049D RID: 1181
		private Prompt _prompt;

		// Token: 0x0400049E RID: 1182
		private Exception _exception;

		// Token: 0x0400049F RID: 1183
		private int _defaultRate;

		// Token: 0x040004A0 RID: 1184
		private int _volume = 100;

		// Token: 0x040004A1 RID: 1185
		private ResourceLoader _resourceLoader;

		// Token: 0x040004A2 RID: 1186
		private TtsEventMapper _eventMapper;

		// Token: 0x040004A3 RID: 1187
		private const int WAVE_FORMAT_PCM = 1;
	}
}
