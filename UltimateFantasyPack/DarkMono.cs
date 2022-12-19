using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000007 RID: 7
	public class DarkMono : MonoBehaviour
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000025CB File Offset: 0x000007CB
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.canUse = true;
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025FC File Offset: 0x000007FC
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = this.canUse && !this.active && action == 2;
			if (flag)
			{
				Debug.Log("portal opened");
				this.active = true;
				Catalog.GetData<ItemData>("DarkPortal", true).SpawnAsync(delegate(Item item2)
				{
					this.portalItem = item2;
					this.portalItem.transform.position = Player.currentCreature.transform.position;
					this.portalItem.transform.localScale = new Vector3(1f, 5f, 1f);
				}, null, null, null, true, null);
				base.StartCoroutine(this.CoolDown());
			}
			else
			{
				bool flag2 = action == 2 && this.active;
				if (flag2)
				{
					Debug.Log("portal closed");
					Player.local.transform.position = this.portalItem.transform.position;
					this.portalItem.Despawn(0.1f);
					this.active = false;
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026D0 File Offset: 0x000008D0
		private IEnumerator CoolDown()
		{
			this.canUse = false;
			yield return new WaitForSeconds(this.cooldown);
			this.canUse = true;
			yield break;
		}

		// Token: 0x0400000F RID: 15
		private Item item;

		// Token: 0x04000010 RID: 16
		private bool active;

		// Token: 0x04000011 RID: 17
		private Color purple = new Color(1f, 0f, 1f, 1f);

		// Token: 0x04000012 RID: 18
		private Color clear = new Color(0f, 0f, 0f);

		// Token: 0x04000013 RID: 19
		private float cooldown = 15f;

		// Token: 0x04000014 RID: 20
		private Item portalItem;

		// Token: 0x04000015 RID: 21
		private bool canUse;
	}
}
