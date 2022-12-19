using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000D RID: 13
	internal class FirePitShovelItem : ItemModule
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002BFC File Offset: 0x00000DFC
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePitShovel>();
		}
	}
}
