using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000047 RID: 71
	public class BlackjackModule : MonoBehaviour
	{
		// Token: 0x060001DD RID: 477 RVA: 0x0000CD7A File Offset: 0x0000AF7A
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
			Creature creature;
			if (hitRagdollPart == null)
			{
				creature = null;
			}
			else
			{
				Ragdoll ragdoll = hitRagdollPart.ragdoll;
				creature = ((ragdoll != null) ? ragdoll.creature : null);
			}
			Creature target = creature;
			bool flag = target != null && target.brain.state != 5;
			if (flag)
			{
				bool flag2 = collisionInstance.damageStruct.hitRagdollPart == target.ragdoll.headPart && collisionInstance.damageStruct.damageType == 3;
				if (flag2)
				{
					KnockOutBehaviour temp = target.gameObject.AddComponent<KnockOutBehaviour>();
					temp.Setup(this.KnockOutMinutes);
				}
			}
		}

		// Token: 0x04000150 RID: 336
		private Item item;

		// Token: 0x04000151 RID: 337
		public float KnockOutMinutes = BlackjackParser.KnockOutMinutes;
	}
}
