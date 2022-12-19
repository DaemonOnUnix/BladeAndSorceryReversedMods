using System;
using ThunderRoad;

namespace MeteorFall
{
	// Token: 0x02000003 RID: 3
	public class MeteorProjectile : ItemModule
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002094 File Offset: 0x00000294
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<MeteorProjectileComponent>();
		}
	}
}
