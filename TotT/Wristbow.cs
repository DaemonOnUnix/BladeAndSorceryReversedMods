using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000048 RID: 72
	public class Wristbow : ItemModule
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000CE5E File Offset: 0x0000B05E
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			WristbowParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<WristbowMono>();
		}

		// Token: 0x04000152 RID: 338
		public toggleMethod toggleMethod;
	}
}
