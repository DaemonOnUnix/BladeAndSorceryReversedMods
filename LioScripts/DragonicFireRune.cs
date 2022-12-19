using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000008 RID: 8
	internal class DragonicFireRune : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000284C File Offset: 0x00000A4C
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGravEvent);
			this.shootPoint = base.transform.Find("ShootPoint");
			this.projectileData = Catalog.GetData<ItemData>("DynamicProjectile", true);
			this.projEffectData = Catalog.GetData<EffectData>("SpellFireball", true);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000028D0 File Offset: 0x00000AD0
		private void Item_OnGravEvent(Handle handle, RagdollHand ragdollHand)
		{
			bool flag = !this.hasBeenGrabbed;
			if (flag)
			{
				this.hasBeenGrabbed = true;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028F4 File Offset: 0x00000AF4
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == null && this.timer <= 0f;
			if (flag)
			{
				base.StartCoroutine(this.ThrowFireballs());
				this.timer = 2f;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002936 File Offset: 0x00000B36
		private IEnumerator ThrowFireballs()
		{
			int maxb = Random.Range(1, 7);
			int num;
			for (int i = 0; i < maxb; i = num + 1)
			{
				DragonicFireRune.<>c__DisplayClass10_0 CS$<>8__locals1 = new DragonicFireRune.<>c__DisplayClass10_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.dir = this.shootPoint.forward;
				this.projectileData.SpawnAsync(delegate(Item boule)
				{
					boule.transform.position = CS$<>8__locals1.<>4__this.shootPoint.position;
					boule.transform.rotation = CS$<>8__locals1.<>4__this.shootPoint.rotation;
					ItemMagicProjectile component = boule.GetComponent<ItemMagicProjectile>();
					component.item.rb.isKinematic = false;
					component.guidance = 0;
					component.speed = 10f;
					component.imbueEnergyTransfered = 0.5f;
					component.allowDeflect = true;
					component.homing = false;
					component.Fire(CS$<>8__locals1.dir * 20f, CS$<>8__locals1.<>4__this.projEffectData, null, null, 0);
					foreach (CollisionHandler collisionHandler in boule.collisionHandlers)
					{
						foreach (Damager damager in collisionHandler.damagers)
						{
							damager.Load(Catalog.GetData<DamagerData>("Fireball", true), collisionHandler);
						}
					}
				}, null, null, null, true, null);
				yield return new WaitForSeconds(0.2f);
				CS$<>8__locals1 = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002948 File Offset: 0x00000B48
		private void Update()
		{
			bool flag = this.timer > 0f;
			if (flag)
			{
				this.timer -= Time.deltaTime;
			}
		}

		// Token: 0x04000018 RID: 24
		private Item item;

		// Token: 0x04000019 RID: 25
		private Transform shootPoint;

		// Token: 0x0400001A RID: 26
		private ItemData projectileData;

		// Token: 0x0400001B RID: 27
		private EffectData projEffectData;

		// Token: 0x0400001C RID: 28
		private float timer = 0f;

		// Token: 0x0400001D RID: 29
		private float spread = 1f;

		// Token: 0x0400001E RID: 30
		private bool hasBeenGrabbed = false;
	}
}
