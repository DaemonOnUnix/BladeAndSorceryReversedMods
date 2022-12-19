using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000162 RID: 354
	public abstract class TtsEngineSsml
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0002816E File Offset: 0x0002716E
		protected TtsEngineSsml(string registryKey)
		{
		}

		// Token: 0x0600090C RID: 2316
		public abstract IntPtr GetOutputFormat(SpeakOutputFormat speakOutputFormat, IntPtr targetWaveFormat);

		// Token: 0x0600090D RID: 2317
		public abstract void AddLexicon(Uri uri, string mediaType, ITtsEngineSite site);

		// Token: 0x0600090E RID: 2318
		public abstract void RemoveLexicon(Uri uri, ITtsEngineSite site);

		// Token: 0x0600090F RID: 2319
		public abstract void Speak(TextFragment[] fragment, IntPtr waveHeader, ITtsEngineSite site);
	}
}
