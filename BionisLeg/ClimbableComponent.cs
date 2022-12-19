using System;
using ThunderRoad;
using UnityEngine;

namespace BionisLeg
{
	// Token: 0x02000003 RID: 3
	public class ClimbableComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020F4 File Offset: 0x000002F4
		public void OnCollisionStay(Collision c)
		{
			bool flag = c.collider.GetComponentInParent<Player>() != null;
			if (flag)
			{
				RagdollHandClimb.climbFree = true;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002120 File Offset: 0x00000320
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
