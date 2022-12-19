using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000039 RID: 57
	public class RailMounts : ItemModule
	{
		// Token: 0x06000195 RID: 405 RVA: 0x0000B4E2 File Offset: 0x000096E2
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<RailMountsMono>();
		}
	}
}
