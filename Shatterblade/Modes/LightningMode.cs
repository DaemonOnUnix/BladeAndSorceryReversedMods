using System;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000017 RID: 23
	internal class LightningMode : SpellMode<SpellCastLightning>
	{
		// Token: 0x06000174 RID: 372 RVA: 0x0000A6A0 File Offset: 0x000088A0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A6AB File Offset: 0x000088AB
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			Debug.Log(this.explosionForce);
			this.target = new GameObject();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A6D2 File Offset: 0x000088D2
		private float CooldownTime()
		{
			return 1f - Mathf.Clamp01(Mathf.InverseLerp(0f, this.cooldown, Time.time - this.lastTriggerReleased));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A6FC File Offset: 0x000088FC
		public override Vector3 GetPos(int index, Rigidbody rb, BladePart part)
		{
			Vector3 vector;
			if (index != 1)
			{
				if (index != 2)
				{
					if (index >= 10)
					{
						vector = this.Hand().Palm() + Quaternion.AngleAxis((float)(index - 10) * 0.2f * 360f + this.rotation * -1f, this.ForwardDir()) * this.UpDir() * (0.1f + this.CooldownTime() * 0.1f) + Random.Range(-1f, 1f) * this.ForwardDir() * (base.IsTriggerPressed() ? 0.1f : 0f) * Mathf.Clamp01(Time.time - this.lastTriggerPress);
					}
					else
					{
						vector = this.Hand().Palm() + Quaternion.AngleAxis((float)(index - 2) * 0.14285715f * 360f + this.rotation, this.ForwardDir()) * this.UpDir() * (0.15f + this.CooldownTime() * 0.1f) + this.ForwardDir() * -0.2f + Random.Range(-1f, 1f) * this.ForwardDir() * (base.IsTriggerPressed() ? 0.1f : 0f) * this.CooldownTime();
					}
				}
				else
				{
					vector = this.Center() + this.ForwardDir() * (0.3f + this.CooldownTime() * 0.2f);
				}
			}
			else
			{
				vector = this.Center() + this.ForwardDir() * (0.1f + this.CooldownTime() * 0.1f);
			}
			return vector;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A8DF File Offset: 0x00008ADF
		public override string GetUseAnnotation()
		{
			return base.IsTriggerPressed() ? "Release to fire" : "Hold Trigger to charge";
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A8F5 File Offset: 0x00008AF5
		public override bool GetUseAnnotationShown()
		{
			return true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A8F8 File Offset: 0x00008AF8
		public override Quaternion GetRot(int index, Rigidbody rb, BladePart part)
		{
			Quaternion quaternion;
			if (index != 1)
			{
				if (index != 2)
				{
					if (index >= 10)
					{
						quaternion = Quaternion.LookRotation(this.ForwardDir(), rb.transform.position - this.Hand().Palm());
					}
					else
					{
						quaternion = Quaternion.LookRotation(rb.transform.position - (this.Hand().Palm() - this.ForwardDir() * -0.2f), this.ForwardDir());
					}
				}
				else
				{
					quaternion = Quaternion.LookRotation(this.ForwardDir(), -this.SideDir());
				}
			}
			else
			{
				quaternion = Quaternion.LookRotation(this.ForwardDir(), this.SideDir());
			}
			return quaternion;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A9B5 File Offset: 0x00008BB5
		public override float Cooldown()
		{
			return 1f;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A9BC File Offset: 0x00008BBC
		public override void OnTriggerPressed()
		{
			base.OnTriggerPressed();
			this.chargeEffect = Catalog.GetData<EffectData>("ShatterbladeLightningCharge", true).Spawn(this.Center(), Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			this.chargeEffect.Play(0, false);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public override void OnTriggerHeld()
		{
			base.OnTriggerHeld();
			this.wasPressed = true;
			this.rotation += Time.deltaTime * Mathf.Lerp(80f, 300f, Mathf.Clamp01(Time.time - this.lastTriggerPress));
			EffectInstance effectInstance = this.chargeEffect;
			if (effectInstance != null)
			{
				effectInstance.SetIntensity(Mathf.Clamp01(Time.time - this.lastTriggerPress));
			}
			EffectInstance effectInstance2 = this.chargeEffect;
			if (effectInstance2 != null)
			{
				effectInstance2.SetPosition(this.Center());
			}
			this.Hand().HapticTick(Mathf.Clamp01(Time.time - this.lastTriggerPress) * 0.5f, 20f);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000AABE File Offset: 0x00008CBE
		public override void OnTriggerNotHeld()
		{
			base.OnTriggerNotHeld();
			this.rotation += Time.deltaTime * 80f * (1f + this.CooldownTime() * 4f);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public override void OnTriggerReleased()
		{
			base.OnTriggerReleased();
			bool flag = !this.wasPressed;
			if (!flag)
			{
				this.wasPressed = false;
				EffectInstance effect = Catalog.GetData<EffectData>("ShatterbladeLightning", true).Spawn(this.Center(), Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
				this.chargeEffect.End(false, -1f);
				RagdollPart part = this.GetRagdollPartHit();
				Vector3 point = this.GetTargetPoint();
				this.target.transform.position = ((part != null) ? part.transform.position : point);
				effect.SetSource(base.GetPart().transform);
				effect.SetTarget(this.target.transform);
				effect.Play(0, false);
				bool flag2 = part;
				if (flag2)
				{
					DamageStruct damageStruct;
					damageStruct..ctor(4, this.damage);
					damageStruct.hitRagdollPart = part;
					CollisionInstance collisionInstance = new CollisionInstance(damageStruct, null, null)
					{
						casterHand = this.Hand().caster,
						impactVelocity = (this.target.transform.position - this.Center()).normalized * 30f,
						contactPoint = this.target.transform.position,
						contactNormal = (this.target.transform.position - this.Center()).normalized,
						targetColliderGroup = part.colliderGroup
					};
					part.ragdoll.creature.Damage(collisionInstance);
					part.ragdoll.creature.TryElectrocute(5f, 5f, false, false, null);
				}
				Utils.Explosion(this.target.transform.position + this.ForwardDir() * -0.5f, this.explosionForce, this.explosionRadius, true, true, true, false, 0f);
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000ACEC File Offset: 0x00008EEC
		private Vector3 GetTargetPoint()
		{
			IOrderedEnumerable<RaycastHit> hits = from hit in Physics.RaycastAll(this.Center(), this.ForwardDir(), 50f, -5, 1).Where(delegate(RaycastHit hit)
				{
					Rigidbody rigidbody = hit.rigidbody;
					bool flag4;
					if (((rigidbody != null) ? rigidbody.GetComponentInParent<BladePart>() : null) == null)
					{
						Rigidbody rigidbody2 = hit.rigidbody;
						bool? flag2;
						if (rigidbody2 == null)
						{
							flag2 = null;
						}
						else
						{
							RagdollPart componentInParent = rigidbody2.GetComponentInParent<RagdollPart>();
							flag2 = ((componentInParent != null) ? new bool?(componentInParent.ragdoll.creature.isPlayer) : null);
						}
						bool? flag3 = flag2;
						flag4 = !flag3.GetValueOrDefault();
					}
					else
					{
						flag4 = false;
					}
					return flag4;
				})
				orderby Vector3.Distance(hit.point, this.Center())
				select hit;
			bool flag = hits.Any<RaycastHit>();
			Vector3 vector;
			if (flag)
			{
				vector = hits.First<RaycastHit>().point;
			}
			else
			{
				vector = this.Center() + this.ForwardDir() * 50f;
			}
			return vector;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000AD83 File Offset: 0x00008F83
		private RagdollPart GetRagdollPartHit()
		{
			return (from part in Utils.ConeCastRagdollPart(this.Center(), 5f, this.ForwardDir(), 50f, 30f, true, true, null)
				orderby Vector3.Distance(part.transform.position, this.Center()) / 30f * 2f * Vector3.Angle(this.ForwardDir(), part.transform.position - this.Center()) / 50f
				select part).FirstOrDefault<RagdollPart>();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000ADBE File Offset: 0x00008FBE
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		public override void Exit()
		{
			EffectInstance effectInstance = this.chargeEffect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			base.Exit();
		}

		// Token: 0x04000074 RID: 116
		public float cooldown = 1f;

		// Token: 0x04000075 RID: 117
		public float damage = 50f;

		// Token: 0x04000076 RID: 118
		public float explosionForce = 50f;

		// Token: 0x04000077 RID: 119
		public float explosionRadius = 3f;

		// Token: 0x04000078 RID: 120
		private float rotation;

		// Token: 0x04000079 RID: 121
		private GameObject target;

		// Token: 0x0400007A RID: 122
		private EffectInstance chargeEffect;

		// Token: 0x0400007B RID: 123
		private bool wasPressed;
	}
}
