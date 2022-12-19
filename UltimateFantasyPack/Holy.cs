using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000011 RID: 17
	internal class Holy : ItemModule
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002EE3 File Offset: 0x000010E3
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<HolyMono>();
		}
	}
}
