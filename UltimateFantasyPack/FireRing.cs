using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200000A RID: 10
	internal class FireRing : ItemModule
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002859 File Offset: 0x00000A59
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FireRingMono>();
		}
	}
}
