using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002C RID: 44
	public abstract class TtsEngineSsml
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00003BF5 File Offset: 0x00001DF5
		protected TtsEngineSsml(string registryKey)
		{
		}

		// Token: 0x060000C2 RID: 194
		public abstract IntPtr GetOutputFormat(SpeakOutputFormat speakOutputFormat, IntPtr targetWaveFormat);

		// Token: 0x060000C3 RID: 195
		public abstract void AddLexicon(Uri uri, string mediaType, ITtsEngineSite site);

		// Token: 0x060000C4 RID: 196
		public abstract void RemoveLexicon(Uri uri, ITtsEngineSite site);

		// Token: 0x060000C5 RID: 197
		public abstract void Speak(TextFragment[] fragment, IntPtr waveHeader, ITtsEngineSite site);
	}
}
