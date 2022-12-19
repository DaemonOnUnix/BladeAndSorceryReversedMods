using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000022 RID: 34
	public class CrossbowModule : MonoBehaviour
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00007974 File Offset: 0x00005B74
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.holder = this.item.childHolders[0];
			this.ModuleHolder = this.item.childHolders[1];
			this.anim = this.item.gameObject.GetComponentInChildren<Animator>();
			this.NormalBoltAmount = this.NormalBoltMax;
			this.SleepDartAmount = this.SleepDartMax;
			this.StingBoltAmount = this.StingBoltMax;
			this.canShoot = false;
			this.item.TryGetCustomData<CrossbowSave>(ref this.customData);
			bool flag = this.customData != null;
			if (flag)
			{
				bool flag2 = this.customData.AmmoTypes != null;
				if (flag2)
				{
					this.AmmoTypes = this.customData.AmmoTypes;
					this.AmmoSelector = -1;
				}
			}
			else
			{
				this.AmmoTypes = new List<string>();
				this.AmmoTypes.Add(this.NormalBolt);
				this.AmmoSelector = -1;
				this.customData = new CrossbowSave();
			}
			this.State = BoltType.Start;
			this.PrevState = BoltType.Start;
			this.BoltRest = this.item.GetCustomReference("BoltRest", true);
			this.BoltSpawn = this.item.GetCustomReference("BoltSpawn", true);
			this.loadFirstHolder();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnSnapEvent += new Item.HolderDelegate(this.Item_OnSnapEvent);
			this.ModuleHolder.Snapped += new Holder.HolderDelegate(this.ModuleHolder_Snapped);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007B08 File Offset: 0x00005D08
		private void Item_OnSnapEvent(Holder holder)
		{
			this.customData.AmmoTypes = this.AmmoTypes;
			this.item.RemoveCustomData<CrossbowSave>();
			this.item.AddCustomData<CrossbowSave>(this.customData);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007B3C File Offset: 0x00005D3C
		private void ModuleHolder_Snapped(Item input)
		{
			AmmoModuleMono tempModule = input.gameObject.GetComponent<AmmoModuleMono>();
			bool flag = tempModule != null;
			if (flag)
			{
				string AmmoType = tempModule.BoltID;
				bool flag2 = !this.AmmoTypes.Contains(AmmoType);
				if (flag2)
				{
					this.AmmoTypes.Add(AmmoType);
				}
				else
				{
					string text = AmmoType;
					string text2 = text;
					if (!(text2 == "GrooveSlinger.Dishonored.Bolt"))
					{
						if (!(text2 == "GrooveSlinger.Dishonored.SleepDart"))
						{
							if (text2 == "GrooveSlinger.Dishonored.StingBolt")
							{
								this.StingBoltAmount = this.StingBoltMax;
							}
						}
						else
						{
							this.SleepDartAmount = this.SleepDartMax;
						}
					}
					else
					{
						this.NormalBoltAmount = this.NormalBoltMax;
					}
					bool flag3 = this.AmmoTypes[this.AmmoSelector] == AmmoType;
					if (flag3)
					{
						this.ReloadHolder(this.AmmoTypes[this.AmmoSelector]);
					}
				}
			}
			input.holder.UnSnap(input, true, true);
			input.Despawn();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007C44 File Offset: 0x00005E44
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == null && this.canShoot;
			if (flag)
			{
				this.canShoot = false;
				bool flag2 = this.holder.currentQuantity > 0;
				if (flag2)
				{
					this.ShootBolt();
					base.StartCoroutine(this.RTF(0.75f));
					base.StartCoroutine(this.ReloadSound(0.133f));
					this.anim.SetTrigger("Fire");
				}
				else
				{
					bool flag3 = this.bolt == null && this.holder.currentQuantity <= 0;
					if (flag3)
					{
						this.anim.SetTrigger("HalfFire");
						Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredFire", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
					}
					else
					{
						this.ShootBolt();
						this.anim.SetTrigger("HalfFire");
					}
				}
				this.item.mainHandler.playerHand.controlHand.HapticShort(5f);
			}
			else
			{
				bool flag4 = action == 2;
				if (flag4)
				{
					this.item.mainHandler.playerHand.controlHand.HapticShort(5f);
					bool flag5 = this.State == BoltType.Start && this.bolt == null && this.holder.currentQuantity > 0;
					if (flag5)
					{
						this.LoadFirstArrow();
						this.State = BoltType.Loaded;
						this.anim.SetTrigger("Reload");
					}
					else
					{
						bool flag6 = this.State == BoltType.Unloaded && this.bolt == null && this.holder.currentQuantity > 0;
						if (flag6)
						{
							base.StartCoroutine(this.ReloadSound(0f));
							base.StartCoroutine(this.LoadBolt(0.517f));
							base.StartCoroutine(this.RTF(0.517f));
							this.State = this.PrevState;
							this.anim.SetTrigger("Reload");
							this.PrevState = BoltType.Unloaded;
						}
						else
						{
							bool flag7 = (this.canShoot || this.State == BoltType.Unloaded) && this.AmmoTypes.Count > 1;
							if (flag7)
							{
								this.canShoot = false;
								bool flag8 = (this.anim.GetCurrentAnimatorStateInfo(0).IsName("HalfBase1") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("HalfBase2")) && this.State == BoltType.Unloaded;
								if (flag8)
								{
									this.State = this.PrevState;
									this.BoltSwap(true);
								}
								else
								{
									this.BoltSwap(false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007F0C File Offset: 0x0000610C
		private void ShootBolt()
		{
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredFire", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
			Object.Destroy(this.fj);
			foreach (ColliderGroup g in this.bolt.colliderGroups)
			{
				g.gameObject.SetActive(true);
			}
			this.bolt.rb.useGravity = true;
			this.bolt.disallowDespawn = false;
			this.bolt.Throw(1f, 2);
			this.bolt.rb.AddForce(this.BoltRest.forward * 40f, 1);
			this.bolt.RefreshCollision(true);
			this.bolt.IgnoreObjectCollision(this.item);
			foreach (Handle h in this.bolt.handles)
			{
				h.data.allowTelekinesis = true;
				h.data.disableTouch = false;
			}
			this.bolt = null;
			bool forceAutoSpawnArrow = ItemModuleBow.forceAutoSpawnArrow;
			if (forceAutoSpawnArrow)
			{
				base.StartCoroutine(this.LoadInfiniteBolt(0.65f));
			}
			else
			{
				bool flag = this.holder.currentQuantity > 0;
				if (flag)
				{
					base.StartCoroutine(this.LoadBolt(0.65f));
				}
				else
				{
					this.PrevState = this.State;
					this.State = BoltType.Unloaded;
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000080DC File Offset: 0x000062DC
		private IEnumerator LoadBolt(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.bolt = this.holder.UnSnapOne();
			bool flag = this.bolt != null;
			if (flag)
			{
				this.bolt.transform.position = this.BoltRest.position;
				this.bolt.transform.rotation = this.BoltRest.rotation;
				this.bolt.rb.useGravity = false;
				this.bolt.disallowDespawn = true;
				this.bolt.IgnoreObjectCollision(this.item);
				foreach (ColliderGroup g in this.bolt.colliderGroups)
				{
					g.gameObject.SetActive(false);
					g = null;
				}
				List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
				foreach (Handle h in this.bolt.handles)
				{
					h.data.allowTelekinesis = false;
					h.data.disableTouch = true;
					h = null;
				}
				List<Handle>.Enumerator enumerator2 = default(List<Handle>.Enumerator);
				this.fj = this.item.rb.gameObject.AddComponent<FixedJoint>();
				this.fj.connectedBody = this.bolt.rb;
				this.fj.enableCollision = false;
				this.fj.breakForce = float.PositiveInfinity;
				this.fj.breakTorque = float.PositiveInfinity;
			}
			else
			{
				Debug.Log("Why Bolt null?");
			}
			yield break;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000080F2 File Offset: 0x000062F2
		private IEnumerator LoadInfiniteBolt(float delay)
		{
			yield return new WaitForSeconds(delay);
			string boltID = this.holder.items[0].itemId;
			Catalog.GetData<ItemData>(boltID, true).SpawnAsync(delegate(Item temp)
			{
				this.bolt = temp;
				this.bolt.transform.position = this.BoltRest.position;
				this.bolt.transform.rotation = this.BoltRest.rotation;
				this.bolt.rb.useGravity = false;
				this.bolt.disallowDespawn = true;
				this.bolt.IgnoreObjectCollision(this.item);
				foreach (ColliderGroup g in this.bolt.colliderGroups)
				{
					g.gameObject.SetActive(false);
				}
				foreach (Handle h in this.bolt.handles)
				{
					h.data.allowTelekinesis = false;
					h.data.disableTouch = true;
				}
				this.fj = this.item.rb.gameObject.AddComponent<FixedJoint>();
				this.fj.connectedBody = this.bolt.rb;
				this.fj.enableCollision = false;
				this.fj.breakForce = float.PositiveInfinity;
				this.fj.breakTorque = float.PositiveInfinity;
			}, new Vector3?(this.BoltRest.position), new Quaternion?(this.BoltRest.rotation), null, true, null);
			yield break;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00008108 File Offset: 0x00006308
		public IEnumerator LoadArrowFast(float delay)
		{
			yield return new WaitForSeconds(delay);
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredEquip", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
			bool flag = this.holder.currentQuantity > 0;
			if (flag)
			{
				this.bolt = this.holder.UnSnapOne();
				this.bolt.transform.position = this.BoltRest.position;
				this.bolt.transform.rotation = this.BoltRest.rotation;
				this.bolt.rb.useGravity = false;
				this.bolt.disallowDespawn = true;
				this.bolt.IgnoreObjectCollision(this.item);
				foreach (ColliderGroup g in this.bolt.colliderGroups)
				{
					g.gameObject.SetActive(false);
					g = null;
				}
				List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
				foreach (Handle h in this.bolt.handles)
				{
					h.data.allowTelekinesis = false;
					h.data.disableTouch = true;
					h = null;
				}
				List<Handle>.Enumerator enumerator2 = default(List<Handle>.Enumerator);
				this.fj = this.item.rb.gameObject.AddComponent<FixedJoint>();
				this.fj.connectedBody = this.bolt.rb;
				this.fj.enableCollision = false;
				this.fj.breakForce = float.PositiveInfinity;
				this.fj.breakTorque = float.PositiveInfinity;
				this.canShoot = true;
			}
			yield break;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008120 File Offset: 0x00006320
		public void LoadFirstArrow()
		{
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredEquip", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
			this.AmmoSelector = 0;
			bool flag = this.holder.currentQuantity > 0;
			if (flag)
			{
				this.bolt = this.holder.UnSnapOne();
				this.bolt.transform.position = this.BoltRest.position;
				this.bolt.transform.rotation = this.BoltRest.rotation;
				this.bolt.disallowDespawn = true;
				this.bolt.rb.useGravity = false;
				this.bolt.IgnoreObjectCollision(this.item);
				foreach (ColliderGroup g in this.bolt.colliderGroups)
				{
					g.gameObject.SetActive(false);
				}
				foreach (Handle h in this.bolt.handles)
				{
					h.data.allowTelekinesis = false;
					h.data.disableTouch = true;
				}
				this.fj = this.item.rb.gameObject.AddComponent<FixedJoint>();
				this.fj.connectedBody = this.bolt.rb;
				this.fj.enableCollision = false;
				this.fj.breakForce = float.PositiveInfinity;
				this.fj.breakTorque = float.PositiveInfinity;
				this.canShoot = true;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008308 File Offset: 0x00006508
		public void loadFirstHolder()
		{
			HolderData newData = this.item.childHolders[0].data;
			newData.spawnItemID = this.NormalBolt;
			newData.maxQuantity = this.NormalBoltMax;
			newData.highlightDefaultTitle = "Bolt";
			newData.spawnQuantity = this.NormalBoltMax;
			newData.slots[0] = this.NormalBolt + ".Slot";
			this.holder.Load(newData);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008388 File Offset: 0x00006588
		public void ReloadHolder(string itemID)
		{
			HolderData newData = this.item.childHolders[0].data;
			newData.spawnItemID = itemID;
			int maxQ;
			int spawnQ;
			string highlight;
			if (!(itemID == "GrooveSlinger.Dishonored.Bolt"))
			{
				if (!(itemID == "GrooveSlinger.Dishonored.SleepDart"))
				{
					if (!(itemID == "GrooveSlinger.Dishonored.StingBolt"))
					{
						maxQ = 1;
						spawnQ = 0;
						highlight = "Error";
					}
					else
					{
						maxQ = this.StingBoltMax;
						spawnQ = this.StingBoltAmount;
						highlight = "Sting Bolt";
					}
				}
				else
				{
					maxQ = this.SleepDartMax;
					spawnQ = this.SleepDartAmount;
					highlight = "Sleep Dart";
				}
			}
			else
			{
				maxQ = this.NormalBoltMax;
				spawnQ = this.NormalBoltAmount;
				highlight = "Bolt";
			}
			newData.maxQuantity = maxQ;
			newData.spawnQuantity = spawnQ;
			newData.highlightDefaultTitle = highlight;
			this.holder.Load(newData);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008457 File Offset: 0x00006657
		public IEnumerator ReloadSound(float delay)
		{
			yield return new WaitForSeconds(delay);
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredReload", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000846D File Offset: 0x0000666D
		public IEnumerator EquipSound(float delay)
		{
			yield return new WaitForSeconds(delay);
			Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredEquip", true).Spawn(this.BoltSpawn, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008483 File Offset: 0x00006683
		public IEnumerator RTF(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.canShoot = true;
			yield break;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000849C File Offset: 0x0000669C
		public void BoltSwap(bool empty)
		{
			bool flag = this.bolt != null;
			if (flag)
			{
				Object.Destroy(this.fj);
				this.bolt.rb.useGravity = true;
				this.bolt.disallowDespawn = false;
				this.bolt.mainHandleRight.data.allowTelekinesis = true;
				this.bolt.mainHandleRight.data.disableTouch = false;
				this.holder.Snap(this.bolt, true, true);
				this.bolt = null;
			}
			string text = this.AmmoTypes[this.AmmoSelector];
			string text2 = text;
			if (!(text2 == "GrooveSlinger.Dishonored.Bolt"))
			{
				if (!(text2 == "GrooveSlinger.Dishonored.SleepDart"))
				{
					if (text2 == "GrooveSlinger.Dishonored.StingBolt")
					{
						this.StingBoltAmount = this.holder.currentQuantity;
					}
				}
				else
				{
					this.SleepDartAmount = this.holder.currentQuantity;
				}
			}
			else
			{
				this.NormalBoltAmount = this.holder.currentQuantity;
			}
			bool flag2 = this.AmmoTypes.Count == this.AmmoSelector + 1;
			if (flag2)
			{
				this.AmmoSelector = 0;
			}
			else
			{
				this.AmmoSelector++;
			}
			string itemname = this.AmmoTypes[this.AmmoSelector];
			string text3 = this.AmmoTypes[this.AmmoSelector];
			string text4 = text3;
			int max;
			int amount;
			string title;
			if (!(text4 == "GrooveSlinger.Dishonored.Bolt"))
			{
				if (!(text4 == "GrooveSlinger.Dishonored.SleepDart"))
				{
					if (!(text4 == "GrooveSlinger.Dishonored.StingBolt"))
					{
						max = 1;
						amount = 0;
						title = "Error";
					}
					else
					{
						max = this.StingBoltMax;
						amount = this.StingBoltAmount;
						title = "Sting Bolt";
					}
				}
				else
				{
					max = this.SleepDartMax;
					amount = this.SleepDartAmount;
					title = "Sleep Dart";
				}
			}
			else
			{
				max = this.NormalBoltMax;
				amount = this.NormalBoltAmount;
				title = "Bolt";
			}
			this.ReLoadBolt(itemname, amount, max, title, empty);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008694 File Offset: 0x00006894
		public void ReLoadBolt(string itemname, int amount, int max, string title, bool empty)
		{
			HolderData newData = this.item.childHolders[0].data;
			newData.spawnItemID = itemname;
			newData.maxQuantity = max;
			newData.highlightDefaultTitle = title;
			newData.slots[0] = itemname + ".Slot";
			bool flag = amount > 0;
			if (flag)
			{
				newData.spawnQuantity = amount;
				if (empty)
				{
					this.anim.SetTrigger("Reload");
					base.StartCoroutine(this.ReloadSound(0f));
					base.StartCoroutine(this.LoadBolt(0.517f));
					base.StartCoroutine(this.RTF(0.517f));
				}
				else
				{
					this.anim.SetTrigger("Reload");
					base.StartCoroutine(this.LoadArrowFast(0.1f));
				}
				this.holder.Load(newData);
			}
			else
			{
				if (empty)
				{
					this.anim.SetTrigger("HalfReload");
					base.StartCoroutine(this.EquipSound(0.1f));
				}
				else
				{
					this.anim.SetTrigger("Reload");
					this.anim.SetTrigger("HalfFire");
					base.StartCoroutine(this.EquipSound(0.1f));
				}
				newData.spawnQuantity = 1;
				this.holder.Load(newData);
				foreach (Item im in this.holder.items.ToList<Item>())
				{
					this.holder.UnSnap(im, true, true);
					im.Despawn();
				}
				this.PrevState = this.State;
				this.State = BoltType.Unloaded;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008878 File Offset: 0x00006A78
		public void Update()
		{
		}

		// Token: 0x040000BB RID: 187
		private Item item;

		// Token: 0x040000BC RID: 188
		private Holder holder;

		// Token: 0x040000BD RID: 189
		private Holder ModuleHolder;

		// Token: 0x040000BE RID: 190
		private Animator anim;

		// Token: 0x040000BF RID: 191
		private CrossbowSave customData;

		// Token: 0x040000C0 RID: 192
		private BoltType State;

		// Token: 0x040000C1 RID: 193
		private BoltType PrevState;

		// Token: 0x040000C2 RID: 194
		private Item bolt;

		// Token: 0x040000C3 RID: 195
		private FixedJoint fj;

		// Token: 0x040000C4 RID: 196
		private string NormalBolt = "GrooveSlinger.Dishonored.Bolt";

		// Token: 0x040000C5 RID: 197
		private int NormalBoltMax = BoltAmount.NormalBoltMax;

		// Token: 0x040000C6 RID: 198
		private int NormalBoltAmount;

		// Token: 0x040000C7 RID: 199
		private string SleepDart = "GrooveSlinger.Dishonored.SleepDart";

		// Token: 0x040000C8 RID: 200
		private int SleepDartMax = BoltAmount.SleepDartMax;

		// Token: 0x040000C9 RID: 201
		private int SleepDartAmount;

		// Token: 0x040000CA RID: 202
		private string StingBolt = "GrooveSlinger.Dishonored.StingBolt";

		// Token: 0x040000CB RID: 203
		private int StingBoltMax = BoltAmount.StingBoltMax;

		// Token: 0x040000CC RID: 204
		private int StingBoltAmount;

		// Token: 0x040000CD RID: 205
		private List<string> AmmoTypes;

		// Token: 0x040000CE RID: 206
		private int AmmoSelector;

		// Token: 0x040000CF RID: 207
		private bool canShoot;

		// Token: 0x040000D0 RID: 208
		private Transform BoltRest;

		// Token: 0x040000D1 RID: 209
		private Transform BoltSpawn;
	}
}
