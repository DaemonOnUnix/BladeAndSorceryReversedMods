using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200004A RID: 74
	public class WristbowMono : ArmModule
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000CE9C File Offset: 0x0000B09C
		public override void OnStart()
		{
			base.OnStart();
			this.HasAltMode = false;
			ModuleHandWatcher[] checker = this.item.GetComponents<ModuleHandWatcher>();
			foreach (ModuleHandWatcher watcher in checker)
			{
				Object.Destroy(watcher);
			}
			this.OnOff = true;
			bool flag = this.item.holder != null;
			if (flag)
			{
				this.OnSnapEvent(this.item.holder);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000CF14 File Offset: 0x0000B114
		public override void Activate()
		{
			this.Activated = true;
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredFire", true).Spawn(this.Hand.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			this.TestBolt.SpawnAsync(delegate(Item bolt)
			{
				Creature currentCreature = Player.currentCreature;
				bolt.IgnoreRagdollCollision((currentCreature != null) ? currentCreature.ragdoll : null);
				bolt.Throw(1f, 2);
				bolt.rb.AddForce(-this.Hand.transform.right * 10f, 1);
				this.Activated = false;
			}, new Vector3?(this.item.flyDirRef.transform.position), new Quaternion?(this.item.flyDirRef.transform.rotation), null, true, null);
			base.Activate();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000CFAA File Offset: 0x0000B1AA
		public override void On()
		{
			this.OnOff = true;
			base.On();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000CFBB File Offset: 0x0000B1BB
		public override void Off()
		{
			this.OnOff = false;
			base.Off();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			base.giveHand(hand);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		public override void OnUnSnapEvent(Holder holder)
		{
			base.OnUnSnapEvent(holder);
			bool flag = this.handWatcher != null;
			if (flag)
			{
				this.handWatcher.Delete();
				this.handWatcher = null;
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D01C File Offset: 0x0000B21C
		public override void OnSnapEvent(Holder holder)
		{
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Hand = Player.currentCreature.handRight;
				this.handWatcher = this.item.gameObject.AddComponent<ModuleHandWatcher>();
				this.handWatcher.Setup(this.Hand, this);
			}
			else
			{
				bool flag2 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag2)
				{
					this.Hand = Player.currentCreature.handLeft;
					this.handWatcher = this.item.gameObject.AddComponent<ModuleHandWatcher>();
					this.handWatcher.Setup(this.Hand, this);
				}
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		public void TriggerAction()
		{
			bool flag = this.toggleMethod == toggleMethod.Trigger;
			if (flag)
			{
				this.Hand.playerHand.controlHand.HapticShort(2f);
				bool onOff = this.OnOff;
				if (onOff)
				{
					this.OnOff = false;
				}
				else
				{
					this.OnOff = true;
				}
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D154 File Offset: 0x0000B354
		public void AltUseAction()
		{
			bool flag = this.toggleMethod == toggleMethod.AltUse;
			if (flag)
			{
				this.Hand.playerHand.controlHand.HapticShort(2f);
				bool onOff = this.OnOff;
				if (onOff)
				{
					this.OnOff = false;
				}
				else
				{
					this.OnOff = true;
				}
			}
		}

		// Token: 0x04000154 RID: 340
		private ItemData TestBolt = Catalog.GetData<ItemData>("GrooveSlinger.Dishonored.Bolt", true);

		// Token: 0x04000155 RID: 341
		private toggleMethod toggleMethod = WristbowParser.toggleMethod;

		// Token: 0x04000156 RID: 342
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x04000157 RID: 343
		private Color CurrentColor;

		// Token: 0x04000158 RID: 344
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);
	}
}
