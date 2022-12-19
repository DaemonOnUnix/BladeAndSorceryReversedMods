using System;

namespace HarmonyLib
{
	// Token: 0x02000046 RID: 70
	public enum HarmonyPatchType
	{
		// Token: 0x040000FB RID: 251
		All,
		// Token: 0x040000FC RID: 252
		Prefix,
		// Token: 0x040000FD RID: 253
		Postfix,
		// Token: 0x040000FE RID: 254
		Transpiler,
		// Token: 0x040000FF RID: 255
		Finalizer,
		// Token: 0x04000100 RID: 256
		ReversePatch
	}
}
