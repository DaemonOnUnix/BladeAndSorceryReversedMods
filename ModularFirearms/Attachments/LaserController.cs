using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x02000020 RID: 32
	public class LaserController : MonoBehaviour
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00008C80 File Offset: 0x00006E80
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			if (!string.IsNullOrEmpty(this.module.laserRef))
			{
				this.attachedLaser = this.item.GetCustomReference(this.module.laserRef, true).GetComponent<LineRenderer>();
			}
			if (this.attachedLaser != null)
			{
				if (!string.IsNullOrEmpty(this.module.laserStartRef))
				{
					this.laserStart = this.item.GetCustomReference(this.module.laserStartRef, true);
				}
				if (!string.IsNullOrEmpty(this.module.laserEndRef))
				{
					this.laserEnd = this.item.GetCustomReference(this.module.laserEndRef, true);
				}
				if (!string.IsNullOrEmpty(this.module.laserRayCastPointRef))
				{
					this.rayCastPoint = this.item.GetCustomReference(this.module.laserRayCastPointRef, true);
				}
				LayerMask layerMask = 536870912;
				LayerMask layerMask2 = 268435456;
				LayerMask layerMask3 = 33554432;
				LayerMask layerMask4 = 8388608;
				LayerMask layerMask5 = 512;
				LayerMask layerMask6 = 32;
				LayerMask layerMask7 = 2;
				this.laserIgnore = layerMask | layerMask2 | layerMask3 | layerMask4 | layerMask5 | layerMask6 | layerMask7;
				this.laserIgnore = ~this.laserIgnore;
				this.maxLaserDistance = this.module.maxLaserDistance;
				this.laserEnd.localPosition = new Vector3(this.laserEnd.localPosition.x, this.laserEnd.localPosition.y, this.laserEnd.localPosition.z);
			}
			if (!string.IsNullOrEmpty(this.module.laserActivationSoundRef))
			{
				this.activationSound = this.item.GetCustomReference(this.module.laserActivationSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.laserHandleRef))
			{
				this.attachmentHandle = this.item.GetCustomReference(this.module.laserHandleRef, true).GetComponent<Handle>();
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008EEF File Offset: 0x000070EF
		protected void Start()
		{
			if (!this.module.laserStartActivated && this.attachedLaser != null)
			{
				this.attachedLaser.enabled = false;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008F18 File Offset: 0x00007118
		protected void StartLongPress()
		{
			this.checkForLongPress = true;
			this.lastSpellMenuPress = Time.time;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008F2C File Offset: 0x0000712C
		public void CancelLongPress()
		{
			this.checkForLongPress = false;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008F38 File Offset: 0x00007138
		public void LateUpdate()
		{
			if (this.checkForLongPress)
			{
				if (this.spellMenuPressed)
				{
					if (Time.time - this.lastSpellMenuPress > this.module.longPressTime)
					{
						if (this.module.longPressToActivate)
						{
							this.ToggleLaser();
						}
						this.CancelLongPress();
					}
				}
				else
				{
					this.CancelLongPress();
					if (!this.module.longPressToActivate)
					{
						this.ToggleLaser();
					}
				}
			}
			this.UpdateLaserPoint();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008FA8 File Offset: 0x000071A8
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.attachmentHandle))
			{
				if (action == 2)
				{
					this.spellMenuPressed = true;
					this.StartLongPress();
				}
				if (action == 3)
				{
					this.spellMenuPressed = false;
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008FD4 File Offset: 0x000071D4
		public void UpdateLaserPoint()
		{
			if (this.attachedLaser == null)
			{
				return;
			}
			if (this.attachedLaser.enabled)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(new Ray(this.rayCastPoint.position, this.rayCastPoint.forward), ref raycastHit, this.maxLaserDistance, this.laserIgnore))
				{
					this.laserEnd.localPosition = new Vector3(this.laserEnd.localPosition.x, this.laserEnd.localPosition.y, this.rayCastPoint.localPosition.z + raycastHit.distance);
					AnimationCurve animationCurve = new AnimationCurve();
					animationCurve.AddKey(0f, 0.0075f);
					animationCurve.AddKey(1f, 0.0075f);
					this.attachedLaser.widthCurve = animationCurve;
					this.attachedLaser.SetPosition(0, this.laserStart.position);
					this.attachedLaser.SetPosition(1, this.laserEnd.position);
					return;
				}
				this.laserEnd.localPosition = new Vector3(this.laserEnd.localPosition.x, this.laserEnd.localPosition.y, this.maxLaserDistance);
				AnimationCurve animationCurve2 = new AnimationCurve();
				animationCurve2.AddKey(0f, 0.0075f);
				animationCurve2.AddKey(1f, 0f);
				this.attachedLaser.widthCurve = animationCurve2;
				this.attachedLaser.SetPosition(0, this.laserStart.position);
				this.attachedLaser.SetPosition(1, this.laserEnd.position);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000917C File Offset: 0x0000737C
		private void ToggleLaser()
		{
			if (this.attachedLaser == null)
			{
				return;
			}
			if (this.activationSound != null)
			{
				this.activationSound.Play();
			}
			this.attachedLaser.enabled = !this.attachedLaser.enabled;
		}

		// Token: 0x040001BA RID: 442
		protected Item item;

		// Token: 0x040001BB RID: 443
		protected AttachmentModule module;

		// Token: 0x040001BC RID: 444
		private LineRenderer attachedLaser;

		// Token: 0x040001BD RID: 445
		private AudioSource activationSound;

		// Token: 0x040001BE RID: 446
		private LayerMask laserIgnore;

		// Token: 0x040001BF RID: 447
		private Transform laserStart;

		// Token: 0x040001C0 RID: 448
		private Transform laserEnd;

		// Token: 0x040001C1 RID: 449
		private Transform rayCastPoint;

		// Token: 0x040001C2 RID: 450
		private float maxLaserDistance;

		// Token: 0x040001C3 RID: 451
		private Handle attachmentHandle;

		// Token: 0x040001C4 RID: 452
		public float lastSpellMenuPress;

		// Token: 0x040001C5 RID: 453
		public bool isLongPress;

		// Token: 0x040001C6 RID: 454
		public bool checkForLongPress;

		// Token: 0x040001C7 RID: 455
		public bool spellMenuPressed;
	}
}
