using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000021 RID: 33
	public class GroundPoundItem : ItemModule
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000047C6 File Offset: 0x000029C6
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<GroundPound>().Setup(this.explodePower1);
			base.OnItemLoaded(item);
		}

		// Token: 0x0400005D RID: 93
		public float explodePower1;
	}
}
