using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000043 RID: 67
	public class DishonoredStingBolt : ItemModule
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<StingBoltModule>();
		}
	}
}
