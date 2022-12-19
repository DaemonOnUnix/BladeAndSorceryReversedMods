using System;
using ThunderRoad;
using UnityEngine;

namespace LuckyBlocksLite
{
	// Token: 0x02000006 RID: 6
	public class LuckyBlockStack : ItemModule
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000022D2 File Offset: 0x000004D2
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022F4 File Offset: 0x000004F4
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 0;
			if (flag)
			{
				Vector3 vector = Player.currentCreature.ragdoll.headPart.transform.position + Player.currentCreature.ragdoll.headPart.transform.forward * 2f;
				Catalog.GetData<ItemData>("LuckyBlock", true).SpawnAsync(delegate(Item Item)
				{
					Debug.Log(Item.name);
				}, new Vector3?(vector), null, null, true, null);
			}
		}
	}
}
