using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200003B RID: 59
	public class RailMountsMono : MonoBehaviour
	{
		// Token: 0x06000197 RID: 407 RVA: 0x0000B504 File Offset: 0x00009704
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.mode = RailMode.Undecided;
			this.canGrab = true;
			this.topHolder = this.item.GetCustomReference("TopHolder", true).GetComponent<Holder>();
			this.bottomHolder = this.item.GetCustomReference("BottomHolder", true).GetComponent<Holder>();
			this.topHolder.Snapped += new Holder.HolderDelegate(this.TopHolder_Snapped);
			this.topHolder.UnSnapped += new Holder.HolderDelegate(this.TopHolder_UnSnapped);
			this.bottomHolder.Snapped += new Holder.HolderDelegate(this.BottomHolder_Snapped);
			this.bottomHolder.UnSnapped += new Holder.HolderDelegate(this.BottomHolder_UnSnapped);
			RailHandWatcher[] checker = this.item.GetComponents<RailHandWatcher>();
			foreach (RailHandWatcher watcher in checker)
			{
				Object.Destroy(watcher);
			}
			this.item.OnSnapEvent += new Item.HolderDelegate(this.Item_OnSnap);
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.Item_OnUnSnap);
			this.GetSavedValues();
			bool flag = this.item.holder != null;
			if (flag)
			{
				this.Item_OnSnap(this.item.holder);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B650 File Offset: 0x00009850
		public void GetSavedValues()
		{
			this.item.TryGetCustomData<RailMountSave>(ref this.customData);
			bool flag = this.customData != null;
			if (flag)
			{
				bool onOff = this.customData.OnOff;
				if (onOff)
				{
					this.On = true;
				}
				else
				{
					this.On = false;
				}
				this.mode = this.customData.railMode;
				bool flag2 = this.customData.topItem != null;
				if (flag2)
				{
					Catalog.GetData<ItemData>(this.customData.topItem, true).SpawnAsync(delegate(Item sitem)
					{
						this.topHolder.Snap(sitem, true, true);
					}, new Vector3?(this.item.transform.position), new Quaternion?(this.item.transform.rotation), null, true, null);
				}
				bool flag3 = this.customData.bottomItem != null;
				if (flag3)
				{
					Catalog.GetData<ItemData>(this.customData.bottomItem, true).SpawnAsync(delegate(Item sitem)
					{
						this.bottomHolder.Snap(sitem, true, true);
					}, new Vector3?(this.item.transform.position), new Quaternion?(this.item.transform.rotation), null, true, null);
				}
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag4 = this.topModule != null;
						if (flag4)
						{
							this.topModule.Off();
						}
						bool flag5 = this.bottomModule != null;
						if (flag5)
						{
							this.bottomModule.On();
						}
					}
				}
				else
				{
					bool flag6 = this.topModule != null;
					if (flag6)
					{
						this.topModule.On();
					}
					bool flag7 = this.bottomModule != null;
					if (flag7)
					{
						this.bottomModule.Off();
					}
				}
			}
			else
			{
				this.customData = new RailMountSave();
				this.customData.OnOff = true;
				this.On = true;
				this.customData.topItem = null;
				this.customData.bottomItem = null;
				this.customData.railMode = RailMode.Undecided;
				this.mode = RailMode.Undecided;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B871 File Offset: 0x00009A71
		public void SaveData()
		{
			this.item.RemoveCustomData<RailMountSave>();
			this.item.AddCustomData<RailMountSave>(this.customData);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B894 File Offset: 0x00009A94
		public void SaveAllValues()
		{
			bool on = this.On;
			if (on)
			{
				this.customData.OnOff = true;
			}
			else
			{
				this.customData.OnOff = false;
			}
			bool flag = this.mode == RailMode.Top;
			if (flag)
			{
				this.customData.railMode = RailMode.Top;
			}
			else
			{
				bool flag2 = this.mode == RailMode.Bottom;
				if (flag2)
				{
					this.customData.railMode = RailMode.Bottom;
				}
				else
				{
					this.customData.railMode = RailMode.Undecided;
				}
			}
			bool flag3 = this.topModule != null;
			if (flag3)
			{
				this.customData.topItem = this.topModule.item.data.id;
			}
			bool flag4 = this.bottomModule != null;
			if (flag4)
			{
				this.customData.bottomItem = this.bottomModule.item.data.id;
			}
			this.SaveData();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B988 File Offset: 0x00009B88
		public void SaveMode()
		{
			bool flag = this.mode == RailMode.Top;
			if (flag)
			{
				this.customData.railMode = RailMode.Top;
			}
			else
			{
				bool flag2 = this.mode == RailMode.Bottom;
				if (flag2)
				{
					this.customData.railMode = RailMode.Bottom;
				}
				else
				{
					this.customData.railMode = RailMode.Undecided;
				}
			}
			this.SaveData();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		public void SaveOnOff()
		{
			bool on = this.On;
			if (on)
			{
				this.customData.OnOff = true;
			}
			else
			{
				this.customData.OnOff = false;
			}
			this.SaveData();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000BA28 File Offset: 0x00009C28
		public void SaveTopModule()
		{
			bool flag = this.topModule != null;
			if (flag)
			{
				RailMountSave railMountSave = this.customData;
				Item item = this.topModule.item;
				string text;
				if (item == null)
				{
					text = null;
				}
				else
				{
					ItemData data = item.data;
					text = ((data != null) ? data.id : null);
				}
				railMountSave.topItem = text;
			}
			this.SaveData();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000BA80 File Offset: 0x00009C80
		public void SaveBottomModule()
		{
			bool flag = this.bottomModule != null;
			if (flag)
			{
				RailMountSave railMountSave = this.customData;
				Item item = this.bottomModule.item;
				string text;
				if (item == null)
				{
					text = null;
				}
				else
				{
					ItemData data = item.data;
					text = ((data != null) ? data.id : null);
				}
				railMountSave.bottomItem = text;
			}
			this.SaveData();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		public void TriggerAction()
		{
			bool on = this.On;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag = !this.bottomModule.Activated && this.topModule != null;
						if (flag)
						{
							this.bottomModule.Off();
							this.topModule.On();
							this.mode = RailMode.Top;
						}
						this.Hand.playerHand.controlHand.HapticShort(1f);
					}
				}
				else
				{
					bool flag2 = !this.topModule.Activated && this.bottomModule != null;
					if (flag2)
					{
						this.topModule.Off();
						this.bottomModule.On();
						this.mode = RailMode.Bottom;
					}
					this.Hand.playerHand.controlHand.HapticShort(1f);
				}
				this.SaveMode();
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		public void HoldAction()
		{
			bool on = this.On;
			if (on)
			{
				this.On = false;
				bool flag = this.topModule;
				if (flag)
				{
					this.topModule.Off();
				}
				bool flag2 = this.bottomModule;
				if (flag2)
				{
					this.bottomModule.Off();
				}
				this.Hand.playerHand.controlHand.HapticShort(1f);
			}
			else
			{
				this.On = true;
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						this.bottomModule.On();
					}
				}
				else
				{
					this.topModule.On();
				}
				this.Hand.playerHand.controlHand.HapticShort(1f);
			}
			this.SaveOnOff();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000BCAC File Offset: 0x00009EAC
		public void Activate()
		{
			bool on = this.On;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag = this.bottomModule;
						if (flag)
						{
							bool flag2 = !this.bottomModule.Activated;
							if (flag2)
							{
								ArmModule armModule = this.bottomModule;
								if (armModule != null)
								{
									armModule.Activate();
								}
							}
						}
					}
				}
				else
				{
					bool flag3 = this.topModule;
					if (flag3)
					{
						bool flag4 = !this.topModule.Activated;
						if (flag4)
						{
							ArmModule armModule2 = this.topModule;
							if (armModule2 != null)
							{
								armModule2.Activate();
							}
						}
					}
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000BD58 File Offset: 0x00009F58
		public void Deactivate()
		{
			bool on = this.On;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag = this.bottomModule;
						if (flag)
						{
							bool flag2 = this.bottomModule.Activated && this.bottomModule.useDeactivate;
							if (flag2)
							{
								ArmModule armModule = this.bottomModule;
								if (armModule != null)
								{
									armModule.Deactivate();
								}
							}
						}
					}
				}
				else
				{
					bool flag3 = this.topModule;
					if (flag3)
					{
						bool flag4 = this.topModule.Activated && this.topModule.useDeactivate;
						if (flag4)
						{
							ArmModule armModule2 = this.topModule;
							if (armModule2 != null)
							{
								armModule2.Deactivate();
							}
						}
					}
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000BE20 File Offset: 0x0000A020
		public void AltMode()
		{
			bool on = this.On;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag = this.bottomModule;
						if (flag)
						{
							bool hasAltMode = this.bottomModule.HasAltMode;
							if (hasAltMode)
							{
								this.Hand.playerHand.controlHand.HapticShort(2f);
								ArmModule armModule = this.bottomModule;
								if (armModule != null)
								{
									armModule.AltMode();
								}
							}
						}
					}
				}
				else
				{
					bool flag2 = this.topModule;
					if (flag2)
					{
						bool hasAltMode2 = this.topModule.HasAltMode;
						if (hasAltMode2)
						{
							this.Hand.playerHand.controlHand.HapticShort(2f);
							ArmModule armModule2 = this.topModule;
							if (armModule2 != null)
							{
								armModule2.AltMode();
							}
						}
					}
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000BF00 File Offset: 0x0000A100
		public bool HasCreatureActivate()
		{
			bool on = this.On;
			bool flag;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					flag = railMode2 == RailMode.Bottom && this.bottomModule.HasCreatureActivate;
				}
				else
				{
					flag = this.topModule.HasCreatureActivate;
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000BF54 File Offset: 0x0000A154
		public void CreatureActivate()
		{
			bool on = this.On;
			if (on)
			{
				RailMode railMode = this.mode;
				RailMode railMode2 = railMode;
				if (railMode2 != RailMode.Top)
				{
					if (railMode2 == RailMode.Bottom)
					{
						bool flag = this.bottomModule;
						if (flag)
						{
							ArmModule armModule = this.bottomModule;
							if (armModule != null)
							{
								armModule.CreatureActivate();
							}
						}
					}
				}
				else
				{
					bool flag2 = this.topModule;
					if (flag2)
					{
						ArmModule armModule2 = this.topModule;
						if (armModule2 != null)
						{
							armModule2.CreatureActivate();
						}
					}
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000BFD0 File Offset: 0x0000A1D0
		private void Item_OnSnap(Holder holder)
		{
			holder.SendMessage("GrabOff");
			this.canGrab = false;
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Hand = Player.currentCreature.handRight;
				this.handWatcher = this.item.gameObject.AddComponent<RailHandWatcher>();
				this.handWatcher.Setup(this.Hand, this);
			}
			else
			{
				this.Hand = Player.currentCreature.handLeft;
				this.handWatcher = this.item.gameObject.AddComponent<RailHandWatcher>();
				this.handWatcher.Setup(this.Hand, this);
			}
			bool flag2 = this.topModule != null;
			if (flag2)
			{
				this.topModule.giveHand(this.Hand);
			}
			bool flag3 = this.bottomModule != null;
			if (flag3)
			{
				this.bottomModule.giveHand(this.Hand);
			}
			this.SaveAllValues();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public RagdollHand GetHand()
		{
			bool flag = this.Hand != null;
			RagdollHand ragdollHand;
			if (flag)
			{
				ragdollHand = this.Hand;
			}
			else
			{
				ragdollHand = null;
			}
			return ragdollHand;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C104 File Offset: 0x0000A304
		private void Item_OnUnSnap(Holder holder)
		{
			holder.SendMessage("GrabOn");
			this.canGrab = true;
			bool flag = this.topModule != null;
			if (flag)
			{
				this.topModule.OnUnSnapEvent(this.topHolder);
			}
			bool flag2 = this.bottomModule != null;
			if (flag2)
			{
				this.bottomModule.OnUnSnapEvent(this.bottomHolder);
			}
			this.handWatcher.Delete();
			this.handWatcher = null;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C180 File Offset: 0x0000A380
		private void TopHolder_Snapped(Item Nitem)
		{
			this.topModule = Nitem.gameObject.GetComponent<ArmModule>();
			bool flag = this.Hand != null;
			if (flag)
			{
				this.topModule.giveHand(this.Hand);
			}
			this.SaveTopModule();
			bool flag2 = this.mode == RailMode.Undecided;
			if (flag2)
			{
				this.mode = RailMode.Top;
				bool on = this.On;
				if (on)
				{
					this.topModule.On();
				}
				else
				{
					this.topModule.Off();
				}
			}
			else
			{
				bool flag3 = this.mode == RailMode.Bottom;
				if (flag3)
				{
					this.topModule.Off();
				}
				else
				{
					bool on2 = this.On;
					if (on2)
					{
						this.topModule.On();
					}
					else
					{
						this.topModule.Off();
					}
				}
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000C254 File Offset: 0x0000A454
		private void TopHolder_UnSnapped(Item Nitem)
		{
			this.topModule = null;
			this.customData.topItem = null;
			this.SaveData();
			bool flag = this.mode == RailMode.Top && this.bottomModule != null;
			if (flag)
			{
				this.mode = RailMode.Bottom;
				bool on = this.On;
				if (on)
				{
					this.bottomModule.On();
				}
			}
			else
			{
				bool flag2 = this.mode == RailMode.Bottom;
				if (!flag2)
				{
					this.mode = RailMode.Undecided;
				}
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		private void BottomHolder_Snapped(Item Nitem)
		{
			this.bottomModule = Nitem.gameObject.GetComponent<ArmModule>();
			bool flag = this.Hand != null;
			if (flag)
			{
				this.bottomModule.giveHand(this.Hand);
			}
			this.SaveBottomModule();
			bool flag2 = this.mode == RailMode.Undecided;
			if (flag2)
			{
				this.mode = RailMode.Bottom;
				bool on = this.On;
				if (on)
				{
					this.bottomModule.On();
				}
				else
				{
					this.bottomModule.Off();
				}
			}
			else
			{
				bool flag3 = this.mode == RailMode.Top;
				if (flag3)
				{
					this.bottomModule.Off();
				}
				else
				{
					bool on2 = this.On;
					if (on2)
					{
						this.bottomModule.On();
					}
					else
					{
						this.bottomModule.Off();
					}
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		private void BottomHolder_UnSnapped(Item Nitem)
		{
			this.bottomModule = null;
			this.customData.bottomItem = null;
			this.SaveData();
			bool flag = this.mode == RailMode.Bottom && this.topModule != null;
			if (flag)
			{
				this.mode = RailMode.Top;
				bool on = this.On;
				if (on)
				{
					this.topModule.On();
				}
			}
			else
			{
				bool flag2 = this.mode == RailMode.Top;
				if (!flag2)
				{
					this.mode = RailMode.Undecided;
				}
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000C430 File Offset: 0x0000A630
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

		// Token: 0x060001AE RID: 430 RVA: 0x0000C478 File Offset: 0x0000A678
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

		// Token: 0x060001AF RID: 431 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
		public void RemoveWatcher()
		{
			this.handWatcher.Delete();
			this.handWatcher = null;
		}

		// Token: 0x0400012B RID: 299
		private Item item;

		// Token: 0x0400012C RID: 300
		private RailHandWatcher handWatcher;

		// Token: 0x0400012D RID: 301
		private RagdollHand Hand;

		// Token: 0x0400012E RID: 302
		private RailMountSave customData;

		// Token: 0x0400012F RID: 303
		private Holder topHolder;

		// Token: 0x04000130 RID: 304
		private Holder bottomHolder;

		// Token: 0x04000131 RID: 305
		private ArmModule topModule;

		// Token: 0x04000132 RID: 306
		private ArmModule bottomModule;

		// Token: 0x04000133 RID: 307
		private RailMode mode;

		// Token: 0x04000134 RID: 308
		public bool On;

		// Token: 0x04000135 RID: 309
		public bool canGrab;
	}
}
