using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000018 RID: 24
	internal class CannonMode : GrabbedShardMode
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000AE8B File Offset: 0x0000908B
		public override int TargetPartNum()
		{
			return 10;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000AE90 File Offset: 0x00009090
		public override Vector3 Center()
		{
			return this.Hand().transform.position + this.UpDir() * 0.1f;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000AEC8 File Offset: 0x000090C8
		public override void Enter(Shatterblade sword)
		{
			this.magazine = (from rb in sword.jointRBs
				where rb.name != "Blade_1"
				where rb.name != "Blade_10"
				select rb).ToList<Rigidbody>();
			base.Enter(sword);
			this.spinEffect = Catalog.GetData<EffectData>("ShatterbladeSpin", true).Spawn(this.Center(), Quaternion.identity, null, null, false, null, false, Array.Empty<Type>());
			sword.IgnoreRagdoll(Player.currentCreature.ragdoll, true);
			this.lastHand = this.Hand();
			this.ChamberRound();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000AF87 File Offset: 0x00009187
		public override bool GetUseAnnotationShown()
		{
			return true;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000AF8A File Offset: 0x0000918A
		public override string GetUseAnnotation()
		{
			return base.IsButtonPressed() ? "Pull trigger to burst fire" : "Pull trigger to fire";
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public override bool GetAltUseAnnotationShown()
		{
			return !base.IsButtonPressed();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000AFAB File Offset: 0x000091AB
		public override string GetAltUseAnnotation()
		{
			return "Hold [[BUTTON]] to charge up a burst shot";
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000AFB4 File Offset: 0x000091B4
		public void Reload()
		{
			bool flag = Time.time - this.lastReload <= 1f;
			if (!flag)
			{
				this.lastReload = Time.time;
				this.reloading = true;
				this.magazine = (from rb in this.sword.jointRBs
					where rb.name != "Blade_1"
					where rb.name != "Blade_10"
					select rb).ToList<Rigidbody>();
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B050 File Offset: 0x00009250
		public void ChamberRound(Rigidbody round)
		{
			this.chamberedRound = null;
			bool flag = !this.magazine.Any<Rigidbody>();
			if (!flag)
			{
				this.chamberedRound = round;
				this.Hand().HapticTick(0.2f, 100f);
				this.magazine.Remove(this.chamberedRound);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000B0A8 File Offset: 0x000092A8
		public void ChamberRound()
		{
			bool flag = !this.magazine.Any<Rigidbody>();
			if (!flag)
			{
				this.ChamberRound(this.magazine[Random.Range(0, this.magazine.Count<Rigidbody>())]);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000B0F0 File Offset: 0x000092F0
		public void Fire(Rigidbody round)
		{
			this.Hand().PlayHapticClipOver(AnimationCurve.EaseInOut(0f, 1f, 1f, 0f), 0.2f);
			this.sword.rbMap[round].Detach(false);
			this.sword.rbMap[round].item.rb.AddForce(Utils.HomingThrow(this.sword.rbMap[round].item.rb, this.ForwardDir() * this.fireVelocity, 30f, null), 1);
			this.sword.rbMap[round].item.Throw(1f, 2);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000B1BC File Offset: 0x000093BC
		public void Fire()
		{
			bool flag = !this.chamberedRound;
			if (!flag)
			{
				Catalog.GetData<EffectData>("ShatterbladeShoot", true).Spawn(this.chamberedRound.position, this.chamberedRound.rotation, null, null, false, null, false, Array.Empty<Type>()).Play(0, false);
				this.Fire(this.chamberedRound);
				this.chamberedRound = null;
				this.ChamberRound();
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B234 File Offset: 0x00009434
		public override Vector3 GetPos(int index, Rigidbody rb, BladePart part)
		{
			bool flag = rb == this.chamberedRound;
			Vector3 vector;
			if (flag)
			{
				vector = this.Center() + this.UpDir() * 0.05f;
			}
			else
			{
				bool flag2 = index == 1;
				if (flag2)
				{
					vector = this.Center() - this.Hand().ThumbDir() * 0.03f + this.ForwardDir() * 0.2f;
				}
				else
				{
					bool flag3 = this.magazine.Any<Rigidbody>();
					if (flag3)
					{
						vector = this.Center() + Quaternion.AngleAxis((float)this.magazine.IndexOf(rb) * 360f / (float)this.magazine.Count<Rigidbody>() + this.rotation, this.ForwardDir()) * this.UpDir() * (base.IsButtonPressed() ? 0.2f : 0.3f);
					}
					else
					{
						vector = this.Center() + this.UpDir() * (base.IsButtonPressed() ? 0.2f : 0.3f);
					}
				}
			}
			return vector;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B35C File Offset: 0x0000955C
		public override Quaternion GetRot(int index, Rigidbody rb, BladePart part)
		{
			bool flag = rb == this.chamberedRound;
			Quaternion quaternion;
			if (flag)
			{
				quaternion = Quaternion.LookRotation(this.ForwardDir(), this.UpDir());
			}
			else
			{
				bool flag2 = index == 1;
				if (flag2)
				{
					quaternion = Quaternion.LookRotation(this.ForwardDir(), this.SideDir());
				}
				else
				{
					quaternion = Quaternion.LookRotation(this.ForwardDir(), rb.transform.position - this.Center());
				}
			}
			return quaternion;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B3D4 File Offset: 0x000095D4
		public override void OnButtonHeld()
		{
			base.OnButtonHeld();
			this.rotation += Time.deltaTime * Mathf.Lerp(20f, 400f, Mathf.Clamp01(Time.time - this.lastButtonPress) * 2f);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B424 File Offset: 0x00009624
		public override void OnButtonNotHeld()
		{
			base.OnButtonNotHeld();
			this.rotation += Time.deltaTime * Mathf.Lerp(400f, 80f, Mathf.Clamp01((Time.time - this.lastShot) * 0.5f));
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B474 File Offset: 0x00009674
		public override void OnButtonPressed()
		{
			base.OnButtonPressed();
			this.spinEffect = Catalog.GetData<EffectData>("ShatterbladeSpin", true).Spawn(this.Center(), Quaternion.identity, null, null, false, null, false, Array.Empty<Type>());
			this.spinEffect.Play(0, false);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B4C2 File Offset: 0x000096C2
		public override void OnButtonReleased()
		{
			base.OnButtonReleased();
			this.spinEffect.End(false, -1f);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B4E0 File Offset: 0x000096E0
		public override void OnTriggerHeld()
		{
			base.OnTriggerHeld();
			bool flag = Time.time - this.lastShot > this.fireRate;
			if (flag)
			{
				this.Fire();
				bool flag2 = base.IsButtonPressed();
				if (flag2)
				{
					this.lastShot = Time.time - (this.fireRate - 0.05f);
				}
				else
				{
					this.lastShot = Time.time;
				}
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B549 File Offset: 0x00009749
		public override void OnTriggerReleased()
		{
			base.OnTriggerReleased();
			this.lastShot = 0f;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B560 File Offset: 0x00009760
		public override void Update()
		{
			base.Update();
			bool flag = this.Hand() != this.lastHand;
			if (flag)
			{
				this.sword.IgnoreCollider(this.lastHand, false, 0f);
				this.lastHand = this.Hand();
				this.sword.IgnoreCollider(this.lastHand, true, 0f);
			}
			this.spinEffect.SetPosition(this.Center());
			bool flag2 = !this.magazine.Any<Rigidbody>() && !this.chamberedRound;
			if (flag2)
			{
				this.Reload();
			}
			bool flag3 = this.reloading;
			if (flag3)
			{
				bool flag4 = this.magazine.All((Rigidbody part) => Vector3.Distance(this.sword.rbMap[part].transform.position, this.sword.GetPart(10).transform.position) < 1f);
				if (!flag4)
				{
					return;
				}
				this.reloading = false;
			}
			bool flag5 = this.chamberedRound == null;
			if (flag5)
			{
				this.ChamberRound();
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B654 File Offset: 0x00009854
		public override void Exit()
		{
			base.Exit();
			this.sword.jointRBs.ForEach(delegate(Rigidbody rb)
			{
				rb.transform.parent = this.sword.animator.transform;
			});
			this.sword.animator.enabled = true;
			EffectInstance effectInstance = this.spinEffect;
			if (effectInstance != null)
			{
				effectInstance.Despawn();
			}
			this.sword.IgnoreRagdoll(Player.currentCreature.ragdoll, false);
			this.sword.shouldLock = true;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B6D0 File Offset: 0x000098D0
		public override bool ShouldReform(BladePart part)
		{
			return (this.magazine.Contains(this.sword.partMap[part]) && part != this.sword.GetPart(10)) || part == this.sword.GetPart(1);
		}

		// Token: 0x0400007C RID: 124
		public float fireRate = 0.4f;

		// Token: 0x0400007D RID: 125
		public float fireVelocity = 60f;

		// Token: 0x0400007E RID: 126
		private List<Rigidbody> magazine;

		// Token: 0x0400007F RID: 127
		private Rigidbody chamberedRound;

		// Token: 0x04000080 RID: 128
		private float lastShot;

		// Token: 0x04000081 RID: 129
		private float lastReload;

		// Token: 0x04000082 RID: 130
		private EffectInstance spinEffect;

		// Token: 0x04000083 RID: 131
		private float rotation = 0f;

		// Token: 0x04000084 RID: 132
		private RagdollHand lastHand;

		// Token: 0x04000085 RID: 133
		private bool reloading;
	}
}
