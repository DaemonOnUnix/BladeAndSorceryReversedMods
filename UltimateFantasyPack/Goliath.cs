using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200000E RID: 14
	public class Goliath : ItemModule
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002B02 File Offset: 0x00000D02
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<GoliathMono>().settings = this;
		}

		// Token: 0x0400001C RID: 28
		public float delay = 3f;
	}
}
