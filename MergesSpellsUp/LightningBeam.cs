using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace MergesSpellsUp
{
	// Token: 0x02000005 RID: 5
	public class LightningBeam : MonoBehaviour
	{
		// Token: 0x0600007D RID: 125 RVA: 0x0000631C File Offset: 0x0000451C
		public void Init(Vector3 origin, Vector3 directionOfBeam, float durationOfBeam)
		{
			this.beamEffectData = Catalog.GetData<EffectData>("SpellLightningMergeBeam", true);
			this.imbueSpell = Catalog.GetData<SpellCastCharge>("Lightning", true);
			this.chainEffectData = Catalog.GetData<EffectData>("SpellLightningBolt", true);
			this.electrocuteEffectData = Catalog.GetData<EffectData>("ImbueLightningRagdoll", true);
			this.beamImpactEffectData = Catalog.GetData<EffectData>("SpellLightningMergeBeamImpact", true);
			this.collidersHit = new Collider[20];
			this.beamForceCurve.postWrapMode = 2;
			this.creaturesHit = new HashSet<Creature>();
			this.beamRay.origin = origin;
			this.beamRay.direction = directionOfBeam;
			this.instance = new SpellCastLightning();
			this.duration = durationOfBeam;
			this.startTime = Time.time;
			this.Fire(true);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000063E4 File Offset: 0x000045E4
		public void Update()
		{
			bool flag = Time.time - this.lastZap > this.zapInterval;
			if (flag)
			{
				this.lastZap = Time.time + Random.Range(-0.5f, 0.5f);
				this.instance.ShockInRadius(this.beamRay.origin, 3f, null, null, 1f, false);
			}
			bool flag2 = this.startTime < Time.time - this.duration;
			if (flag2)
			{
				this.Dispose();
			}
			else
			{
				bool flag3 = !this.beamActive;
				if (flag3)
				{
					this.beamActive = true;
					this.beamEffect = this.beamEffectData.Spawn(this.beamStart, true, null, false, Array.Empty<Type>());
					EffectInstance effectInstance = this.beamEffect;
					bool flag4 = effectInstance != null;
					if (flag4)
					{
						effectInstance.SetIntensity(1f);
					}
					EffectInstance effectInstance2 = this.beamEffect;
					bool flag5 = effectInstance2 != null;
					if (flag5)
					{
						effectInstance2.Play(0, false);
					}
					bool flag6 = this.beamEffect != null;
					if (flag6)
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
					this.beamStart.transform.SetPositionAndRotation(this.beamRay.origin, Quaternion.LookRotation(this.beamRay.direction));
				}
				bool flag7 = !this.beamActive;
				if (!flag7)
				{
					this.beamStart.transform.SetPositionAndRotation(this.beamRay.origin, Quaternion.Slerp(this.beamStart.transform.rotation, Quaternion.LookRotation(this.beamRay.direction), Time.deltaTime * 3f));
					bool flag8 = this.hookedCreature && Vector3.Angle(this.beamRay.direction, this.hookedCreature.creature.ragdoll.GetPart(4).transform.position - this.beamRay.origin) > this.beamHookMaxAngle;
					if (flag8)
					{
						this.hookedCreature.Unhook();
						this.hookedCreature = null;
					}
					RaycastHit hitInfo;
					bool flag9 = !Physics.SphereCast(this.beamRay, 0.1f, ref hitInfo, 20f, this.beamMask, 1);
					if (flag9)
					{
						this.beamHitPoint.SetPositionAndRotation(this.beamRay.GetPoint(20f), Quaternion.LookRotation(-this.beamRay.direction));
						bool flag10 = this.beamImpactEffect != null;
						if (flag10)
						{
							this.beamImpactEffect.End(false, -1f);
							this.beamImpactEffect = null;
						}
					}
					else
					{
						this.beamHitPoint.SetPositionAndRotation(hitInfo.point + this.beamRay.direction * 5f, Quaternion.LookRotation(-this.beamRay.direction));
						bool flag11 = this.beamEffectData != null && this.beamImpactEffect == null;
						if (flag11)
						{
							this.beamImpactEffect = this.beamEffectData.Spawn(this.beamHitPoint, true, null, false, Array.Empty<Type>());
							this.beamImpactEffect.Play(0, false);
						}
						bool flag12 = hitInfo.collider.GetComponentInParent<Creature>() != null;
						if (flag12)
						{
							Creature componentInParent = hitInfo.collider.GetComponentInParent<Creature>();
							bool flag13 = componentInParent != null;
							if (flag13)
							{
								this.creaturesHit.Add(componentInParent);
								componentInParent.ragdoll.AddPhysicToggleModifier(this);
							}
						}
						bool flag14 = hitInfo.rigidbody == null;
						if (!flag14)
						{
							CollisionHandler component = hitInfo.rigidbody.GetComponent<CollisionHandler>();
							bool flag15 = component == null;
							if (!flag15)
							{
								component.rb.AddForceAtPosition(this.beamRay.direction * this.beamForce, hitInfo.point, 2);
								bool isItem = component.isItem;
								if (isItem)
								{
									ColliderGroup componentInParent2 = hitInfo.collider.GetComponentInParent<ColliderGroup>();
									bool flag16 = componentInParent2 != null && componentInParent2.imbue;
									if (flag16)
									{
										componentInParent2.imbue.Transfer(this.imbueSpell, this.imbueAmount * Time.deltaTime);
									}
								}
								else
								{
									RagdollPart ragdollPart = component.ragdollPart;
									bool flag17 = ragdollPart == null || !(ragdollPart.ragdoll.creature != Player.local.creature);
									if (!flag17)
									{
										Creature creature = ragdollPart.ragdoll.creature;
										bool flag18 = creature != null;
										if (flag18)
										{
											bool flag19 = Time.time - this.lastDamageTick > this.damageDelay;
											if (flag19)
											{
												this.lastDamageTick = Time.time;
												Creature creature2 = creature;
												DamageStruct damageStruct;
												damageStruct..ctor(4, this.damageAmount);
												damageStruct.pushLevel = 2;
												creature2.Damage(new CollisionInstance(damageStruct, null, null)
												{
													casterHand = Player.local.creature.handRight.caster,
													contactPoint = hitInfo.point,
													contactNormal = hitInfo.normal,
													targetColliderGroup = hitInfo.collider.GetComponentInParent<ColliderGroup>()
												});
												creature.TryElectrocute(1f, 5f, true, false, this.electrocuteEffectData);
												this.TryHookCreature(creature);
											}
											bool flag20 = Time.time - this.lastChainTick <= this.chainDelay;
											if (!flag20)
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
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006A6C File Offset: 0x00004C6C
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

		// Token: 0x06000080 RID: 128 RVA: 0x00006ADC File Offset: 0x00004CDC
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

		// Token: 0x06000081 RID: 129 RVA: 0x00006C1C File Offset: 0x00004E1C
		public void Fire(bool active)
		{
			if (active)
			{
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
			}
			this.beamActive = false;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006D84 File Offset: 0x00004F84
		private void Dispose()
		{
			this.Fire(false);
			Object.Destroy(this);
		}

		// Token: 0x0400000A RID: 10
		protected EffectData beamEffectData;

		// Token: 0x0400000B RID: 11
		protected EffectInstance beamEffect;

		// Token: 0x0400000C RID: 12
		public LayerMask beamMask = 144718849;

		// Token: 0x0400000D RID: 13
		public float beamForce = 50f;

		// Token: 0x0400000E RID: 14
		protected SpellCastCharge imbueSpell;

		// Token: 0x0400000F RID: 15
		public float imbueAmount = 10f;

		// Token: 0x04000010 RID: 16
		public float damageDelay = 0.5f;

		// Token: 0x04000011 RID: 17
		public float damageAmount = 10f;

		// Token: 0x04000012 RID: 18
		public AnimationCurve beamForceCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 10f),
			new Keyframe(0.05f, 25f),
			new Keyframe(0.1f, 10f)
		});

		// Token: 0x04000013 RID: 19
		public float beamHandPositionSpringMultiplier = 1f;

		// Token: 0x04000014 RID: 20
		public float beamHandPositionDamperMultiplier = 1f;

		// Token: 0x04000015 RID: 21
		public float beamHandRotationSpringMultiplier = 0.2f;

		// Token: 0x04000016 RID: 22
		public float beamHandRotationDamperMultiplier = 0.6f;

		// Token: 0x04000017 RID: 23
		public float beamHandLocomotionVelocityCorrectionMultiplier = 1f;

		// Token: 0x04000018 RID: 24
		public float beamLocomotionPushForce = 10f;

		// Token: 0x04000019 RID: 25
		public float beamCastMinHandAngle = 20f;

		// Token: 0x0400001A RID: 26
		public string beamImpactEffectId;

		// Token: 0x0400001B RID: 27
		protected EffectData beamImpactEffectData;

		// Token: 0x0400001C RID: 28
		public float chainRadius = 4f;

		// Token: 0x0400001D RID: 29
		public float chainDelay = 1f;

		// Token: 0x0400001E RID: 30
		protected EffectData electrocuteEffectData;

		// Token: 0x0400001F RID: 31
		protected EffectData chainEffectData;

		// Token: 0x04000020 RID: 32
		public bool beamActive;

		// Token: 0x04000021 RID: 33
		public Ray beamRay;

		// Token: 0x04000022 RID: 34
		public Transform beamStart;

		// Token: 0x04000023 RID: 35
		public Transform beamHitPoint;

		// Token: 0x04000024 RID: 36
		protected float lastDamageTick;

		// Token: 0x04000025 RID: 37
		protected float lastChainTick;

		// Token: 0x04000026 RID: 38
		protected Collider[] collidersHit;

		// Token: 0x04000027 RID: 39
		protected HashSet<Creature> creaturesHit;

		// Token: 0x04000028 RID: 40
		public float beamHookDamper = 150f;

		// Token: 0x04000029 RID: 41
		public float beamHookSpring = 1000f;

		// Token: 0x0400002A RID: 42
		public float beamHookSpeed = 20f;

		// Token: 0x0400002B RID: 43
		public float beamHookMaxAngle = 30f;

		// Token: 0x0400002C RID: 44
		public float zapInterval = 0.7f;

		// Token: 0x0400002D RID: 45
		private LightningHookMergeUp hookedCreature;

		// Token: 0x0400002E RID: 46
		private float lastZap;

		// Token: 0x0400002F RID: 47
		private EffectInstance beamImpactEffect;

		// Token: 0x04000030 RID: 48
		private ParticleSystem.CollisionModule collisionModule = default(ParticleSystem.CollisionModule);

		// Token: 0x04000031 RID: 49
		private ParticleSystem.CollisionModule childCollisionModule = default(ParticleSystem.CollisionModule);

		// Token: 0x04000032 RID: 50
		public bool isCasting;

		// Token: 0x04000033 RID: 51
		private SpellCastLightning instance;

		// Token: 0x04000034 RID: 52
		private float duration;

		// Token: 0x04000035 RID: 53
		private float startTime;
	}
}
