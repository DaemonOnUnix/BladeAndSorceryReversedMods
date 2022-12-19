using System;

namespace Mono.Cecil
{
	// Token: 0x02000144 RID: 324
	[Flags]
	public enum MethodImplAttributes : ushort
	{
		// Token: 0x04000384 RID: 900
		CodeTypeMask = 3,
		// Token: 0x04000385 RID: 901
		IL = 0,
		// Token: 0x04000386 RID: 902
		Native = 1,
		// Token: 0x04000387 RID: 903
		OPTIL = 2,
		// Token: 0x04000388 RID: 904
		Runtime = 3,
		// Token: 0x04000389 RID: 905
		ManagedMask = 4,
		// Token: 0x0400038A RID: 906
		Unmanaged = 4,
		// Token: 0x0400038B RID: 907
		Managed = 0,
		// Token: 0x0400038C RID: 908
		ForwardRef = 16,
		// Token: 0x0400038D RID: 909
		PreserveSig = 128,
		// Token: 0x0400038E RID: 910
		InternalCall = 4096,
		// Token: 0x0400038F RID: 911
		Synchronized = 32,
		// Token: 0x04000390 RID: 912
		NoOptimization = 64,
		// Token: 0x04000391 RID: 913
		NoInlining = 8,
		// Token: 0x04000392 RID: 914
		AggressiveInlining = 256
	}
}
