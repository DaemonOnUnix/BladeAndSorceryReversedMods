using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000042 RID: 66
	[Guid("2D0FA0DB-AEA2-4AE2-9F8A-7AFC7794E56B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITtsEngineSsml
	{
		// Token: 0x06000130 RID: 304
		void GetOutputFormat(SpeakOutputFormat speakOutputFormat, IntPtr targetWaveFormat, out IntPtr waveHeader);

		// Token: 0x06000131 RID: 305
		void AddLexicon(string location, string mediaType, IntPtr site);

		// Token: 0x06000132 RID: 306
		void RemoveLexicon(string location, IntPtr site);

		// Token: 0x06000133 RID: 307
		void Speak(IntPtr fragments, int count, IntPtr waveHeader, IntPtr site);
	}
}
