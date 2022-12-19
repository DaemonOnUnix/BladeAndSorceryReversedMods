using System;
using System.Collections.Generic;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200001B RID: 27
	public class PBBHolderSave : ContentCustomData
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000071F9 File Offset: 0x000053F9
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00007201 File Offset: 0x00005401
		public List<string> AmmoType { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000720A File Offset: 0x0000540A
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00007212 File Offset: 0x00005412
		public List<string> AmmoName { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000721B File Offset: 0x0000541B
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00007223 File Offset: 0x00005423
		public List<int> AmmoMax { get; set; }
	}
}
