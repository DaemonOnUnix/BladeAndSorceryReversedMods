using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Armiger
{
	// Token: 0x02000003 RID: 3
	public class ArmigerComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.tkhandle = this.item.GetCustomReference("TKHandle", true).gameObject;
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.Item_OnTelekinesisGrabEvent);
			this.tkhandle.SetActive(false);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC
		private void Item_OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			teleGrabber.spinFromCenterOfMass = true;
			bool flag = this.warp;
			if (flag)
			{
				base.StartCoroutine(this.Warp(teleGrabber.spellCaster.ragdollHand, handle));
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002136 File Offset: 0x00000336
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			this.warp = false;
			this.tkhandle.SetActive(false);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002150 File Offset: 0x00000350
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			bool flag = PlayerControl.GetHand(ragdollHand.side).castPressed || PlayerControl.GetHand(ragdollHand.side).alternateUsePressed;
			if (flag)
			{
				this.warp = true;
				this.tkhandle.SetActive(true);
				foreach (Damager damager in this.item.GetComponentsInChildren<Damager>())
				{
					damager.data.penetrationTempModifierDamperOut += 100000f;
				}
				this.item.IgnoreRagdollCollision(ragdollHand.ragdoll);
			}
			else
			{
				this.warp = false;
				this.tkhandle.SetActive(false);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002200 File Offset: 0x00000400
		public void FixedUpdate()
		{
			bool flag = this.item.data.id == "StarOfTheRogue" && this.item.isFlying;
			if (flag)
			{
				this.item.flyDirRef.Rotate(new Vector3(0f, -2160f, 0f) * Time.fixedDeltaTime);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000226D File Offset: 0x0000046D
		public IEnumerator Warp(RagdollHand hand, Handle handle)
		{
			yield return new WaitForEndOfFrame();
			hand.caster.telekinesis.TryRelease(false);
			this.warp = false;
			Quaternion rotation = Player.local.transform.rotation;
			Common.MoveAlign(Player.local.transform, hand.grip, this.item.GetMainHandle(hand.side).GetDefaultOrientation(hand.side).transform, null);
			Player.local.transform.rotation = rotation;
			Player.local.locomotion.prevPosition = handle.transform.position;
			Player.local.locomotion.rb.velocity = Vector3.zero;
			Player.local.locomotion.velocity = Vector3.zero;
			bool flag = !this.item.isPenetrating;
			if (flag)
			{
				Common.MoveAlign(this.item.transform, this.item.GetMainHandle(hand.side).GetDefaultOrientation(hand.side).transform, hand.grip, null);
			}
			bool flag2 = hand.grabbedHandle == null;
			if (flag2)
			{
				hand.Grab(this.item.GetMainHandle(hand.side));
			}
			yield return new WaitForSeconds(0.5f);
			Player.local.locomotion.rb.velocity = Vector3.zero;
			Player.local.locomotion.velocity = Vector3.zero;
			foreach (Damager damager in this.item.GetComponentsInChildren<Damager>())
			{
				damager.data.penetrationTempModifierDamperOut -= 100000f;
				damager = null;
			}
			Damager[] array = null;
			yield break;
		}

		// Token: 0x04000001 RID: 1
		private Item item;

		// Token: 0x04000002 RID: 2
		private bool warp = false;

		// Token: 0x04000003 RID: 3
		private GameObject tkhandle;
	}
}
