using System;
using System.Collections.Generic;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000A RID: 10
	internal class FirePit : ThunderBehaviour
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000029BF File Offset: 0x00000BBF
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.CheckedItems = new List<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollidingEventStart);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000029F8 File Offset: 0x00000BF8
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

		// Token: 0x0400001F RID: 31
		private Item item;

		// Token: 0x04000020 RID: 32
		private List<Item> CheckedItems;
	}
}
