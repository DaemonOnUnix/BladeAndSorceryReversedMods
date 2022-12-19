using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000FC RID: 252
	[Flags]
	internal enum SsmlElement
	{
		// Token: 0x0400049C RID: 1180
		Speak = 1,
		// Token: 0x0400049D RID: 1181
		Voice = 2,
		// Token: 0x0400049E RID: 1182
		Audio = 4,
		// Token: 0x0400049F RID: 1183
		Lexicon = 8,
		// Token: 0x040004A0 RID: 1184
		Meta = 16,
		// Token: 0x040004A1 RID: 1185
		MetaData = 32,
		// Token: 0x040004A2 RID: 1186
		Sentence = 64,
		// Token: 0x040004A3 RID: 1187
		Paragraph = 128,
		// Token: 0x040004A4 RID: 1188
		SayAs = 256,
		// Token: 0x040004A5 RID: 1189
		Phoneme = 512,
		// Token: 0x040004A6 RID: 1190
		Sub = 1024,
		// Token: 0x040004A7 RID: 1191
		Emphasis = 2048,
		// Token: 0x040004A8 RID: 1192
		Break = 4096,
		// Token: 0x040004A9 RID: 1193
		Prosody = 8192,
		// Token: 0x040004AA RID: 1194
		Mark = 16384,
		// Token: 0x040004AB RID: 1195
		Desc = 32768,
		// Token: 0x040004AC RID: 1196
		Text = 65536,
		// Token: 0x040004AD RID: 1197
		PromptEngineOutput = 131072,
		// Token: 0x040004AE RID: 1198
		PromptEngineDatabase = 262144,
		// Token: 0x040004AF RID: 1199
		PromptEngineDiv = 524288,
		// Token: 0x040004B0 RID: 1200
		PromptEngineId = 1048576,
		// Token: 0x040004B1 RID: 1201
		PromptEngineTTS = 2097152,
		// Token: 0x040004B2 RID: 1202
		PromptEngineWithTag = 4194304,
		// Token: 0x040004B3 RID: 1203
		PromptEngineRule = 8388608,
		// Token: 0x040004B4 RID: 1204
		ParagraphOrSentence = 192,
		// Token: 0x040004B5 RID: 1205
		AudioMarkTextWithStyle = 229126,
		// Token: 0x040004B6 RID: 1206
		PromptEngineChildren = 16515072
	}
}
