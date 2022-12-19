using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000CC RID: 204
	internal class TTSVoice
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0001CD3C File Offset: 0x0001AF3C
		internal TTSVoice(ITtsEngineProxy engine, VoiceInfo voiceId)
		{
			this._engine = engine;
			this._voiceId = voiceId;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001CD60 File Offset: 0x0001AF60
		public override bool Equals(object obj)
		{
			TTSVoice ttsvoice = obj as TTSVoice;
			return ttsvoice != null && this._voiceId.Equals(ttsvoice.VoiceInfo);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001CD8A File Offset: 0x0001AF8A
		public override int GetHashCode()
		{
			return this._voiceId.GetHashCode();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001CD98 File Offset: 0x0001AF98
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

		// Token: 0x0600070F RID: 1807 RVA: 0x0001CE5C File Offset: 0x0001B05C
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
					gchandle = GCHandle.Alloc(targetWaveFormat, GCHandleType.Pinned);
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001CF58 File Offset: 0x0001B158
		internal ITtsEngineProxy TtsEngine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001CF60 File Offset: 0x0001B160
		internal VoiceInfo VoiceInfo
		{
			get
			{
				return this._voiceId;
			}
		}

		// Token: 0x0400054C RID: 1356
		private ITtsEngineProxy _engine;

		// Token: 0x0400054D RID: 1357
		private VoiceInfo _voiceId;

		// Token: 0x0400054E RID: 1358
		private List<LexiconEntry> _lexicons = new List<LexiconEntry>();

		// Token: 0x0400054F RID: 1359
		private byte[] _waveFormat;
	}
}
