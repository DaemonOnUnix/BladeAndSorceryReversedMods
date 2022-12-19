using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000015 RID: 21
	public class ModuleHandWatcher : MonoBehaviour
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public void Start()
		{
			this.Set = false;
			this.CurrentTime = this.TimerMax;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005C0E File Offset: 0x00003E0E
		public void Setup(RagdollHand hand, ArmModule director)
		{
			this.Hand = hand;
			this.Director = director;
			this.setup = true;
			this.CurrentTime = this.TimerMax;
			this.tap1 = false;
			this.tap2 = false;
			this.canTap2 = false;
			this.Timer = false;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005C4E File Offset: 0x00003E4E
		public void Delete()
		{
			this.setup = false;
			this.Hand = null;
			this.Director = null;
			Object.Destroy(this);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005C70 File Offset: 0x00003E70
		public void Update()
		{
			bool flag = this.setup;
			if (flag)
			{
				bool flag2 = this.Hand.side == 1 && WheelMenuSpell.left.isShown && !this.Director.canGrab;
				if (flag2)
				{
					this.Director.turnGrabOn();
				}
				else
				{
					bool flag3 = this.Hand.side == 1 && !WheelMenuSpell.left.isShown && this.Director.canGrab;
					if (flag3)
					{
						this.Director.turnGrabOff();
					}
					else
					{
						bool flag4 = this.Hand.side == null && WheelMenuSpell.right.isShown && !this.Director.canGrab;
						if (flag4)
						{
							this.Director.turnGrabOn();
						}
						else
						{
							bool flag5 = this.Hand.side == null && !WheelMenuSpell.right.isShown && this.Director.canGrab;
							if (flag5)
							{
								this.Director.turnGrabOff();
							}
						}
					}
				}
			}
			bool flag6 = this.setup && this.Director.OnOff;
			if (flag6)
			{
				bool flag7 = this.Hand.playerHand.controlHand.usePressed && !this.tap1;
				if (flag7)
				{
					this.tap1 = true;
					this.Timer = true;
				}
				bool flag8 = !this.Hand.playerHand.controlHand.usePressed && this.tap1;
				if (flag8)
				{
					this.canTap2 = true;
				}
				bool flag9 = this.Hand.playerHand.controlHand.usePressed && this.tap1 && this.canTap2;
				if (flag9)
				{
					this.CurrentTime = 0f;
					this.tap2 = true;
				}
				bool flag10 = this.Hand.playerHand.controlHand.gripPressed && this.Hand.playerHand.controlHand.usePressed && !this.Set;
				if (flag10)
				{
					this.Set = true;
					bool flag11 = ExtensionMethods.IsGrabbingCreature(this.Hand) && this.Director.OnOff && this.Director.HasCreatureActivate;
					if (flag11)
					{
						this.Director.CreatureActivate();
					}
					else
					{
						bool flag12 = ExtensionMethods.IsEmpty(this.Hand) && !this.Director.Activated && this.Director.OnOff;
						if (flag12)
						{
							this.Director.Activate();
						}
					}
				}
				bool flag13 = this.Set && (!this.Hand.playerHand.controlHand.usePressed || !this.Hand.playerHand.controlHand.gripPressed);
				if (flag13)
				{
					bool flag14 = this.Director.Activated && this.Director.useDeactivate;
					if (flag14)
					{
						this.Director.Deactivate();
					}
					this.Set = false;
				}
			}
			bool timer = this.Timer;
			if (timer)
			{
				this.CurrentTime -= Time.deltaTime;
				bool flag15 = this.CurrentTime <= 0f;
				if (flag15)
				{
					bool flag16 = this.tap1 && this.tap2 && this.Director.OnOff && !this.Set && ExtensionMethods.IsEmpty(this.Hand);
					if (flag16)
					{
						bool hasAltMode = this.Director.HasAltMode;
						if (hasAltMode)
						{
							this.Hand.playerHand.controlHand.HapticShort(2f);
							this.Director.AltMode();
						}
					}
					this.tap1 = false;
					this.tap2 = false;
					this.canTap2 = false;
					this.CurrentTime = this.TimerMax;
					this.Timer = false;
				}
			}
		}

		// Token: 0x0400006F RID: 111
		private RagdollHand Hand;

		// Token: 0x04000070 RID: 112
		private ArmModule Director;

		// Token: 0x04000071 RID: 113
		private bool setup = false;

		// Token: 0x04000072 RID: 114
		private bool Set;

		// Token: 0x04000073 RID: 115
		private bool tap1;

		// Token: 0x04000074 RID: 116
		private bool canTap2;

		// Token: 0x04000075 RID: 117
		private bool tap2;

		// Token: 0x04000076 RID: 118
		private bool Timer;

		// Token: 0x04000077 RID: 119
		private float TimerMax = 0.2f;

		// Token: 0x04000078 RID: 120
		private float CurrentTime;
	}
}
