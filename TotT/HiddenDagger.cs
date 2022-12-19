using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000029 RID: 41
	public class HiddenDagger : ItemModule
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00009459 File Offset: 0x00007659
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			HiddenBladeParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<HiddenDaggerMono>();
		}

		// Token: 0x040000DF RID: 223
		public toggleMethod toggleMethod;
	}
}
