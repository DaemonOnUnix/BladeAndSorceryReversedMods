using System;
using ThunderRoad;

namespace ExplodingFireballs
{
	// Token: 0x02000003 RID: 3
	public class BurnModule : ItemModule
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020B9 File Offset: 0x000002B9
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<BurnComponent>().Setup(this.burnDamagePerSecond, this.burnDuration, this.burnEffectId);
		}

		// Token: 0x04000005 RID: 5
		public float burnDamagePerSecond = 5f;

		// Token: 0x04000006 RID: 6
		public float burnDuration = 10f;

		// Token: 0x04000007 RID: 7
		public string burnEffectId = "ImbueFireRagdoll";
	}
}
