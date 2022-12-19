using System;
using ThunderRoad;
using UnityEngine;

namespace CaptainModule
{
	// Token: 0x02000002 RID: 2
	public class LoadModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<ModuleBehavior>().Data = this;
			base.OnItemLoaded(item);
		}

		// Token: 0x04000001 RID: 1
		public float ReturnForce;

		// Token: 0x04000002 RID: 2
		public float GrabDistance;

		// Token: 0x04000003 RID: 3
		public float BounceForce;

		// Token: 0x04000004 RID: 4
		public int MaximumBounces;

		// Token: 0x04000005 RID: 5
		public float DetectionRadius;

		// Token: 0x04000006 RID: 6
		public float AttackForce;

		// Token: 0x04000007 RID: 7
		public Vector3 RotationSpeed;
	}
}
