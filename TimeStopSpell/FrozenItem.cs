using System;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x02000005 RID: 5
	public class FrozenItem : MonoBehaviour
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002828 File Offset: 0x00000A28
		private void Start()
		{
			this._item = base.gameObject.GetComponentInParent<Item>();
			this._defaultLocalVelocity = this._item.transform.InverseTransformDirection(this._item.rb.velocity);
			this._defaultLocalAngularVelocity = this._item.transform.InverseTransformDirection(this._item.rb.angularVelocity);
			this._item.rb.constraints = 126;
			this._item.OnGrabEvent += new Item.GrabDelegate(this.ItemOnOnGrabEvent);
			this._item.OnUngrabEvent += new Item.ReleaseDelegate(this.ItemOnOnUngrabEvent);
			this._item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.ItemOnOnTelekinesisGrabEvent);
			this._item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.ItemOnOnTelekinesisReleaseEvent);
			this._item.OnDespawnEvent += new Item.SpawnEvent(this.ItemOnOnDespawnEvent);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002917 File Offset: 0x00000B17
		private void ItemOnOnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis telegrabber)
		{
			this.FreezeItem();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000291F File Offset: 0x00000B1F
		private void ItemOnOnUngrabEvent(Handle handle, RagdollHand ragdollhand, bool throwing)
		{
			this.FreezeItem();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002927 File Offset: 0x00000B27
		private void UnFreezeItem()
		{
			this._item.rb.constraints = 0;
			this._item.rb.ResetInertiaTensor();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000294A File Offset: 0x00000B4A
		private void FreezeItem()
		{
			this._item.rb.constraints = 126;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000295E File Offset: 0x00000B5E
		private void ItemOnOnTelekinesisGrabEvent(Handle handle, SpellTelekinesis telegrabber)
		{
			this.UnFreezeItem();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002966 File Offset: 0x00000B66
		private void ItemOnOnGrabEvent(Handle handle, RagdollHand ragdollhand)
		{
			this.UnFreezeItem();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000296E File Offset: 0x00000B6E
		private void ItemOnOnDespawnEvent(EventTime eventtime)
		{
			if (eventtime == 1)
			{
				Object.Destroy(this);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000297C File Offset: 0x00000B7C
		private void OnDestroy()
		{
			this._item.rb.constraints = 0;
			this._item.rb.ResetInertiaTensor();
			this._item.rb.velocity = this._item.transform.TransformDirection(this._defaultLocalVelocity);
			this._item.rb.angularVelocity = this._item.transform.TransformDirection(this._defaultLocalAngularVelocity);
			this._item.OnGrabEvent -= new Item.GrabDelegate(this.ItemOnOnGrabEvent);
			this._item.OnUngrabEvent -= new Item.ReleaseDelegate(this.ItemOnOnUngrabEvent);
			this._item.OnTelekinesisGrabEvent -= new Item.TelekinesisDelegate(this.ItemOnOnTelekinesisGrabEvent);
			this._item.OnTelekinesisReleaseEvent -= new Item.TelekinesisDelegate(this.ItemOnOnTelekinesisReleaseEvent);
			this._item.OnDespawnEvent -= new Item.SpawnEvent(this.ItemOnOnDespawnEvent);
		}

		// Token: 0x04000008 RID: 8
		private Item _item;

		// Token: 0x04000009 RID: 9
		private Vector3 _defaultLocalVelocity;

		// Token: 0x0400000A RID: 10
		private Vector3 _defaultLocalAngularVelocity;
	}
}
