using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Items
{
	// Token: 0x0200001B RID: 27
	public class InteractiveAmmo : MonoBehaviour
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00008474 File Offset: 0x00006674
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AmmoModule>();
			FrameworkCore.DisableCulling(this.item, false);
			this.thisAmmoType = this.module.GetSelectedType();
			this.capacity = this.module.ammoCapacity;
			if (this.module.handleRef != null)
			{
				this.ammoHandle = this.item.GetCustomReference(this.module.handleRef, true).GetComponent<Handle>();
			}
			if (this.module.bulletMeshRef != null)
			{
				this.bulletMesh = this.item.GetCustomReference(this.module.bulletMeshRef, true).gameObject;
			}
			this.Refill();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00008535 File Offset: 0x00006735
		public FrameworkCore.AmmoType GetAmmoType()
		{
			return this.thisAmmoType;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000853D File Offset: 0x0000673D
		public int GetAmmoTypeInt()
		{
			return (int)Enum.Parse(typeof(FrameworkCore.FireMode), this.module.ammoType);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000855E File Offset: 0x0000675E
		public string GetAmmoID()
		{
			return this.item.data.id;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00008570 File Offset: 0x00006770
		public int GetAmmoCount()
		{
			return this.capacity;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00008578 File Offset: 0x00006778
		public void Consume(int i = 1)
		{
			this.capacity -= i;
			this.SetMeshState(this.bulletMesh, false);
			if (this.capacity <= 0)
			{
				this.isLoaded = false;
			}
			if (this.ammoHandle != null)
			{
				this.ammoHandle.data.allowTelekinesis = false;
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000085D0 File Offset: 0x000067D0
		public void Refill()
		{
			this.capacity = this.module.ammoCapacity;
			this.SetMeshState(this.bulletMesh, true);
			this.isLoaded = true;
			if (this.ammoHandle != null)
			{
				this.ammoHandle.data.allowTelekinesis = true;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008621 File Offset: 0x00006821
		protected void SetMeshState(GameObject ammoMesh, bool newState = false)
		{
			if (ammoMesh != null)
			{
				ammoMesh.SetActive(newState);
			}
		}

		// Token: 0x04000198 RID: 408
		protected Item item;

		// Token: 0x04000199 RID: 409
		protected AmmoModule module;

		// Token: 0x0400019A RID: 410
		private GameObject bulletMesh;

		// Token: 0x0400019B RID: 411
		private Handle ammoHandle;

		// Token: 0x0400019C RID: 412
		private FrameworkCore.AmmoType thisAmmoType;

		// Token: 0x0400019D RID: 413
		private int capacity;

		// Token: 0x0400019E RID: 414
		public bool isLoaded = true;
	}
}
