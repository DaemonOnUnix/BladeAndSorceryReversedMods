using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000038 RID: 56
	public class PhantomBladeBolt : MonoBehaviour
	{
		// Token: 0x0600018B RID: 395 RVA: 0x0000B405 File Offset: 0x00009605
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.OnStart();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000B438 File Offset: 0x00009638
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
				this.OnCreatureHit(target);
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B47F File Offset: 0x0000967F
		public void Update()
		{
			this.OnUpdate();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B489 File Offset: 0x00009689
		public virtual void OnStart()
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B48C File Offset: 0x0000968C
		public virtual void OnUpdate()
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000B490 File Offset: 0x00009690
		public virtual string GetItemID()
		{
			return "";
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000B4A8 File Offset: 0x000096A8
		public virtual Color GetColor()
		{
			return this.eColor;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000B4C0 File Offset: 0x000096C0
		public virtual void OnCreatureHit(Creature creature)
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B4C3 File Offset: 0x000096C3
		public IEnumerator timeToDie(float delay)
		{
			MeshRenderer temp = this.item.gameObject.GetComponentInChildren<MeshRenderer>();
			temp.enabled = false;
			this.item.mainHandleLeft.data.disableTouch = true;
			this.item.mainHandleLeft.data.allowTelekinesis = false;
			yield return new WaitForSeconds(delay);
			this.item.Despawn();
			yield break;
		}

		// Token: 0x04000124 RID: 292
		public Item item;

		// Token: 0x04000125 RID: 293
		public int ammoMax;

		// Token: 0x04000126 RID: 294
		public Color eColor;
	}
}
