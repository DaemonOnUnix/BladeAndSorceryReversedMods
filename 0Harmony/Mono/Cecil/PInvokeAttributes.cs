using System;

namespace Mono.Cecil
{
	// Token: 0x02000253 RID: 595
	[Flags]
	public enum PInvokeAttributes : ushort
	{
		// Token: 0x0400048E RID: 1166
		NoMangle = 1,
		// Token: 0x0400048F RID: 1167
		CharSetMask = 6,
		// Token: 0x04000490 RID: 1168
		CharSetNotSpec = 0,
		// Token: 0x04000491 RID: 1169
		CharSetAnsi = 2,
		// Token: 0x04000492 RID: 1170
		CharSetUnicode = 4,
		// Token: 0x04000493 RID: 1171
		CharSetAuto = 6,
		// Token: 0x04000494 RID: 1172
		SupportsLastError = 64,
		// Token: 0x04000495 RID: 1173
		CallConvMask = 1792,
		// Token: 0x04000496 RID: 1174
		CallConvWinapi = 256,
		// Token: 0x04000497 RID: 1175
		CallConvCdecl = 512,
		// Token: 0x04000498 RID: 1176
		CallConvStdCall = 768,
		// Token: 0x04000499 RID: 1177
		CallConvThiscall = 1024,
		// Token: 0x0400049A RID: 1178
		CallConvFastcall = 1280,
		// Token: 0x0400049B RID: 1179
		BestFitMask = 48,
		// Token: 0x0400049C RID: 1180
		BestFitEnabled = 16,
		// Token: 0x0400049D RID: 1181
		BestFitDisabled = 32,
		// Token: 0x0400049E RID: 1182
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x0400049F RID: 1183
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x040004A0 RID: 1184
		ThrowOnUnmappableCharDisabled = 8192
	}
}
