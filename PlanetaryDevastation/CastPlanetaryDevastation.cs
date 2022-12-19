using System;
using ThunderRoad;
using UnityEngine;

namespace PlanetaryDevastation
{
	// Token: 0x02000002 RID: 2
	internal class CastPlanetaryDevastation : SpellCastProjectile
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Fire(bool active)
		{
			base.Fire(active);
			bool flag = !active;
			if (!flag)
			{
				Catalog.GetData<ItemData>("PlanetaryDevastationObject", true).SpawnAsync(new Action<Item>(this.SpawnDevastation), null, null, null, true, null);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
		public void SpawnDevastation(Item spawnedItem)
		{
			bool flag = this.item != null;
			if (flag)
			{
				this.item.Despawn();
			}
			bool flag2 = spawnedItem.gameObject.GetComponent<Planet>() == null;
			if (flag2)
			{
				spawnedItem.gameObject.AddComponent<Planet>();
			}
			this.item = spawnedItem;
			spawnedItem.transform.position = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y + 100f, Player.local.head.transform.position.z);
			spawnedItem.rb.useGravity = false;
			spawnedItem.rb.drag = 0f;
			spawnedItem.rb.mass = 1000f;
		}

		// Token: 0x04000001 RID: 1
		private Item item;
	}
}
