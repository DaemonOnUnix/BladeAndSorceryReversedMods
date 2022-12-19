using System;

namespace Mono.Cecil
{
	// Token: 0x020000BF RID: 191
	[Flags]
	public enum AssemblyAttributes : uint
	{
		// Token: 0x0400023B RID: 571
		PublicKey = 1U,
		// Token: 0x0400023C RID: 572
		SideBySideCompatible = 0U,
		// Token: 0x0400023D RID: 573
		Retargetable = 256U,
		// Token: 0x0400023E RID: 574
		WindowsRuntime = 512U,
		// Token: 0x0400023F RID: 575
		DisableJITCompileOptimizer = 16384U,
		// Token: 0x04000240 RID: 576
		EnableJITCompileTracking = 32768U
	}
}
