using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x0200001F RID: 31
	public class CompassController : MonoBehaviour
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00008B2C File Offset: 0x00006D2C
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			if (!string.IsNullOrEmpty(this.module.compassRef))
			{
				this.compass = this.item.GetCustomReference(this.module.compassRef, true).gameObject;
			}
			if (this.compass != null)
			{
				this.currentIndex = -1;
				this.compassIndex = 0;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00008BAB File Offset: 0x00006DAB
		public void LateUpdate()
		{
			this.UpdateCompassPosition();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00008BB4 File Offset: 0x00006DB4
		public void UpdateCompassPosition()
		{
			if (this.compass == null)
			{
				return;
			}
			this.compassIndex = (int)Mathf.Floor(this.compass.transform.rotation.eulerAngles.y / 45f);
			if (this.currentIndex != this.compassIndex)
			{
				this.currentIndex = this.compassIndex;
				this.compass.transform.Rotate(0f, 0f, -1f * this.compass.transform.rotation.eulerAngles.z, 1);
				this.compass.transform.Rotate(0f, 0f, (float)this.compassIndex * 45f, 1);
			}
		}

		// Token: 0x040001B5 RID: 437
		protected Item item;

		// Token: 0x040001B6 RID: 438
		protected AttachmentModule module;

		// Token: 0x040001B7 RID: 439
		private GameObject compass;

		// Token: 0x040001B8 RID: 440
		private int currentIndex;

		// Token: 0x040001B9 RID: 441
		private int compassIndex;
	}
}
