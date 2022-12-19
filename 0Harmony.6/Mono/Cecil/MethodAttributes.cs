using System;

namespace Mono.Cecil
{
	// Token: 0x02000140 RID: 320
	[Flags]
	public enum MethodAttributes : ushort
	{
		// Token: 0x04000352 RID: 850
		MemberAccessMask = 7,
		// Token: 0x04000353 RID: 851
		CompilerControlled = 0,
		// Token: 0x04000354 RID: 852
		Private = 1,
		// Token: 0x04000355 RID: 853
		FamANDAssem = 2,
		// Token: 0x04000356 RID: 854
		Assembly = 3,
		// Token: 0x04000357 RID: 855
		Family = 4,
		// Token: 0x04000358 RID: 856
		FamORAssem = 5,
		// Token: 0x04000359 RID: 857
		Public = 6,
		// Token: 0x0400035A RID: 858
		Static = 16,
		// Token: 0x0400035B RID: 859
		Final = 32,
		// Token: 0x0400035C RID: 860
		Virtual = 64,
		// Token: 0x0400035D RID: 861
		HideBySig = 128,
		// Token: 0x0400035E RID: 862
		VtableLayoutMask = 256,
		// Token: 0x0400035F RID: 863
		ReuseSlot = 0,
		// Token: 0x04000360 RID: 864
		NewSlot = 256,
		// Token: 0x04000361 RID: 865
		CheckAccessOnOverride = 512,
		// Token: 0x04000362 RID: 866
		Abstract = 1024,
		// Token: 0x04000363 RID: 867
		SpecialName = 2048,
		// Token: 0x04000364 RID: 868
		PInvokeImpl = 8192,
		// Token: 0x04000365 RID: 869
		UnmanagedExport = 8,
		// Token: 0x04000366 RID: 870
		RTSpecialName = 4096,
		// Token: 0x04000367 RID: 871
		HasSecurity = 16384,
		// Token: 0x04000368 RID: 872
		RequireSecObject = 32768
	}
}
