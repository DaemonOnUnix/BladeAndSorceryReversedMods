using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200002B RID: 43
	public class HiddenDaggerMono : ArmModule
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00009494 File Offset: 0x00007694
		public override void OnStart()
		{
			base.OnStart();
			this.anim = base.GetComponent<Animator>();
			this.HasAltMode = false;
			this.CurrentColor = this.OnColor;
			this.canInteract = true;
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

		// Token: 0x0600012E RID: 302 RVA: 0x00009584 File Offset: 0x00007784
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

		// Token: 0x0600012F RID: 303 RVA: 0x000095D1 File Offset: 0x000077D1
		public override void Activate()
		{
			this.Activated = true;
			base.StartCoroutine(this.BladeOutAnim());
			base.Activate();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000095EF File Offset: 0x000077EF
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			base.On();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009613 File Offset: 0x00007813
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			base.Off();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009638 File Offset: 0x00007838
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			bool flag = this.bladeItem == null;
			if (flag)
			{
				this.Blade.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Blade_OnHeldAction);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.bladeItem = sitem;
					this.bladeAnim = sitem.GetComponent<Animator>();
					this.RightHandle = sitem.GetCustomReference("HandleRight", true).GetComponent<Handle>();
					this.LeftHandle = sitem.GetCustomReference("HandleLeft", true).GetComponent<Handle>();
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			base.giveHand(hand);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000096AC File Offset: 0x000078AC
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
			bool flag = text == "GrooveSlinger.TotT.HiddenDagger.Blade";
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

		// Token: 0x06000134 RID: 308 RVA: 0x00009734 File Offset: 0x00007934
		public override void OnSnapEvent(Holder holder)
		{
			base.OnSnapEvent(holder);
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Blade.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
					sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Blade_OnHeldAction);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.bladeItem = sitem;
					this.bladeAnim = sitem.GetComponent<Animator>();
					this.RightHandle = sitem.GetCustomReference("HandleRight", true).GetComponent<Handle>();
					this.LeftHandle = sitem.GetCustomReference("HandleLeft", true).GetComponent<Handle>();
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
						sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Blade_OnHeldAction);
						sitem.disallowDespawn = true;
						sitem.disallowRoomDespawn = true;
						sitem.Set("cullingDetectionEnabled", false);
						sitem.gameObject.SetActive(false);
						this.bladeItem = sitem;
						this.bladeAnim = sitem.GetComponent<Animator>();
						this.RightHandle = sitem.GetCustomReference("HandleRight", true).GetComponent<Handle>();
						this.LeftHandle = sitem.GetCustomReference("HandleLeft", true).GetComponent<Handle>();
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
							sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Blade_OnHeldAction);
							sitem.disallowDespawn = true;
							sitem.disallowRoomDespawn = true;
							sitem.Set("cullingDetectionEnabled", false);
							sitem.gameObject.SetActive(false);
							this.bladeItem = sitem;
							this.bladeAnim = sitem.GetComponent<Animator>();
							this.RightHandle = sitem.GetCustomReference("HandleRight", true).GetComponent<Handle>();
							this.LeftHandle = sitem.GetCustomReference("HandleLeft", true).GetComponent<Handle>();
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					}
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000988C File Offset: 0x00007A8C
		private void Blade_OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = this.canInteract && action == 2;
			if (flag)
			{
				foreach (Damager dam in this.bladeItem.GetComponentsInChildren<Damager>())
				{
					dam.UnPenetrateAll();
				}
				bool flag2 = this.Hand.side == 0;
				if (flag2)
				{
					this.bladeAnim.SetTrigger("RotateRight");
					base.StartCoroutine(this.EvaluateHandle());
				}
				else
				{
					this.bladeAnim.SetTrigger("RotateLeft");
					base.StartCoroutine(this.EvaluateHandle());
				}
				base.StartCoroutine(this.DelayInteraction());
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009940 File Offset: 0x00007B40
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
			bool flag = this.IsRight(this.bladeAnim.GetCurrentAnimatorStateInfo(0));
			if (flag)
			{
				base.StartCoroutine(this.RightBladeInAnim());
			}
			else
			{
				bool flag2 = this.IsLeft(this.bladeAnim.GetCurrentAnimatorStateInfo(0));
				if (flag2)
				{
					base.StartCoroutine(this.LeftBladeInAnim());
				}
				else
				{
					base.StartCoroutine(this.BladeInAnim());
				}
			}
			this.bladeItem.gameObject.SetActive(false);
			ragdollHand.ClearTouch();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00009A50 File Offset: 0x00007C50
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

		// Token: 0x06000138 RID: 312 RVA: 0x00009ACC File Offset: 0x00007CCC
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

		// Token: 0x06000139 RID: 313 RVA: 0x00009B48 File Offset: 0x00007D48
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
				this.Hand.Grab(this.bladeItem.GetMainHandle(this.Hand.side));
				base.WaterFix(this.bladeItem);
				this.bladeAnim.Play("Extend");
			}
			yield break;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009B57 File Offset: 0x00007D57
		public IEnumerator BladeInSilent()
		{
			this.anim.SetTrigger("Toggle");
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009B66 File Offset: 0x00007D66
		public IEnumerator BladeInAnim()
		{
			this.anim.SetTrigger("Toggle");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009B75 File Offset: 0x00007D75
		public IEnumerator RightBladeInAnim()
		{
			this.anim.Play("RightIn");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009B84 File Offset: 0x00007D84
		public IEnumerator LeftBladeInAnim()
		{
			this.anim.Play("LeftIn");
			this.BladeInSFX.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return new WaitForSeconds(0.2f);
			this.Activated = false;
			yield break;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009B93 File Offset: 0x00007D93
		public IEnumerator DelayInteraction()
		{
			this.canInteract = false;
			yield return new WaitForSeconds(0.2f);
			this.canInteract = true;
			yield break;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009BA2 File Offset: 0x00007DA2
		public IEnumerator EvaluateHandle()
		{
			yield return new WaitForSeconds(0.2f);
			bool flag = this.bladeItem.mainHandler.side == 0;
			if (flag)
			{
				bool flag2 = this.bladeItem.mainHandleRight == this.bladeItem.mainHandler.grabbedHandle;
				if (flag2)
				{
					this.bladeItem.OnUngrabEvent -= new Item.ReleaseDelegate(this.Blade_OnUngrab);
					this.bladeItem.mainHandler.UnGrab(false);
					this.Hand.Grab(this.RightHandle, true);
					this.bladeItem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
				}
				else
				{
					this.bladeItem.OnUngrabEvent -= new Item.ReleaseDelegate(this.Blade_OnUngrab);
					this.bladeItem.mainHandler.UnGrab(false);
					this.Hand.Grab(this.bladeItem.mainHandleRight, true);
					this.bladeItem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
				}
			}
			else
			{
				bool flag3 = this.bladeItem.mainHandleLeft == this.bladeItem.mainHandler.grabbedHandle;
				if (flag3)
				{
					this.bladeItem.OnUngrabEvent -= new Item.ReleaseDelegate(this.Blade_OnUngrab);
					this.bladeItem.mainHandler.UnGrab(false);
					this.Hand.Grab(this.LeftHandle, true);
					this.bladeItem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
				}
				else
				{
					this.bladeItem.OnUngrabEvent -= new Item.ReleaseDelegate(this.Blade_OnUngrab);
					this.bladeItem.mainHandler.UnGrab(false);
					this.Hand.Grab(this.bladeItem.mainHandleLeft, true);
					this.bladeItem.OnUngrabEvent += new Item.ReleaseDelegate(this.Blade_OnUngrab);
				}
			}
			yield break;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009BB1 File Offset: 0x00007DB1
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

		// Token: 0x06000141 RID: 321 RVA: 0x00009BC8 File Offset: 0x00007DC8
		public bool IsRight(AnimatorStateInfo animStateInfo)
		{
			return animStateInfo.IsName("RotateRight") || animStateInfo.IsName("RightBase") || animStateInfo.IsName("RightToBase");
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009C10 File Offset: 0x00007E10
		public bool IsLeft(AnimatorStateInfo animStateInfo)
		{
			return animStateInfo.IsName("RotateLeft") || animStateInfo.IsName("LeftBase") || animStateInfo.IsName("LeftToBase");
		}

		// Token: 0x040000E1 RID: 225
		private Animator anim;

		// Token: 0x040000E2 RID: 226
		private Item bladeItem;

		// Token: 0x040000E3 RID: 227
		private Animator bladeAnim;

		// Token: 0x040000E4 RID: 228
		private bool canInteract;

		// Token: 0x040000E5 RID: 229
		private Handle RightHandle;

		// Token: 0x040000E6 RID: 230
		private Handle LeftHandle;

		// Token: 0x040000E7 RID: 231
		private toggleMethod toggleMethod = HiddenBladeParser.toggleMethod;

		// Token: 0x040000E8 RID: 232
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x040000E9 RID: 233
		private Color CurrentColor;

		// Token: 0x040000EA RID: 234
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x040000EB RID: 235
		private ItemData Blade = Catalog.GetData<ItemData>("GrooveSlinger.TotT.HiddenDagger.Blade", true);

		// Token: 0x040000EC RID: 236
		private EffectData BladeOutSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeOut", true);

		// Token: 0x040000ED RID: 237
		private EffectData BladeInSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectHiddenBladeIn", true);
	}
}
