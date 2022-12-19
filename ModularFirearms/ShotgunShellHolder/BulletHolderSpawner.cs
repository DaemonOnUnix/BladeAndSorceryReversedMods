using System;
using ThunderRoad;
using UnityEngine;

namespace ShotgunShellHolder
{
	// Token: 0x02000002 RID: 2
	internal class BulletHolderSpawner : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<BulletHolderModule>();
			if (!string.IsNullOrEmpty(this.module.holderRef))
			{
				this.bulletHolder = this.item.GetCustomReference(this.module.holderRef, true).GetComponent<Holder>();
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B4 File Offset: 0x000002B4
		private void Start()
		{
			foreach (Transform transform in this.bulletHolder.slots)
			{
				this.SpawnAndSnap(this.module.ammoID);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002118 File Offset: 0x00000318
		private void SpawnAndSnap(string ammoID)
		{
			ItemData data = Catalog.GetData<ItemData>(ammoID, true);
			if (data == null)
			{
				Debug.LogError("[Fisher-BulletHolderSpawner][ERROR] No Ammo named " + ammoID.ToString());
				return;
			}
			data.SpawnAsync(delegate(Item i)
			{
				try
				{
					this.bulletHolder.Snap(i, false, true);
				}
				catch
				{
					Debug.Log("[Fisher-BulletHolderSpawner] EXCEPTION IN SNAPPING AMMO ");
				}
			}, new Vector3?(this.item.transform.position), new Quaternion?(Quaternion.Euler(this.item.transform.rotation.eulerAngles)), null, false, null);
		}

		// Token: 0x04000001 RID: 1
		protected Item item;

		// Token: 0x04000002 RID: 2
		protected BulletHolderModule module;

		// Token: 0x04000003 RID: 3
		private Holder bulletHolder;
	}
}
