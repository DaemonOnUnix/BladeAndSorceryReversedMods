using System;
using System.IO;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x020000E0 RID: 224
	public interface ITtsEngineSite
	{
		// Token: 0x06000515 RID: 1301
		void AddEvents(SpeechEventInfo[] events, int count);

		// Token: 0x06000516 RID: 1302
		int Write(IntPtr data, int count);

		// Token: 0x06000517 RID: 1303
		SkipInfo GetSkipInfo();

		// Token: 0x06000518 RID: 1304
		void CompleteSkip(int skipped);

		// Token: 0x06000519 RID: 1305
		Stream LoadResource(Uri uri, string mediaType);

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600051A RID: 1306
		int EventInterest { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600051B RID: 1307
		int Actions { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600051C RID: 1308
		int Rate { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600051D RID: 1309
		int Volume { get; }
	}
}
