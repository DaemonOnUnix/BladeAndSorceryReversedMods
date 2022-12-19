using System;

namespace Mono.Cecil
{
	// Token: 0x02000233 RID: 563
	[Flags]
	public enum MethodAttributes : ushort
	{
		// Token: 0x04000384 RID: 900
		MemberAccessMask = 7,
		// Token: 0x04000385 RID: 901
		CompilerControlled = 0,
		// Token: 0x04000386 RID: 902
		Private = 1,
		// Token: 0x04000387 RID: 903
		FamANDAssem = 2,
		// Token: 0x04000388 RID: 904
		Assembly = 3,
		// Token: 0x04000389 RID: 905
		Family = 4,
		// Token: 0x0400038A RID: 906
		FamORAssem = 5,
		// Token: 0x0400038B RID: 907
		Public = 6,
		// Token: 0x0400038C RID: 908
		Static = 16,
		// Token: 0x0400038D RID: 909
		Final = 32,
		// Token: 0x0400038E RID: 910
		Virtual = 64,
		// Token: 0x0400038F RID: 911
		HideBySig = 128,
		// Token: 0x04000390 RID: 912
		VtableLayoutMask = 256,
		// Token: 0x04000391 RID: 913
		ReuseSlot = 0,
		// Token: 0x04000392 RID: 914
		NewSlot = 256,
		// Token: 0x04000393 RID: 915
		CheckAccessOnOverride = 512,
		// Token: 0x04000394 RID: 916
		Abstract = 1024,
		// Token: 0x04000395 RID: 917
		SpecialName = 2048,
		// Token: 0x04000396 RID: 918
		PInvokeImpl = 8192,
		// Token: 0x04000397 RID: 919
		UnmanagedExport = 8,
		// Token: 0x04000398 RID: 920
		RTSpecialName = 4096,
		// Token: 0x04000399 RID: 921
		HasSecurity = 16384,
		// Token: 0x0400039A RID: 922
		RequireSecObject = 32768
	}
}
