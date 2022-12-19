using System;

namespace Mono.Cecil
{
	// Token: 0x02000157 RID: 343
	[Flags]
	public enum ModuleCharacteristics
	{
		// Token: 0x04000416 RID: 1046
		HighEntropyVA = 32,
		// Token: 0x04000417 RID: 1047
		DynamicBase = 64,
		// Token: 0x04000418 RID: 1048
		NoSEH = 1024,
		// Token: 0x04000419 RID: 1049
		NXCompat = 256,
		// Token: 0x0400041A RID: 1050
		AppContainer = 4096,
		// Token: 0x0400041B RID: 1051
		TerminalServerAware = 32768
	}
}
