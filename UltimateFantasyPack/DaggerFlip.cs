using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200001A RID: 26
	public class DaggerFlip : MonoBehaviour
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00003AA9 File Offset: 0x00001CA9
		private void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.onHeld);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003AD0 File Offset: 0x00001CD0
		public void FlipDagger(Handle handle, RagdollHand ragdollHand)
		{
			ragdollHand.UnGrab(false);
			this.item.transform.Rotate(180f, 0f, 0f, 1);
			ragdollHand.GrabRelative(handle);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003B04 File Offset: 0x00001D04
		private void onHeld(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 0;
			if (flag)
			{
				bool flag2 = !this.item.isPenetrating;
				if (flag2)
				{
					this.FlipDagger(handle, this.item.mainHandler);
				}
			}
		}

		// Token: 0x0400004B RID: 75
		private Item item;
	}
}
