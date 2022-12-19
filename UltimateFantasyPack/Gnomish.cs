using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200000B RID: 11
	internal class Gnomish : ItemModule
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002879 File Offset: 0x00000A79
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<GnomishMono>().settings = this;
		}

		// Token: 0x04000017 RID: 23
		public float Cooldown = 10f;
	}
}
