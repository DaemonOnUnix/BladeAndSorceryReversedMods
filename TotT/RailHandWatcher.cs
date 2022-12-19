using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200003C RID: 60
	public class RailHandWatcher : MonoBehaviour
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x0000C503 File Offset: 0x0000A703
		public void Start()
		{
			this.Set = false;
			this.CurrentTime = this.TimerMax;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C519 File Offset: 0x0000A719
		public void Setup(RagdollHand hand, RailMountsMono director)
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

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C559 File Offset: 0x0000A759
		public void Delete()
		{
			this.setup = false;
			this.Hand = null;
			this.Director = null;
			Object.Destroy(this);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C578 File Offset: 0x0000A778
		public void GetHand()
		{
			this.Hand = this.Director.GetHand();
			bool flag = this.Hand == null;
			if (flag)
			{
				Debug.Log("TotT-Error: Hand not found! removing HandWatcher as a precaution");
				this.Director.RemoveWatcher();
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
		public void Update()
		{
			bool flag = this.setup && this.Director.On;
			if (flag)
			{
				bool flag2 = this.Hand == null;
				if (flag2)
				{
					this.GetHand();
				}
				bool flag3 = this.Hand.playerHand.controlHand.usePressed && !this.tap1;
				if (flag3)
				{
					this.tap1 = true;
					this.Timer = true;
				}
				bool flag4 = !this.Hand.playerHand.controlHand.usePressed && this.tap1;
				if (flag4)
				{
					this.canTap2 = true;
				}
				bool flag5 = this.Hand.playerHand.controlHand.usePressed && this.tap1 && this.canTap2;
				if (flag5)
				{
					this.CurrentTime = 0f;
					this.tap2 = true;
				}
				bool flag6 = this.Hand.side == 1 && WheelMenuSpell.left.isShown && !this.Director.canGrab;
				if (flag6)
				{
					this.Director.turnGrabOn();
				}
				else
				{
					bool flag7 = this.Hand.side == 1 && !WheelMenuSpell.left.isShown && this.Director.canGrab;
					if (flag7)
					{
						this.Director.turnGrabOff();
					}
					else
					{
						bool flag8 = this.Hand.side == null && WheelMenuSpell.right.isShown && !this.Director.canGrab;
						if (flag8)
						{
							this.Director.turnGrabOn();
						}
						else
						{
							bool flag9 = this.Hand.side == null && !WheelMenuSpell.right.isShown && this.Director.canGrab;
							if (flag9)
							{
								this.Director.turnGrabOff();
							}
						}
					}
				}
				bool flag10 = this.Hand.playerHand.controlHand.gripPressed && this.Hand.playerHand.controlHand.usePressed && !this.Set;
				if (flag10)
				{
					this.Set = true;
					bool flag11 = ExtensionMethods.IsGrabbingCreature(this.Hand) && this.Director.On && this.Director.HasCreatureActivate();
					if (flag11)
					{
						this.Director.CreatureActivate();
					}
					else
					{
						bool flag12 = ExtensionMethods.IsEmpty(this.Hand) && this.Director.On;
						if (flag12)
						{
							this.Director.Activate();
						}
					}
				}
				bool flag13 = this.Set && (!this.Hand.playerHand.controlHand.usePressed || !this.Hand.playerHand.controlHand.gripPressed);
				if (flag13)
				{
					this.Director.Deactivate();
					this.Set = false;
				}
				bool timer = this.Timer;
				if (timer)
				{
					this.CurrentTime -= Time.deltaTime;
					bool flag14 = this.CurrentTime <= 0f;
					if (flag14)
					{
						bool flag15 = this.tap1 && this.tap2 && this.Director.On && !this.Set && ExtensionMethods.IsEmpty(this.Hand);
						if (flag15)
						{
							this.Director.AltMode();
						}
						this.tap1 = false;
						this.tap2 = false;
						this.canTap2 = false;
						this.CurrentTime = this.TimerMax;
						this.Timer = false;
					}
				}
			}
		}

		// Token: 0x04000136 RID: 310
		private RagdollHand Hand;

		// Token: 0x04000137 RID: 311
		private RailMountsMono Director;

		// Token: 0x04000138 RID: 312
		private bool setup = false;

		// Token: 0x04000139 RID: 313
		private bool Set;

		// Token: 0x0400013A RID: 314
		private bool tap1;

		// Token: 0x0400013B RID: 315
		private bool canTap2;

		// Token: 0x0400013C RID: 316
		private bool tap2;

		// Token: 0x0400013D RID: 317
		private bool Timer;

		// Token: 0x0400013E RID: 318
		private float TimerMax = 0.2f;

		// Token: 0x0400013F RID: 319
		private float CurrentTime;
	}
}
