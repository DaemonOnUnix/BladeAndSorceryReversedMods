using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000004 RID: 4
	public class ArchWraith : ItemModule
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002280 File Offset: 0x00000480
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<ArcWraithMono>();
		}
	}
}
