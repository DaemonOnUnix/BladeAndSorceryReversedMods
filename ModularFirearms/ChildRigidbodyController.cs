using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms
{
	// Token: 0x02000004 RID: 4
	public class ChildRigidbodyController
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000220C File Offset: 0x0000040C
		public ChildRigidbodyController(Item Parent, FirearmModule ParentModule)
		{
			this.parentItem = Parent;
			this.parentModule = ParentModule;
			this.slideForwardForce = this.parentModule.slideForwardForce;
			this.slideBlowbackForce = this.parentModule.slideBlowbackForce;
			this.lockedAnchorOffset = this.parentModule.slideNeutralLockOffset;
			this.lockedBackAnchorOffset = -1f * this.parentModule.slideTravelDistance;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002284 File Offset: 0x00000484
		public void InitializeSlide(GameObject slideObject)
		{
			try
			{
				this.rb = slideObject.GetComponent<Rigidbody>();
				this.slideHandle = slideObject.GetComponent<Handle>();
				this.slideForce = slideObject.GetComponent<ConstantForce>();
				this.connectedJoint = this.parentItem.gameObject.GetComponent<ConfigurableJoint>();
				this.thisSlideObject = slideObject;
				if (!string.IsNullOrEmpty(this.parentModule.chamberBulletRef))
				{
					this.chamberBullet = this.parentItem.GetCustomReference(this.parentModule.chamberBulletRef, true).gameObject;
				}
			}
			catch
			{
				Debug.LogError("[ModularFirearmsFramework][EXCEPTION] Unable to Initialize CRC ! ");
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002328 File Offset: 0x00000528
		public void SetupSlide()
		{
			this.originalAnchor = new Vector3(0f, 0f, -0.5f * this.parentModule.slideTravelDistance);
			this.lockedBackAnchor = new Vector3(0f, 0f, this.lockedBackAnchorOffset);
			this.lockedNeutralAnchor = new Vector3(0f, 0f, this.lockedAnchorOffset);
			this.currentAnchor = this.originalAnchor;
			this.connectedJoint.anchor = this.currentAnchor;
			this.ChamberRoundVisible(false);
			this.LockSlide(true);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023BC File Offset: 0x000005BC
		public void SetLockedState(bool forward = true)
		{
			this.SetRelativeSlideForce(new Vector3(0f, 0f, 0f));
			this.connectedJoint.zMotion = 0;
			if (forward)
			{
				this.currentAnchor = this.lockedNeutralAnchor;
				this.connectedJoint.anchor = this.currentAnchor;
				return;
			}
			this.currentAnchor = this.lockedBackAnchor;
			this.connectedJoint.anchor = this.currentAnchor;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002430 File Offset: 0x00000630
		public void LockSlide(bool disable_touch = true)
		{
			this.SetRelativeSlideForce(new Vector3(0f, 0f, 0f));
			this.connectedJoint.zMotion = 0;
			if (this.isLockedBack)
			{
				this.currentAnchor = this.lockedBackAnchor;
				this.connectedJoint.anchor = this.currentAnchor;
			}
			else
			{
				this.currentAnchor = this.lockedNeutralAnchor;
				this.connectedJoint.anchor = this.currentAnchor;
			}
			if (disable_touch)
			{
				this.DisableTouch();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024B0 File Offset: 0x000006B0
		public void UnlockSlide(bool enable_touch = true)
		{
			if (enable_touch)
			{
				this.EnableTouch();
			}
			this.SetRelativeSlideForce(new Vector3(0f, 0f, this.directionModifer * this.slideForwardForce));
			this.connectedJoint.zMotion = 1;
			this.currentAnchor = this.originalAnchor;
			this.connectedJoint.anchor = this.currentAnchor;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002514 File Offset: 0x00000714
		public void ForwardState()
		{
			this.isLockedBack = false;
			this.directionModifer = 1f;
			this.SetRelativeSlideForce(new Vector3(0f, 0f, this.directionModifer * this.slideForwardForce));
			this.connectedJoint.zMotion = 1;
			this.currentAnchor = this.originalAnchor;
			this.connectedJoint.anchor = this.currentAnchor;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002580 File Offset: 0x00000780
		public void LockedBackState()
		{
			this.isLockedBack = true;
			this.directionModifer = -1f;
			this.SetRelativeSlideForce(new Vector3(0f, 0f, this.directionModifer * this.slideForwardForce));
			this.connectedJoint.zMotion = 1;
			this.currentAnchor = this.originalAnchor;
			this.connectedJoint.anchor = this.currentAnchor;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025EC File Offset: 0x000007EC
		public void BlowBack(bool lastShot = false)
		{
			this.SetRelativeSlideForce(new Vector3(0f, 0f, this.slideForwardForce * 0.1f));
			this.rb.AddRelativeForce(Vector3.forward * -1f * this.slideBlowbackForce, 1);
			if (!lastShot)
			{
				this.SetRelativeSlideForce(new Vector3(0f, 0f, this.directionModifer * this.slideForwardForce));
			}
			this.ChamberRoundVisible(false);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000266C File Offset: 0x0000086C
		public void LastShot()
		{
			this.BlowBack(true);
			this.LockedBackState();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000267B File Offset: 0x0000087B
		public void SetHeld(bool status)
		{
			this.isHeld = status;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002684 File Offset: 0x00000884
		public bool IsHeld()
		{
			return this.isHeld;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000268C File Offset: 0x0000088C
		public bool IsLocked()
		{
			return this.isLockedBack;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002694 File Offset: 0x00000894
		public void DisableTouch()
		{
			this.slideHandle.SetTouch(false);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026A2 File Offset: 0x000008A2
		public void EnableTouch()
		{
			this.slideHandle.SetTouch(true);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026B0 File Offset: 0x000008B0
		public void ChamberRoundVisible(bool isVisible = false)
		{
			if (this.chamberBullet != null)
			{
				this.chamberBullet.SetActive(isVisible);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026CC File Offset: 0x000008CC
		public void SetRelativeSlideForce(Vector3 newSlideForce)
		{
			this.slideForce.relativeForce = newSlideForce;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026DC File Offset: 0x000008DC
		public void FixCustomComponents()
		{
			if (this.connectedJoint.anchor.z != this.currentAnchor.z)
			{
				this.connectedJoint.anchor = new Vector3(0f, 0f, this.currentAnchor.z);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000272B File Offset: 0x0000092B
		public void DumpJoint()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000272B File Offset: 0x0000092B
		public void DumpRB()
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000272D File Offset: 0x0000092D
		public GameObject GetConnectedObj()
		{
			return this.connectedJoint.gameObject;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000273A File Offset: 0x0000093A
		public void DestroyJoint()
		{
			Object.Destroy(this.slideForce);
			Object.Destroy(this.connectedJoint);
			this.thisSlideObject.transform.parent = null;
		}

		// Token: 0x04000006 RID: 6
		public GameObject thisSlideObject;

		// Token: 0x04000007 RID: 7
		public Rigidbody rb;

		// Token: 0x04000008 RID: 8
		public bool initialCheck;

		// Token: 0x04000009 RID: 9
		protected Item parentItem;

		// Token: 0x0400000A RID: 10
		protected FirearmModule parentModule;

		// Token: 0x0400000B RID: 11
		private readonly float slideForwardForce;

		// Token: 0x0400000C RID: 12
		private readonly float slideBlowbackForce;

		// Token: 0x0400000D RID: 13
		private readonly float lockedAnchorOffset;

		// Token: 0x0400000E RID: 14
		private readonly float lockedBackAnchorOffset;

		// Token: 0x0400000F RID: 15
		private Handle slideHandle;

		// Token: 0x04000010 RID: 16
		private ConstantForce slideForce;

		// Token: 0x04000011 RID: 17
		private ConfigurableJoint connectedJoint;

		// Token: 0x04000012 RID: 18
		private GameObject chamberBullet;

		// Token: 0x04000013 RID: 19
		private Vector3 currentAnchor;

		// Token: 0x04000014 RID: 20
		private Vector3 originalAnchor;

		// Token: 0x04000015 RID: 21
		private Vector3 lockedBackAnchor;

		// Token: 0x04000016 RID: 22
		private Vector3 lockedNeutralAnchor;

		// Token: 0x04000017 RID: 23
		private bool isHeld;

		// Token: 0x04000018 RID: 24
		private float directionModifer = 1f;

		// Token: 0x04000019 RID: 25
		private bool isLockedBack;
	}
}
