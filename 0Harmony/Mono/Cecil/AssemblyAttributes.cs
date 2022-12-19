using System;

namespace Mono.Cecil
{
	// Token: 0x020001B1 RID: 433
	[Flags]
	public enum AssemblyAttributes : uint
	{
		// Token: 0x04000269 RID: 617
		PublicKey = 1U,
		// Token: 0x0400026A RID: 618
		SideBySideCompatible = 0U,
		// Token: 0x0400026B RID: 619
		Retargetable = 256U,
		// Token: 0x0400026C RID: 620
		WindowsRuntime = 512U,
		// Token: 0x0400026D RID: 621
		DisableJITCompileOptimizer = 16384U,
		// Token: 0x0400026E RID: 622
		EnableJITCompileTracking = 32768U
	}
}
