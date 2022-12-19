using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000A RID: 10
	internal class FirePitShovelItem : ItemModule
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002864 File Offset: 0x00000A64
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePitShovel>();
		}
	}
}
