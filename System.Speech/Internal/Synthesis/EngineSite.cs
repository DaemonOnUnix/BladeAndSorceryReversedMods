using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E2 RID: 226
	internal class EngineSite : ITtsEngineSite, ITtsEventSink
	{
		// Token: 0x06000520 RID: 1312 RVA: 0x00016B28 File Offset: 0x00015B28
		internal EngineSite(ResourceLoader resourceLoader)
		{
			this._resourceLoader = resourceLoader;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00016B47 File Offset: 0x00015B47
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00016B4F File Offset: 0x00015B4F
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

		// Token: 0x06000523 RID: 1315 RVA: 0x00016B58 File Offset: 0x00015B58
		public void AddEvents([MarshalAs(42, SizeParamIndex = 1)] SpeechEventInfo[] events, int ulCount)
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

		// Token: 0x06000524 RID: 1316 RVA: 0x00016C10 File Offset: 0x00015C10
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

		// Token: 0x06000525 RID: 1317 RVA: 0x00016C58 File Offset: 0x00015C58
		public SkipInfo GetSkipInfo()
		{
			return new SkipInfo(1, 1);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00016C61 File Offset: 0x00015C61
		public void CompleteSkip(int ulNumSkipped)
		{
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00016C63 File Offset: 0x00015C63
		public int EventInterest
		{
			get
			{
				return this._eventInterest;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00016C6B File Offset: 0x00015C6B
		public int Actions
		{
			get
			{
				return (int)this._actions;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00016C73 File Offset: 0x00015C73
		public int Rate
		{
			get
			{
				this._actions &= ~SPVESACTIONS.SPVES_RATE;
				return this._defaultRate;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00016C8A File Offset: 0x00015C8A
		public int Volume
		{
			get
			{
				this._actions &= ~SPVESACTIONS.SPVES_VOLUME;
				return this._volume;
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00016CA4 File Offset: 0x00015CA4
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

		// Token: 0x0600052C RID: 1324 RVA: 0x00016D58 File Offset: 0x00015D58
		public void AddEvent(TTSEvent evt)
		{
			this._audio.InjectEvent(evt);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00016D66 File Offset: 0x00015D66
		public void FlushEvent()
		{
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00016D68 File Offset: 0x00015D68
		internal void SetEventsInterest(int eventInterest)
		{
			this._eventInterest = eventInterest;
			if (this._eventMapper != null)
			{
				this._eventMapper.FlushEvent();
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00016D9B File Offset: 0x00015D9B
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00016D84 File Offset: 0x00015D84
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00016DBA File Offset: 0x00015DBA
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00016DA3 File Offset: 0x00015DA3
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00016DCB File Offset: 0x00015DCB
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00016DC2 File Offset: 0x00015DC2
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

		// Token: 0x06000535 RID: 1333 RVA: 0x00016DD3 File Offset: 0x00015DD3
		internal void Abort()
		{
			this._actions = SPVESACTIONS.SPVES_ABORT;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00016DDC File Offset: 0x00015DDC
		internal void InitRun(AudioBase audioDevice, int defaultRate, Prompt prompt)
		{
			this._audio = audioDevice;
			this._prompt = prompt;
			this._defaultRate = defaultRate;
			this._actions = SPVESACTIONS.SPVES_RATE | SPVESACTIONS.SPVES_VOLUME;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00016DFC File Offset: 0x00015DFC
		private TTSEvent CreateTtsEvent(SpeechEventInfo sapiEvent)
		{
			switch (sapiEvent.EventId)
			{
			case 4:
			{
				string text = Marshal.PtrToStringUni(sapiEvent.Param2);
				return new TTSEvent((TtsEventId)sapiEvent.EventId, this._prompt, null, null, this._audio.Duration, this._audio.Position, text, (uint)sapiEvent.Param1, sapiEvent.Param2);
			}
			case 6:
				return TTSEvent.CreatePhonemeEvent("" + (char)((int)sapiEvent.Param2 & 65535), "" + (char)(sapiEvent.Param1 & 65535), TimeSpan.FromMilliseconds((double)(sapiEvent.Param1 >> 16)), (SynthesizerEmphasis)((uint)(int)sapiEvent.Param2 >> 16), this._prompt, this._audio.Duration);
			}
			return new TTSEvent((TtsEventId)sapiEvent.EventId, this._prompt, null, null, this._audio.Duration, this._audio.Position, null, (uint)sapiEvent.Param1, sapiEvent.Param2);
		}

		// Token: 0x04000407 RID: 1031
		private const int WAVE_FORMAT_PCM = 1;

		// Token: 0x04000408 RID: 1032
		private int _eventInterest;

		// Token: 0x04000409 RID: 1033
		private SPVESACTIONS _actions = SPVESACTIONS.SPVES_RATE | SPVESACTIONS.SPVES_VOLUME;

		// Token: 0x0400040A RID: 1034
		private AudioBase _audio;

		// Token: 0x0400040B RID: 1035
		private Prompt _prompt;

		// Token: 0x0400040C RID: 1036
		private Exception _exception;

		// Token: 0x0400040D RID: 1037
		private int _defaultRate;

		// Token: 0x0400040E RID: 1038
		private int _volume = 100;

		// Token: 0x0400040F RID: 1039
		private ResourceLoader _resourceLoader;

		// Token: 0x04000410 RID: 1040
		private TtsEventMapper _eventMapper;
	}
}
