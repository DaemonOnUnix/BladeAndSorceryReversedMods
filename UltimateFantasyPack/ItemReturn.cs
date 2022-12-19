using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000016 RID: 22
	public class ItemReturn : MonoBehaviour
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00003429 File Offset: 0x00001629
		public void Setup(float returnPower)
		{
			this.item = base.GetComponentInParent<Item>();
			this.returnPower = returnPower;
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Grab);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003458 File Offset: 0x00001658
		private void Grab(Handle handle, RagdollHand ragdollHand)
		{
			bool flag = this.lastHolder;
			if (flag)
			{
				this.lastHolder.OnGrabEvent -= new RagdollHand.GrabEvent(this.LastHolderGrab);
			}
			this.lastHolder = ragdollHand;
			this.lastHolder.OnGrabEvent += new RagdollHand.GrabEvent(this.LastHolderGrab);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000034AC File Offset: 0x000016AC
		private void LastHolderGrab(Side side, Handle handle, float axisPosition, HandlePose orientation, EventTime eventTime)
		{
			bool flag = handle.item && handle.item != this.item;
			if (flag)
			{
				this.lastHolder = null;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000034E8 File Offset: 0x000016E8
		private void Update()
		{
			this.item.rb.useGravity = !this.item.isFlying;
			RagdollHand ragdollHand = this.lastHolder;
			bool flag;
			if (ragdollHand == null)
			{
				flag = null != null;
			}
			else
			{
				PlayerHand playerHand = ragdollHand.playerHand;
				flag = ((playerHand != null) ? playerHand.controlHand : null) != null;
			}
			bool flag2 = flag && this.lastHolder.playerHand.controlHand.gripPressed && !this.lastHolder.playerHand.controlHand.castPressed && !this.item.mainHandler && !this.item.holder && !this.lastHolder.grabbedHandle;
			if (flag2)
			{
				this.item.rb.AddForce((this.lastHolder.transform.position - this.item.transform.position).normalized * this.returnPower, 2);
				bool flag3 = (this.item.transform.position - this.lastHolder.transform.position).sqrMagnitude < 1f;
				if (flag3)
				{
					this.lastHolder.Grab(this.item.GetMainHandle(this.lastHolder.side));
				}
			}
		}

		// Token: 0x0400003A RID: 58
		private Item item;

		// Token: 0x0400003B RID: 59
		private float returnPower;

		// Token: 0x0400003C RID: 60
		private RagdollHand lastHolder;
	}
}
