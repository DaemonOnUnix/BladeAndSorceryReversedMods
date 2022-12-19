using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200000B RID: 11
	public class PirateBladeMono : ArmModule
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public override void OnStart()
		{
			base.OnStart();
			this.anim = base.GetComponent<Animator>();
			this.HasAltMode = false;
			this.CurrentColor = this.OnColor;
			this.canInteract = true;
			this.indicator = this.item.GetCustomReference("Indicator", true).GetComponent<Renderer>();
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
				this.OnOff = false;
				this.customData = new ArmModuleSave();
			}
			bool flag2 = this.item.holder != null;
			if (flag2)
			{
				this.OnSnapEvent(this.item.holder);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003EAC File Offset: 0x000020AC
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

		// Token: 0x0600004A RID: 74 RVA: 0x00003EF9 File Offset: 0x000020F9
		public override void Activate()
		{
			this.Activated = true;
			base.StartCoroutine(this.BladeOutAnim());
			base.Activate();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003F17 File Offset: 0x00002117
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			base.On();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003F3B File Offset: 0x0000213B
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			base.Off();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003F60 File Offset: 0x00002160
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
			base.giveHand(hand);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003FD4 File Offset: 0x000021D4
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
			bool flag = text == "GrooveSlinger.TotT.PirateBlade.Blade";
			if (flag)
			{
				this.Hand.UnGrab(false);
			}
			bool flag2 = this.bladeItem != null;
			if (flag2)
			{
				this.bladeItem.Despawn();
				this.bladeItem = null;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000405C File Offset: 0x0000225C
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
			}
			else
			{
				bool flag2 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag2)
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
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
					bool flag3 = this.Hand != null;
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
					}
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000041B4 File Offset: 0x000023B4
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

		// Token: 0x06000051 RID: 81 RVA: 0x00004270 File Offset: 0x00002470
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

		// Token: 0x06000052 RID: 82 RVA: 0x000042EC File Offset: 0x000024EC
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

		// Token: 0x06000053 RID: 83 RVA: 0x00004368 File Offset: 0x00002568
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

		// Token: 0x06000054 RID: 84 RVA: 0x00004377 File Offset: 0x00002577
		public IEnumerator BladeInSilent()
		{
			this.anim.SetTrigger("Toggle");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004386 File Offset: 0x00002586
		public IEnumerator BladeInAnim()
		{
			this.anim.SetTrigger("Toggle");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004395 File Offset: 0x00002595
		public IEnumerator DelayInteraction()
		{
			this.canInteract = false;
			yield return new WaitForSeconds(0.2f);
			this.canInteract = true;
			yield break;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000043A4 File Offset: 0x000025A4
		public IEnumerator ChangeColor(Color newColor)
		{
			yield return new WaitForSeconds(0.1f);
			float tts = 0.2f;
			float timeElapsed = 0f;
			Color toHitC = newColor;
			Color CurrentC = this.CurrentColor;
			Material i = this.indicator.material;
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
			this.CurrentColor = newColor;
			yield break;
		}

		// Token: 0x0400002D RID: 45
		private Animator anim;

		// Token: 0x0400002E RID: 46
		private Item bladeItem;

		// Token: 0x0400002F RID: 47
		private Animator bladeAnim;

		// Token: 0x04000030 RID: 48
		private bool canInteract;

		// Token: 0x04000031 RID: 49
		private toggleMethod toggleMethod = HiddenBladeParser.toggleMethod;

		// Token: 0x04000032 RID: 50
		private Renderer indicator;

		// Token: 0x04000033 RID: 51
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x04000034 RID: 52
		private Color CurrentColor;

		// Token: 0x04000035 RID: 53
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x04000036 RID: 54
		private ItemData Blade = Catalog.GetData<ItemData>("GrooveSlinger.TotT.PirateBlade.Blade", true);

		// Token: 0x04000037 RID: 55
		private EffectData BladeOutSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeOut", true);

		// Token: 0x04000038 RID: 56
		private EffectData BladeInSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeIn", true);
	}
}
