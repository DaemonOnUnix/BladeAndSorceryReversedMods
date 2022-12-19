using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace DominoSpell
{
	// Token: 0x02000008 RID: 8
	public class SpellCastDomino : SpellCastCharge
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00005C70 File Offset: 0x00003E70
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			SpellCastDomino.markerData = Catalog.GetData<EffectData>("DominoMarker", true);
			SpellCastDomino.targetData = Catalog.GetData<EffectData>("DominoTarget", true);
			SpellCastDomino.singleTargetData = Catalog.GetData<EffectData>("DominoTargetSingle", true);
			SpellCastDomino.linkData = Catalog.GetData<EffectData>("DominoLink", true);
			SpellCastDomino.handEffectData = Catalog.GetData<EffectData>("DominoHand", true);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public override void Unload()
		{
			base.Unload();
			EffectInstance effectInstance = this.targetEffectA;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			this.targetEffectA = null;
			EffectInstance effectInstance2 = this.targetEffectB;
			if (effectInstance2 != null)
			{
				effectInstance2.End(false, -1f);
			}
			this.targetEffectB = null;
			EffectInstance effectInstance3 = this.handEffect;
			if (effectInstance3 != null)
			{
				effectInstance3.End(false, -1f);
			}
			this.handEffect = null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005D4C File Offset: 0x00003F4C
		public void PulseArm(float multiplier = 1f)
		{
			bool flag = this.pulsing;
			if (!flag)
			{
				EffectInstance effect = this.handEffect;
				bool flag2 = effect == null;
				if (!flag2)
				{
					this.pulsing = true;
					this.spellCaster.StartCoroutine(Utils.LoopOver(delegate(float time)
					{
						EffectInstance effect = effect;
						if (effect != null)
						{
							string text = "Intensity";
							float[] array = new float[] { 0f, 0f, 0.75f, 0.5f, 0.25f, 0f };
							array[1] = multiplier;
							effect.SetVFXProperty(text, time.Curve(array));
						}
					}, 0.4f, delegate
					{
						EffectInstance effect = effect;
						if (effect != null)
						{
							effect.SetVFXProperty("Intensity", 0);
						}
						this.pulsing = false;
					}));
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005DCA File Offset: 0x00003FCA
		public Vector3 ArmPos()
		{
			return Vector3.Lerp(this.spellCaster.ragdollHand.transform.position, this.spellCaster.ragdollHand.lowerArmPart.transform.position, 0.5f);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005E05 File Offset: 0x00004005
		public Quaternion ArmDir()
		{
			return Quaternion.LookRotation(this.spellCaster.ragdollHand.transform.position - this.spellCaster.ragdollHand.lowerArmPart.transform.position, Vector3.up);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005E48 File Offset: 0x00004048
		public override void Fire(bool active)
		{
			base.Fire(active);
			this.active = active;
			if (active)
			{
				this.handEffect = SpellCastDomino.handEffectData.Spawn(this.ArmPos(), this.ArmDir(), this.spellCaster.ragdollHand.lowerArmPart.transform, null, false, null, false, Array.Empty<Type>());
				this.handEffect.Play(0, false);
				this.spellCaster.ragdollHand.PlayHapticClipOver(new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 1f),
					new Keyframe(1f, 0f),
					new Keyframe(1f, 1f)
				}), 0.5f);
			}
			else
			{
				this.PulseArm(1f);
				EffectInstance effectInstance = this.handEffect;
				if (effectInstance != null)
				{
					effectInstance.End(false, -1f);
				}
				EffectInstance effectInstance2 = this.targetEffectA;
				if (effectInstance2 != null)
				{
					effectInstance2.End(false, -1f);
				}
				this.targetEffectA = null;
				EffectInstance effectInstance3 = this.targetEffectB;
				if (effectInstance3 != null)
				{
					effectInstance3.End(false, -1f);
				}
				this.targetEffectB = null;
				bool flag = this.spellCaster.ragdollHand.Gripping();
				if (flag)
				{
					this.targetedCreature = Player.currentCreature;
				}
				bool flag2 = this.targetedCreature == null;
				if (!flag2)
				{
					Creature other = this.lastTarget;
					bool flag3 = this.lastTarget == this.targetedCreature || this.lastTarget == null;
					if (flag3)
					{
						other = this.Target(null);
					}
					bool flag4 = other == this.targetedCreature || other == null;
					if (!flag4)
					{
						this.targetedCreature.gameObject.GetOrAddComponent<DominoBehaviour>().Link(other.gameObject.GetOrAddComponent<DominoBehaviour>());
						Catalog.GetData<EffectData>("DominoTargetAcquire", true).Spawn(this.targetedCreature.GetHead().transform.position + Vector3.up * 0.5f, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
						this.spellCaster.ragdollHand.PlayHapticClipOver(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 1f),
							new Keyframe(1f, 0f)
						}), 0.3f);
						this.targetedCreature = null;
						this.lastTarget = null;
					}
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000060E0 File Offset: 0x000042E0
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			Creature creature2 = this.lastTarget;
			bool flag;
			if (creature2 == null || !creature2.isKilled)
			{
				Creature creature3 = this.lastTarget;
				flag = ((creature3 != null) ? creature3.gameObject : null) == null;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.lastTarget = null;
				EffectInstance effectInstance = this.targetEffectB;
				if (effectInstance != null)
				{
					effectInstance.End(false, -1f);
				}
				this.targetEffectB = null;
				EffectInstance effectInstance2 = this.targetEffectA;
				if (effectInstance2 != null)
				{
					effectInstance2.End(false, -1f);
				}
				this.targetEffectA = null;
			}
			Creature creature = this.Target(null);
			bool flag3 = creature != this.lastTarget;
			if (flag3)
			{
				bool flag4 = this.active;
				if (flag4)
				{
					bool flag5 = this.targetedCreature == null;
					if (flag5)
					{
						this.targetedCreature = this.lastTarget;
						bool flag6 = this.targetedCreature != null;
						if (flag6)
						{
							Catalog.GetData<EffectData>("DominoTargetAcquire", true).Spawn(this.targetedCreature.GetHead().transform.position + Vector3.up * 0.5f, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
						}
					}
				}
				bool flag7 = creature != null;
				if (flag7)
				{
					this.lastTarget = creature;
					bool flag8 = this.lastTarget != null;
					if (flag8)
					{
						bool flag9 = Time.time - this.lastVibrate > 0.3f;
						if (flag9)
						{
							this.lastVibrate = Time.time;
							this.spellCaster.ragdollHand.HapticTick(0.5f, 30f);
							this.PulseArm(0.2f);
						}
						bool flag10 = this.targetEffectB == null && this.active;
						if (flag10)
						{
							this.targetEffectB = this.targetEffectA;
							this.targetEffectA = null;
						}
						bool flag11 = this.targetEffectA == null;
						if (flag11)
						{
							this.targetEffectA = SpellCastDomino.singleTargetData.Spawn(creature.GetHead().transform.position + Vector3.up * 0.5f, Quaternion.identity, null, null, false, null, false, Array.Empty<Type>());
							this.targetEffectA.Play(0, false);
						}
					}
				}
			}
			bool flag12 = this.active && this.targetedCreature;
			if (flag12)
			{
				EffectInstance effectInstance3 = this.targetEffectB;
				if (effectInstance3 != null)
				{
					effectInstance3.SetPosition(this.targetedCreature.GetHead().transform.position + Vector3.up * 0.5f);
				}
			}
			else
			{
				EffectInstance effectInstance4 = this.targetEffectB;
				if (effectInstance4 != null)
				{
					effectInstance4.Despawn();
				}
				this.targetEffectB = null;
			}
			bool flag13 = this.lastTarget;
			if (flag13)
			{
				EffectInstance effectInstance5 = this.targetEffectA;
				if (effectInstance5 != null)
				{
					effectInstance5.SetPosition(this.lastTarget.GetHead().transform.position + Vector3.up * 0.5f);
				}
			}
			else
			{
				EffectInstance effectInstance6 = this.targetEffectA;
				if (effectInstance6 != null)
				{
					effectInstance6.Despawn();
				}
				this.targetEffectA = null;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000640C File Offset: 0x0000460C
		public void SetTargetEffect(Vector3 source, Vector3? target = null)
		{
			Vector3 newTarget = target ?? source;
			EffectInstance effectInstance = this.targetEffectB;
			if (effectInstance != null)
			{
				effectInstance.SetPosition(Vector3.Lerp(source, newTarget, 0.5f));
			}
			EffectInstance effectInstance2 = this.targetEffectB;
			if (effectInstance2 != null)
			{
				effectInstance2.SetVFXProperty("Active", this.active);
			}
			EffectInstance effectInstance3 = this.targetEffectB;
			if (effectInstance3 != null)
			{
				effectInstance3.SetVFXProperty("Source", source);
			}
			EffectInstance effectInstance4 = this.targetEffectB;
			if (effectInstance4 != null)
			{
				effectInstance4.SetVFXProperty("Target", newTarget);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000649C File Offset: 0x0000469C
		public Creature Target(Creature toIgnore = null)
		{
			return Utils.TargetCreature(this.spellCaster.ragdollHand.transform.position, this.spellCaster.ragdollHand.PointDir(), 30f, 20f, toIgnore);
		}

		// Token: 0x04000014 RID: 20
		public Creature targetedCreature;

		// Token: 0x04000015 RID: 21
		private bool active;

		// Token: 0x04000016 RID: 22
		private Creature lastTarget;

		// Token: 0x04000017 RID: 23
		private float lastVibrate;

		// Token: 0x04000018 RID: 24
		public static EffectData markerData;

		// Token: 0x04000019 RID: 25
		public static EffectData targetData;

		// Token: 0x0400001A RID: 26
		public static EffectData singleTargetData;

		// Token: 0x0400001B RID: 27
		public static EffectData linkData;

		// Token: 0x0400001C RID: 28
		public static EffectData handEffectData;

		// Token: 0x0400001D RID: 29
		public EffectInstance targetEffectA;

		// Token: 0x0400001E RID: 30
		public EffectInstance targetEffectB;

		// Token: 0x0400001F RID: 31
		public EffectInstance handEffect;

		// Token: 0x04000020 RID: 32
		public float orgPhysicRadius = 0f;

		// Token: 0x04000021 RID: 33
		private bool pulsing;
	}
}
