using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200000C RID: 12
	public class PhantomBlade : ItemModule
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000045BC File Offset: 0x000027BC
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			PhantomBladeParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<PhantomBladeMono>();
		}

		// Token: 0x04000039 RID: 57
		public toggleMethod toggleMethod;
	}
}
