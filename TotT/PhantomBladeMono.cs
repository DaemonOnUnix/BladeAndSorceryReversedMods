using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200000E RID: 14
	public class PhantomBladeMono : ArmModule
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000045F8 File Offset: 0x000027F8
		public override void OnStart()
		{
			base.OnStart();
			this.anim = base.GetComponent<Animator>();
			this.HasAltMode = true;
			this.Indicator = this.item.GetCustomReference("Indicator", true).GetComponent<Renderer>();
			this.BoltRef = this.item.GetCustomReference("Bolt", true).gameObject;
			this.FirePoint = this.item.GetCustomReference("FirePoint", true);
			this.BoltRef.SetActive(false);
			this.CurrentColor = this.OnColor;
			this.BoltLoader = this.item.GetCustomReference("BoltLoader", true).gameObject;
			this.BoltLoader.AddComponent<BoltLoaderMono>();
			this.customData = new ArmModuleSave();
			ModuleHandWatcher[] checker = this.item.GetComponents<ModuleHandWatcher>();
			foreach (ModuleHandWatcher watcher in checker)
			{
				Object.Destroy(watcher);
			}
			this.item.TryGetCustomData<ArmModuleSave>(ref this.customData);
			bool flag = this.customData != null;
			if (flag)
			{
				bool onOff = this.customData.OnOff;
				if (onOff)
				{
					this.OnOff = true;
				}
				else
				{
					this.OnOff = false;
				}
			}
			else
			{
				this.customData = new ArmModuleSave();
			}
			this.OnOff = true;
			bool flag2 = this.item.holder != null;
			if (flag2)
			{
				this.OnSnapEvent(this.item.holder);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004770 File Offset: 0x00002970
		public void SaveData()
		{
			bool flag = this.customData != null;
			if (flag)
			{
				this.customData.OnOff = this.OnOff;
				this.item.RemoveCustomData<ArmModuleSave>();
				this.item.AddCustomData<ArmModuleSave>(this.customData);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000047C0 File Offset: 0x000029C0
		public override void OnUpdate()
		{
			bool flag = this.BoltLoader.layer != LayerMask.NameToLayer("Default");
			if (flag)
			{
				this.BoltLoader.layer = LayerMask.NameToLayer("Default");
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004804 File Offset: 0x00002A04
		public override void Activate()
		{
			this.Activated = true;
			bool flag = !this.AltModeOn;
			if (flag)
			{
				base.StartCoroutine(this.BladeOutAnim());
			}
			else
			{
				base.StartCoroutine(this.EquipSFX());
				base.StartCoroutine(this.CrossbowOutAnim());
			}
			base.Activate();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000485C File Offset: 0x00002A5C
		public override void AltMode()
		{
			bool altModeOn = this.AltModeOn;
			if (altModeOn)
			{
				this.AltModeOn = false;
				this.OnColor = this.OnColorGreen;
				base.StartCoroutine(this.ChangeColor(this.OnColor));
			}
			else
			{
				this.AltModeOn = true;
				this.OnColor = this.OnColorBlue;
				base.StartCoroutine(this.ChangeColor(this.OnColor));
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000048C6 File Offset: 0x00002AC6
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			this.SaveData();
			base.On();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000048F1 File Offset: 0x00002AF1
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			this.SaveData();
			base.Off();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000491C File Offset: 0x00002B1C
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			bool flag = this.bladeItem == null;
			if (flag)
			{
				this.Blade.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.bladeItem = sitem;
					this.bladeAnim = sitem.GetComponent<Animator>();
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			bool flag2 = this.controlItem == null;
			if (flag2)
			{
				this.Controller.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
					sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.controlItem = sitem;
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			base.giveHand(hand);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000049E8 File Offset: 0x00002BE8
		public override void OnUnSnapEvent(Holder holder)
		{
			base.OnUnSnapEvent(holder);
			RagdollHand hand = this.Hand;
			string text;
			if (hand == null)
			{
				text = null;
			}
			else
			{
				Handle grabbedHandle = hand.grabbedHandle;
				if (grabbedHandle == null)
				{
					text = null;
				}
				else
				{
					Item item = grabbedHandle.item;
					text = ((item != null) ? item.data.id : null);
				}
			}
			bool flag;
			if (!(text == "GrooveSlinger.TotT.PhantomBlade.Blade"))
			{
				RagdollHand hand2 = this.Hand;
				string text2;
				if (hand2 == null)
				{
					text2 = null;
				}
				else
				{
					Handle grabbedHandle2 = hand2.grabbedHandle;
					if (grabbedHandle2 == null)
					{
						text2 = null;
					}
					else
					{
						Item item2 = grabbedHandle2.item;
						text2 = ((item2 != null) ? item2.data.id : null);
					}
				}
				flag = text2 == "GrooveSlinger.TotT.GrappleController";
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.Hand.UnGrab(false);
			}
			bool flag3 = this.bladeItem != null;
			if (flag3)
			{
				this.bladeItem.Despawn();
				this.bladeItem = null;
			}
			bool flag4 = this.controlItem != null;
			if (flag4)
			{
				this.controlItem.Despawn();
				this.controlItem = null;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004AD4 File Offset: 0x00002CD4
		public override void OnSnapEvent(Holder holder)
		{
			base.OnSnapEvent(holder);
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Blade.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.bladeItem = sitem;
					this.bladeAnim = sitem.GetComponent<Animator>();
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
				bool flag2 = this.controlItem == null;
				if (flag2)
				{
					this.Controller.SpawnAsync(delegate(Item sitem)
					{
						sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
						sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
						sitem.disallowDespawn = true;
						sitem.disallowRoomDespawn = true;
						sitem.Set("cullingDetectionEnabled", false);
						sitem.gameObject.SetActive(false);
						this.controlItem = sitem;
					}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
				}
			}
			else
			{
				bool flag3 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag3)
				{
					this.Blade.SpawnAsync(delegate(Item sitem)
					{
						sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
						sitem.disallowDespawn = true;
						sitem.disallowRoomDespawn = true;
						sitem.Set("cullingDetectionEnabled", false);
						sitem.gameObject.SetActive(false);
						this.bladeItem = sitem;
						this.bladeAnim = sitem.GetComponent<Animator>();
					}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					bool flag4 = this.controlItem == null;
					if (flag4)
					{
						this.Controller.SpawnAsync(delegate(Item sitem)
						{
							sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
							sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
							sitem.disallowDespawn = true;
							sitem.disallowRoomDespawn = true;
							sitem.Set("cullingDetectionEnabled", false);
							sitem.gameObject.SetActive(false);
							this.controlItem = sitem;
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					}
				}
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
					bool flag5 = this.Hand != null;
					if (flag5)
					{
						this.Blade.SpawnAsync(delegate(Item sitem)
						{
							sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
							sitem.disallowDespawn = true;
							sitem.disallowRoomDespawn = true;
							sitem.Set("cullingDetectionEnabled", false);
							sitem.gameObject.SetActive(false);
							this.bladeItem = sitem;
							this.bladeAnim = sitem.GetComponent<Animator>();
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
						bool flag6 = this.controlItem == null;
						if (flag6)
						{
							this.Controller.SpawnAsync(delegate(Item sitem)
							{
								sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
								sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
								sitem.disallowDespawn = true;
								sitem.disallowRoomDespawn = true;
								sitem.Set("cullingDetectionEnabled", false);
								sitem.gameObject.SetActive(false);
								this.controlItem = sitem;
							}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
						}
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004D44 File Offset: 0x00002F44
		private void Blade_OnUngrab(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			foreach (Damager dam in this.bladeItem.GetComponentsInChildren<Damager>())
			{
				dam.UnPenetrateAll();
			}
			foreach (RevealDecal r in this.bladeItem.revealDecals)
			{
				r.Reset();
			}
			this.bladeItem.UpdateReveal();
			base.StartCoroutine(this.BladeInAnim());
			this.bladeItem.gameObject.SetActive(false);
			ragdollHand.ClearTouch();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004E00 File Offset: 0x00003000
		private void Controller_OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == null && this.CanFire && this.Loaded;
			if (flag)
			{
				base.StartCoroutine(this.FireSFX());
				base.StartCoroutine(this.CrossbowFire());
				this.LoadedItem.SpawnAsync(delegate(Item bolt)
				{
					Creature currentCreature = Player.currentCreature;
					bolt.IgnoreRagdollCollision((currentCreature != null) ? currentCreature.ragdoll : null);
					bolt.IgnoreObjectCollision(this.controlItem);
					bolt.Throw(1f, 2);
					bolt.rb.AddForce(this.item.flyDirRef.forward * 50f, 1);
				}, new Vector3?(this.item.flyDirRef.transform.position), new Quaternion?(this.item.flyDirRef.transform.rotation), null, true, null);
				this.BoltRef.SetActive(false);
				bool forceAutoSpawnArrow = ItemModuleBow.forceAutoSpawnArrow;
				if (forceAutoSpawnArrow)
				{
					this.Loaded = true;
					base.StartCoroutine(this.ReloadBolt(this.LoadedItemID, this.LoadedColor));
				}
				else
				{
					this.Loaded = false;
				}
			}
			bool flag2 = action == 2 && this.CanFire && this.Loaded;
			if (flag2)
			{
				this.Loaded = false;
				this.BoltRef.SetActive(false);
				RagdollHand tHand = this.controlItem.mainHandler;
				this.controlItem.mainHandler.TryRelease();
				this.LoadedItem.SpawnAsync(delegate(Item bolt)
				{
					tHand.Grab(bolt.GetMainHandle(tHand.side));
				}, new Vector3?(this.item.flyDirRef.transform.position), new Quaternion?(this.item.flyDirRef.transform.rotation), null, true, null);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004F80 File Offset: 0x00003180
		private void Controller_OnUngrab(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			base.StartCoroutine(this.UnequipSFX());
			base.StartCoroutine(this.CrossbowInAnim());
			this.controlItem.gameObject.SetActive(false);
			ragdollHand.ClearTouch();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004FB8 File Offset: 0x000031B8
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
					base.StartCoroutine(this.ChangeColor(this.OffColor));
				}
				else
				{
					this.OnOff = true;
					base.StartCoroutine(this.ChangeColor(this.OnColor));
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005034 File Offset: 0x00003234
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
					base.StartCoroutine(this.ChangeColor(this.OffColor));
				}
				else
				{
					this.OnOff = true;
					base.StartCoroutine(this.ChangeColor(this.OnColor));
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000050B0 File Offset: 0x000032B0
		public bool isLoaded()
		{
			return this.Loaded;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000050C8 File Offset: 0x000032C8
		public void LoadBolt(string id, Color color)
		{
			this.Loaded = true;
			this.BoltRef.SetActive(true);
			this.LoadedItemID = id;
			this.LoadedItem = Catalog.GetData<ItemData>(id, true);
			this.LoadedColor = color;
			this.BoltRef.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.LoadedColor);
			base.StartCoroutine(this.LoadSFX());
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005133 File Offset: 0x00003333
		public IEnumerator CrossbowFire()
		{
			this.CanFire = false;
			this.anim.SetTrigger("Fire");
			yield return new WaitForSeconds(0.41666666f);
			this.CanFire = true;
			yield break;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005142 File Offset: 0x00003342
		public IEnumerator ReloadBolt(string ID, Color color)
		{
			yield return new WaitForSeconds(0.41666666f);
			this.Loaded = true;
			this.BoltRef.SetActive(true);
			this.LoadedItemID = ID;
			this.LoadedItem = Catalog.GetData<ItemData>(ID, true);
			this.LoadedColor = color;
			this.BoltRef.GetComponent<Renderer>().material.SetColor("_EmissionColor", this.LoadedColor);
			yield break;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000515F File Offset: 0x0000335F
		public IEnumerator BladeOutAnim()
		{
			bool flag = this.Hand.side == 1;
			Vector3 offset;
			if (flag)
			{
				offset = new Vector3(0f, -95f, -75f);
			}
			else
			{
				offset = new Vector3(0f, -95f, -75f);
			}
			this.anim.SetTrigger("Toggle");
			this.BladeOutSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			bool flag2 = this.bladeItem != null;
			if (flag2)
			{
				this.bladeItem.gameObject.SetActive(true);
				this.bladeItem.gameObject.transform.position = this.Hand.transform.position;
				this.bladeItem.gameObject.transform.rotation = this.Hand.transform.rotation * Quaternion.Euler(offset.x, offset.y, offset.z);
				foreach (Renderer r in this.bladeItem.renderers)
				{
					r.enabled = true;
					r = null;
				}
				List<Renderer>.Enumerator enumerator = default(List<Renderer>.Enumerator);
				this.Hand.Grab(this.bladeItem.GetMainHandle(this.Hand.side), true);
				base.WaterFix(this.bladeItem);
				this.bladeAnim.Play("Extend");
			}
			yield break;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000516E File Offset: 0x0000336E
		public IEnumerator BladeInSilent()
		{
			this.anim.SetTrigger("Toggle");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000517D File Offset: 0x0000337D
		public IEnumerator BladeInAnim()
		{
			this.anim.SetTrigger("Toggle");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000518C File Offset: 0x0000338C
		public IEnumerator CrossbowOutAnim()
		{
			bool flag = this.Hand.side == 1;
			Vector3 offset;
			if (flag)
			{
				offset = new Vector3(0f, -95f, -75f);
			}
			else
			{
				offset = new Vector3(0f, -95f, -75f);
			}
			this.anim.SetTrigger("AltToggle");
			bool flag2 = this.controlItem != null;
			if (flag2)
			{
				this.controlItem.gameObject.SetActive(true);
				this.controlItem.gameObject.transform.position = this.Hand.transform.position;
				this.controlItem.gameObject.transform.rotation = this.Hand.transform.rotation * Quaternion.Euler(offset.x, offset.y, offset.z);
				foreach (Renderer r in this.controlItem.renderers)
				{
					r.enabled = true;
					r = null;
				}
				List<Renderer>.Enumerator enumerator = default(List<Renderer>.Enumerator);
				this.Hand.Grab(this.controlItem.GetMainHandle(this.Hand.side), true);
				base.WaterFix(this.controlItem);
			}
			yield return new WaitForSeconds(0.2f);
			this.CanFire = true;
			yield break;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000519B File Offset: 0x0000339B
		public IEnumerator CrossbowInAnim()
		{
			this.CanFire = false;
			this.anim.SetTrigger("AltToggle");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000051AA File Offset: 0x000033AA
		public IEnumerator LoadSFX()
		{
			this.Load.Spawn(this.item.flyDirRef, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000051B9 File Offset: 0x000033B9
		public IEnumerator FireSFX()
		{
			this.Fire.Spawn(this.item.flyDirRef, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000051C8 File Offset: 0x000033C8
		public IEnumerator EquipSFX()
		{
			this.Equip.Spawn(this.item.flyDirRef, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000051D7 File Offset: 0x000033D7
		public IEnumerator UnequipSFX()
		{
			this.Unequip.Spawn(this.item.flyDirRef, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield break;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000051E6 File Offset: 0x000033E6
		public IEnumerator ChangeColor(Color newColor)
		{
			yield return new WaitForSeconds(0.2f);
			float tts = 0.2f;
			float timeElapsed = 0f;
			Color toHitC = newColor;
			Color CurrentC = this.CurrentColor;
			Material i = this.Indicator.material;
			while (timeElapsed <= tts)
			{
				float tempR = Mathf.Lerp(CurrentC.r, toHitC.r, timeElapsed / tts);
				float tempG = Mathf.Lerp(CurrentC.g, toHitC.g, timeElapsed / tts);
				float tempB = Mathf.Lerp(CurrentC.b, toHitC.b, timeElapsed / tts);
				float tempA = Mathf.Lerp(CurrentC.a, toHitC.a, timeElapsed / tts);
				Color tempColor = new Color(tempR, tempG, tempB, tempA);
				i.SetColor("_EmissionColor", tempColor);
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			i.SetColor("_EmissionColor", newColor);
			this.CurrentColor = newColor;
			yield break;
		}

		// Token: 0x0400003B RID: 59
		private Animator anim;

		// Token: 0x0400003C RID: 60
		private Item bladeItem;

		// Token: 0x0400003D RID: 61
		private Item controlItem;

		// Token: 0x0400003E RID: 62
		private Animator bladeAnim;

		// Token: 0x0400003F RID: 63
		private toggleMethod toggleMethod = PhantomBladeParser.toggleMethod;

		// Token: 0x04000040 RID: 64
		private Renderer Indicator;

		// Token: 0x04000041 RID: 65
		private GameObject BoltRef;

		// Token: 0x04000042 RID: 66
		private GameObject BoltLoader;

		// Token: 0x04000043 RID: 67
		private bool AltModeOn = false;

		// Token: 0x04000044 RID: 68
		private Transform FirePoint;

		// Token: 0x04000045 RID: 69
		private bool Loaded = false;

		// Token: 0x04000046 RID: 70
		private bool CanFire = false;

		// Token: 0x04000047 RID: 71
		private string LoadedItemID;

		// Token: 0x04000048 RID: 72
		private ItemData LoadedItem;

		// Token: 0x04000049 RID: 73
		private Color LoadedColor;

		// Token: 0x0400004A RID: 74
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x0400004B RID: 75
		private Color OnColorGreen = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x0400004C RID: 76
		private Color OnColorBlue = new Color(0f, 0.4960452f, 3.564869f, 1f);

		// Token: 0x0400004D RID: 77
		private Color CurrentColor;

		// Token: 0x0400004E RID: 78
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x0400004F RID: 79
		private ItemData Blade = Catalog.GetData<ItemData>("GrooveSlinger.TotT.PhantomBlade.Blade", true);

		// Token: 0x04000050 RID: 80
		private ItemData Controller = Catalog.GetData<ItemData>("GrooveSlinger.TotT.GrappleController", true);

		// Token: 0x04000051 RID: 81
		private ItemData TestBolt = Catalog.GetData<ItemData>("GrooveSlinger.Dishonored.Bolt", true);

		// Token: 0x04000052 RID: 82
		private EffectData BladeOutSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeOut", true);

		// Token: 0x04000053 RID: 83
		private EffectData BladeInSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeIn", true);

		// Token: 0x04000054 RID: 84
		private EffectData Unequip = Catalog.GetData<EffectData>("GrooveSlinger.Effect.PhantomBlade.Unequip", true);

		// Token: 0x04000055 RID: 85
		private EffectData Equip = Catalog.GetData<EffectData>("GrooveSlinger.Effect.PhantomBlade.Equip", true);

		// Token: 0x04000056 RID: 86
		private EffectData Fire = Catalog.GetData<EffectData>("GrooveSlinger.Effect.PhantomBlade.Fire", true);

		// Token: 0x04000057 RID: 87
		private EffectData Load = Catalog.GetData<EffectData>("GrooveSlinger.Effect.PhantomBlade.Load", true);
	}
}
