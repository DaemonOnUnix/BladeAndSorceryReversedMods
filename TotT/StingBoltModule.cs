using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000044 RID: 68
	public class StingBoltModule : MonoBehaviour
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000CC98 File Offset: 0x0000AE98
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
			bool flag = target != null;
			if (flag)
			{
				RageAttackBehaviour temp = target.gameObject.AddComponent<RageAttackBehaviour>();
				temp.Setup(this.AggroTimeInSeconds);
				Damager dam = this.item.GetComponentInChildren<Damager>();
				dam.UnPenetrateAll();
				base.StartCoroutine(this.timeToDie(30f));
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000CD15 File Offset: 0x0000AF15
		public IEnumerator timeToDie(float v)
		{
			MeshRenderer temp = this.item.gameObject.GetComponentInChildren<MeshRenderer>();
			temp.enabled = false;
			this.item.mainHandleLeft.data.disableTouch = true;
			this.item.mainHandleLeft.data.allowTelekinesis = false;
			yield return new WaitForSeconds(v);
			this.item.Despawn();
			yield break;
		}

		// Token: 0x0400014C RID: 332
		private Item item;

		// Token: 0x0400014D RID: 333
		private float AggroTimeInSeconds = 30f;
	}
}
