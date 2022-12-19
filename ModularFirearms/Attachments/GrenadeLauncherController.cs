using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x0200001E RID: 30
	public class GrenadeLauncherController : MonoBehaviour
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00008AEE File Offset: 0x00006CEE
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000272B File Offset: 0x0000092B
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
		}

		// Token: 0x040001B3 RID: 435
		protected Item item;

		// Token: 0x040001B4 RID: 436
		protected AttachmentModule module;
	}
}
