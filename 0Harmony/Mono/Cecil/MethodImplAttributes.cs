using System;

namespace Mono.Cecil
{
	// Token: 0x02000237 RID: 567
	[Flags]
	public enum MethodImplAttributes : ushort
	{
		// Token: 0x040003B6 RID: 950
		CodeTypeMask = 3,
		// Token: 0x040003B7 RID: 951
		IL = 0,
		// Token: 0x040003B8 RID: 952
		Native = 1,
		// Token: 0x040003B9 RID: 953
		OPTIL = 2,
		// Token: 0x040003BA RID: 954
		Runtime = 3,
		// Token: 0x040003BB RID: 955
		ManagedMask = 4,
		// Token: 0x040003BC RID: 956
		Unmanaged = 4,
		// Token: 0x040003BD RID: 957
		Managed = 0,
		// Token: 0x040003BE RID: 958
		ForwardRef = 16,
		// Token: 0x040003BF RID: 959
		PreserveSig = 128,
		// Token: 0x040003C0 RID: 960
		InternalCall = 4096,
		// Token: 0x040003C1 RID: 961
		Synchronized = 32,
		// Token: 0x040003C2 RID: 962
		NoOptimization = 64,
		// Token: 0x040003C3 RID: 963
		NoInlining = 8,
		// Token: 0x040003C4 RID: 964
		AggressiveInlining = 256
	}
}
