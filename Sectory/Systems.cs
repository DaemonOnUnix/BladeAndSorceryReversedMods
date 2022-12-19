using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ThunderRoad;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200001C RID: 28
	public class Systems : MonoBehaviour
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003CC0 File Offset: 0x00001EC0
		private bool CheckBleedUnconscious
		{
			get
			{
				return this.bloodLost / this.creature.maxHealth > Entry.inst.unconsciousInfo.bloodLossKnockoutPercentage / 100f * Entry.inst.generalInfo.ScriptDifficulty + this.adrenaline / Entry.inst.unconsciousInfo.maxAdrenaline * (Entry.inst.unconsciousInfo.bloodLossKnockoutPercentage / 100f * Entry.inst.generalInfo.ScriptDifficulty * 0.5f);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003D4C File Offset: 0x00001F4C
		private bool CheckPainUnconscious
		{
			get
			{
				return this.painAccumulated / this.creature.maxHealth > Entry.inst.unconsciousInfo.averageBodyPainShutDownPercentage / 100f * Entry.inst.generalInfo.ScriptDifficulty + this.adrenaline / Entry.inst.unconsciousInfo.maxAdrenaline * (Entry.inst.unconsciousInfo.averageBodyPainShutDownPercentage / 100f * Entry.inst.generalInfo.ScriptDifficulty * 0.5f);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003DD5 File Offset: 0x00001FD5
		private bool CheckAdrenaline
		{
			get
			{
				return this.adrenaline <= -Entry.inst.unconsciousInfo.maxAdrenaline * Entry.inst.unconsciousInfo.negativeAdrenalineSleepAmount;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003E02 File Offset: 0x00002002
		private bool AllowDestabilize
		{
			get
			{
				return !this.paralyzed && this.unconsciousCoroutine == null && !this.creature.isKilled;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003E28 File Offset: 0x00002028
		private void Awake()
		{
			this.creature = base.GetComponentInParent<Creature>();
			this.creature.OnDamageEvent += new Creature.DamageEvent(this.Hit);
			this.creature.ragdoll.OnStateChange += new Ragdoll.StateChange(this.StateChange);
			this.creature.ragdoll.OnSliceEvent += new Ragdoll.SliceEvent(this.Slice);
			this.creature.OnResurrectEvent += new Creature.ResurrectEvent(this.Resurrect);
			this.creature.OnHealEvent += new Creature.HealEvent(this.Heal);
			this.creature.OnDespawnEvent += new Creature.DespawnEvent(this.Despawn);
			this.creature.OnKillEvent += new Creature.KillEvent(this.Kill);
			this.defaultAnimatorSpeed = this.creature.animator.speed;
			this.Reset();
			this.ConfigureValues();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003F18 File Offset: 0x00002118
		private void Kill(CollisionInstance collisionInstance, EventTime eventTime)
		{
			bool flag = eventTime == null && this.respiration;
			if (flag)
			{
				this.creature.brain.instance.GetModule<BrainModuleSpeak>(true).audioEnabled = false;
			}
			this.faking = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003F5C File Offset: 0x0000215C
		private void Despawn(EventTime eventTime)
		{
			foreach (Coroutine coroutine in this.bleedCoroutines)
			{
				base.StopCoroutine(coroutine);
			}
			foreach (EffectInstance effectInstance in this.bleedEffects)
			{
				effectInstance.Despawn();
			}
			this.bleedEffects.Clear();
			this.bleedCoroutines.Clear();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004010 File Offset: 0x00002210
		private void Heal(float heal, Creature healer)
		{
			this.painAccumulated -= heal;
			this.bloodLost -= heal;
			foreach (RagdollPart ragdollPart in this.partToInjury.Keys.ToList<RagdollPart>())
			{
				bool flag = !this.partToInjury[ragdollPart].broken;
				if (flag)
				{
					this.partToInjury[ragdollPart].damageAbsorbed -= heal * 0.1f * Time.fixedDeltaTime;
				}
			}
			foreach (RagdollPart ragdollPart2 in this.partToPain.Keys.ToList<RagdollPart>())
			{
				bool flag2 = this.partToPain[ragdollPart2] > 0f;
				if (flag2)
				{
					Dictionary<RagdollPart, float> dictionary = this.partToPain;
					RagdollPart ragdollPart3 = ragdollPart2;
					dictionary[ragdollPart3] -= heal * 0.4f * Time.fixedDeltaTime * (this.unbalanced ? 0.75f : 1f);
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000416C File Offset: 0x0000236C
		private void Resurrect(float newHealth, Creature resurrector)
		{
			this.Reset();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004178 File Offset: 0x00002378
		private void Slice(RagdollPart ragdollPart, EventTime eventTime)
		{
			base.StartCoroutine(this.InflictBleed(Entry.inst.bleedInfo.dismemberBleedHealthAway, 5000f, null));
			this.PartBroken(ragdollPart);
			ragdollPart.springPositionMultiplier = 0f;
			ragdollPart.springRotationMultiplier = 0f;
			bool flag = this.creature.handRight.grabbedHandle;
			if (flag)
			{
				this.creature.handRight.UnGrab(false);
			}
			bool flag2 = this.creature.handLeft.grabbedHandle;
			if (flag2)
			{
				this.creature.handLeft.UnGrab(false);
			}
			ragdollPart.damperPositionMultiplier = 1E+13f;
			ragdollPart.damperRotationMultiplier = 1E+13f;
			bool flag3 = ragdollPart.type == 2 || ragdollPart.type == 1;
			if (flag3)
			{
				this.decapped = true;
			}
			bool flag4 = ragdollPart.type == 8 || ragdollPart.type == 32;
			if (flag4)
			{
				foreach (BrainData.Module module in this.creature.brain.instance.modules)
				{
					module.useArmLeft = false;
				}
			}
			bool flag5 = ragdollPart.type == 16 || ragdollPart.type == 64;
			if (flag5)
			{
				foreach (BrainData.Module module2 in this.creature.brain.instance.modules)
				{
					module2.useArmRight = false;
				}
			}
			bool flag6 = ragdollPart.type == 128 || ragdollPart.type == 256;
			if (flag6)
			{
				foreach (BrainData.Module module3 in this.creature.brain.instance.modules)
				{
					module3.useLegs = false;
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000043B4 File Offset: 0x000025B4
		private void StateChange(Ragdoll.State previousState, Ragdoll.State newState, Ragdoll.PhysicStateChange physicsStateChange, EventTime time)
		{
			bool survivableDismemberment = Entry.inst.generalInfo.survivableDismemberment;
			if (survivableDismemberment)
			{
				foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts.Where((RagdollPart i) => i.name.Contains("Right") || i.name.Contains("Left")))
				{
					ragdollPart.data.sliceForceKill = false;
				}
			}
			this.ConfigureValues();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004454 File Offset: 0x00002654
		private void ConfigureValues()
		{
			bool flag = !Entry.inst.ragdollInfo.sectoryChangeRagdollWeights;
			if (!flag)
			{
				try
				{
					foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts)
					{
						RagdollInfo.Info info = Entry.inst.ragdollInfo.GetInfo(ragdollPart.name);
						ragdollPart.rb.drag = info.drag;
						ragdollPart.rb.mass = info.mass;
						ragdollPart.rb.angularDrag = info.angularDrag;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000452C File Offset: 0x0000272C
		private void Hit(CollisionInstance collisionInstance)
		{
			try
			{
				bool flag = Level.current.data.id.Contains("Dungeon") && this.creature.brain.state == 0;
				if (flag)
				{
					this.creature.brain.SetState(3);
				}
				bool flag2 = collisionInstance.damageStruct.damageType == 0;
				if (!flag2)
				{
					base.StartCoroutine(this.InjectAdrenaline(Entry.inst.unconsciousInfo.maxAdrenaline * Entry.inst.unconsciousInfo.adrenalineGainPercentageHit, collisionInstance.damageStruct.damage * 5f));
					bool pressureDismemberment = Entry.inst.generalInfo.pressureDismemberment;
					if (pressureDismemberment)
					{
						this.DrawDismemberPart(collisionInstance);
					}
					float num = collisionInstance.damageStruct.damage / Entry.inst.generalInfo.ScriptDifficulty;
					bool flag3 = !this.faking && !this.creature.isKilled && this.creature.currentHealth - num < this.randomFakeValue;
					if (flag3)
					{
						this.creature.Kill(new CollisionInstance(default(DamageStruct), null, null)
						{
							targetColliderGroup = this.creature.ragdoll.targetPart.colliderGroup
						});
						this.faking = true;
					}
					bool flag4 = this.faking && this.creature.isKilled;
					if (flag4)
					{
						bool flag5 = this.randomFakeValue > num;
						if (flag5)
						{
							this.randomFakeValue -= num * Mathf.Abs(Entry.inst.generalInfo.fakingDamageReduction / 100f - 1f) / Entry.inst.generalInfo.ScriptDifficulty;
						}
						else
						{
							this.faking = false;
							foreach (RagdollPart ragdollPart in this.partToInjury.Keys.Where((RagdollPart i) => this.partToInjury[i].broken))
							{
								this.PartBroken(ragdollPart);
							}
						}
					}
					this.ConfigureValues();
					RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
					bool flag6 = hitRagdollPart != null;
					if (flag6)
					{
						float num2 = (((collisionInstance.damageStruct.damager.penetrationDepth >= 0.055f && collisionInstance.damageStruct.damageType == 2) || collisionInstance.damageStruct.damager.data.id.Contains("Axe")) ? (Entry.inst.generalInfo.axeDamageMult / Entry.inst.generalInfo.ScriptDifficulty) : 0f);
						float num3 = ((collisionInstance.damageStruct.damageType == 3) ? (Entry.inst.generalInfo.bluntDamageMult / Entry.inst.generalInfo.ScriptDifficulty) : 0f);
						float num4 = 0f;
						Damager damager = collisionInstance.damageStruct.damager;
						Item item;
						if (damager == null)
						{
							item = null;
						}
						else
						{
							CollisionHandler collisionHandler = damager.collisionHandler;
							item = ((collisionHandler != null) ? collisionHandler.item : null);
						}
						Item item2 = item;
						bool flag7 = item2 != null;
						if (flag7)
						{
							bool isFlying = item2.isFlying;
							if (isFlying)
							{
								num4 += Entry.inst.generalInfo.rangedDamageMult / Entry.inst.generalInfo.ScriptDifficulty;
							}
						}
						float num5 = num * this.GetPainMultiplier(hitRagdollPart.name, collisionInstance.damageStruct.damageType);
						this.painAccumulated += num5;
						this.bloodLost += num5 * Entry.inst.unconsciousInfo.damageInfluenceBloodLossMult;
						Creature creature = this.creature;
						DamageStruct damageStruct = new DamageStruct(0, (collisionInstance.damageStruct.damage - num) * (num2 + num3 + num4));
						damageStruct.hitRagdollPart = hitRagdollPart;
						damageStruct.damageType = 0;
						creature.Damage(new CollisionInstance(damageStruct, null, null));
						float num6 = num * (this.GetInjuryMultiplier(hitRagdollPart.name, collisionInstance.damageStruct.damageType) + num2 + num4 + num3);
						float num7 = num * (this.GetBleedMultiplier(hitRagdollPart.name, collisionInstance.damageStruct.damageType, collisionInstance) + num2 + num4);
						bool flag8 = Entry.inst.injuryInfo.injurySystem && num6 >= Entry.inst.injuryInfo.injuryMinAtk;
						if (flag8)
						{
							bool flag9 = this.partToInjury.ContainsKey(hitRagdollPart);
							if (flag9)
							{
								this.partToInjury[hitRagdollPart].damageAbsorbed += num6;
								bool flag10 = this.partToInjury[hitRagdollPart].damageAbsorbed >= Entry.inst.injuryInfo.maxInjury * Entry.inst.generalInfo.ScriptDifficulty && !this.partToInjury[hitRagdollPart].broken;
								if (flag10)
								{
									this.PartBroken(hitRagdollPart);
									this.partToInjury[hitRagdollPart].broken = true;
								}
							}
							else
							{
								bool flag11 = num6 >= Entry.inst.injuryInfo.maxInjury * Entry.inst.generalInfo.ScriptDifficulty;
								this.partToInjury.Add(hitRagdollPart, new Systems.Injury(num6, flag11));
								bool flag12 = flag11;
								if (flag12)
								{
									this.PartBroken(hitRagdollPart);
								}
							}
						}
						bool flag13 = this.partToPain.ContainsKey(hitRagdollPart);
						if (flag13)
						{
							Dictionary<RagdollPart, float> dictionary = this.partToPain;
							RagdollPart ragdollPart2 = hitRagdollPart;
							dictionary[ragdollPart2] += num5;
						}
						else
						{
							this.partToPain.Add(hitRagdollPart, num5);
						}
						bool flag14 = collisionInstance.damageStruct.damageType == 1 && hitRagdollPart.type == 1 && collisionInstance.damageStruct.penetrationDepth > Entry.inst.generalInfo.skullPenetrateKillDepth / 100f;
						if (flag14)
						{
							this.creature.Kill(new CollisionInstance(default(DamageStruct), null, null)
							{
								targetColliderGroup = this.creature.ragdoll.targetPart.colliderGroup
							});
							this.faking = false;
						}
						bool flag15 = Entry.inst.bleedInfo.bleeding && (collisionInstance.damageStruct.damageType == 2 || collisionInstance.damageStruct.damageType == 1) && num7 >= Entry.inst.bleedInfo.bleedMinimum * Entry.inst.generalInfo.ScriptDifficulty;
						if (flag15)
						{
							base.StartCoroutine(this.InflictBleed(num7 * Entry.inst.bleedInfo.bleedPercentage / 100f / Entry.inst.bleedInfo.bleedTime / Entry.inst.generalInfo.ScriptDifficulty, Entry.inst.bleedInfo.bleedTime, Entry.inst.bleedData.Spawn(hitRagdollPart.transform.position, hitRagdollPart.transform.rotation, hitRagdollPart.transform, null, true, null, false, Array.Empty<Type>())));
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004C64 File Offset: 0x00002E64
		private IEnumerator Unconscious()
		{
			bool flag = !Entry.inst.unconsciousInfo.unconsciousEnabled;
			if (flag)
			{
				yield break;
			}
			BrainModuleDetection detection = this.creature.brain.instance.GetModule<BrainModuleDetection>(false);
			bool originalHear = detection.canHear;
			float originalFovH = detection.sightDetectionHorizontalFov;
			float originalFovV = detection.sightDetectionVerticalFov;
			detection.canHear = false;
			detection.sightDetectionHorizontalFov = 0f;
			detection.sightDetectionVerticalFov = 0f;
			this.creature.autoEyeClipsActive = false;
			this.creature.PlayEyeClip("DeathSad");
			this.creature.brain.isMuffled = true;
			this.creature.ragdoll.SetState(0);
			this.creature.brain.AddNoStandUpModifier(this);
			bool flag2 = !Entry.inst.unconsciousInfo.holdWeaponsWhileAsleep;
			if (flag2)
			{
				this.creature.handLeft.TryRelease();
				this.creature.handRight.TryRelease();
			}
			while ((this.CheckBleedUnconscious || this.CheckPainUnconscious || this.CheckAdrenaline) && !this.creature.isKilled)
			{
				bool flag3 = this.painAccumulated > 0f;
				if (flag3)
				{
					this.painAccumulated -= Entry.inst.generalInfo.enemyRecoveryRate * Time.fixedDeltaTime;
				}
				bool flag4 = this.bloodLost > 0f;
				if (flag4)
				{
					this.bloodLost -= Entry.inst.generalInfo.enemyRecoveryRate * Time.fixedDeltaTime;
				}
				bool flag5 = this.adrenaline < 0f;
				if (flag5)
				{
					this.adrenaline -= Entry.inst.generalInfo.enemyRecoveryRate * 0.2f * Time.fixedDeltaTime;
				}
				yield return null;
			}
			this.creature.brain.RemoveNoStandUpModifier(this);
			detection.canHear = originalHear;
			this.creature.autoEyeClipsActive = true;
			detection.sightDetectionHorizontalFov = originalFovH;
			detection.sightDetectionVerticalFov = originalFovV;
			this.creature.brain.isMuffled = false;
			this.unconsciousCoroutine = null;
			yield break;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004C73 File Offset: 0x00002E73
		public IEnumerator InjectAdrenaline(float adrenaline, float overTime)
		{
			float perSecond = adrenaline / overTime;
			while (overTime > 0f && this.adrenaline < Entry.inst.unconsciousInfo.maxAdrenaline && this.creature.loaded && !this.creature.isKilled)
			{
				overTime -= Time.deltaTime;
				this.adrenaline += perSecond * Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004C90 File Offset: 0x00002E90
		public IEnumerator Paralysis()
		{
			this.paralyzed = true;
			while (!this.creature.isKilled)
			{
				foreach (RagdollPart part in this.creature.ragdoll.parts)
				{
					bool flag = this.partToPain.ContainsKey(part);
					if (flag)
					{
						this.partToPain[part] = Entry.inst.unconsciousInfo.maxLimbPain;
					}
					else
					{
						this.partToPain.Add(part, Entry.inst.unconsciousInfo.maxLimbPain);
					}
					part = null;
				}
				List<RagdollPart>.Enumerator enumerator = default(List<RagdollPart>.Enumerator);
				this.creature.ragdoll.SetState(0);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004C9F File Offset: 0x00002E9F
		private IEnumerator InflictBleed(float bleedDamage, float bleedTime, EffectInstance effect = null)
		{
			bool flag = !Entry.inst.bleedInfo.bleeding;
			if (flag)
			{
				yield break;
			}
			bool flag2 = effect != null;
			if (flag2)
			{
				effect.onEffectFinished += delegate(EffectInstance effect1)
				{
					effect1.Play(0, false);
				};
				this.bleedEffects.Add(effect);
			}
			if (effect != null)
			{
				effect.Play(0, false);
			}
			while (bleedTime > 0f)
			{
				bleedTime -= Time.deltaTime;
				bool flag3 = this.creature.currentHealth > 0f;
				if (!flag3)
				{
					this.creature.Kill(new CollisionInstance(default(DamageStruct), null, null)
					{
						sourceColliderGroup = this.creature.ragdoll.headPart.colliderGroup,
						casterHand = this.creature.lastInteractionCreature.handRight.caster
					});
					if (effect != null)
					{
						effect.Despawn();
					}
					yield break;
				}
				float damage = bleedDamage * Time.deltaTime;
				this.creature.currentHealth -= damage;
				this.bloodLost += damage;
				this.painAccumulated += damage * Entry.inst.unconsciousInfo.bleedingInfluencePainMult;
				yield return null;
			}
			if (effect != null)
			{
				effect.Despawn();
			}
			yield break;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004CC4 File Offset: 0x00002EC4
		private void DrawDismemberPart(CollisionInstance collisionInstance)
		{
			Systems.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.collisionInstance = collisionInstance;
			bool flag = CS$<>8__locals1.collisionInstance.damageStruct.penetration == 1 && CS$<>8__locals1.collisionInstance.damageStruct.damageType == 2 && CS$<>8__locals1.collisionInstance.damageStruct.damager.data.dismembermentAllowed;
			if (flag)
			{
				RagdollPart hitRagdollPart = CS$<>8__locals1.collisionInstance.damageStruct.hitRagdollPart;
				bool isSliced = hitRagdollPart.isSliced;
				if (!isSliced)
				{
					RagdollPart ragdollPart = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "LeftHand");
					RagdollPart ragdollPart2 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "RightHand");
					RagdollPart ragdollPart3 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "LeftForeArm");
					RagdollPart ragdollPart4 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "RightForeArm");
					RagdollPart ragdollPart5 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "LeftLeg");
					RagdollPart ragdollPart6 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "RightLeg");
					RagdollPart ragdollPart7 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "LeftFoot");
					RagdollPart ragdollPart8 = this.creature.ragdoll.parts.Find((RagdollPart temp) => temp.name == "RightFoot");
					Vector3 vector;
					Vector3 vector2;
					ragdollPart.GetSlicePositionAndDirection(ref vector, ref vector2);
					Vector3 vector3;
					Vector3 vector4;
					ragdollPart2.GetSlicePositionAndDirection(ref vector3, ref vector4);
					Vector3 vector5;
					Vector3 vector6;
					ragdollPart3.GetSlicePositionAndDirection(ref vector5, ref vector6);
					Vector3 vector7;
					Vector3 vector8;
					ragdollPart4.GetSlicePositionAndDirection(ref vector7, ref vector8);
					Vector3 vector9;
					Vector3 vector10;
					ragdollPart5.GetSlicePositionAndDirection(ref vector9, ref vector10);
					Vector3 vector11;
					Vector3 vector12;
					ragdollPart6.GetSlicePositionAndDirection(ref vector11, ref vector12);
					Vector3 vector13;
					Vector3 vector14;
					ragdollPart7.GetSlicePositionAndDirection(ref vector13, ref vector14);
					Vector3 vector15;
					Vector3 vector16;
					ragdollPart8.GetSlicePositionAndDirection(ref vector15, ref vector16);
					Vector3 vector17;
					Vector3 vector18;
					hitRagdollPart.GetSlicePositionAndDirection(ref vector17, ref vector18);
					string name = hitRagdollPart.name;
					string text = name;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num <= 2449536012U)
					{
						if (num <= 928972447U)
						{
							if (num != 819656463U)
							{
								if (num == 928972447U)
								{
									if (text == "RightForeArm")
									{
										bool flag2 = ragdollPart2.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(64, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector3).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
										if (flag2)
										{
											this.creature.ragdoll.TrySlice(ragdollPart2);
										}
										bool flag3 = hitRagdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(16, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector17).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
										if (flag3)
										{
											this.creature.ragdoll.TrySlice(hitRagdollPart);
										}
									}
								}
							}
							else if (text == "LeftUpLeg")
							{
								bool flag4 = ragdollPart5.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(128, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector9).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
								if (flag4)
								{
									this.creature.ragdoll.TrySlice(ragdollPart5);
								}
							}
						}
						else if (num != 977255260U)
						{
							if (num == 2449536012U)
							{
								if (text == "LeftLeg")
								{
									bool flag5 = ragdollPart7.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(512, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector13).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
									if (flag5)
									{
										this.creature.ragdoll.TrySlice(ragdollPart7);
									}
									bool flag6 = hitRagdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(128, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector17).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
									if (flag6)
									{
										this.creature.ragdoll.TrySlice(hitRagdollPart);
									}
								}
							}
						}
						else if (text == "RightUpLeg")
						{
							bool flag7 = ragdollPart6.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(256, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector11).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
							if (flag7)
							{
								this.creature.ragdoll.TrySlice(ragdollPart6);
							}
						}
					}
					else if (num <= 3001187991U)
					{
						if (num != 2925602325U)
						{
							if (num == 3001187991U)
							{
								if (text == "RightLeg")
								{
									bool flag8 = ragdollPart8.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(1024, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector15).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
									if (flag8)
									{
										this.creature.ragdoll.TrySlice(ragdollPart8);
									}
									bool flag9 = hitRagdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(256, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector17).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
									if (flag9)
									{
										this.creature.ragdoll.TrySlice(hitRagdollPart);
									}
								}
							}
						}
						else if (text == "RightArm")
						{
							bool flag10 = ragdollPart4.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(16, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector7).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
							if (flag10)
							{
								this.creature.ragdoll.TrySlice(ragdollPart4);
							}
						}
					}
					else if (num != 3119934960U)
					{
						if (num != 3425019186U)
						{
							if (num == 4018002826U)
							{
								if (text == "LeftArm")
								{
									bool flag11 = ragdollPart3.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(8, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector5).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
									if (flag11)
									{
										this.creature.ragdoll.TrySlice(ragdollPart3);
									}
								}
							}
						}
						else if (text == "Neck")
						{
							bool flag12 = hitRagdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(2, ref CS$<>8__locals1) && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
							if (flag12)
							{
								this.creature.ragdoll.TrySlice(hitRagdollPart);
							}
						}
					}
					else if (text == "LeftForeArm")
					{
						bool flag13 = ragdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(32, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
						if (flag13)
						{
							this.creature.ragdoll.TrySlice(ragdollPart);
						}
						bool flag14 = hitRagdollPart.sliceAllowed && Systems.<DrawDismemberPart>g__FailDismemberment|46_0(8, ref CS$<>8__locals1) && (CS$<>8__locals1.collisionInstance.contactPoint - vector17).sqrMagnitude < 0.0625f && CS$<>8__locals1.collisionInstance.damageStruct.damage > Entry.inst.generalInfo.drawDismembermentDamageRequirement;
						if (flag14)
						{
							this.creature.ragdoll.TrySlice(hitRagdollPart);
						}
					}
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000056B8 File Offset: 0x000038B8
		private float GetInjuryMultiplier(string part, DamageType type)
		{
			float num = Entry.inst.injuryInfo.partToInjuryMults.Single((PartToMultiplier i) => i.partName == part).multiplier;
			bool flag = num == 0f;
			if (flag)
			{
				Debug.LogWarning("Sectory - Caught injury multiplier at 0. If this is unintentional, there is a problem with your jsons. The multiplier was forced to 1 for this collisionInstance.");
				num = 1f;
			}
			switch (type)
			{
			case 1:
				num *= Entry.inst.injuryInfo.partToInjuryMults.Single((PartToMultiplier i) => i.partName == part).multiplierPierce;
				break;
			case 2:
				num *= Entry.inst.injuryInfo.partToInjuryMults.Single((PartToMultiplier i) => i.partName == part).multiplierSlash;
				break;
			case 3:
				num *= Entry.inst.injuryInfo.partToInjuryMults.Single((PartToMultiplier i) => i.partName == part).multiplierBlunt;
				break;
			case 4:
				num *= Entry.inst.injuryInfo.partToInjuryMults.Single((PartToMultiplier i) => i.partName == part).multiplierEnergy;
				break;
			}
			return num / Entry.inst.generalInfo.ScriptDifficulty;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000057FC File Offset: 0x000039FC
		private float GetPainMultiplier(string part, DamageType type)
		{
			float num = Entry.inst.unconsciousInfo.partToPainMults.Single((PartToMultiplier i) => i.partName == part).multiplier;
			bool flag = num == 0f;
			if (flag)
			{
				Debug.LogWarning("Sectory - Caught injury multiplier at 0. If this is unintentional, there is a problem with your jsons. The multiplier was forced to 1 for this collisionInstance.");
				num = 1f;
			}
			switch (type)
			{
			case 1:
				num *= Entry.inst.unconsciousInfo.partToPainMults.Single((PartToMultiplier i) => i.partName == part).multiplierPierce;
				break;
			case 2:
				num *= Entry.inst.unconsciousInfo.partToPainMults.Single((PartToMultiplier i) => i.partName == part).multiplierSlash;
				break;
			case 3:
				num *= Entry.inst.unconsciousInfo.partToPainMults.Single((PartToMultiplier i) => i.partName == part).multiplierBlunt;
				break;
			case 4:
				num *= Entry.inst.unconsciousInfo.partToPainMults.Single((PartToMultiplier i) => i.partName == part).multiplierEnergy;
				break;
			}
			return num / Entry.inst.generalInfo.ScriptDifficulty;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005940 File Offset: 0x00003B40
		private float GetBleedMultiplier(string part, DamageType type, CollisionInstance instance)
		{
			float num = Entry.inst.bleedInfo.partToBleedMults.Single((PartToMultiplier i) => i.partName == part).multiplier;
			switch (type)
			{
			case 1:
				num *= Entry.inst.bleedInfo.partToBleedMults.Single((PartToMultiplier i) => i.partName == part).multiplierPierce;
				break;
			case 2:
				num *= Entry.inst.bleedInfo.partToBleedMults.Single((PartToMultiplier i) => i.partName == part).multiplierSlash;
				break;
			case 3:
				num *= Entry.inst.bleedInfo.partToBleedMults.Single((PartToMultiplier i) => i.partName == part).multiplierBlunt;
				break;
			case 4:
				num *= Entry.inst.bleedInfo.partToBleedMults.Single((PartToMultiplier i) => i.partName == part).multiplierEnergy;
				break;
			}
			bool flag = num == 0f;
			if (flag)
			{
				Debug.LogWarning("Sectory - Caught bleed multiplier at 0. If this is unintentional, there is a problem with your jsons. The multiplier was forced to 1 for this collisionInstance.");
				num = 1f;
			}
			Vector3 contactPoint = instance.contactPoint;
			bool internalsEnabled = Entry.inst.internalsInfo.internalsEnabled;
			if (internalsEnabled)
			{
				foreach (InternalsInfo.Internal @internal in Entry.inst.internalsInfo.internals)
				{
					bool flag2 = instance.damageStruct.hitRagdollPart.name == @internal.host && (instance.damageStruct.hitRagdollPart.transform.TransformPoint(@internal.offset) - contactPoint).magnitude <= @internal.size && ((type == 1 && @internal.pierceAllowed) || (type == 2 && @internal.slashAllowed) || (type == 3 && @internal.bluntAllowed && instance.damageStruct.damage > @internal.durability * Entry.inst.internalsInfo.bluntTriggerInternalRatio)) && (@internal.bluntAllowed || @internal.penetrationDepth <= instance.damageStruct.penetrationDepth);
					if (flag2)
					{
						bool flag3 = this.internalToDurability.ContainsKey(@internal);
						if (flag3)
						{
							bool flag4 = this.internalToDurability[@internal].durability > 0f;
							if (flag4)
							{
								this.internalToDurability[@internal].durability -= instance.damageStruct.damage;
							}
							bool flag5 = this.internalToDurability[@internal].durability <= 0f;
							if (flag5)
							{
								this.internalToDurability[@internal].durability = 0.001f;
							}
						}
						else
						{
							bool flag6 = instance.damageStruct.damage > @internal.durability;
							this.internalToDurability.Add(@internal, new Systems.InternalInjury(false, flag6 ? 0f : (@internal.durability - instance.damageStruct.damage)));
						}
						float num2 = @internal.bleedMultiplier * Mathf.Abs(this.internalToDurability[@internal].durability / @internal.durability - 1f);
						num *= num2;
						GameObject gameObject = GameObject.Find(@internal.name + " Indicator");
						bool flag7 = gameObject != null;
						if (flag7)
						{
							base.StartCoroutine(InternalHelper.FlashRed(gameObject));
						}
						bool flag8 = this.internalToDurability[@internal].durability <= @internal.durability * 0.5f && !this.internalToDurability[@internal].abilityTriggered;
						if (flag8)
						{
							this.internalToDurability[@internal].abilityTriggered = true;
							switch (@internal.action)
							{
							case InternalInjuryAction.Respiration:
								this.respiration = true;
								base.StartCoroutine(this.InjectAdrenaline(-Entry.inst.unconsciousInfo.maxAdrenaline * 0.5f, Entry.inst.internalsInfo.respirationOxygenCutOffTimer));
								break;
							case InternalInjuryAction.Balance:
							{
								this.unbalanced = true;
								bool allowDestabilize = this.AllowDestabilize;
								if (allowDestabilize)
								{
									this.creature.ragdoll.SetState(1);
								}
								break;
							}
							case InternalInjuryAction.CriticalBleeding:
								base.StartCoroutine(this.InjectAdrenaline(-Entry.inst.unconsciousInfo.maxAdrenaline * @internal.bleedMultiplier, Entry.inst.internalsInfo.criticalBleedingDuration));
								break;
							case InternalInjuryAction.Paralysis:
								base.StartCoroutine(this.Paralysis());
								break;
							}
						}
					}
				}
			}
			return num / Entry.inst.generalInfo.ScriptDifficulty;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005E1C File Offset: 0x0000401C
		public void PartBroken(RagdollPart part)
		{
			bool flag = !Entry.inst.injuryInfo.injurySystem;
			if (!flag)
			{
				bool flag2 = part.type != 4;
				if (flag2)
				{
					foreach (RagdollPart ragdollPart in part.ragdoll.parts)
					{
						bool flag3 = !ragdollPart.parentPart;
						if (!flag3)
						{
							bool flag4 = ragdollPart.parentPart == part;
							if (flag4)
							{
								this.PartBroken(ragdollPart);
							}
						}
					}
				}
				bool flag5 = !this.partToInjury.ContainsKey(part);
				if (flag5)
				{
					this.partToInjury.Add(part, new Systems.Injury(0f, true));
				}
				else
				{
					this.partToInjury[part].broken = true;
				}
				EffectInstance effectInstance = null;
				RagdollPart.Type type = part.type;
				RagdollPart.Type type2 = type;
				if (type2 <= 128)
				{
					switch (type2)
					{
					case 1:
						effectInstance = Catalog.GetData<EffectData>("SkullFracture", true).Spawn(part.transform, true, null, false, Array.Empty<Type>());
						this.creature.mana.manaRegenMultiplier = -10f;
						base.StartCoroutine(this.InflictBleed(Entry.inst.bleedInfo.bleedPercentage / 100f * 10f / Entry.inst.generalInfo.ScriptDifficulty, Entry.inst.bleedInfo.bleedTime / Entry.inst.generalInfo.ScriptDifficulty, null));
						goto IL_3FD;
					case 2:
					{
						bool neckBreaking = Entry.inst.injuryInfo.neckBreaking;
						if (neckBreaking)
						{
							NeckSnapSystem component = this.creature.gameObject.GetComponent<NeckSnapSystem>();
							bool flag6 = component;
							if (flag6)
							{
								bool flag7 = !component.isSnapped;
								if (flag7)
								{
									CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(0, 50f + Entry.inst.generalInfo.CreatureAddHealth), null, null);
									collisionInstance.contactPoint = part.transform.position;
									this.creature.Damage(collisionInstance);
									bool flag8 = !this.decapped;
									if (flag8)
									{
										effectInstance = Catalog.GetData<EffectData>("NeckBreak", true).Spawn(part.transform, false, null, false, Array.Empty<Type>());
									}
									component.isSnapped = true;
								}
								else
								{
									effectInstance = Catalog.GetData<EffectData>("BoneBreak", true).Spawn(part.transform, false, null, false, Array.Empty<Type>());
								}
							}
							else
							{
								CollisionInstance collisionInstance2 = new CollisionInstance(new DamageStruct(0, 50f + Entry.inst.generalInfo.CreatureAddHealth), null, null);
								collisionInstance2.contactPoint = part.transform.position;
								this.creature.Damage(collisionInstance2);
								bool flag9 = !this.decapped;
								if (flag9)
								{
									effectInstance = Catalog.GetData<EffectData>("NeckBreak", true).Spawn(part.transform, false, null, false, Array.Empty<Type>());
								}
							}
						}
						goto IL_3FD;
					}
					case 3:
						goto IL_3DC;
					case 4:
						base.StartCoroutine(this.InflictBleed(Entry.inst.bleedInfo.bleedPercentage / 100f * 10f / Entry.inst.generalInfo.ScriptDifficulty, Entry.inst.bleedInfo.bleedTime / Entry.inst.generalInfo.ScriptDifficulty, null));
						effectInstance = Catalog.GetData<EffectData>("BoneBreak", true).Spawn(part.transform, true, null, false, Array.Empty<Type>());
						goto IL_3FD;
					default:
						if (type2 != 128)
						{
							goto IL_3DC;
						}
						break;
					}
				}
				else if (type2 != 256 && type2 != 512 && type2 != 1024)
				{
					goto IL_3DC;
				}
				effectInstance = Catalog.GetData<EffectData>("BoneBreak", true).Spawn(part.transform, true, null, false, Array.Empty<Type>());
				goto IL_3FD;
				IL_3DC:
				effectInstance = Catalog.GetData<EffectData>("BoneBreak", true).Spawn(part.transform, true, null, false, Array.Empty<Type>());
				IL_3FD:
				bool flag10 = effectInstance != null;
				if (flag10)
				{
					bool injurySounds = Entry.inst.injuryInfo.injurySounds;
					if (injurySounds)
					{
						effectInstance.Play(0, false);
					}
					effectInstance.onEffectFinished += new EffectInstance.EffectFinishEvent(this.Despawn);
				}
				this.creature.brain.instance.GetModule<BrainModuleDetection>(true);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000628C File Offset: 0x0000448C
		private void Stamina()
		{
			bool flag = this.creature == Player.currentCreature && !Entry.inst.generalInfo.playerEffected;
			if (flag)
			{
				Object.Destroy(this);
			}
			bool flag2 = !this.creature.isKilled && Entry.inst.generalInfo.enemyRecoveryRate > 0f && this.creature.currentHealth < this.creature.maxHealth;
			if (flag2)
			{
				this.creature.Heal(Entry.inst.generalInfo.enemyRecoveryRate * Time.deltaTime, this.creature);
			}
			bool flag3 = this.adrenaline > 0f;
			if (flag3)
			{
				this.adrenaline -= Time.deltaTime;
			}
			bool flag4 = this.adrenaline < 0f;
			if (flag4)
			{
				this.adrenaline += Time.deltaTime * 0.01f;
			}
			bool flag5 = this.adrenaline < -Entry.inst.unconsciousInfo.maxAdrenaline * 0.8f;
			if (flag5)
			{
				this.adrenaline = -Entry.inst.unconsciousInfo.maxAdrenaline * 0.8f;
			}
			bool flag6 = this.adrenaline > Entry.inst.unconsciousInfo.maxAdrenaline;
			if (flag6)
			{
				this.adrenaline = Entry.inst.unconsciousInfo.maxAdrenaline;
			}
			bool flag7 = this.unbalanced && this.AllowDestabilize;
			if (flag7)
			{
				this.creature.ragdoll.SetState(1);
			}
			bool flag8 = this.creature.isKilled && Entry.inst.generalInfo.cinematicDeath;
			if (flag8)
			{
				this.creature.animator.speed = 0.25f * this.defaultAnimatorSpeed;
			}
			bool flag9 = Entry.inst.unconsciousInfo.unconsciousEnabled && !this.creature.isKilled && this.unconsciousCoroutine == null && (this.CheckBleedUnconscious || this.CheckPainUnconscious || this.CheckAdrenaline);
			if (flag9)
			{
				this.unconsciousCoroutine = base.StartCoroutine(this.Unconscious());
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000064BC File Offset: 0x000046BC
		private void Update()
		{
			bool flag = !this.creature || !this.creature.loaded;
			if (!flag)
			{
				bool flag2 = !Entry.inst.generalInfo.deathAnimations && this.creature.isKilled;
				if (flag2)
				{
					this.creature.animator.speed = 1000f;
				}
				bool flag3 = this.faking && !this.creature.IsVisible() && this.fakeCoroutine == null;
				if (flag3)
				{
					this.fakeCoroutine = base.StartCoroutine(this.DeathFaking());
				}
				foreach (RagdollPart ragdollPart in this.partToInjury.Keys)
				{
					bool flag4 = (this.partToInjury[ragdollPart].broken || ragdollPart.isSliced || (this.partToPain.ContainsKey(ragdollPart) && this.partToPain[ragdollPart] > Entry.inst.unconsciousInfo.maxLimbPain)) && ragdollPart.type != 4;
					if (flag4)
					{
						ragdollPart.rb.useGravity = true;
						bool flag5 = (ragdollPart.type == 32 || ragdollPart.type == 8) && this.creature.handLeft.grabbedHandle;
						if (flag5)
						{
							this.creature.GetHand(1).UnGrab(false);
						}
						bool flag6 = (ragdollPart.type == 64 || ragdollPart.type == 16) && this.creature.handRight.grabbedHandle;
						if (flag6)
						{
							this.creature.GetHand(0).UnGrab(false);
						}
					}
					else
					{
						bool flag7 = !this.partToInjury[ragdollPart].broken && !this.creature.isKilled;
						if (flag7)
						{
							this.partToInjury[ragdollPart].damageAbsorbed -= Entry.inst.generalInfo.enemyRecoveryRate * 0.1f;
						}
					}
				}
				bool injurySystem = Entry.inst.injuryInfo.injurySystem;
				if (injurySystem)
				{
					foreach (RagdollPart ragdollPart2 in this.partToPain.Keys.Where((RagdollPart part) => this.partToPain[part] > Entry.inst.unconsciousInfo.maxLimbPain && part.type != 1 && part.type != 4))
					{
						this.creature.ragdoll.SetPinForceMultiplier(0f, 100f, 0f, 0f, true, true, ragdollPart2.type);
					}
					foreach (RagdollPart ragdollPart3 in this.partToInjury.Keys.Where((RagdollPart part) => this.partToInjury[part].broken && part.type != 1 && part.type != 4))
					{
						bool flag8 = !Entry.inst.injuryInfo.neckBreaking && ragdollPart3.type == 2;
						if (!flag8)
						{
							this.creature.ragdoll.SetPinForceMultiplier(0f, 100f, 0f, 0f, false, true, ragdollPart3.type);
						}
					}
				}
				bool allowDestabilize = this.AllowDestabilize;
				if (allowDestabilize)
				{
					bool flag9 = this.partToPain.Keys.Count((RagdollPart part) => this.partToPain[part] > Entry.inst.unconsciousInfo.maxLimbPain && (part.type == 128 || part.type == 256 || part.type == 512 || part.type == 1024)) > 1;
					if (flag9)
					{
						this.creature.ragdoll.SetState(1);
					}
					bool flag10 = this.partToInjury.Keys.Count((RagdollPart part) => this.partToInjury[part].broken && (part.type == 128 || part.type == 256 || part.type == 512 || part.type == 1024)) > 1;
					if (flag10)
					{
						this.creature.ragdoll.SetState(1);
					}
					foreach (RagdollPart ragdollPart4 in this.creature.ragdoll.parts)
					{
						bool flag11 = (ragdollPart4.isSliced || (ragdollPart4.parentPart && ragdollPart4.parentPart.isSliced)) && (ragdollPart4.type == 128 || ragdollPart4.type == 256 || ragdollPart4.type == 512 || ragdollPart4.type == 1024) && this.unconsciousCoroutine == null;
						if (flag11)
						{
							this.creature.ragdoll.SetState(1);
						}
					}
				}
				bool flag12 = (float)this.partToPain.Keys.Count((RagdollPart part) => this.partToPain[part] >= Entry.inst.unconsciousInfo.maxLimbPain) >= (float)this.creature.ragdoll.parts.Count * 0.8f && !this.paralyzed;
				if (flag12)
				{
					base.StartCoroutine(this.Paralysis());
				}
				this.Stamina();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00006A34 File Offset: 0x00004C34
		private IEnumerator DeathFaking()
		{
			bool resurrect = false;
			float timeLeft = 13.5f;
			while (timeLeft > 0f)
			{
				timeLeft -= Time.deltaTime;
				bool flag = !this.creature.IsVisible();
				if (flag)
				{
					resurrect = true;
					break;
				}
				yield return null;
			}
			bool flag2 = resurrect && this.faking;
			if (flag2)
			{
				this.creature.Resurrect(this.randomFakeValue * 4f, this.creature);
				this.creature.brain.Load(this.creature.brain.instance.id);
				this.faking = false;
				this.randomFakeValue = -1f;
				foreach (RagdollPart part in this.partToInjury.Keys.Where((RagdollPart i) => this.partToInjury[i].broken))
				{
					this.PartBroken(part);
					part = null;
				}
				IEnumerator<RagdollPart> enumerator = null;
			}
			this.faking = false;
			this.fakeCoroutine = null;
			yield break;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006A44 File Offset: 0x00004C44
		private void Despawn(EffectInstance effect)
		{
			bool flag = effect != null;
			if (flag)
			{
				effect.Despawn();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006A64 File Offset: 0x00004C64
		public void Reset()
		{
			this.faking = false;
			this.decapped = false;
			this.respiration = false;
			this.unbalanced = false;
			this.paralyzed = false;
			this.creature.currentHealth = this.creature.maxHealth;
			this.bloodLost = 0f;
			this.painAccumulated = 0f;
			this.adrenaline = 0f;
			bool flag = Entry.inst.generalInfo.deathFaking && Random.Range(0f, 100f) <= Entry.inst.generalInfo.deathFakingChance * Entry.inst.generalInfo.ScriptDifficulty;
			if (flag)
			{
				this.randomFakeValue = Random.Range(this.creature.maxHealth * 0.3f, this.creature.maxHealth * 0.8f);
			}
			this.creature.mana.manaRegenMultiplier = 1f;
			this.Despawn(0);
			this.partToPain.Clear();
			this.partToInjury.Clear();
			this.internalToDurability.Clear();
			bool flag2 = !Entry.inst.stealthInfo.useVanillaStealthSettingsInstead;
			if (flag2)
			{
				this.ConfigStealth();
			}
			this.creature.animator.speed = this.defaultAnimatorSpeed;
			foreach (RagdollPart.Type type in this.creature.ragdoll.parts.Select((RagdollPart i) => i.type))
			{
				this.creature.ragdoll.ResetPinForce(true, false, type);
			}
			bool liftFromAnywhere = Entry.inst.generalInfo.liftFromAnywhere;
			if (liftFromAnywhere)
			{
				HandleRagdoll[] componentsInChildren = this.creature.GetComponentsInChildren<HandleRagdoll>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					HandleRagdoll ragdollHandle = componentsInChildren[j];
					ragdollHandle.Grabbed += delegate(RagdollHand hand, Handle handle, EventTime eventTime)
					{
						this.creature.brain.SetState(3);
						NoiseManager.AddNoise(ragdollHandle.transform.position, 0.75f, hand.creature);
					};
					ragdollHandle.handleRagdollData.liftBehaviour = 1;
				}
			}
			bool cleanerDismemberment = Entry.inst.generalInfo.cleanerDismemberment;
			if (cleanerDismemberment)
			{
				foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts)
				{
					ragdollPart.data.sliceSeparationForce = 0f;
				}
			}
			this.moduleParry = this.creature.brain.instance.GetModule<BrainModuleDefense>(true);
			bool flag3 = this.defaultParryMaxSpeed == 0f;
			if (flag3)
			{
				this.defaultParryMaxSpeed = this.moduleParry.armMaxForceMultiplier;
			}
			bool flag4 = this.defaultParrySpring == 0f;
			if (flag4)
			{
				this.defaultParrySpring = this.moduleParry.armSpringMultiplier;
			}
			this.moduleParry.armMaxForceMultiplier = this.defaultParryMaxSpeed * Entry.inst.aiSettings.npcParryMultiplier;
			this.moduleParry.armSpringMultiplier = this.defaultParrySpring * Entry.inst.aiSettings.npcParryMultiplier;
			this.moduleDodge = this.creature.brain.instance.GetModule<BrainModuleDodge>(true);
			this.moduleDodge.dodgeChance = Entry.inst.aiSettings.dodgeChance / 100f;
			this.moduleDodge.dodgeWhenGrabbed = Entry.inst.aiSettings.canDodgeWhenGrabbed;
			this.moduleMelee = this.creature.brain.instance.GetModule<BrainModuleMelee>(true);
			this.moduleMelee.animationSpeedMultiplier = Entry.inst.aiSettings.attackAnimationSpeedMult;
			this.moduleMove = this.creature.brain.instance.GetModule<BrainModuleMove>(true);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006E74 File Offset: 0x00005074
		private void ConfigStealth()
		{
			BrainModuleDetection module = this.creature.brain.instance.GetModule<BrainModuleDetection>(true);
			module.hearMaxDistance = Entry.inst.stealthInfo.averageHearRange + (float)Random.Range(-2, 2);
			module.sightDetectionHorizontalFov = Entry.inst.stealthInfo.creatureFovInDegrees;
			module.sightDetectionVerticalFov = Entry.inst.stealthInfo.creatureSightHeightMax;
			module.hearRememberDuration = Entry.inst.stealthInfo.averageInvestigationDuration;
			module.canHear = Entry.inst.stealthInfo.enemiesCanHear;
			bool flag = Player.currentCreature;
			if (flag)
			{
				Player.currentCreature.brain.instance.GetModule<BrainModuleSightable>(true).sightMaxDistance = Entry.inst.stealthInfo.sightMaxRange;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00006F98 File Offset: 0x00005198
		[CompilerGenerated]
		internal static bool <DrawDismemberPart>g__FailDismemberment|46_0(RagdollPart.Type type, ref Systems.<>c__DisplayClass46_0 A_1)
		{
			if (type <= 32)
			{
				if (type <= 8)
				{
					if (type != 2)
					{
						if (type != 8)
						{
							goto IL_232;
						}
					}
					else
					{
						bool flag = (float)Random.Range(0, 100) < Entry.inst.generalInfo.neckSliceDifficulty * 100f - A_1.collisionInstance.pressureRelativeVelocity.magnitude / 100f * Entry.inst.generalInfo.slicingDifficultyDamageOverride;
						if (flag)
						{
							return true;
						}
						goto IL_232;
					}
				}
				else if (type != 16)
				{
					if (type != 32)
					{
						goto IL_232;
					}
					goto IL_186;
				}
				bool flag2 = (float)Random.Range(0, 100) / 150f < Entry.inst.generalInfo.elbowSliceDifficulty * 100f - A_1.collisionInstance.pressureRelativeVelocity.magnitude / 100f * Entry.inst.generalInfo.slicingDifficultyDamageOverride;
				if (flag2)
				{
					return true;
				}
				goto IL_232;
			}
			else
			{
				if (type <= 128)
				{
					if (type == 64)
					{
						goto IL_186;
					}
					if (type != 128)
					{
						goto IL_232;
					}
				}
				else if (type != 256)
				{
					if (type != 512 && type != 1024)
					{
						goto IL_232;
					}
					bool flag3 = (float)Random.Range(0, 100) < Entry.inst.generalInfo.ankleSliceDifficulty * 100f - A_1.collisionInstance.pressureRelativeVelocity.magnitude / 100f * Entry.inst.generalInfo.slicingDifficultyDamageOverride;
					if (flag3)
					{
						return true;
					}
					goto IL_232;
				}
				bool flag4 = (float)Random.Range(0, 100) < Entry.inst.generalInfo.kneeSliceDifficulty * 100f - A_1.collisionInstance.pressureRelativeVelocity.magnitude / 100f * Entry.inst.generalInfo.slicingDifficultyDamageOverride;
				if (flag4)
				{
					return true;
				}
				goto IL_232;
			}
			IL_186:
			bool flag5 = (float)Random.Range(0, 100) < Entry.inst.generalInfo.wristSliceDifficulty * 100f - A_1.collisionInstance.pressureRelativeVelocity.magnitude / 100f * Entry.inst.generalInfo.slicingDifficultyDamageOverride;
			if (flag5)
			{
				return true;
			}
			IL_232:
			return false;
		}

		// Token: 0x0400009B RID: 155
		public HashSet<EffectInstance> bleedEffects = new HashSet<EffectInstance>();

		// Token: 0x0400009C RID: 156
		public Dictionary<RagdollPart, Systems.Injury> partToInjury = new Dictionary<RagdollPart, Systems.Injury>();

		// Token: 0x0400009D RID: 157
		public Dictionary<RagdollPart, float> partToPain = new Dictionary<RagdollPart, float>();

		// Token: 0x0400009E RID: 158
		public Dictionary<InternalsInfo.Internal, Systems.InternalInjury> internalToDurability = new Dictionary<InternalsInfo.Internal, Systems.InternalInjury>();

		// Token: 0x0400009F RID: 159
		public float bloodLost;

		// Token: 0x040000A0 RID: 160
		public float painAccumulated;

		// Token: 0x040000A1 RID: 161
		public float adrenaline;

		// Token: 0x040000A2 RID: 162
		public bool faking;

		// Token: 0x040000A3 RID: 163
		public bool decapped;

		// Token: 0x040000A4 RID: 164
		public bool unbalanced;

		// Token: 0x040000A5 RID: 165
		public bool respiration;

		// Token: 0x040000A6 RID: 166
		public bool paralyzed;

		// Token: 0x040000A7 RID: 167
		private Creature creature;

		// Token: 0x040000A8 RID: 168
		private float randomFakeValue;

		// Token: 0x040000A9 RID: 169
		private float defaultSpeedMult;

		// Token: 0x040000AA RID: 170
		private float defaultAnimatorSpeed;

		// Token: 0x040000AB RID: 171
		private float defaultParryMaxSpeed;

		// Token: 0x040000AC RID: 172
		private float defaultParrySpring;

		// Token: 0x040000AD RID: 173
		private Coroutine fakeCoroutine;

		// Token: 0x040000AE RID: 174
		private Coroutine unconsciousCoroutine;

		// Token: 0x040000AF RID: 175
		private HashSet<Coroutine> bleedCoroutines = new HashSet<Coroutine>();

		// Token: 0x040000B0 RID: 176
		private BrainModuleMove moduleMove;

		// Token: 0x040000B1 RID: 177
		private BrainModuleDodge moduleDodge;

		// Token: 0x040000B2 RID: 178
		private BrainModuleDefense moduleParry;

		// Token: 0x040000B3 RID: 179
		private BrainModuleMelee moduleMelee;

		// Token: 0x02000032 RID: 50
		public class Injury
		{
			// Token: 0x060000B9 RID: 185 RVA: 0x000086DF File Offset: 0x000068DF
			public Injury(float damageAbsorbed, bool broken)
			{
				this.broken = broken;
				this.damageAbsorbed = damageAbsorbed;
			}

			// Token: 0x0400010E RID: 270
			public bool broken;

			// Token: 0x0400010F RID: 271
			public float damageAbsorbed;
		}

		// Token: 0x02000033 RID: 51
		public class InternalInjury
		{
			// Token: 0x060000BA RID: 186 RVA: 0x000086F7 File Offset: 0x000068F7
			public InternalInjury(bool abilityTriggered, float durability)
			{
				this.abilityTriggered = abilityTriggered;
				this.durability = durability;
			}

			// Token: 0x04000110 RID: 272
			public bool abilityTriggered;

			// Token: 0x04000111 RID: 273
			public float durability;
		}
	}
}
