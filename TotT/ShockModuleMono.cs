using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200004D RID: 77
	public class ShockModuleMono : ArmModule
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		public override void OnStart()
		{
			base.OnStart();
			this.HasAltMode = false;
			this.useDeactivate = true;
			this.HasCreatureActivate = true;
			this.anim = this.item.gameObject.GetComponent<Animator>();
			this.Battery = this.item.GetCustomReference("Battery", true).GetComponent<Renderer>();
			this.Base = this.item.GetCustomReference("Base", true).GetComponent<Renderer>();
			this.shockPoint = this.item.GetCustomReference("ShockPoint", true).gameObject;
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

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D42C File Offset: 0x0000B62C
		public override void OnUpdate()
		{
			base.OnUpdate();
			bool flag = !this.Discharging && this.Energy < this.EnergyMax;
			if (flag)
			{
				this.Energy += 0.05f * this.RechargeMultiplier;
			}
			bool flag2 = this.Energy > this.EnergyMax;
			if (flag2)
			{
				this.Energy = this.EnergyMax;
			}
			bool discharging = this.Discharging;
			if (discharging)
			{
				this.Energy -= 0.1f;
			}
			bool flag3 = this.Discharging && this.Energy < 0f;
			if (flag3)
			{
				this.Discharging = false;
				this.Energy = 0f;
			}
			this.UpdateEnergy();
			this.UpdateBattery();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		public void UpdateEnergy()
		{
			float tempR = Mathf.Lerp(this.Energy0.r, this.Energy100.r, this.Energy / this.EnergyMax);
			float tempG = Mathf.Lerp(this.Energy0.g, this.Energy100.g, this.Energy / this.EnergyMax);
			float tempB = Mathf.Lerp(this.Energy0.b, this.Energy100.b, this.Energy / this.EnergyMax);
			float tempA = Mathf.Lerp(this.Energy0.a, this.Energy100.a, this.Energy / this.EnergyMax);
			Color tempColor;
			tempColor..ctor(tempR, tempG, tempB, tempA);
			this.Base.materials[1].SetColor("_EmissionColor", tempColor);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		public void UpdateBattery()
		{
			float tempR = Mathf.Lerp(this.Battery0.r, this.Battery100.r, this.Energy / this.EnergyMax);
			float tempG = Mathf.Lerp(this.Battery0.g, this.Battery100.g, this.Energy / this.EnergyMax);
			float tempB = Mathf.Lerp(this.Battery0.b, this.Battery100.b, this.Energy / this.EnergyMax);
			float tempA = Mathf.Lerp(this.Battery0.a, this.Battery100.a, this.Energy / this.EnergyMax);
			Color tempColor;
			tempColor..ctor(tempR, tempG, tempB, tempA);
			this.Battery.material.SetColor("_BaseColor", tempColor);
			this.Battery.material.SetFloat("_Amount", this.Energy / this.EnergyMax);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
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

		// Token: 0x060001FA RID: 506 RVA: 0x0000D715 File Offset: 0x0000B915
		public override void Activate()
		{
			this.Activated = true;
			this.Hand.playerHand.controlHand.HapticShort(2f);
			this.Discharging = true;
			base.StartCoroutine(this.WhipRoutine());
			base.Activate();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D755 File Offset: 0x0000B955
		public override void Deactivate()
		{
			this.Activated = false;
			this.Discharging = false;
			base.Deactivate();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D770 File Offset: 0x0000B970
		public override void CreatureActivate()
		{
			Handle grabbedHandle = this.Hand.grabbedHandle;
			Creature c = ((grabbedHandle != null) ? grabbedHandle.GetComponentInParent<Creature>() : null);
			bool flag = c != null && this.Energy >= 20f && c.state > 0;
			if (flag)
			{
				c.TryElectrocute(1f, 5f, true, false, this.Electrocuted);
				this.Zap.Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
				bool flag2 = c.brain.state != 5;
				if (flag2)
				{
					KnockOutBehaviour temp = c.gameObject.AddComponent<KnockOutBehaviour>();
					temp.Setup(1f);
				}
				this.Energy -= 20f;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D841 File Offset: 0x0000BA41
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			this.SaveData();
			base.On();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D86C File Offset: 0x0000BA6C
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			this.SaveData();
			base.Off();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000D897 File Offset: 0x0000BA97
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			base.giveHand(hand);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000D8A9 File Offset: 0x0000BAA9
		public override void OnUnSnapEvent(Holder holder)
		{
			base.OnUnSnapEvent(holder);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
		public override void OnSnapEvent(Holder holder)
		{
			base.OnSnapEvent(holder);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
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

		// Token: 0x06000203 RID: 515 RVA: 0x0000D93C File Offset: 0x0000BB3C
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

		// Token: 0x06000204 RID: 516 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		public IEnumerator WhipRoutine()
		{
			SpellCastLightning spell = Catalog.GetData<SpellCastLightning>("Lightning", true);
			EffectData arcStaffEffectData = Catalog.GetData<EffectData>(spell.arcStaffEffectId, true);
			LightningTrailNode prevNode = LightningTrailNode.New(this.Hand.transform.position, spell, null, null);
			LightningTrailNode nextNode = LightningTrailNode.New(this.Hand.transform.position, spell, this.Hand.transform, prevNode);
			EffectInstance arcStaffEffect = arcStaffEffectData.Spawn(this.shockPoint.transform, true, null, false, Array.Empty<Type>());
			float arcWhooshIntensity = 0f;
			arcStaffEffect.Play(0, false);
			this.anim.SetTrigger("Toggle");
			Vector3 force = -this.Hand.transform.right * (Mathf.Clamp(this.Hand.rb.GetPointVelocity(this.Hand.transform.position).magnitude, 0f, spell.maxBeamNodeInputVelocity) * spell.beamNodeVelocityMult);
			this.Hand.HapticTick(1f, 0.5f);
			prevNode.rb.AddForce(force, 2);
			yield return new WaitForEndOfFrame();
			while (this.Discharging && nextNode != null)
			{
				bool flag = prevNode == null || (nextNode.transform.position - prevNode.transform.position).magnitude > spell.minBeamNodeDistance;
				if (flag)
				{
					nextNode.transform.SetParent(null);
					prevNode = nextNode;
					nextNode = LightningTrailNode.New(this.Hand.transform.position, spell, this.Hand.transform, prevNode);
					force = -this.Hand.transform.right * (Mathf.Clamp(this.Hand.rb.GetPointVelocity(this.Hand.transform.position).magnitude, 0f, spell.maxBeamNodeInputVelocity) * spell.beamNodeVelocityMult);
					prevNode.rb.AddForce(force, 2);
				}
				bool flag2 = !this.Hand.rb.IsSleeping();
				if (flag2)
				{
					Vector3 pointV = this.Hand.rb.GetPointVelocity(this.Hand.transform.position);
					arcWhooshIntensity = Mathf.Lerp(arcWhooshIntensity, Mathf.InverseLerp(5f, 12f, pointV.magnitude), 0.1f);
					arcStaffEffect.SetSpeed(arcWhooshIntensity);
					bool flag3 = this.Hand;
					if (flag3)
					{
						arcStaffEffect.source = this.Hand;
					}
					bool flag4 = arcWhooshIntensity > 0f && !arcStaffEffect.isPlaying;
					if (flag4)
					{
						arcStaffEffect.Play(0, false);
					}
					pointV = default(Vector3);
				}
				else
				{
					bool flag5 = arcWhooshIntensity > 0f;
					if (flag5)
					{
						arcWhooshIntensity = Mathf.Lerp(arcWhooshIntensity, 0f, 0.1f);
					}
				}
				yield return 0;
			}
			arcStaffEffect.End(false, -1f);
			bool flag6 = !nextNode;
			if (flag6)
			{
				yield break;
			}
			nextNode.transform.SetParent(null);
			nextNode.EndTrail();
			this.Hand.HapticTick(1f, 0.5f);
			this.anim.SetTrigger("Toggle");
			nextNode.rb.AddForce(-this.Hand.transform.right * (Mathf.Clamp(this.Hand.rb.GetPointVelocity(this.Hand.transform.position).magnitude, 0f, spell.maxBeamNodeInputVelocity) * spell.beamNodeVelocityMult));
			yield break;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000D9C7 File Offset: 0x0000BBC7
		public IEnumerator ChangeColor(Color newColor)
		{
			yield return new WaitForSeconds(0.1f);
			float tts = 0.2f;
			float timeElapsed = 0f;
			Color toHitC = newColor;
			Color CurrentC = this.CurrentColor;
			Material i = this.item.renderers[0].materials[0];
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

		// Token: 0x0400015D RID: 349
		private toggleMethod toggleMethod = ShockParser.toggleMethod;

		// Token: 0x0400015E RID: 350
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x0400015F RID: 351
		private Color CurrentColor;

		// Token: 0x04000160 RID: 352
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);

		// Token: 0x04000161 RID: 353
		private Color Energy100 = new Color(0f, 7.853402f, 33.37696f, 1f);

		// Token: 0x04000162 RID: 354
		private Color Energy0 = new Color(0f, 0f, 0f, 1f);

		// Token: 0x04000163 RID: 355
		private Color Battery100 = new Color(0.2925533f, 1f, 0f, 1f);

		// Token: 0x04000164 RID: 356
		private Color Battery0 = new Color(1f, 0.3475744f, 0f, 1f);

		// Token: 0x04000165 RID: 357
		public bool Discharging = false;

		// Token: 0x04000166 RID: 358
		public float RechargeMultiplier = ShockParser.RechargeMultiplier;

		// Token: 0x04000167 RID: 359
		public float Energy = 100f;

		// Token: 0x04000168 RID: 360
		public float EnergyMax = 100f;

		// Token: 0x04000169 RID: 361
		private EffectData Electrocuted = Catalog.GetData<EffectData>("ImbueLightningRagdoll", true);

		// Token: 0x0400016A RID: 362
		private EffectData Zap = Catalog.GetData<EffectData>("GrooveSlinger.TotT.Effect.Zap", true);

		// Token: 0x0400016B RID: 363
		private Animator anim;

		// Token: 0x0400016C RID: 364
		private Renderer Battery;

		// Token: 0x0400016D RID: 365
		private Renderer Base;

		// Token: 0x0400016E RID: 366
		private GameObject shockPoint;
	}
}
