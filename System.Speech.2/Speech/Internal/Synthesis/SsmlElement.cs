using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C4 RID: 196
	[Flags]
	internal enum SsmlElement
	{
		// Token: 0x04000513 RID: 1299
		Speak = 1,
		// Token: 0x04000514 RID: 1300
		Voice = 2,
		// Token: 0x04000515 RID: 1301
		Audio = 4,
		// Token: 0x04000516 RID: 1302
		Lexicon = 8,
		// Token: 0x04000517 RID: 1303
		Meta = 16,
		// Token: 0x04000518 RID: 1304
		MetaData = 32,
		// Token: 0x04000519 RID: 1305
		Sentence = 64,
		// Token: 0x0400051A RID: 1306
		Paragraph = 128,
		// Token: 0x0400051B RID: 1307
		SayAs = 256,
		// Token: 0x0400051C RID: 1308
		Phoneme = 512,
		// Token: 0x0400051D RID: 1309
		Sub = 1024,
		// Token: 0x0400051E RID: 1310
		Emphasis = 2048,
		// Token: 0x0400051F RID: 1311
		Break = 4096,
		// Token: 0x04000520 RID: 1312
		Prosody = 8192,
		// Token: 0x04000521 RID: 1313
		Mark = 16384,
		// Token: 0x04000522 RID: 1314
		Desc = 32768,
		// Token: 0x04000523 RID: 1315
		Text = 65536,
		// Token: 0x04000524 RID: 1316
		PromptEngineOutput = 131072,
		// Token: 0x04000525 RID: 1317
		PromptEngineDatabase = 262144,
		// Token: 0x04000526 RID: 1318
		PromptEngineDiv = 524288,
		// Token: 0x04000527 RID: 1319
		PromptEngineId = 1048576,
		// Token: 0x04000528 RID: 1320
		PromptEngineTTS = 2097152,
		// Token: 0x04000529 RID: 1321
		PromptEngineWithTag = 4194304,
		// Token: 0x0400052A RID: 1322
		PromptEngineRule = 8388608,
		// Token: 0x0400052B RID: 1323
		ParagraphOrSentence = 192,
		// Token: 0x0400052C RID: 1324
		AudioMarkTextWithStyle = 229126,
		// Token: 0x0400052D RID: 1325
		PromptEngineChildren = 16515072
	}
}
