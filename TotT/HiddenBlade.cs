using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000026 RID: 38
	public class HiddenBlade : ItemModule
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00008C26 File Offset: 0x00006E26
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			HiddenBladeParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<HiddenBladeMono>();
		}

		// Token: 0x040000D4 RID: 212
		public toggleMethod toggleMethod;
	}
}
