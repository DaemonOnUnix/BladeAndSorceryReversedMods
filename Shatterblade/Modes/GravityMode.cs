using System;
using System.Collections;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000015 RID: 21
	internal class GravityMode : SpellMode<SpellCastGravity>
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00009703 File Offset: 0x00007903
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00009710 File Offset: 0x00007910
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			this.targetPoint = new GameObject().AddComponent<Rigidbody>();
			this.targetPoint.useGravity = false;
			this.targetPoint.isKinematic = true;
			this.targetPoint.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000976C File Offset: 0x0000796C
		public void AcquireTarget()
		{
			IOrderedEnumerable<Item> items = from item in Utils.ConeCastItem(this.sword.GetRB(1).transform.position, 5f, this.ForwardDir(), 30f, 50f)
				where item.GetComponent<BladePart>() == null && item != this.sword.item && !item.rb.isKinematic
				orderby Vector3.Distance(item.transform.position, this.sword.GetRB(1).transform.position) / 30f * 2f * Vector3.Angle(this.ForwardDir(), item.transform.position - this.sword.GetRB(1).transform.position) / 50f
				select item;
			Item itemTarget = items.FirstOrDefault<Item>();
			bool flag = itemTarget != null;
			if (flag)
			{
				this.targetItem = itemTarget;
				this.targetItem.Depenetrate();
				this.targetItem.collisionHandlers.ForEach(delegate(CollisionHandler ch)
				{
					ch.SetPhysicModifier(this, new float?(0f), 1f, 5f, -1f, -1f, null);
				});
				Object.Destroy(this.joint);
				this.joint = Utils.CreateSimpleJoint(this.targetItem.rb, this.targetPoint, 100f * this.targetItem.GetMassModifier(), 5f * this.targetItem.GetMassModifier(), float.PositiveInfinity, true);
				this.effect = Catalog.GetData<EffectData>("ShatterbladeGravity", true).Spawn(this.HeldCenter(), this.targetItem.transform.rotation, null, null, false, null, false, Array.Empty<Type>());
				EffectInstance effectInstance = this.effect;
				if (effectInstance != null)
				{
					effectInstance.SetPosition(this.HeldCenter());
				}
				EffectInstance effectInstance2 = this.effect;
				if (effectInstance2 != null)
				{
					effectInstance2.Play(0, false);
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000098C4 File Offset: 0x00007AC4
		public void ThrowHeldThing()
		{
			bool flag = this.targetItem;
			if (flag)
			{
				this.Hand().HapticTick(1f, 5f);
				this.targetItem.Throw(1f, 2);
				this.targetItem.rb.AddForce(Utils.HomingThrow(this.targetItem.rb, this.ForwardDir(), 30f, null) * this.targetItem.GetMassModifier() * this.itemThrowForce, 1);
				Catalog.GetData<EffectData>("ShatterbladeGravityFire", true).Spawn(this.targetItem.transform.position, this.targetItem.transform.rotation, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
				this.sword.StartCoroutine(this.ThrowEffectCoroutine());
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000099AC File Offset: 0x00007BAC
		private void UpdateEffectParams(EffectInstance effect, float intensity, Vector3 position, Quaternion rotation)
		{
			bool flag = effect == null;
			if (!flag)
			{
				effect.SetIntensity(intensity);
				effect.SetScale(Vector3.one * this.HeldRadius() * intensity * 2f);
				effect.SetPosition(position);
				effect.SetRotation(rotation);
				foreach (EffectMesh mesh in from meshEffect in effect.effects
					where meshEffect is EffectMesh
					select meshEffect as EffectMesh)
				{
					mesh.meshRenderer.material.SetFloat("_Amount", intensity);
				}
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009AA4 File Offset: 0x00007CA4
		public IEnumerator ThrowEffectCoroutine()
		{
			EffectInstance effect = this.effect;
			float startTime = Time.time;
			float duration = 0.5f;
			while (Time.time - startTime < duration)
			{
				float ratio = (Time.time - startTime) / duration;
				this.UpdateEffectParams(effect, ratio, this.Center(), Quaternion.identity);
				yield return 0;
			}
			yield break;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009AB4 File Offset: 0x00007CB4
		public void GravityPush()
		{
			this.Hand().HapticTick(1f, 5f);
			Catalog.GetData<EffectData>("ShatterbladeGravityAoE", true).Spawn(this.Center() + this.ForwardDir() * 0.2f, Quaternion.LookRotation(this.ForwardDir(), this.UpDir()), null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
			Utils.PushForce(this.Hand().transform.position + this.ForwardDir() * 1f, this.ForwardDir(), 1f, 4f, this.ForwardDir() * this.altFireForce, true, true);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009B74 File Offset: 0x00007D74
		public bool HoldingSomething()
		{
			return this.targetItem != null;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009B84 File Offset: 0x00007D84
		public float HeldRadius()
		{
			bool flag = this.targetItem;
			float num;
			if (flag)
			{
				Item item = this.targetItem;
				this.lastHeldRadius = Mathf.Min((item != null) ? (item.GetRadius() + 0.1f) : 0.5f, 2f);
				num = this.lastHeldRadius;
			}
			else
			{
				num = this.lastHeldRadius;
			}
			return num;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00009BE2 File Offset: 0x00007DE2
		public Vector3 HeldCenter()
		{
			return this.targetItem.rb.worldCenterOfMass;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009BF4 File Offset: 0x00007DF4
		public Vector3 PincerPos(int i)
		{
			bool flag = this.HoldingSomething();
			Vector3 vector;
			if (flag)
			{
				vector = this.HeldCenter() + Quaternion.AngleAxis((float)((i == 1) ? 0 : 180) + this.rotation / 3f, this.HeldCenter() - this.Center()) * this.UpDir() * (0.1f + this.HeldRadius());
			}
			else
			{
				bool flag2 = base.IsButtonPressed();
				if (flag2)
				{
					vector = this.Center() + this.SideDir() * ((i == 1) ? 0.1f : (-0.1f)) + this.ForwardDir() * 0.15f;
				}
				else
				{
					vector = this.Center() + this.UpDir() * ((i == 1) ? 1f : (-1f)) * 0.1f + this.ForwardDir() * 0.15f;
				}
			}
			return vector;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00009D00 File Offset: 0x00007F00
		public Vector3 InnerRing(int i)
		{
			Debug.Log(i);
			bool flag = base.IsButtonPressed();
			Vector3 vector;
			if (flag)
			{
				vector = this.Center() + Quaternion.AngleAxis((float)(i - 10) * 0.2f * 360f + this.rotation, this.ForwardDir()) * this.UpDir() * 0.1f;
			}
			else
			{
				vector = this.Center() + Quaternion.AngleAxis((float)(i - 10) * 0.2f * 360f + this.rotation, this.ForwardDir()) * this.UpDir() * (this.HoldingSomething() ? 0.1f : 0.3f) + this.ForwardDir() * (this.HoldingSomething() ? 0.2f : (-0.1f));
			}
			return vector;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public Vector3 OuterRing(int i)
		{
			bool flag = !this.HoldingSomething() && base.IsButtonPressed();
			Vector3 vector;
			if (flag)
			{
				vector = this.Center() + Quaternion.AngleAxis((float)(i - 2) * 0.14285715f * 360f + this.rotation * -1f, this.ForwardDir()) * this.UpDir() * 0.3f + this.ForwardDir() * 0.1f;
			}
			else
			{
				vector = this.Center() + Quaternion.AngleAxis((float)(i - 2) * 0.14285715f * 360f + this.rotation * -1f, this.ForwardDir()) * this.UpDir() * (this.HoldingSomething() ? 0.2f : 0.3f) + this.ForwardDir() * (this.HoldingSomething() ? 0.1f : (-0.2f));
			}
			return vector;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009EEC File Offset: 0x000080EC
		public override Vector3 GetPos(int i, Rigidbody rb, BladePart part)
		{
			bool flag = i < 3;
			Vector3 vector;
			if (flag)
			{
				vector = this.PincerPos(i);
			}
			else
			{
				bool flag2 = i < 10;
				if (flag2)
				{
					vector = this.OuterRing(i);
				}
				else
				{
					vector = this.InnerRing(i);
				}
			}
			return vector;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009F2D File Offset: 0x0000812D
		public Transform GetTargetTransform()
		{
			Item item = this.targetItem;
			return (item != null) ? item.transform : null;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009F44 File Offset: 0x00008144
		public override Quaternion GetRot(int i, Rigidbody rb, BladePart part)
		{
			bool flag = i <= 2;
			Quaternion quaternion;
			if (flag)
			{
				bool flag2 = this.HoldingSomething();
				if (flag2)
				{
					quaternion = Quaternion.LookRotation(this.GetTargetTransform().position - rb.transform.position, this.Center() - rb.transform.position);
				}
				else
				{
					bool flag3 = base.IsButtonPressed();
					if (flag3)
					{
						quaternion = Quaternion.LookRotation(this.UpDir(), this.Center() - this.ForwardDir() * 0.1f - rb.transform.position);
					}
					else
					{
						quaternion = Quaternion.LookRotation(this.ForwardDir(), this.SideDir());
					}
				}
			}
			else
			{
				bool flag4 = this.HoldingSomething();
				if (flag4)
				{
					quaternion = Quaternion.LookRotation(this.ForwardDir(), rb.transform.position - this.Center());
				}
				else
				{
					bool flag5 = base.IsButtonPressed();
					if (flag5)
					{
						quaternion = Quaternion.LookRotation(this.Center() + this.ForwardDir() * -0.2f - rb.transform.position, this.Center() + this.ForwardDir() * 0.3f - rb.transform.position);
					}
					else
					{
						quaternion = Quaternion.LookRotation(rb.transform.position - this.Center(), this.ForwardDir());
					}
				}
			}
			return quaternion;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A0CB File Offset: 0x000082CB
		public override void OnButtonPressed()
		{
			base.OnButtonPressed();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A0D8 File Offset: 0x000082D8
		public override void OnTriggerPressed()
		{
			this.effectIntensity = 0f;
			EffectInstance effectInstance = this.effect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			bool flag = base.IsButtonPressed();
			if (flag)
			{
				this.GravityPush();
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A11C File Offset: 0x0000831C
		public override void OnTriggerHeld()
		{
			base.OnTriggerHeld();
			bool flag = !base.IsButtonPressed() && !this.HoldingSomething();
			if (flag)
			{
				this.AcquireTarget();
			}
			this.Hand().HapticTick((Mathf.Sin((Time.time - this.lastTriggerPress) * 10f) + 1f) / 2f * 0.5f, 20f);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A18C File Offset: 0x0000838C
		public override void OnTriggerReleased()
		{
			base.OnTriggerReleased();
			bool flag = this.joint;
			if (flag)
			{
				Object.Destroy(this.joint);
			}
			Item item = this.targetItem;
			if (item != null)
			{
				item.collisionHandlers.ForEach(delegate(CollisionHandler ch)
				{
					ch.RemovePhysicModifier(this);
				});
			}
			bool flag2 = this.HoldingSomething();
			if (flag2)
			{
				this.ThrowHeldThing();
			}
			EffectInstance effectInstance = this.effect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			this.targetItem = null;
			this.joint = null;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A216 File Offset: 0x00008416
		public override string GetUseAnnotation()
		{
			return base.IsButtonPressed() ? "Pull trigger for gravity blast" : (this.HoldingSomething() ? "Release trigger to throw" : "Pull trigger to attract an object");
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A23B File Offset: 0x0000843B
		public override bool GetUseAnnotationShown()
		{
			return true;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A23E File Offset: 0x0000843E
		public override string GetAltUseAnnotation()
		{
			return "Hold button to switch modes";
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A245 File Offset: 0x00008445
		public override bool GetAltUseAnnotationShown()
		{
			return !base.IsButtonPressed() && !base.IsTriggerPressed();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A25B File Offset: 0x0000845B
		public override float Cooldown()
		{
			return base.IsButtonPressed() ? this.cooldown : 0f;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A274 File Offset: 0x00008474
		public override void Update()
		{
			base.Update();
			this.targetPoint.transform.position = this.Center() + this.ForwardDir() * (0.2f + this.HeldRadius() * 1.5f);
			this.targetPoint.transform.rotation = Quaternion.LookRotation(this.ForwardDir(), this.UpDir());
			bool flag = this.HoldingSomething();
			if (flag)
			{
				this.targetItem.rb.velocity *= Mathf.Lerp(0.7f, 1f, Mathf.InverseLerp(0f, 4f, Vector3.Distance(this.targetItem.transform.position, this.targetPoint.position)));
				this.targetItem.rb.velocity = this.targetItem.rb.velocity.magnitude * (this.targetPoint.transform.position - this.targetItem.transform.position).normalized;
				bool flag2 = this.effect != null;
				if (flag2)
				{
					this.effectIntensity = Mathf.Min(this.effectIntensity + Time.deltaTime * 3f, 1f);
					this.UpdateEffectParams(this.effect, this.effectIntensity, this.HeldCenter(), this.targetItem.transform.rotation);
				}
				this.rotation += Time.deltaTime * Mathf.Lerp(80f, 300f, Mathf.Clamp01((Time.time - this.lastTriggerPress) * 1f));
			}
			else
			{
				this.effectIntensity = Mathf.Max(this.effectIntensity - Time.deltaTime * 3f, 0f);
				this.UpdateEffectParams(this.effect, this.effectIntensity, this.Center() + this.ForwardDir() * (0.2f + this.HeldRadius() * 1.5f), Quaternion.identity);
				this.rotation += Time.deltaTime * 80f;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A4BC File Offset: 0x000086BC
		public override void Exit()
		{
			base.Exit();
			Object.Destroy(this.joint);
			Object.Destroy(this.targetPoint);
			EffectInstance effectInstance = this.effect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			Item item = this.targetItem;
			if (item != null)
			{
				item.collisionHandlers.ForEach(delegate(CollisionHandler ch)
				{
					ch.RemovePhysicModifier(this);
				});
			}
		}

		// Token: 0x0400006A RID: 106
		public float cooldown = 1f;

		// Token: 0x0400006B RID: 107
		public float itemThrowForce = 30f;

		// Token: 0x0400006C RID: 108
		public float altFireForce = 30f;

		// Token: 0x0400006D RID: 109
		private float rotation;

		// Token: 0x0400006E RID: 110
		private Item targetItem;

		// Token: 0x0400006F RID: 111
		private ConfigurableJoint joint;

		// Token: 0x04000070 RID: 112
		private Rigidbody targetPoint;

		// Token: 0x04000071 RID: 113
		private float effectIntensity;

		// Token: 0x04000072 RID: 114
		private EffectInstance effect;

		// Token: 0x04000073 RID: 115
		private float lastHeldRadius;
	}
}
