using System;
using ThunderRoad;
using UnityEngine;

namespace HighEntiaTomb
{
	// Token: 0x02000003 RID: 3
	public class ClimbableComponent : MonoBehaviour
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000238C File Offset: 0x0000058C
		public void OnCollisionStay(Collision c)
		{
			bool flag = c.collider.GetComponentInParent<Player>() != null;
			if (flag)
			{
				RagdollHandClimb.climbFree = true;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023B8 File Offset: 0x000005B8
		public void OnCollisionExit(Collision c)
		{
			bool flag = c.collider.GetComponentInParent<Player>() != null;
			if (flag)
			{
				RagdollHandClimb.climbFree = false;
			}
		}
	}
}
