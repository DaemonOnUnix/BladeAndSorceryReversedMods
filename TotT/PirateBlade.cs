using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000009 RID: 9
	public class PirateBlade : ItemModule
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003D64 File Offset: 0x00001F64
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			PirateBladeParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<PirateBladeMono>();
		}

		// Token: 0x0400002B RID: 43
		public toggleMethod toggleMethod;
	}
}
