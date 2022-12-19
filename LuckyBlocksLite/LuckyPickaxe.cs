using System;
using ThunderRoad;

namespace LuckyBlocksLite
{
	// Token: 0x02000005 RID: 5
	public class LuckyPickaxe : ItemModule
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000021F0 File Offset: 0x000003F0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			foreach (CollisionHandler collisionHandler in item.collisionHandlers)
			{
				collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.Item_OnCollisionStartEvent);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000225C File Offset: 0x0000045C
		public void Item_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = collisionInstance.IsDoneByPlayer();
			if (flag)
			{
				bool flag2 = collisionInstance.targetColliderGroup != null && collisionInstance.targetColliderGroup.GetComponentInParent<Item>();
				if (flag2)
				{
					bool flag3 = collisionInstance.targetColliderGroup.GetComponentInParent<Item>().data.HasModule<LuckyBlock>();
					if (flag3)
					{
						LuckyBlocksMethods.LootBlock(collisionInstance.targetColliderGroup.GetComponentInParent<Item>());
					}
				}
			}
		}
	}
}
