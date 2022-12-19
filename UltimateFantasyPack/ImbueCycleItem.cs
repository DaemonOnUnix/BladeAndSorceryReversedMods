using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000023 RID: 35
	public class ImbueCycleItem : ItemModule
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00004A65 File Offset: 0x00002C65
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<ImbueCycle>();
			base.OnItemLoaded(item);
		}
	}
}
