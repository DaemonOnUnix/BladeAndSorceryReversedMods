using System;
using ThunderRoad;

namespace Yamato
{
	// Token: 0x02000004 RID: 4
	public class DaggerModule : ItemModule
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021FA File Offset: 0x000003FA
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<DaggerEffects>();
		}
	}
}
