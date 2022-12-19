using System;
using ThunderRoad;

namespace WristFlamethrower
{
	// Token: 0x02000002 RID: 2
	public class WristFlamethrower : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FlameMono>();
		}
	}
}
