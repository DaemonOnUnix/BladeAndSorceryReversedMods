using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000013 RID: 19
	internal class FlamethrowerMode : SpellMode<SpellCastProjectile>
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000086EA File Offset: 0x000068EA
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000086F5 File Offset: 0x000068F5
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			this.fireballEffects = new List<EffectInstance>();
			this.fireballTargets = new List<GameObject>();
			this.revealFire = Catalog.GetData<EffectData>("ShatterbladeRevealFire", true);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00008728 File Offset: 0x00006928
		private Vector3 NormalPos(int i)
		{
			bool flag = i == 1;
			Vector3 vector;
			if (flag)
			{
				vector = this.Center();
			}
			else
			{
				bool flag2 = i == 2;
				if (flag2)
				{
					vector = this.Center() + this.UpDir() * 0.5f;
				}
				else
				{
					bool flag3 = i < 10;
					if (flag3)
					{
						vector = this.Center() + Quaternion.AngleAxis((float)(i - 2) * 0.14285715f * 360f + this.rotation, this.ForwardDir()) * this.UpDir() * 0.3f;
					}
					else
					{
						vector = this.Center() + Quaternion.AngleAxis((float)(i - 9) * 0.2f * 360f + this.rotation * -1f, this.ForwardDir()) * this.UpDir() * 0.2f;
					}
				}
			}
			return vector;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008814 File Offset: 0x00006A14
		private Vector3 GetBasePos(int i)
		{
			return this.Center() + Quaternion.AngleAxis((float)i / 3f * 360f + this.rotation, this.ForwardDir()) * this.UpDir() * 0.3f;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00008864 File Offset: 0x00006A64
		private Vector3 PressedPos(int i)
		{
			bool flag = i == 1;
			Vector3 vector;
			if (flag)
			{
				vector = this.Center() + this.ForwardDir() * 0.2f;
			}
			else
			{
				bool flag2 = i == 2;
				if (flag2)
				{
					vector = this.Center();
				}
				else
				{
					vector = this.GetBasePos((i - 2) / 4) + this.ForwardDir() * 0.15f + Quaternion.AngleAxis((float)((i - 2) % 4) / 4f * 360f + this.rotation * -1.5f, this.ForwardDir()) * this.UpDir() * 0.15f;
				}
			}
			return vector;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00008914 File Offset: 0x00006B14
		public override Vector3 GetPos(int i, Rigidbody rb, BladePart part)
		{
			return base.IsButtonPressed() ? this.PressedPos(i) : this.NormalPos(i);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008930 File Offset: 0x00006B30
		public override Quaternion GetRot(int i, Rigidbody rb, BladePart part)
		{
			bool flag = i <= 2;
			Quaternion quaternion;
			if (flag)
			{
				quaternion = Quaternion.LookRotation(this.ForwardDir(), this.UpDir());
			}
			else
			{
				bool flag2 = base.IsButtonPressed();
				if (flag2)
				{
					quaternion = Quaternion.LookRotation(rb.transform.position - this.GetBasePos((i - 2) / 4) + this.ForwardDir() * (base.IsTriggerPressed() ? 0.25f : 15f), this.ForwardDir());
				}
				else
				{
					quaternion = Quaternion.LookRotation(rb.transform.position - this.Center(), this.ForwardDir());
				}
			}
			return quaternion;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000089DC File Offset: 0x00006BDC
		public override void OnTriggerPressed()
		{
			base.OnTriggerPressed();
			bool flag = base.IsButtonPressed();
			if (flag)
			{
				this.fireballEffects.ForEach(delegate(EffectInstance effect)
				{
					if (effect != null)
					{
						effect.End(false, -1f);
					}
				});
				this.fireballEffects.Clear();
				this.fireballTargets.Clear();
				for (int subPos = 0; subPos < 3; subPos++)
				{
					GameObject target = new GameObject();
					target.transform.position = this.GetBasePos(subPos) + this.ForwardDir() * 0.25f;
					EffectInstance effect2 = Catalog.GetData<EffectData>("SpellFireCharge", true).Spawn(target.transform, false, null, false, Array.Empty<Type>());
					effect2.Play(0, false);
					effect2.SetIntensity(0f);
					this.fireballEffects.Add(effect2);
					this.fireballTargets.Add(target);
				}
			}
			else
			{
				this.flameEffect = Catalog.GetData<EffectData>("ShatterbladeFlamethrowerFlames", true).Spawn(this.sword.GetRB(1).transform.position, Quaternion.LookRotation(this.ForwardDir(), this.UpDir()), null, null, false, null, false, Array.Empty<Type>());
				this.flameEffect.Play(0, false);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008B34 File Offset: 0x00006D34
		public override void OnTriggerHeld()
		{
			base.OnTriggerHeld();
			this.rotation += Time.deltaTime * Mathf.Lerp(80f, 300f, this.FireballChargeLerped());
			bool flag = base.IsButtonPressed();
			if (flag)
			{
				int i = 0;
				this.fireballTargets.ForEach(delegate(GameObject target)
				{
					Transform transform = target.transform;
					FlamethrowerMode <>4__this = this;
					int i2 = i;
					i = i2 + 1;
					transform.position = <>4__this.GetBasePos(i2) + this.ForwardDir() * 0.25f;
				});
				this.fireballEffects.ForEach(delegate(EffectInstance effect)
				{
					effect.SetIntensity(this.FireballChargeLerped());
				});
				this.Hand().HapticTick(this.FireballChargeLerped() * 0.5f, 10f);
			}
			else
			{
				bool flag2 = this.flameEffect != null;
				if (flag2)
				{
					FlamethrowerMode.<>c__DisplayClass18_1 CS$<>8__locals2 = new FlamethrowerMode.<>c__DisplayClass18_1();
					CS$<>8__locals2.<>4__this = this;
					this.flameEffect.SetPosition(this.sword.GetRB(1).transform.position);
					this.flameEffect.SetRotation(Quaternion.LookRotation(this.ForwardDir(), this.UpDir()));
					this.Hand().HapticTick(0.3f, 20f);
					CS$<>8__locals2.creaturesHit = new List<Creature>();
					RaycastHit[] array = Utils.ConeCastAll(this.Center(), 0.001f, this.ForwardDir() + Utils.RandomVector(-0.1f, 0.1f, 0), 3f, 10f);
					for (int j = 0; j < array.Length; j++)
					{
						FlamethrowerMode.<>c__DisplayClass18_2 CS$<>8__locals3 = new FlamethrowerMode.<>c__DisplayClass18_2();
						CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
						CS$<>8__locals3.hit = array[j];
						FlamethrowerMode.<>c__DisplayClass18_3 CS$<>8__locals4 = new FlamethrowerMode.<>c__DisplayClass18_3();
						CS$<>8__locals4.CS$<>8__locals2 = CS$<>8__locals3;
						FlamethrowerMode.<>c__DisplayClass18_3 CS$<>8__locals5 = CS$<>8__locals4;
						Rigidbody rigidbody = CS$<>8__locals4.CS$<>8__locals2.hit.rigidbody;
						CS$<>8__locals5.part = ((rigidbody != null) ? rigidbody.gameObject.GetComponent<RagdollPart>() : null);
						bool flag3 = CS$<>8__locals4.part != null;
						if (flag3)
						{
							Creature creature = CS$<>8__locals4.part.ragdoll.creature;
							bool flag4 = creature != null && !creature.isPlayer;
							if (flag4)
							{
								bool flag5 = CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.creaturesHit.Where((Creature p) => p == CS$<>8__locals4.part.ragdoll.creature).Count<Creature>() < 3;
								if (flag5)
								{
									this.sword.RunAfter(delegate
									{
										bool flag6 = creature == null;
										if (!flag6)
										{
											CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.creaturesHit.Add(creature);
											DamageStruct damageStruct;
											damageStruct..ctor(4, CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.flamethrowerDamage);
											damageStruct.hitRagdollPart = CS$<>8__locals4.part;
											CollisionInstance collisionInstance = new CollisionInstance(damageStruct, null, null)
											{
												targetColliderGroup = CS$<>8__locals4.part.colliderGroup,
												contactPoint = CS$<>8__locals4.CS$<>8__locals2.hit.point,
												contactNormal = CS$<>8__locals4.CS$<>8__locals2.hit.normal,
												casterHand = CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Hand().caster,
												hasEffect = true,
												active = true
											};
											EffectInstance effect = CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.revealFire.Spawn(CS$<>8__locals4.CS$<>8__locals2.hit.point, Quaternion.AngleAxis((float)Random.Range(0, 360), CS$<>8__locals4.CS$<>8__locals2.hit.normal), CS$<>8__locals4.part.transform, collisionInstance, true, null, false, Array.Empty<Type>());
											effect.SetIntensity(Random.Range(1f, 2f));
											effect.Play(0, false);
											bool flag7 = !creature.isKilled;
											if (flag7)
											{
												creature.TryPush(2, CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.ForwardDir(), 0, 0);
											}
											creature.Damage(collisionInstance);
										}
									}, 0.5f * Vector3.Distance(CS$<>8__locals4.CS$<>8__locals2.hit.point, this.Center()));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008DF4 File Offset: 0x00006FF4
		public void EndEffects()
		{
			this.fireballEffects.ForEach(delegate(EffectInstance effect)
			{
				effect.End(false, -1f);
			});
			this.fireballEffects.Clear();
			this.fireballTargets.Clear();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008E48 File Offset: 0x00007048
		public void SpawnFireball(Vector3 position)
		{
			Catalog.GetData<ItemData>("DynamicProjectile", true).SpawnAsync(delegate(Item projectile)
			{
				projectile.transform.position = position;
				foreach (CollisionHandler collisionHandler in projectile.collisionHandlers)
				{
					foreach (Damager damager in collisionHandler.damagers)
					{
						damager.Load(Catalog.GetData<DamagerData>("Fireball", true), collisionHandler);
					}
				}
				ItemMagicProjectile component = projectile.GetComponent<ItemMagicProjectile>();
				projectile.rb.useGravity = this.fireballGravity;
				bool flag = component;
				if (flag)
				{
					component.guidance = 0;
					component.speed = 30f;
					component.item.lastHandler = this.Hand();
					component.allowDeflect = true;
					component.deflectEffectData = Catalog.GetData<EffectData>("HitFireBallDeflect", true);
					component.imbueSpellCastCharge = null;
					component.Fire(Utils.HomingThrow(projectile.rb, this.ForwardDir() * 30f, 10f, null), Catalog.GetData<EffectData>("SpellFireball", true), null, null);
				}
				else
				{
					projectile.rb.AddForce(Utils.HomingThrow(projectile.rb, this.ForwardDir() * this.fireballVelocity, 10f, null), 1);
					projectile.Throw(1f, 2);
				}
			}, null, null, null, true, null);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008E9C File Offset: 0x0000709C
		public void ThrowFireballs()
		{
			this.EndEffects();
			for (int i = 0; i < 3; i++)
			{
				this.SpawnFireball(this.GetBasePos(i) + this.ForwardDir() * 0.25f);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008EE6 File Offset: 0x000070E6
		public void CancelFireballs()
		{
			this.EndEffects();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008EF0 File Offset: 0x000070F0
		public override void OnButtonReleased()
		{
			base.OnButtonReleased();
			bool flag = base.IsTriggerPressed();
			if (flag)
			{
				bool flag2 = this.IsFireballCharged();
				if (flag2)
				{
					this.ThrowFireballs();
				}
				else
				{
					this.CancelFireballs();
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008F2F File Offset: 0x0000712F
		public override void OnTriggerNotHeld()
		{
			base.OnTriggerNotHeld();
			this.rotation += Time.deltaTime * 80f;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008F51 File Offset: 0x00007151
		private float FireballChargeLerped()
		{
			return Mathf.Clamp01(Mathf.InverseLerp(0f, this.fireballChargeTime, Time.time - this.lastTriggerPress));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008F74 File Offset: 0x00007174
		private bool IsFireballCharged()
		{
			return this.FireballChargeLerped() == 1f;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008F84 File Offset: 0x00007184
		public override void OnTriggerReleased()
		{
			base.OnTriggerReleased();
			EffectInstance effectInstance = this.flameEffect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			bool flag = base.IsButtonPressed();
			if (flag)
			{
				bool flag2 = this.IsFireballCharged();
				if (flag2)
				{
					this.ThrowFireballs();
				}
				else
				{
					this.CancelFireballs();
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008FDB File Offset: 0x000071DB
		public override string GetUseAnnotation()
		{
			return base.IsButtonPressed() ? ((base.IsTriggerPressed() && this.IsFireballCharged()) ? "Release to fire!" : "Pull trigger to charge fireballs!") : "Pull trigger to burn your foes";
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009008 File Offset: 0x00007208
		public override bool GetUseAnnotationShown()
		{
			return true;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000900B File Offset: 0x0000720B
		public override string GetAltUseAnnotation()
		{
			return "Hold button to switch modes";
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009012 File Offset: 0x00007212
		public override bool GetAltUseAnnotationShown()
		{
			return !base.IsButtonPressed();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000901D File Offset: 0x0000721D
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00009027 File Offset: 0x00007227
		public override void Exit()
		{
			base.Exit();
			EffectInstance effectInstance = this.flameEffect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			this.CancelFireballs();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009050 File Offset: 0x00007250
		public override bool ShouldReform(BladePart part)
		{
			return part != this.sword.GetPart(11);
		}

		// Token: 0x04000055 RID: 85
		public float flamethrowerDamage = 0.7f;

		// Token: 0x04000056 RID: 86
		public float flamethrowerPushback = 5f;

		// Token: 0x04000057 RID: 87
		public float fireballChargeTime = 1f;

		// Token: 0x04000058 RID: 88
		public float fireballVelocity = 30f;

		// Token: 0x04000059 RID: 89
		public bool fireballGravity = true;

		// Token: 0x0400005A RID: 90
		private float rotation;

		// Token: 0x0400005B RID: 91
		private EffectInstance flameEffect;

		// Token: 0x0400005C RID: 92
		private EffectData revealFire;

		// Token: 0x0400005D RID: 93
		private List<EffectInstance> fireballEffects;

		// Token: 0x0400005E RID: 94
		private List<GameObject> fireballTargets;
	}
}
