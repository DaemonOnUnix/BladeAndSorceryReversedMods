using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000F RID: 15
	internal class FirePitWoodItem : ItemModule
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00002CF2 File Offset: 0x00000EF2
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePitWood>();
		}
	}
}
