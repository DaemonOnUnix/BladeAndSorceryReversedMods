using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000042 RID: 66
	public class SleepDartModule : MonoBehaviour
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000CB11 File Offset: 0x0000AD11
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000CB40 File Offset: 0x0000AD40
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
				Damager dam = this.item.GetComponentInChildren<Damager>();
				dam.UnPenetrateAll();
				bool flag2 = target.gameObject.GetComponent<KnockOutBehaviour>() == null;
				if (flag2)
				{
					KnockOutBehaviour temp = target.gameObject.AddComponent<KnockOutBehaviour>();
					bool flag3 = target.brain.state == 5 && this.CombatDelaySeconds > 0f;
					if (flag3)
					{
						temp.SetupWithDelay(this.KnockOutMinutes, this.CombatDelaySeconds);
					}
					else
					{
						temp.Setup(this.KnockOutMinutes);
					}
				}
				base.StartCoroutine(this.timeToDie(30f));
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000CC17 File Offset: 0x0000AE17
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

		// Token: 0x04000149 RID: 329
		private Item item;

		// Token: 0x0400014A RID: 330
		public float KnockOutMinutes = SleepDartParser.KnockOutMinutes;

		// Token: 0x0400014B RID: 331
		public float CombatDelaySeconds = SleepDartParser.CombatDelaySeconds;
	}
}
