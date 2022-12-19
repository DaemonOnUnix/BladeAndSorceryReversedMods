using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200003D RID: 61
	public class RailMountSave : ContentCustomData
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000C97C File Offset: 0x0000AB7C
		// (set) Token: 0x060001BA RID: 442 RVA: 0x0000C984 File Offset: 0x0000AB84
		public string topItem { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C98D File Offset: 0x0000AB8D
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000C995 File Offset: 0x0000AB95
		public string bottomItem { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000C99E File Offset: 0x0000AB9E
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000C9A6 File Offset: 0x0000ABA6
		public bool OnOff { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000C9AF File Offset: 0x0000ABAF
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000C9B7 File Offset: 0x0000ABB7
		public RailMode railMode { get; set; }
	}
}
