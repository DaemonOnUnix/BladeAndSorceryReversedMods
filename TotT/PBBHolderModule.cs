using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200001A RID: 26
	public class PBBHolderModule : MonoBehaviour
	{
		// Token: 0x060000BD RID: 189 RVA: 0x0000660C File Offset: 0x0000480C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.AmmoHolder = this.item.childHolders[0];
			this.AmmoLoader = this.item.childHolders[1];
			this.isHolstered = false;
			this.item.OnSnapEvent += new Item.HolderDelegate(this.Item_OnSnapEvent);
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.Item_OnUnSnapEvent);
			this.AmmoLoader.Snapped += new Holder.HolderDelegate(this.AmmoLoader_Snapped);
			this.AmmoType = new List<string>();
			this.AmmoName = new List<string>();
			this.AmmoMax = new List<int>();
			this.AmmoCurrent = new List<int>();
			this.AmmoSelector = -1;
			this.LoadValues();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000066E0 File Offset: 0x000048E0
		private void Item_OnSnapEvent(Holder holder)
		{
			this.AmmoLoader.gameObject.SetActive(false);
			Player.local.handLeft.controlHand.OnButtonPressEvent += new PlayerControl.Hand.ButtonEvent(this.LeftHand_OnButtonPressEvent);
			Player.local.handRight.controlHand.OnButtonPressEvent += new PlayerControl.Hand.ButtonEvent(this.RightHand_OnButtonPressEvent);
			this.isHolstered = true;
			this.SaveValues();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006750 File Offset: 0x00004950
		private void RightHand_OnButtonPressEvent(PlayerControl.Hand.Button button, bool pressed)
		{
			float dis = Mathf.Abs(Vector3.Distance(Player.local.handRight.gameObject.transform.position, this.item.holder.transform.position));
			bool flag = this.isHolstered && button == 1 && pressed && dis <= this.item.holder.touchRadius * 1.5f;
			if (flag)
			{
				bool flag2 = this.AmmoType.Count <= 1;
				if (!flag2)
				{
					Player.local.handRight.controlHand.HapticShort(2f);
					bool flag3 = this.AmmoSelector == this.AmmoType.Count - 1;
					int newSelection;
					if (flag3)
					{
						newSelection = 0;
					}
					else
					{
						newSelection = this.AmmoSelector + 1;
					}
					this.LoadHolder(newSelection);
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006838 File Offset: 0x00004A38
		private void LeftHand_OnButtonPressEvent(PlayerControl.Hand.Button button, bool pressed)
		{
			float dis = Mathf.Abs(Vector3.Distance(Player.local.handLeft.gameObject.transform.position, this.item.holder.transform.position));
			bool flag = this.isHolstered && button == 1 && pressed && dis <= this.item.holder.touchRadius * 1.5f;
			if (flag)
			{
				bool flag2 = this.AmmoType.Count <= 1;
				if (!flag2)
				{
					Player.local.handLeft.controlHand.HapticShort(2f);
					bool flag3 = this.AmmoSelector == this.AmmoType.Count - 1;
					int newSelection;
					if (flag3)
					{
						newSelection = 0;
					}
					else
					{
						newSelection = this.AmmoSelector + 1;
					}
					this.LoadHolder(newSelection);
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006920 File Offset: 0x00004B20
		private void Item_OnUnSnapEvent(Holder holder)
		{
			this.AmmoLoader.gameObject.SetActive(true);
			Player.local.handLeft.controlHand.OnButtonPressEvent -= new PlayerControl.Hand.ButtonEvent(this.LeftHand_OnButtonPressEvent);
			Player.local.handRight.controlHand.OnButtonPressEvent -= new PlayerControl.Hand.ButtonEvent(this.RightHand_OnButtonPressEvent);
			this.isHolstered = false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000698C File Offset: 0x00004B8C
		private void Item_OnTouchActionEvent(Interactable interactable, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				bool flag2 = this.AmmoType.Count <= 1;
				if (!flag2)
				{
					bool flag3 = this.AmmoSelector == this.AmmoType.Count - 1;
					int newSelection;
					if (flag3)
					{
						newSelection = 0;
					}
					else
					{
						newSelection = this.AmmoSelector + 1;
					}
					this.LoadHolder(newSelection);
				}
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000069F4 File Offset: 0x00004BF4
		private void AmmoLoader_Snapped(Item sitem)
		{
			PhantomBladeBolt check = sitem.gameObject.GetComponent<PhantomBladeBolt>();
			bool flag = check;
			if (flag)
			{
				string checkID = check.GetItemID();
				Debug.Log("TotT-Debug: Snapping item!");
				bool flag2 = this.AmmoSelector == -1;
				if (flag2)
				{
					Debug.Log("TotT-Debug: First Add!");
					this.AmmoLoader.UnSnap(sitem, true, true);
					this.AddNew(sitem);
				}
				else
				{
					bool flag3 = string.Equals(checkID, this.AmmoType[this.AmmoSelector]);
					if (flag3)
					{
						Debug.Log("TotT-Debug: Doing Nothing because it's currently selected (" + this.AmmoType[this.AmmoSelector] + " is the same as " + checkID);
						this.AmmoLoader.UnSnap(sitem, false, true);
					}
					else
					{
						bool flag4 = this.AmmoType.Contains(checkID);
						if (flag4)
						{
							for (int i = 0; i < this.AmmoType.Count; i++)
							{
								bool flag5 = string.Equals(checkID, this.AmmoType[i]) && this.AmmoCurrent[i] < this.AmmoMax[i];
								if (flag5)
								{
									Debug.Log("TotT-Debug: Added it to it's own Holder!");
									this.AmmoLoader.UnSnap(this.item, true, true);
									sitem.Despawn();
									List<int> ammoCurrent = this.AmmoCurrent;
									int num = i;
									int num2 = ammoCurrent[num];
									ammoCurrent[num] = num2 + 1;
								}
							}
						}
						else
						{
							Debug.Log("TotT-Debug: New Item! Adding new Holder!");
							this.AmmoLoader.UnSnap(sitem, true, true);
							this.AddNew(sitem);
						}
					}
				}
			}
			else
			{
				this.AmmoHolder.UnSnap(sitem, true, true);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006BBC File Offset: 0x00004DBC
		public void AddNew(Item sitem)
		{
			PhantomBladeBolt temp = sitem.GetComponent<PhantomBladeBolt>();
			bool flag = temp;
			if (flag)
			{
				this.AmmoType.Add(sitem.data.id);
				this.AmmoName.Add(sitem.data.displayName);
				this.AmmoMax.Add(temp.ammoMax);
				this.AmmoCurrent.Add(temp.ammoMax);
			}
			sitem.Despawn();
			bool flag2 = this.AmmoType.Count == 1 && this.AmmoSelector == -1;
			if (flag2)
			{
				Debug.Log("TotT-Debug: Added New and now Loading First Holder");
				this.LoadFirstHolder();
			}
			this.SaveValues();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006C70 File Offset: 0x00004E70
		public void LoadValues()
		{
			Debug.Log("TotT-Debug: Loading Holder...");
			this.item.TryGetCustomData<PBBHolderSave>(ref this.customData);
			bool flag = this.customData != null;
			if (flag)
			{
				bool flag2 = this.customData.AmmoType != null;
				if (flag2)
				{
					foreach (string i in this.customData.AmmoType)
					{
						this.AmmoType.Add(i);
						Debug.Log("TotT-Debug: Loading AmmoType " + i);
					}
				}
				bool flag3 = this.customData.AmmoName != null;
				if (flag3)
				{
					foreach (string j in this.customData.AmmoName)
					{
						Debug.Log("TotT-Debug: Loading AmmoName " + j);
						this.AmmoName.Add(j);
					}
				}
				bool flag4 = this.customData.AmmoMax != null;
				if (flag4)
				{
					foreach (int k in this.customData.AmmoMax)
					{
						Debug.Log(string.Format("TotT-Debug: Loading AmmoMax {0}", k));
						this.AmmoMax.Add(k);
						this.AmmoCurrent.Add(k);
					}
				}
				bool flag5 = this.AmmoType.Count != 0 && this.AmmoSelector == -1;
				if (flag5)
				{
					base.StartCoroutine(this.DelayedLoadFirstHolder());
				}
			}
			else
			{
				this.customData = new PBBHolderSave();
				this.customData.AmmoType = new List<string>();
				this.customData.AmmoName = new List<string>();
				this.customData.AmmoMax = new List<int>();
				base.StartCoroutine(this.DelayedLoadNormalBolts());
			}
			bool flag6 = this.item.holder != null;
			if (flag6)
			{
				this.Item_OnSnapEvent(this.item.holder);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006EDC File Offset: 0x000050DC
		public void SaveValues()
		{
			this.customData.AmmoType = new List<string>();
			this.customData.AmmoName = new List<string>();
			this.customData.AmmoMax = new List<int>();
			foreach (string i in this.AmmoType)
			{
				Debug.Log("TotT-Debug: Saving AmmoType " + i);
				this.customData.AmmoType.Add(i);
			}
			foreach (string j in this.AmmoName)
			{
				Debug.Log("TotT-Debug: Saving AmmoName " + j);
				this.customData.AmmoName.Add(j);
			}
			foreach (int k in this.AmmoMax)
			{
				Debug.Log(string.Format("TotT-Debug: Saving AmmoMax {0}", k));
				this.customData.AmmoMax.Add(k);
			}
			this.item.RemoveCustomData<PBBHolderSave>();
			this.item.AddCustomData<PBBHolderSave>(this.customData);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007070 File Offset: 0x00005270
		public void LoadFirstHolder()
		{
			HolderData newData = this.AmmoHolder.data;
			newData.spawnItemID = this.AmmoType[0];
			newData.maxQuantity = this.AmmoMax[0];
			newData.spawnQuantity = this.AmmoCurrent[0];
			newData.highlightDefaultTitle = this.AmmoName[0];
			this.AmmoHolder.Load(newData);
			this.AmmoSelector = 0;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000070E8 File Offset: 0x000052E8
		public void LoadHolder(int Selection)
		{
			this.AmmoCurrent[this.AmmoSelector] = this.AmmoHolder.currentQuantity;
			HolderData newData = this.AmmoHolder.data;
			newData.spawnItemID = this.AmmoType[Selection];
			newData.maxQuantity = this.AmmoMax[Selection];
			newData.spawnQuantity = this.AmmoCurrent[Selection];
			newData.highlightDefaultTitle = this.AmmoName[Selection];
			bool flag = this.AmmoCurrent[Selection] == 0;
			if (flag)
			{
				newData.spawnQuantity = 1;
				this.AmmoHolder.Load(newData);
				base.StartCoroutine(this.DelayDelete());
			}
			else
			{
				this.AmmoHolder.Load(newData);
			}
			this.AmmoSelector = Selection;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000071B5 File Offset: 0x000053B5
		public IEnumerator DelayDelete()
		{
			yield return new WaitForSeconds(0.1f);
			foreach (Item im in this.AmmoHolder.items.ToList<Item>())
			{
				this.AmmoHolder.UnSnap(im, true, true);
				im.Despawn();
				im = null;
			}
			List<Item>.Enumerator enumerator = default(List<Item>.Enumerator);
			yield break;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000071C4 File Offset: 0x000053C4
		public IEnumerator DelayedLoadFirstHolder()
		{
			yield return new WaitForSeconds(0.25f);
			this.LoadFirstHolder();
			yield break;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000071D3 File Offset: 0x000053D3
		public IEnumerator DelayedLoadNormalBolts()
		{
			yield return new WaitForSeconds(0.25f);
			Catalog.GetData<ItemData>("GrooveSlinger.TotT.PhantomBlade.Bolt", true).SpawnAsync(delegate(Item i)
			{
				this.AddNew(i);
			}, null, null, null, true, null);
			yield break;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000071E2 File Offset: 0x000053E2
		public void Update()
		{
		}

		// Token: 0x0400008B RID: 139
		private Item item;

		// Token: 0x0400008C RID: 140
		private Holder AmmoHolder;

		// Token: 0x0400008D RID: 141
		private Holder AmmoLoader;

		// Token: 0x0400008E RID: 142
		private PBBHolderSave customData;

		// Token: 0x0400008F RID: 143
		private int AmmoSelector;

		// Token: 0x04000090 RID: 144
		private List<string> AmmoType;

		// Token: 0x04000091 RID: 145
		private List<string> AmmoName;

		// Token: 0x04000092 RID: 146
		private List<int> AmmoMax;

		// Token: 0x04000093 RID: 147
		private List<int> AmmoCurrent;

		// Token: 0x04000094 RID: 148
		private bool isHolstered;
	}
}
