using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200001B RID: 27
	public class DaggerFlipItem : ItemModule
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00003B4C File Offset: 0x00001D4C
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<DaggerFlip>();
			base.OnItemLoaded(item);
		}
	}
}
