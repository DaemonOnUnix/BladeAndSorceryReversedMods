using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000177 RID: 375
	[InterfaceType(1)]
	[Guid("2D0FA0DB-AEA2-4AE2-9F8A-7AFC7794E56B")]
	[ComImport]
	internal interface ITtsEngineSsml
	{
		// Token: 0x06000971 RID: 2417
		void GetOutputFormat(SpeakOutputFormat speakOutputFormat, IntPtr targetWaveFormat, out IntPtr waveHeader);

		// Token: 0x06000972 RID: 2418
		void AddLexicon(string location, string mediaType, IntPtr site);

		// Token: 0x06000973 RID: 2419
		void RemoveLexicon(string location, IntPtr site);

		// Token: 0x06000974 RID: 2420
		void Speak(IntPtr fragments, int count, IntPtr waveHeader, IntPtr site);
	}
}
