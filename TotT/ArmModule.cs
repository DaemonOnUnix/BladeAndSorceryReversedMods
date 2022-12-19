using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000013 RID: 19
	public class ArmModule : MonoBehaviour
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000058E6 File Offset: 0x00003AE6
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000058F8 File Offset: 0x00003AF8
		public void Start()
		{
			this.canGrab = true;
			this.OnOff = true;
			this.item.OnSnapEvent += new Item.HolderDelegate(this.OnSnapEvent);
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.OnUnSnapEvent);
			this.OnStart();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000594D File Offset: 0x00003B4D
		public void Update()
		{
			this.OnUpdate();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005957 File Offset: 0x00003B57
		public virtual void OnStart()
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000595A File Offset: 0x00003B5A
		public virtual void OnUpdate()
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000595D File Offset: 0x00003B5D
		public virtual void On()
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005960 File Offset: 0x00003B60
		public virtual void Off()
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005963 File Offset: 0x00003B63
		public virtual void Activate()
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005966 File Offset: 0x00003B66
		public virtual void CreatureActivate()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005969 File Offset: 0x00003B69
		public virtual void Deactivate()
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000596C File Offset: 0x00003B6C
		public virtual void AltMode()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000596F File Offset: 0x00003B6F
		public virtual void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000597C File Offset: 0x00003B7C
		public virtual void OnUnSnapEvent(Holder holder)
		{
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID" || holder.data.id == "GrooveSlinger.LeftWristHolderID";
			if (flag)
			{
				holder.SendMessage("GrabOn");
				this.canGrab = true;
			}
			bool flag2 = this.handWatcher != null;
			if (flag2)
			{
				this.handWatcher.Delete();
				this.handWatcher = null;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000059F8 File Offset: 0x00003BF8
		public virtual void OnSnapEvent(Holder holder)
		{
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID" || holder.data.id == "GrooveSlinger.LeftWristHolderID";
			if (flag)
			{
				holder.SendMessage("GrabOff");
				this.OnOff = true;
				this.canGrab = false;
			}
			bool flag2 = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag2)
			{
				this.Hand = Player.currentCreature.handRight;
				this.handWatcher = this.item.gameObject.AddComponent<ModuleHandWatcher>();
				this.handWatcher.Setup(this.Hand, this);
			}
			else
			{
				bool flag3 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag3)
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

		// Token: 0x060000A3 RID: 163 RVA: 0x00005B28 File Offset: 0x00003D28
		public void turnGrabOn()
		{
			bool flag = this.item.holder != null;
			if (flag)
			{
				Holder holder = this.item.holder;
				if (holder != null)
				{
					holder.SendMessage("GrabOn");
				}
				this.canGrab = true;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005B70 File Offset: 0x00003D70
		public void turnGrabOff()
		{
			bool flag = this.item.holder != null;
			if (flag)
			{
				Holder holder = this.item.holder;
				if (holder != null)
				{
					holder.SendMessage("GrabOff");
				}
				this.canGrab = false;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005BB8 File Offset: 0x00003DB8
		public void WaterFix(Item sitem)
		{
			sitem.waterHandler.inWater = true;
			sitem.waterHandler.Reset();
		}

		// Token: 0x04000061 RID: 97
		public Item item;

		// Token: 0x04000062 RID: 98
		public ModuleHandWatcher handWatcher;

		// Token: 0x04000063 RID: 99
		public RailMountsMono Mount;

		// Token: 0x04000064 RID: 100
		public RagdollHand Hand;

		// Token: 0x04000065 RID: 101
		public ArmModuleSave customData;

		// Token: 0x04000066 RID: 102
		public bool OnOff;

		// Token: 0x04000067 RID: 103
		public bool Activated = false;

		// Token: 0x04000068 RID: 104
		public bool HasAltMode = false;

		// Token: 0x04000069 RID: 105
		public bool useDeactivate = false;

		// Token: 0x0400006A RID: 106
		public bool HasCreatureActivate = false;

		// Token: 0x0400006B RID: 107
		public bool canGrab;
	}
}
