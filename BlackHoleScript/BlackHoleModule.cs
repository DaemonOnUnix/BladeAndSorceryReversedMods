using System;
using ThunderRoad;
using UnityEngine;

namespace BlackHole
{
	// Token: 0x02000002 RID: 2
	public class BlackHoleModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<BlackHoleBehaviour>().itemModule = this;
		}

		// Token: 0x04000001 RID: 1
		public float explosionDistancefalloffMultiplier;

		// Token: 0x04000002 RID: 2
		public float gravitationalPullMultiplier;

		// Token: 0x04000003 RID: 3
		public float gravityDistanceMultiplier;

		// Token: 0x04000004 RID: 4
		public float explosionPowerMultiplier;

		// Token: 0x04000005 RID: 5
		public float masseffectMultiplier;

		// Token: 0x04000006 RID: 6
		public float blackHoleDuration;

		// Token: 0x04000007 RID: 7
		public LayerMask LayersToPull;
	}
}
