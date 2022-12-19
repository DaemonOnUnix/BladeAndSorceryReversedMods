using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000006 RID: 6
	public class ExpandingShield : ItemModule
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000034D1 File Offset: 0x000016D1
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			ShieldParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<ShieldMono>();
		}

		// Token: 0x0400001D RID: 29
		public toggleMethod toggleMethod;
	}
}
