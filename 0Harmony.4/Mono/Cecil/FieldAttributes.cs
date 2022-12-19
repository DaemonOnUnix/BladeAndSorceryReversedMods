using System;

namespace Mono.Cecil
{
	// Token: 0x02000200 RID: 512
	[Flags]
	public enum FieldAttributes : ushort
	{
		// Token: 0x04000300 RID: 768
		FieldAccessMask = 7,
		// Token: 0x04000301 RID: 769
		CompilerControlled = 0,
		// Token: 0x04000302 RID: 770
		Private = 1,
		// Token: 0x04000303 RID: 771
		FamANDAssem = 2,
		// Token: 0x04000304 RID: 772
		Assembly = 3,
		// Token: 0x04000305 RID: 773
		Family = 4,
		// Token: 0x04000306 RID: 774
		FamORAssem = 5,
		// Token: 0x04000307 RID: 775
		Public = 6,
		// Token: 0x04000308 RID: 776
		Static = 16,
		// Token: 0x04000309 RID: 777
		InitOnly = 32,
		// Token: 0x0400030A RID: 778
		Literal = 64,
		// Token: 0x0400030B RID: 779
		NotSerialized = 128,
		// Token: 0x0400030C RID: 780
		SpecialName = 512,
		// Token: 0x0400030D RID: 781
		PInvokeImpl = 8192,
		// Token: 0x0400030E RID: 782
		RTSpecialName = 1024,
		// Token: 0x0400030F RID: 783
		HasFieldMarshal = 4096,
		// Token: 0x04000310 RID: 784
		HasDefault = 32768,
		// Token: 0x04000311 RID: 785
		HasFieldRVA = 256
	}
}
