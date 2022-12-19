using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200000C RID: 12
	internal class GnomishMono : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000028AC File Offset: 0x00000AAC
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028FC File Offset: 0x00000AFC
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				GameManager.local.StartCoroutine(this.Ability(this.settings.Cooldown));
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002930 File Offset: 0x00000B30
		public IEnumerator Ability(float cooldown)
		{
			this.onCooldown = true;
			this.item.GetCustomReference("Sparks", true).gameObject.SetActive(true);
			this.active = true;
			yield return new WaitForSeconds(cooldown);
			this.item.GetCustomReference("Sparks", true).gameObject.SetActive(false);
			this.active = false;
			this.onCooldown = false;
			yield break;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002948 File Offset: 0x00000B48
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			Creature creature;
			bool flag;
			if (this.active)
			{
				creature = collisionInstance.targetCollider.GetComponentInParent<Creature>();
				flag = creature != null;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				creature.TryElectrocute(50f, 5f, true, true, Catalog.GetData<EffectData>("ImbueLightningRagdoll", true));
			}
		}

		// Token: 0x04000018 RID: 24
		public Item item;

		// Token: 0x04000019 RID: 25
		public Gnomish settings;

		// Token: 0x0400001A RID: 26
		private bool onCooldown = false;

		// Token: 0x0400001B RID: 27
		private bool active = false;
	}
}
