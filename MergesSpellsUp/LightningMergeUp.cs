using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace MergesSpellsUp
{
	// Token: 0x02000007 RID: 7
	public class LightningMergeUp : SpellCastCharge
	{
		// Token: 0x0600008C RID: 140 RVA: 0x0000731C File Offset: 0x0000551C
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.beamEffectData = Catalog.GetData<EffectData>("SpellLightningMergeBeam", true);
			this.imbueSpell = Catalog.GetData<SpellCastCharge>("Lightning", true);
			this.chainEffectData = Catalog.GetData<EffectData>("SpellLightningBolt", true);
			this.electrocuteEffectData = Catalog.GetData<EffectData>("ImbueLightningRagdoll", true);
			this.beamImpactEffectData = Catalog.GetData<EffectData>("SpellLightningMergeBeamImpact", true);
			this.collidersHit = new Collider[20];
			this.beamForceCurve.postWrapMode = 2;
			this.creaturesHit = new HashSet<Creature>();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000073AC File Offset: 0x000055AC
		public override void Unload()
		{
			base.Unload();
			LightningHookMergeUp lightningHook = this.hookedCreature;
			bool flag = lightningHook != null;
			if (flag)
			{
				lightningHook.Unhook();
			}
			EffectInstance effectInstance = this.beamEffect;
			bool flag2 = effectInstance != null;
			if (flag2)
			{
				effectInstance.End(false, -1f);
			}
			this.beamEffect = null;
			bool flag3 = this.beamImpactEffect != null;
			if (flag3)
			{
				this.beamImpactEffect.End(false, -1f);
				this.beamImpactEffect = null;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000742C File Offset: 0x0000562C
		public override void Fire(bool active)
		{
			base.Fire(active);
			if (active)
			{
				this.currentCharge = 0.35f;
				EffectInstance effectInstance = this.beamEffect;
				bool flag = effectInstance != null;
				if (flag)
				{
					effectInstance.End(false, -1f);
				}
				this.beamEffect = null;
				bool flag2 = this.beamStart == null;
				if (flag2)
				{
					this.beamStart = new GameObject("Beam Target").transform;
				}
				bool flag3 = this.beamHitPoint == null;
				if (flag3)
				{
					this.beamHitPoint = new GameObject("Beam Hit").transform;
				}
			}
			else
			{
				EffectInstance effectInstance2 = this.beamEffect;
				bool flag4 = effectInstance2 != null;
				if (flag4)
				{
					effectInstance2.End(false, -1f);
				}
				this.beamEffect = null;
				foreach (Creature creature in this.creaturesHit)
				{
					creature.ragdoll.RemovePhysicToggleModifier(this);
				}
				bool flag5 = this.beamImpactEffect != null;
				if (flag5)
				{
					this.beamImpactEffect.End(false, -1f);
					this.beamImpactEffect = null;
				}
				LightningHookMergeUp lightningHook = this.hookedCreature;
				bool flag6 = lightningHook != null;
				if (flag6)
				{
					lightningHook.Unhook();
				}
				this.hookedCreature = null;
				this.currentCharge = 0f;
			}
			this.beamActive = false;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000075B4 File Offset: 0x000057B4
		public override void FixedUpdateCaster()
		{
			base.FixedUpdateCaster();
			bool flag = !this.beamActive;
			if (!flag)
			{
				this.spellCaster.ragdollHand.rb.AddForce(-this.beamRay.direction * this.beamForceCurve.Evaluate(Time.time), 0);
				Player.local.locomotion.rb.AddForce(-this.beamRay.direction * this.beamLocomotionPushForce / 2f, 5);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007650 File Offset: 0x00005850
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			this.beamRay.origin = this.spellCaster.magicSource.transform.position;
			this.beamRay.direction = this.spellCaster.ragdollHand.PalmDir;
			bool flag = !this.spellCaster.mana.CanConsumeMana(this.manaConsumption * Time.deltaTime);
			if (flag)
			{
				this.currentCharge = 0f;
			}
			bool flag2 = Time.time - this.lastZap > this.zapInterval;
			if (flag2)
			{
				this.lastZap = Time.time + Random.Range(-0.5f, 0.5f);
				SpellCastLightning spellCastLightning = this.spellCaster.spellInstance as SpellCastLightning;
				bool flag3 = spellCastLightning != null;
				if (flag3)
				{
					spellCastLightning.ShockInRadius(this.spellCaster.magicSource.transform.position, 3f, this.spellCaster.ragdollHand.transform, null, 1f, false);
				}
			}
			bool flag4 = !this.beamActive && this.currentCharge >= 0.8f;
			if (flag4)
			{
				this.beamActive = true;
				this.beamEffect = this.beamEffectData.Spawn(this.beamStart, true, null, false, Array.Empty<Type>());
				EffectInstance effectInstance = this.beamEffect;
				bool flag5 = effectInstance != null;
				if (flag5)
				{
					effectInstance.SetIntensity(1f);
				}
				EffectInstance effectInstance2 = this.beamEffect;
				bool flag6 = effectInstance2 != null;
				if (flag6)
				{
					effectInstance2.Play(0, false);
				}
				bool flag7 = this.beamEffect != null;
				if (flag7)
				{
					foreach (EffectParticle effectParticle in this.beamEffect.effects.OfType<EffectParticle>())
					{
						this.collisionModule = effectParticle.rootParticleSystem.collision;
						this.collisionModule.collidesWith = this.beamMask;
						foreach (EffectParticleChild child in effectParticle.childs)
						{
							this.childCollisionModule = child.particleSystem.collision;
							this.childCollisionModule.collidesWith = this.beamMask;
						}
					}
				}
				this.beamStart.transform.SetPositionAndRotation(this.spellCaster.magicSource.transform.position, Quaternion.LookRotation(this.beamRay.direction));
				this.spellCaster.ragdollHand.playerHand.link.SetJointModifier(this, this.beamHandPositionSpringMultiplier, this.beamHandPositionDamperMultiplier, this.beamHandRotationSpringMultiplier, this.beamHandRotationDamperMultiplier, this.beamHandLocomotionVelocityCorrectionMultiplier);
			}
			bool flag8 = !this.beamActive;
			if (!flag8)
			{
				this.spellCaster.mana.ConsumeMana(this.activeManaConsumption * Time.deltaTime);
				this.spellCaster.ragdollHand.playerHand.controlHand.HapticLoopRefresh(1f, 0.01f);
				Player.local.locomotion.rb.AddForce(-this.beamRay.direction * this.beamLocomotionPushForce, 5);
				this.beamStart.transform.SetPositionAndRotation(this.spellCaster.magicSource.transform.position, Quaternion.Slerp(this.beamStart.transform.rotation, Quaternion.LookRotation(this.beamRay.direction), Time.deltaTime * 3f));
				bool flag9 = this.hookedCreature && Vector3.Angle(this.beamRay.direction, this.hookedCreature.creature.ragdoll.GetPart(4).transform.position - this.beamRay.origin) > this.beamHookMaxAngle;
				if (flag9)
				{
					this.hookedCreature.Unhook();
					this.hookedCreature = null;
				}
				RaycastHit hitInfo;
				bool flag10 = !Physics.SphereCast(this.beamRay, 0.1f, ref hitInfo, 20f, this.beamMask, 1);
				if (flag10)
				{
					this.beamHitPoint.SetPositionAndRotation(this.beamRay.GetPoint(20f), Quaternion.LookRotation(-this.beamRay.direction));
					bool flag11 = this.beamImpactEffect != null;
					if (flag11)
					{
						this.beamImpactEffect.End(false, -1f);
						this.beamImpactEffect = null;
					}
				}
				else
				{
					this.beamHitPoint.SetPositionAndRotation(hitInfo.point + this.beamRay.direction * 5f, Quaternion.LookRotation(-this.beamRay.direction));
					bool flag12 = this.beamEffectData != null && this.beamImpactEffect == null;
					if (flag12)
					{
						this.beamImpactEffect = this.beamEffectData.Spawn(this.beamHitPoint, true, null, false, Array.Empty<Type>());
						this.beamImpactEffect.Play(0, false);
					}
					bool flag13 = hitInfo.collider.GetComponentInParent<Creature>() != null;
					if (flag13)
					{
						Creature componentInParent = hitInfo.collider.GetComponentInParent<Creature>();
						bool flag14 = componentInParent != null;
						if (flag14)
						{
							this.creaturesHit.Add(componentInParent);
							componentInParent.ragdoll.AddPhysicToggleModifier(this);
						}
					}
					bool flag15 = hitInfo.rigidbody == null;
					if (!flag15)
					{
						CollisionHandler component = hitInfo.rigidbody.GetComponent<CollisionHandler>();
						bool flag16 = component == null;
						if (!flag16)
						{
							component.rb.AddForceAtPosition(this.beamRay.direction * this.beamForce, hitInfo.point, 2);
							bool isItem = component.isItem;
							if (isItem)
							{
								ColliderGroup componentInParent2 = hitInfo.collider.GetComponentInParent<ColliderGroup>();
								bool flag17 = componentInParent2 != null && componentInParent2.imbue;
								if (flag17)
								{
									componentInParent2.imbue.Transfer(this.imbueSpell, this.imbueAmount * Time.deltaTime);
								}
							}
							else
							{
								RagdollPart ragdollPart = component.ragdollPart;
								bool flag18 = ragdollPart != null && ragdollPart.ragdoll.creature != this.spellCaster.mana.creature;
								if (flag18)
								{
									Creature creature = ragdollPart.ragdoll.creature;
									bool flag19 = creature != null;
									if (flag19)
									{
										bool flag20 = Time.time - this.lastDamageTick > this.damageDelay;
										if (flag20)
										{
											this.lastDamageTick = Time.time;
											Creature creature2 = creature;
											DamageStruct damageStruct;
											damageStruct..ctor(4, this.damageAmount);
											damageStruct.pushLevel = 2;
											creature2.Damage(new CollisionInstance(damageStruct, null, null)
											{
												casterHand = this.spellCaster.ragdollHand.caster,
												contactPoint = hitInfo.point,
												contactNormal = hitInfo.normal,
												targetColliderGroup = hitInfo.collider.GetComponentInParent<ColliderGroup>()
											});
											creature.TryElectrocute(1f, 5f, true, false, this.electrocuteEffectData);
											this.TryHookCreature(creature);
										}
										bool flag21 = Time.time - this.lastChainTick <= this.chainDelay;
										if (!flag21)
										{
											this.lastChainTick = Time.time;
											this.Chain(creature);
											this.creaturesHit.Add(creature);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00007E2C File Offset: 0x0000602C
		private void TryHookCreature(Creature creature)
		{
			LightningHookMergeUp lightningHook = this.hookedCreature;
			bool flag = ((lightningHook != null) ? lightningHook.creature : null) == creature;
			if (!flag)
			{
				LightningHookMergeUp lightningHook2 = this.hookedCreature;
				bool flag2 = lightningHook2 != null;
				if (flag2)
				{
					lightningHook2.Unhook();
				}
				this.hookedCreature = creature.gameObject.GetOrAddComponent<LightningHookMergeUp>();
				this.hookedCreature.Hook(this);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007E9C File Offset: 0x0000609C
		private void Chain(Creature creature)
		{
			RagdollPart part = creature.ragdoll.GetPart(4);
			int num = Physics.OverlapSphereNonAlloc(part.transform.position, this.chainRadius, this.collidersHit, LayerMask.GetMask(new string[] { "BodyLocomotion" }), 1);
			bool flag = num <= 0;
			if (!flag)
			{
				Creature component = this.collidersHit[Random.Range(0, num)].GetComponent<Creature>();
				bool flag2 = component == null;
				if (!flag2)
				{
					RagdollPart part2 = component.ragdoll.GetPart(4);
					Transform transform = creature.transform;
					EffectInstance effectInstance = this.chainEffectData.Spawn(transform.position, transform.rotation, null, null, true, null, false, Array.Empty<Type>());
					effectInstance.SetSource(creature.ragdoll.GetPart(4).transform);
					effectInstance.SetTarget(part2.transform);
					effectInstance.Play(0, false);
					component.TryElectrocute(1f, 5f, true, false, this.electrocuteEffectData);
					component.TryPush(0, (part2.transform.position - part.transform.position).normalized * 2f, 1, 0);
				}
			}
		}

		// Token: 0x0400003C RID: 60
		protected EffectData beamEffectData;

		// Token: 0x0400003D RID: 61
		protected EffectInstance beamEffect;

		// Token: 0x0400003E RID: 62
		public LayerMask beamMask = 144718849;

		// Token: 0x0400003F RID: 63
		public float beamForce = 50f;

		// Token: 0x04000040 RID: 64
		protected SpellCastCharge imbueSpell;

		// Token: 0x04000041 RID: 65
		public float imbueAmount = 10f;

		// Token: 0x04000042 RID: 66
		public float damageDelay = 0.5f;

		// Token: 0x04000043 RID: 67
		public float damageAmount = 10f;

		// Token: 0x04000044 RID: 68
		public AnimationCurve beamForceCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 10f),
			new Keyframe(0.05f, 25f),
			new Keyframe(0.1f, 10f)
		});

		// Token: 0x04000045 RID: 69
		public float beamHandPositionSpringMultiplier = 1f;

		// Token: 0x04000046 RID: 70
		public float beamHandPositionDamperMultiplier = 1f;

		// Token: 0x04000047 RID: 71
		public float beamHandRotationSpringMultiplier = 0.2f;

		// Token: 0x04000048 RID: 72
		public float beamHandRotationDamperMultiplier = 0.6f;

		// Token: 0x04000049 RID: 73
		public float beamHandLocomotionVelocityCorrectionMultiplier = 1f;

		// Token: 0x0400004A RID: 74
		[Range(0f, 30f)]
		public float beamLocomotionPushForce = 10f;

		// Token: 0x0400004B RID: 75
		public float beamCastMinHandAngle = 20f;

		// Token: 0x0400004C RID: 76
		public string beamImpactEffectId;

		// Token: 0x0400004D RID: 77
		protected EffectData beamImpactEffectData;

		// Token: 0x0400004E RID: 78
		public float chainRadius = 4f;

		// Token: 0x0400004F RID: 79
		public float chainDelay = 1f;

		// Token: 0x04000050 RID: 80
		protected EffectData electrocuteEffectData;

		// Token: 0x04000051 RID: 81
		protected EffectData chainEffectData;

		// Token: 0x04000052 RID: 82
		public bool beamActive;

		// Token: 0x04000053 RID: 83
		public Ray beamRay;

		// Token: 0x04000054 RID: 84
		public Transform beamStart;

		// Token: 0x04000055 RID: 85
		public Transform beamHitPoint;

		// Token: 0x04000056 RID: 86
		protected float lastDamageTick;

		// Token: 0x04000057 RID: 87
		protected float lastChainTick;

		// Token: 0x04000058 RID: 88
		protected Collider[] collidersHit;

		// Token: 0x04000059 RID: 89
		protected HashSet<Creature> creaturesHit;

		// Token: 0x0400005A RID: 90
		public float beamHookDamper = 150f;

		// Token: 0x0400005B RID: 91
		public float beamHookSpring = 1000f;

		// Token: 0x0400005C RID: 92
		public float beamHookSpeed = 20f;

		// Token: 0x0400005D RID: 93
		public float beamHookMaxAngle = 30f;

		// Token: 0x0400005E RID: 94
		public float zapInterval = 0.7f;

		// Token: 0x0400005F RID: 95
		private LightningHookMergeUp hookedCreature;

		// Token: 0x04000060 RID: 96
		private float lastZap;

		// Token: 0x04000061 RID: 97
		private EffectInstance beamImpactEffect;

		// Token: 0x04000062 RID: 98
		private ParticleSystem.CollisionModule collisionModule = default(ParticleSystem.CollisionModule);

		// Token: 0x04000063 RID: 99
		private ParticleSystem.CollisionModule childCollisionModule = default(ParticleSystem.CollisionModule);

		// Token: 0x04000064 RID: 100
		public bool isCasting;
	}
}
