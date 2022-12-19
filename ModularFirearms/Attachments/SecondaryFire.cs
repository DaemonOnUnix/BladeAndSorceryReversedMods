using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x02000022 RID: 34
	public class SecondaryFire : MonoBehaviour
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000949C File Offset: 0x0000769C
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			this.module = this.item.data.GetModule<AttachmentModule>();
			if (!string.IsNullOrEmpty(this.module.mainGripID))
			{
				this.secondaryHandle = this.item.GetCustomReference(this.module.mainGripID, true).GetComponent<Handle>();
			}
			if (!string.IsNullOrEmpty(this.module.fireSoundRef))
			{
				this.fireSound = this.item.GetCustomReference(this.module.fireSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.muzzleFlashRef))
			{
				this.MuzzleFlash = this.item.GetCustomReference(this.module.muzzleFlashRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.muzzlePositionRef))
			{
				this.muzzlePoint = this.item.GetCustomReference(this.module.muzzlePositionRef, true);
				return;
			}
			this.muzzlePoint = this.item.transform;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000095BF File Offset: 0x000077BF
		private void Start()
		{
			this.prevShot = Time.time;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000095CC File Offset: 0x000077CC
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.secondaryHandle) && action == null && Time.time - this.prevShot > this.module.fireDelay)
			{
				this.prevShot = Time.time;
				this.Fire();
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00009609 File Offset: 0x00007809
		public void PreFireEffects()
		{
			if (this.MuzzleFlash != null)
			{
				this.MuzzleFlash.Play();
			}
			if (this.fireSound != null)
			{
				this.fireSound.Play();
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00009640 File Offset: 0x00007840
		private void Fire()
		{
			this.PreFireEffects();
			FrameworkCore.ShootProjectile(this.item, this.module.projectileID, this.muzzlePoint, null, this.module.forceMult, this.module.throwMult, false, null, null);
		}

		// Token: 0x040001D4 RID: 468
		private float prevShot;

		// Token: 0x040001D5 RID: 469
		protected Item item;

		// Token: 0x040001D6 RID: 470
		protected AttachmentModule module;

		// Token: 0x040001D7 RID: 471
		private Handle secondaryHandle;

		// Token: 0x040001D8 RID: 472
		private AudioSource fireSound;

		// Token: 0x040001D9 RID: 473
		private ParticleSystem MuzzleFlash;

		// Token: 0x040001DA RID: 474
		private Transform muzzlePoint;
	}
}
