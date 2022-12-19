using System;
using System.Collections.Generic;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000007 RID: 7
	internal class FirePit : ThunderBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002627 File Offset: 0x00000827
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.CheckedItems = new List<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollidingEventStart);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002660 File Offset: 0x00000860
		private void MainCollisionHandler_OnCollidingEventStart(CollisionInstance collisionInstance)
		{
			bool flag = !collisionInstance.sourceCollider.GetComponentInParent<Item>();
			if (!flag)
			{
				Item it = collisionInstance.sourceCollider.GetComponentInParent<Item>();
				bool flag2 = !this.CheckedItems.Contains(it);
				if (flag2)
				{
					this.CheckedItems.Add(it);
					bool flag3 = it.itemId == "Stick";
					if (flag3)
					{
						Catalog.GetData<ItemData>("FirePitWood", true).SpawnAsync(delegate(Item firepit)
						{
							firepit.transform.position = this.transform.position;
							firepit.transform.rotation = this.transform.rotation;
							Catalog.GetData<EffectData>("FirePitShovelDig", true).Spawn(firepit.transform, true, null, false, Array.Empty<Type>());
							this.CheckedItems = new List<Item>();
							it.Despawn();
							this.item.Despawn(0.1f);
						}, null, null, null, true, null);
					}
				}
			}
		}

		// Token: 0x0400000F RID: 15
		private Item item;

		// Token: 0x04000010 RID: 16
		private List<Item> CheckedItems;
	}
}
