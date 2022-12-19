using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Items
{
	// Token: 0x02000018 RID: 24
	public class InteractiveMagazine : MonoBehaviour
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00008070 File Offset: 0x00006270
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AmmoModule>();
			FrameworkCore.DisableCulling(this.item, false);
			this.holder = this.item.GetComponentInChildren<Holder>();
			this.holder.Snapped += new Holder.HolderDelegate(this.OnAmmoItemInserted);
			this.magazineHandle = this.item.GetCustomReference(this.module.handleRef, true).GetComponent<Handle>();
			this.bulletMesh = this.item.GetCustomReference(this.module.bulletMeshRef, true).gameObject;
			this.RefillAll();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00008120 File Offset: 0x00006320
		public void OnAmmoItemInserted(Item interactiveObject)
		{
			try
			{
				InteractiveAmmo component = interactiveObject.GetComponent<InteractiveAmmo>();
				if (component != null)
				{
					if (component.GetAmmoType() == this.module.GetAcceptedType())
					{
						this.RefillOne();
						this.holder.UnSnap(interactiveObject, false, true);
						interactiveObject.Despawn();
						return;
					}
					this.holder.UnSnap(interactiveObject, false, true);
					Debug.LogWarning("[ModularFirearmsFramework][WARNING] Inserted object has wrong AmmoType, and will be popped out");
				}
				else
				{
					this.holder.UnSnap(interactiveObject, false, true);
					Debug.LogWarning("[ModularFirearmsFramework][WARNING] Inserted object has no ItemAmmo component, and will be popped out");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] Exception in Adding Ammo.");
				Debug.LogError(ex.ToString());
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000081C8 File Offset: 0x000063C8
		public void Insert()
		{
			this.insertedIntoObject = true;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000081D1 File Offset: 0x000063D1
		public void Remove()
		{
			this.insertedIntoObject = false;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000081DA File Offset: 0x000063DA
		public void SetBulletVisibility(bool visible = true)
		{
			this.bulletMesh.SetActive(visible);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000081E8 File Offset: 0x000063E8
		public int GetAmmoCount()
		{
			return this.ammoCount;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000081F0 File Offset: 0x000063F0
		public string GetMagazineID()
		{
			return this.item.data.id;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008202 File Offset: 0x00006402
		public bool IsInserted()
		{
			return this.insertedIntoObject;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000820C File Offset: 0x0000640C
		public void Eject(Item shooterItem = null)
		{
			this.insertedIntoObject = false;
			if (shooterItem != null)
			{
				this.item.IgnoreObjectCollision(shooterItem);
			}
			this.item.IgnoreRagdollCollision(Player.local.creature.ragdoll);
			this.item.rb.AddRelativeForce(new Vector3(this.module.ejectionForceVector[0], this.module.ejectionForceVector[1], this.module.ejectionForceVector[2]), 1);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000828C File Offset: 0x0000648C
		public void ConsumeOne()
		{
			this.ammoCount--;
			if (this.ammoCount <= 0)
			{
				this.SetBulletVisibility(false);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000082AC File Offset: 0x000064AC
		public void ConsumeAll()
		{
			this.ammoCount = 0;
			this.SetBulletVisibility(false);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000082BC File Offset: 0x000064BC
		public void RefillOne()
		{
			if (this.ammoCount <= 0)
			{
				this.SetBulletVisibility(true);
			}
			this.ammoCount++;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000082DC File Offset: 0x000064DC
		public void RefillAll()
		{
			this.ammoCount = this.module.ammoCapacity;
			this.SetBulletVisibility(true);
		}

		// Token: 0x0400018B RID: 395
		protected Item item;

		// Token: 0x0400018C RID: 396
		protected AmmoModule module;

		// Token: 0x0400018D RID: 397
		protected Holder holder;

		// Token: 0x0400018E RID: 398
		protected Handle magazineHandle;

		// Token: 0x0400018F RID: 399
		protected GameObject bulletMesh;

		// Token: 0x04000190 RID: 400
		protected int ammoCount;

		// Token: 0x04000191 RID: 401
		protected bool insertedIntoObject;
	}
}
