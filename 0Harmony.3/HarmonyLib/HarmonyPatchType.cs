using System;

namespace HarmonyLib
{
	// Token: 0x02000045 RID: 69
	public enum HarmonyPatchType
	{
		// Token: 0x040000EF RID: 239
		All,
		// Token: 0x040000F0 RID: 240
		Prefix,
		// Token: 0x040000F1 RID: 241
		Postfix,
		// Token: 0x040000F2 RID: 242
		Transpiler,
		// Token: 0x040000F3 RID: 243
		Finalizer,
		// Token: 0x040000F4 RID: 244
		ReversePatch
	}
}
