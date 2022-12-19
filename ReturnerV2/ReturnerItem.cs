using System;
using ThunderRoad;
using UnityEngine;

namespace ReturnerV2
{
	// Token: 0x02000002 RID: 2
	public class ReturnerItem : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<ReturnerModule>();
			this.minThrow = this.module.minThrowVelocity;
			this.returnSpeed = this.module.returnSpeed;
			this.thrown = false;
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.ItemOnOnUngrabEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.ItemOnOnGrabEvent);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.Item_OnTeleGrabEvent);
			this.item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.Item_OnTeleUnGrabEvent);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002104 File Offset: 0x00000304
		private void ItemOnOnGrabEvent(Handle handle, RagdollHand ragdollhand)
		{
			if (!this.thrown)
			{
				return;
			}
			this.thrown = false;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002118 File Offset: 0x00000318
		private void ItemOnOnUngrabEvent(Handle handle, RagdollHand ragdollhand, bool throwing)
		{
			if ((double)this.item.rb.velocity.magnitude <= (double)this.minThrow)
			{
				return;
			}
			this.thrown = true;
			this.playerHand = ragdollhand.playerHand;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000215B File Offset: 0x0000035B
		private void Item_OnTeleUnGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.isTeleGrabbed = false;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002164 File Offset: 0x00000364
		private void Item_OnTeleGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.isTeleGrabbed = true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002170 File Offset: 0x00000370
		private void Update()
		{
			if (!this.playerHand || !this.thrown || (!PlayerControl.GetHand(this.playerHand.side).gripPressed && !this.returnClicked) || this.item.isGripped || this.isTeleGrabbed)
			{
				return;
			}
			if (this.playerHand.ragdollHand.grabbedHandle || this.playerHand.ragdollHand.caster.telekinesis.catchedHandle)
			{
				this.playerHand = null;
				return;
			}
			this.ReturnItem();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002210 File Offset: 0x00000410
		public void ReturnItem()
		{
			this.returnClicked = true;
			if ((double)Vector3.Distance(this.item.mainHandleRight.transform.position, this.playerHand.transform.position) < 0.300000011920929)
			{
				this.playerHand.ragdollHand.TryRelease();
				this.playerHand.ragdollHand.Grab(this.item.mainHandleRight);
				this.returnClicked = false;
				return;
			}
			this.item.rb.velocity = (this.playerHand.transform.position - this.item.mainHandleRight.transform.position) * this.returnSpeed;
		}

		// Token: 0x04000001 RID: 1
		private Item item;

		// Token: 0x04000002 RID: 2
		private ReturnerModule module;

		// Token: 0x04000003 RID: 3
		private float returnSpeed;

		// Token: 0x04000004 RID: 4
		private float minThrow;

		// Token: 0x04000005 RID: 5
		private bool thrown;

		// Token: 0x04000006 RID: 6
		private PlayerHand playerHand;

		// Token: 0x04000007 RID: 7
		private bool isTeleGrabbed;

		// Token: 0x04000008 RID: 8
		private bool returnClicked;

		// Token: 0x04000009 RID: 9
		private bool returning;
	}
}
