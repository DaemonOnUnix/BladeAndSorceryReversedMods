using System;

namespace Mono.Cecil
{
	// Token: 0x020001B2 RID: 434
	public enum AssemblyHashAlgorithm : uint
	{
		// Token: 0x04000270 RID: 624
		None,
		// Token: 0x04000271 RID: 625
		MD5 = 32771U,
		// Token: 0x04000272 RID: 626
		SHA1,
		// Token: 0x04000273 RID: 627
		SHA256 = 32780U,
		// Token: 0x04000274 RID: 628
		SHA384,
		// Token: 0x04000275 RID: 629
		SHA512,
		// Token: 0x04000276 RID: 630
		Reserved = 32771U
	}
}
