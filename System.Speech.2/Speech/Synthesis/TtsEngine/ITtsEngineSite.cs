using System;
using System.IO;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002E RID: 46
	public interface ITtsEngineSite
	{
		// Token: 0x060000D4 RID: 212
		void AddEvents(SpeechEventInfo[] events, int count);

		// Token: 0x060000D5 RID: 213
		int Write(IntPtr data, int count);

		// Token: 0x060000D6 RID: 214
		SkipInfo GetSkipInfo();

		// Token: 0x060000D7 RID: 215
		void CompleteSkip(int skipped);

		// Token: 0x060000D8 RID: 216
		Stream LoadResource(Uri uri, string mediaType);

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D9 RID: 217
		int EventInterest { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000DA RID: 218
		int Actions { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000DB RID: 219
		int Rate { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000DC RID: 220
		int Volume { get; }
	}
}
