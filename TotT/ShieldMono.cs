using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000008 RID: 8
	public class ShieldMono : ArmModule
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000350C File Offset: 0x0000170C
		public override void OnStart()
		{
			base.OnStart();
			this.anim = base.GetComponent<Animator>();
			this.indicator = this.item.GetCustomReference("Indicator", true).GetComponent<Renderer>();
			this.HasAltMode = false;
			this.CurrentColor = this.OnColor;
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

		// Token: 0x06000030 RID: 48 RVA: 0x00003610 File Offset: 0x00001810
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

		// Token: 0x06000031 RID: 49 RVA: 0x0000365D File Offset: 0x0000185D
		public override void Activate()
		{
			this.Activated = true;
			base.StartCoroutine(this.BladeOutAnim());
			base.StartCoroutine(this.Counter());
			base.Activate();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003688 File Offset: 0x00001888
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			base.On();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000036AC File Offset: 0x000018AC
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			base.Off();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000036D0 File Offset: 0x000018D0
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			bool flag = this.shieldItem == null;
			if (flag)
			{
				this.Shield.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.shieldItem = sitem;
					this.shieldAnim = sitem.GetComponent<Animator>();
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			base.giveHand(hand);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003744 File Offset: 0x00001944
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
			bool flag = text == "GrooveSlinger.TotT.Shield.Shield";
			if (flag)
			{
				this.Hand.UnGrab(false);
			}
			bool flag2 = this.shieldItem != null;
			if (flag2)
			{
				this.shieldItem.Despawn();
				this.shieldItem = null;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000037CC File Offset: 0x000019CC
		public override void OnSnapEvent(Holder holder)
		{
			base.OnSnapEvent(holder);
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Shield.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.shieldItem = sitem;
					this.shieldAnim = sitem.GetComponent<Animator>();
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			else
			{
				bool flag2 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag2)
				{
					this.Shield.SpawnAsync(delegate(Item sitem)
					{
						sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
						sitem.disallowDespawn = true;
						sitem.disallowRoomDespawn = true;
						sitem.Set("cullingDetectionEnabled", false);
						sitem.gameObject.SetActive(false);
						this.shieldItem = sitem;
						this.shieldAnim = sitem.GetComponent<Animator>();
					}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
				}
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
					bool flag3 = this.Hand != null;
					if (flag3)
					{
						this.Shield.SpawnAsync(delegate(Item sitem)
						{
							sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
							sitem.disallowDespawn = true;
							sitem.disallowRoomDespawn = true;
							sitem.Set("cullingDetectionEnabled", false);
							sitem.gameObject.SetActive(false);
							this.shieldItem = sitem;
							this.shieldAnim = sitem.GetComponent<Animator>();
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					}
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003924 File Offset: 0x00001B24
		private void Blade_OnUngrab(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			foreach (Damager dam in this.shieldItem.GetComponentsInChildren<Damager>())
			{
				dam.UnPenetrateAll();
			}
			foreach (RevealDecal r in this.shieldItem.revealDecals)
			{
				r.Reset();
			}
			this.shieldAnim.Play("Base");
			this.shieldItem.UpdateReveal();
			this.shieldItem.gameObject.SetActive(false);
			ragdollHand.ClearTouch();
			bool quickRelease = this.QuickRelease;
			if (quickRelease)
			{
				base.StartCoroutine(this.BladeInSilent());
			}
			else
			{
				base.StartCoroutine(this.BladeInAnim());
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003A10 File Offset: 0x00001C10
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

		// Token: 0x06000039 RID: 57 RVA: 0x00003A8C File Offset: 0x00001C8C
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

		// Token: 0x0600003A RID: 58 RVA: 0x00003B08 File Offset: 0x00001D08
		public IEnumerator Counter()
		{
			this.QuickRelease = true;
			yield return new WaitForSeconds(0.3f);
			this.QuickRelease = false;
			yield break;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003B17 File Offset: 0x00001D17
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
			this.ShieldOutSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			bool flag2 = this.shieldItem != null;
			if (flag2)
			{
				this.shieldItem.gameObject.SetActive(true);
				this.shieldItem.gameObject.transform.position = this.Hand.transform.position;
				this.shieldItem.gameObject.transform.rotation = this.Hand.transform.rotation * Quaternion.Euler(offset.x, offset.y, offset.z);
				foreach (Renderer r in this.shieldItem.renderers)
				{
					r.enabled = true;
					r = null;
				}
				List<Renderer>.Enumerator enumerator = default(List<Renderer>.Enumerator);
				this.Hand.Grab(this.shieldItem.GetMainHandle(this.Hand.side), true);
				base.WaterFix(this.shieldItem);
				this.shieldAnim.Play("Extend");
			}
			yield break;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003B26 File Offset: 0x00001D26
		public IEnumerator BladeInSilent()
		{
			this.anim.Play("Retract");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003B35 File Offset: 0x00001D35
		public IEnumerator BladeInAnim()
		{
			this.anim.SetTrigger("Toggle");
			this.ShieldInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003B44 File Offset: 0x00001D44
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

		// Token: 0x0400001F RID: 31
		private Animator anim;

		// Token: 0x04000020 RID: 32
		private Item shieldItem;

		// Token: 0x04000021 RID: 33
		private Animator shieldAnim;

		// Token: 0x04000022 RID: 34
		private bool QuickRelease = false;

		// Token: 0x04000023 RID: 35
		private Renderer indicator;

		// Token: 0x04000024 RID: 36
		private toggleMethod toggleMethod = ShieldParser.toggleMethod;

		// Token: 0x04000025 RID: 37
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x04000026 RID: 38
		private Color CurrentColor;

		// Token: 0x04000027 RID: 39
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x04000028 RID: 40
		private ItemData Shield = Catalog.GetData<ItemData>("GrooveSlinger.TotT.Shield.Shield", true);

		// Token: 0x04000029 RID: 41
		private EffectData ShieldOutSFX = Catalog.GetData<EffectData>("GrooveSlinger.TotT.Effect.ShieldOut", true);

		// Token: 0x0400002A RID: 42
		private EffectData ShieldInSFX = Catalog.GetData<EffectData>("GrooveSlinger.TotT.Effect.ShieldIn", true);
	}
}
