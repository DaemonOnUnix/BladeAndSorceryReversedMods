using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000028 RID: 40
	public class HiddenBladeMono : ArmModule
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00008C64 File Offset: 0x00006E64
		public override void OnStart()
		{
			base.OnStart();
			this.anim = base.GetComponent<Animator>();
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

		// Token: 0x06000116 RID: 278 RVA: 0x00008D4C File Offset: 0x00006F4C
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

		// Token: 0x06000117 RID: 279 RVA: 0x00008D99 File Offset: 0x00006F99
		public override void Activate()
		{
			this.Activated = true;
			base.StartCoroutine(this.BladeOutAnim());
			base.Activate();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008DB7 File Offset: 0x00006FB7
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			this.SaveData();
			base.On();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008DE2 File Offset: 0x00006FE2
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			this.SaveData();
			base.Off();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008E10 File Offset: 0x00007010
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
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			base.giveHand(hand);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008E84 File Offset: 0x00007084
		public override void OnUnSnapEvent(Holder holder)
		{
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
			bool flag = text == "GrooveSlinger.TotT.HiddenBlade.Blade";
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
			bool flag3 = this.handWatcher != null;
			if (flag3)
			{
				this.handWatcher.Delete();
				this.handWatcher = null;
			}
			base.OnUnSnapEvent(holder);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008F30 File Offset: 0x00007130
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
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					}
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009088 File Offset: 0x00007288
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
			this.bladeItem.gameObject.SetActive(false);
			ragdollHand.ClearTouch();
			base.StartCoroutine(this.BladeInAnim());
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00009144 File Offset: 0x00007344
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

		// Token: 0x0600011F RID: 287 RVA: 0x000091C0 File Offset: 0x000073C0
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

		// Token: 0x06000120 RID: 288 RVA: 0x0000923C File Offset: 0x0000743C
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
			yield return new WaitForSeconds(0.2f);
			bool flag2 = this.Hand.grabbedHandle != null;
			if (flag2)
			{
				base.StartCoroutine(this.BladeInSilent());
			}
			else
			{
				bool flag3 = this.bladeItem != null;
				if (flag3)
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
					this.Hand.Grab(this.bladeItem.GetMainHandle(this.Hand.side));
					base.WaterFix(this.bladeItem);
				}
			}
			yield break;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000924B File Offset: 0x0000744B
		public IEnumerator BladeInSilent()
		{
			this.anim.SetTrigger("Toggle");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000925A File Offset: 0x0000745A
		public IEnumerator BladeInAnim()
		{
			this.anim.SetTrigger("Toggle");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009269 File Offset: 0x00007469
		public IEnumerator ChangeColor(Color newColor)
		{
			yield return new WaitForSeconds(0.1f);
			float tts = 0.2f;
			float timeElapsed = 0f;
			Color toHitC = newColor;
			Color CurrentC = this.CurrentColor;
			Material i = this.item.renderers[0].material;
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

		// Token: 0x040000D6 RID: 214
		private Animator anim;

		// Token: 0x040000D7 RID: 215
		private Item bladeItem;

		// Token: 0x040000D8 RID: 216
		private toggleMethod toggleMethod = HiddenBladeParser.toggleMethod;

		// Token: 0x040000D9 RID: 217
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x040000DA RID: 218
		private Color CurrentColor;

		// Token: 0x040000DB RID: 219
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x040000DC RID: 220
		private ItemData Blade = Catalog.GetData<ItemData>("GrooveSlinger.TotT.HiddenBlade.Blade", true);

		// Token: 0x040000DD RID: 221
		private EffectData BladeOutSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeOut", true);

		// Token: 0x040000DE RID: 222
		private EffectData BladeInSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeIn", true);
	}
}
