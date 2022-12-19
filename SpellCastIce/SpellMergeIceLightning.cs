using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x0200000C RID: 12
	internal class SpellMergeIceLightning : SpellMergeData
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00003B10 File Offset: 0x00001D10
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.shuriken = Catalog.GetData<ItemData>("IceShuriken", true);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003B2C File Offset: 0x00001D2C
		public override IEnumerator OnCatalogRefreshCoroutine()
		{
			return base.OnCatalogRefreshCoroutine();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003B44 File Offset: 0x00001D44
		public override void Merge(bool active)
		{
			base.Merge(active);
			if (active)
			{
				this.activated = true;
			}
			else
			{
				this.currentCharge = 0f;
				this.activated = false;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003B80 File Offset: 0x00001D80
		public override void Update()
		{
			base.Update();
			bool flag = this.activated && Time.time - this.lastShotTime > 1f / this.fireRate;
			if (flag)
			{
				this.lastShotTime = Time.time;
				this.shuriken.SpawnAsync(delegate(Item item)
				{
					foreach (CollisionHandler collisionHandler in item.collisionHandlers)
					{
						collisionHandler.SetPhysicModifier(this, new float?(0f), 1f, -1f, -1f, -1f, null);
					}
					item.rb.useGravity = false;
					item.rb.AddForce(item.transform.forward * this.shotSpeed, 1);
					item.gameObject.AddComponent<ShurikenItem>().item = item;
					item.IgnoreRagdollCollision(this.mana.creature.ragdoll);
					item.Throw(1f, 2);
				}, new Vector3?(this.mana.mergePoint.position), new Quaternion?(Quaternion.LookRotation(this.mana.casterLeft.magic.transform.up + this.mana.casterRight.magic.transform.up)), null, true, null);
			}
		}

		// Token: 0x04000032 RID: 50
		public float fireRate = 2f;

		// Token: 0x04000033 RID: 51
		public float shotSpeed = 5f;

		// Token: 0x04000034 RID: 52
		private bool activated;

		// Token: 0x04000035 RID: 53
		private float lastShotTime;

		// Token: 0x04000036 RID: 54
		private ItemData shuriken;
	}
}
