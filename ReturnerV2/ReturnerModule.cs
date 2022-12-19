using System;
using ThunderRoad;

namespace ReturnerV2
{
	// Token: 0x02000003 RID: 3
	public class ReturnerModule : ItemModule
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000022DB File Offset: 0x000004DB
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<ReturnerItem>();
		}

		// Token: 0x0400000A RID: 10
		public float returnSpeed = 10f;

		// Token: 0x0400000B RID: 11
		public float minThrowVelocity;
	}
}
