using System;
using ModularFirearms.Shared;
using ModularFirearms.Weapons;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x0200001C RID: 28
	public class AmmoCounterController : MonoBehaviour
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00008644 File Offset: 0x00006844
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			this.parentFirearm = base.GetComponent<BaseFirearmGenerator>();
			if (!string.IsNullOrEmpty(this.module.ammoCounterRef))
			{
				this.ammoCounterMesh = this.item.GetCustomReference(this.module.ammoCounterRef, true).GetComponent<MeshRenderer>();
				this.digitsGridTexture = (Texture2D)this.item.GetCustomReference(this.module.ammoCounterRef, true).GetComponent<MeshRenderer>().material.mainTexture;
			}
			if (this.digitsGridTexture != null && this.ammoCounterMesh != null)
			{
				this.ammoCounter = new TextureProcessor();
				this.ammoCounter.SetGridTexture(this.digitsGridTexture);
				this.ammoCounter.SetTargetRenderer(this.ammoCounterMesh);
			}
			if (this.ammoCounter != null)
			{
				this.ammoCounter.DisplayUpdate(this.newAmmoCount);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00008746 File Offset: 0x00006946
		public void LateUpdate()
		{
			this.newAmmoCount = this.parentFirearm.GetAmmoCounter();
			if (this.lastAmmoCount != this.newAmmoCount)
			{
				this.ammoCounter.DisplayUpdate(this.newAmmoCount);
				this.lastAmmoCount = this.newAmmoCount;
			}
		}

		// Token: 0x0400019F RID: 415
		protected Item item;

		// Token: 0x040001A0 RID: 416
		protected AttachmentModule module;

		// Token: 0x040001A1 RID: 417
		private TextureProcessor ammoCounter;

		// Token: 0x040001A2 RID: 418
		private MeshRenderer ammoCounterMesh;

		// Token: 0x040001A3 RID: 419
		private Texture2D digitsGridTexture;

		// Token: 0x040001A4 RID: 420
		private int lastAmmoCount;

		// Token: 0x040001A5 RID: 421
		private int newAmmoCount;

		// Token: 0x040001A6 RID: 422
		private BaseFirearmGenerator parentFirearm;
	}
}
