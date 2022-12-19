using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000029 RID: 41
	public class Voidborne : ItemModule
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x0000525C File Offset: 0x0000345C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<VoidMono>();
		}
	}
}
