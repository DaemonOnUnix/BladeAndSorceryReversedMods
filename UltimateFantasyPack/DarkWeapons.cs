using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000006 RID: 6
	public class DarkWeapons : ItemModule
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000025AB File Offset: 0x000007AB
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<DarkMono>();
		}
	}
}
