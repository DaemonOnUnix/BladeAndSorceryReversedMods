using System;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000036 RID: 54
	public static class PBBSleepParser
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000B277 File Offset: 0x00009477
		// (set) Token: 0x06000180 RID: 384 RVA: 0x0000B27E File Offset: 0x0000947E
		public static int ammoMax { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000B286 File Offset: 0x00009486
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000B28D File Offset: 0x0000948D
		public static Color EmissionColor { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000B295 File Offset: 0x00009495
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000B29C File Offset: 0x0000949C
		public static float KnockOutMinutes { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000B2A4 File Offset: 0x000094A4
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000B2AB File Offset: 0x000094AB
		public static float CombatDelaySeconds { get; set; }
	}
}
