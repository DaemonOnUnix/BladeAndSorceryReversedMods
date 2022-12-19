using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Items
{
	// Token: 0x02000019 RID: 25
	public class AmmoResupply : MonoBehaviour
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x000082F8 File Offset: 0x000064F8
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AmmoModule>();
			this.holder = this.item.GetComponentInChildren<Holder>();
			this.holder.UnSnapped += new Holder.HolderDelegate(this.OnWeaponItemRemoved);
			if (this.module.ammoCapacity > 0)
			{
				this.usesRemaining = this.module.ammoCapacity;
				this.infiniteUses = false;
				return;
			}
			this.infiniteUses = true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000837D File Offset: 0x0000657D
		protected void Start()
		{
			this.SpawnAndSnap(this.module.magazineID, this.holder);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00008398 File Offset: 0x00006598
		protected void SpawnAndSnap(string spawnedItemID, Holder holder)
		{
			if (this.waitingForSpawn)
			{
				return;
			}
			ItemData data = Catalog.GetData<ItemData>(spawnedItemID, true);
			if (data == null)
			{
				return;
			}
			this.waitingForSpawn = true;
			data.SpawnAsync(delegate(Item thisSpawnedItem)
			{
				try
				{
					if (holder.HasSlotFree())
					{
						holder.Snap(thisSpawnedItem, false, true);
						thisSpawnedItem.SetMeshLayer(GameManager.GetLayer(11));
					}
					this.waitingForSpawn = false;
				}
				catch (Exception ex)
				{
					Debug.Log("[ModularFirearmsFramework] EXCEPTION IN SNAPPING: " + ex.ToString());
				}
			}, null, null, null, true, null);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000083FC File Offset: 0x000065FC
		protected void OnWeaponItemRemoved(Item interactiveObject)
		{
			if (this.waitingForSpawn)
			{
				return;
			}
			if (!this.infiniteUses && this.usesRemaining <= 0)
			{
				this.holder.data.locked = true;
				if (this.module.despawnBagOnEmpty)
				{
					this.item.Despawn();
				}
				return;
			}
			this.SpawnAndSnap(this.module.magazineID, this.holder);
			this.usesRemaining--;
		}

		// Token: 0x04000192 RID: 402
		protected Item item;

		// Token: 0x04000193 RID: 403
		protected AmmoModule module;

		// Token: 0x04000194 RID: 404
		protected Holder holder;

		// Token: 0x04000195 RID: 405
		private bool infiniteUses;

		// Token: 0x04000196 RID: 406
		private int usesRemaining;

		// Token: 0x04000197 RID: 407
		private bool waitingForSpawn;
	}
}
