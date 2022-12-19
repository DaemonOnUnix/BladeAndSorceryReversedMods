using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000C RID: 12
	internal class FirePitWoodItem : ItemModule
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000295A File Offset: 0x00000B5A
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePitWood>();
		}
	}
}
