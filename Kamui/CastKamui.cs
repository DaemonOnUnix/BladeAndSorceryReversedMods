using System;
using ThunderRoad;

namespace Kamui
{
	// Token: 0x02000003 RID: 3
	internal class CastKamui : SpellCastProjectile
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002248 File Offset: 0x00000448
		public override void Fire(bool active)
		{
			base.Fire(active);
			bool flag = !active;
			if (!flag)
			{
				this.kamuiData = Catalog.GetData<ItemData>("KamuiObject", true);
				this.kamuiData.SpawnAsync(new Action<Item>(this.SpawnKamui), null, null, null, true, null);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022A8 File Offset: 0x000004A8
		public void SpawnKamui(Item spawnedItem)
		{
			bool flag = this.currentKamui != null;
			if (flag)
			{
				this.currentKamui.Despawn();
			}
			this.currentKamui = spawnedItem;
			spawnedItem.transform.position = Player.local.head.transform.position;
			spawnedItem.rb.useGravity = false;
			spawnedItem.rb.drag = 0f;
			spawnedItem.IgnoreRagdollCollision(Player.local.creature.ragdoll);
			spawnedItem.rb.AddForce(Player.local.head.transform.forward * (15f * spawnedItem.rb.mass), 1);
		}

		// Token: 0x04000007 RID: 7
		private Item item;

		// Token: 0x04000008 RID: 8
		private ItemData kamuiData;

		// Token: 0x04000009 RID: 9
		private float spawned = 0f;

		// Token: 0x0400000A RID: 10
		private Item currentKamui;
	}
}
