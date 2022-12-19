using System;

namespace Mono.Cecil
{
	// Token: 0x0200015F RID: 351
	[Flags]
	public enum PInvokeAttributes : ushort
	{
		// Token: 0x04000459 RID: 1113
		NoMangle = 1,
		// Token: 0x0400045A RID: 1114
		CharSetMask = 6,
		// Token: 0x0400045B RID: 1115
		CharSetNotSpec = 0,
		// Token: 0x0400045C RID: 1116
		CharSetAnsi = 2,
		// Token: 0x0400045D RID: 1117
		CharSetUnicode = 4,
		// Token: 0x0400045E RID: 1118
		CharSetAuto = 6,
		// Token: 0x0400045F RID: 1119
		SupportsLastError = 64,
		// Token: 0x04000460 RID: 1120
		CallConvMask = 1792,
		// Token: 0x04000461 RID: 1121
		CallConvWinapi = 256,
		// Token: 0x04000462 RID: 1122
		CallConvCdecl = 512,
		// Token: 0x04000463 RID: 1123
		CallConvStdCall = 768,
		// Token: 0x04000464 RID: 1124
		CallConvThiscall = 1024,
		// Token: 0x04000465 RID: 1125
		CallConvFastcall = 1280,
		// Token: 0x04000466 RID: 1126
		BestFitMask = 48,
		// Token: 0x04000467 RID: 1127
		BestFitEnabled = 16,
		// Token: 0x04000468 RID: 1128
		BestFitDisabled = 32,
		// Token: 0x04000469 RID: 1129
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x0400046A RID: 1130
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x0400046B RID: 1131
		ThrowOnUnmappableCharDisabled = 8192
	}
}
