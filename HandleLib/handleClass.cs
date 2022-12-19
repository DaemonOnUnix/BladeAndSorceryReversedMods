using System;
using ThunderRoad;
using UnityEngine;

namespace HandleLib
{
	// Token: 0x02000002 RID: 2
	public class handleClass : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.onHeldAction);
			bool useSpellMenu = this.data.useSpellMenu;
			if (useSpellMenu)
			{
				this.bindedAction = 2;
			}
			else
			{
				this.bindedAction = 0;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A5 File Offset: 0x000002A5
		private void flipHandle(Handle handle, RagdollHand hand)
		{
			hand.UnGrab(false);
			this.item.transform.Rotate(180f, 0f, 0f, 1);
			hand.GrabRelative(handle);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020DC File Offset: 0x000002DC
		private void onHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == this.bindedAction && this.data.flippables.Contains(handle.name) && !this.item.isPenetrating;
			if (flag)
			{
				this.flipHandle(handle, this.item.mainHandler);
			}
		}

		// Token: 0x04000001 RID: 1
		public handleData data;

		// Token: 0x04000002 RID: 2
		private Item item;

		// Token: 0x04000003 RID: 3
		private Interactable.Action bindedAction;
	}
}
