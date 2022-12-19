using System;
using ThunderRoad;

namespace WeaponReturning
{
	// Token: 0x02000002 RID: 2
	public class itemClass : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<itemMono>().settings = this;
			base.OnItemLoaded(item);
		}

		// Token: 0x04000001 RID: 1
		public float returnForce;

		// Token: 0x04000002 RID: 2
		public float rotationPerSecond;

		// Token: 0x04000003 RID: 3
		public float holdDuration;
	}
}
