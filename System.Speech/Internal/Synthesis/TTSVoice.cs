using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000104 RID: 260
	internal class TTSVoice
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x0001BEE4 File Offset: 0x0001AEE4
		internal TTSVoice(ITtsEngineProxy engine, VoiceInfo voiceId)
		{
			this._engine = engine;
			this._voiceId = voiceId;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001BF08 File Offset: 0x0001AF08
		public override bool Equals(object obj)
		{
			TTSVoice ttsvoice = obj as TTSVoice;
			return ttsvoice != null && this._voiceId.Equals(ttsvoice.VoiceInfo);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001BF32 File Offset: 0x0001AF32
		public override int GetHashCode()
		{
			return this._voiceId.GetHashCode();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001BF40 File Offset: 0x0001AF40
		internal void UpdateLexicons(List<LexiconEntry> lexicons)
		{
			for (int i = this._lexicons.Count - 1; i >= 0; i--)
			{
				LexiconEntry lexiconEntry = this._lexicons[i];
				if (!lexicons.Contains(lexiconEntry))
				{
					this._lexicons.RemoveAt(i);
					this.TtsEngine.RemoveLexicon(lexiconEntry._uri);
				}
			}
			foreach (LexiconEntry lexiconEntry2 in lexicons)
			{
				if (!this._lexicons.Contains(lexiconEntry2))
				{
					this.TtsEngine.AddLexicon(lexiconEntry2._uri, lexiconEntry2._mediaType);
					this._lexicons.Add(lexiconEntry2);
				}
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001C004 File Offset: 0x0001B004
		internal byte[] WaveFormat(byte[] targetWaveFormat)
		{
			if (targetWaveFormat == null && this._waveFormat == null && this.VoiceInfo.SupportedAudioFormats.Count > 0)
			{
				targetWaveFormat = this.VoiceInfo.SupportedAudioFormats[0].WaveFormat;
			}
			if (targetWaveFormat == null && this._waveFormat != null)
			{
				return this._waveFormat;
			}
			if (this._waveFormat == null || !object.Equals(targetWaveFormat, this._waveFormat))
			{
				IntPtr intPtr = IntPtr.Zero;
				GCHandle gchandle = default(GCHandle);
				if (targetWaveFormat != null)
				{
					gchandle = GCHandle.Alloc(targetWaveFormat, 3);
				}
				try
				{
					intPtr = this._engine.GetOutputFormat((targetWaveFormat != null) ? gchandle.AddrOfPinnedObject() : IntPtr.Zero);
				}
				finally
				{
					if (targetWaveFormat != null)
					{
						gchandle.Free();
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					this._waveFormat = WAVEFORMATEX.ToBytes(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				else
				{
					this._waveFormat = WAVEFORMATEX.Default.ToBytes();
				}
			}
			return this._waveFormat;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001C0FC File Offset: 0x0001B0FC
		internal ITtsEngineProxy TtsEngine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001C104 File Offset: 0x0001B104
		internal VoiceInfo VoiceInfo
		{
			get
			{
				return this._voiceId;
			}
		}

		// Token: 0x040004D5 RID: 1237
		private ITtsEngineProxy _engine;

		// Token: 0x040004D6 RID: 1238
		private VoiceInfo _voiceId;

		// Token: 0x040004D7 RID: 1239
		private List<LexiconEntry> _lexicons = new List<LexiconEntry>();

		// Token: 0x040004D8 RID: 1240
		private byte[] _waveFormat;
	}
}
