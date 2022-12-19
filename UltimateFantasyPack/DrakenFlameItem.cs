using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200001F RID: 31
	public class DrakenFlameItem : ItemModule
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004372 File Offset: 0x00002572
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<DrakenFlame>();
			base.OnItemLoaded(item);
		}
	}
}
