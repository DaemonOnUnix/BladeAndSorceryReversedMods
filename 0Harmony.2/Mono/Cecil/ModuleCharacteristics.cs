using System;

namespace Mono.Cecil
{
	// Token: 0x0200024B RID: 587
	[Flags]
	public enum ModuleCharacteristics
	{
		// Token: 0x0400044B RID: 1099
		HighEntropyVA = 32,
		// Token: 0x0400044C RID: 1100
		DynamicBase = 64,
		// Token: 0x0400044D RID: 1101
		NoSEH = 1024,
		// Token: 0x0400044E RID: 1102
		NXCompat = 256,
		// Token: 0x0400044F RID: 1103
		AppContainer = 4096,
		// Token: 0x04000450 RID: 1104
		TerminalServerAware = 32768
	}
}
