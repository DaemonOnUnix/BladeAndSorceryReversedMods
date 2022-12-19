using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x02000007 RID: 7
	public class IceSpellMWE : MonoBehaviour
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002DBA File Offset: 0x00000FBA
		public void SlowStartCoroutine(Creature targetCreature, float energy, float maxSlow, float minSlow, float duration)
		{
			base.StartCoroutine(this.SlowCoroutine(targetCreature, energy, maxSlow, minSlow, duration));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002DD1 File Offset: 0x00000FD1
		private IEnumerator SlowCoroutine(Creature targetCreature, float energy, float maxSlow, float minSlow, float duration)
		{
			EffectData imbueHitRagdollEffectData = Catalog.GetData<EffectData>("ImbueIceRagdoll", true);
			this.effectInstance = imbueHitRagdollEffectData.Spawn(targetCreature.ragdoll.rootPart.transform, true, null, false, Array.Empty<Type>());
			this.effectInstance.SetRenderer(targetCreature.GetRendererForVFX(), false);
			this.effectInstance.Play(0, false);
			this.effectInstance.SetIntensity(1f);
			float animSpeed = Mathf.Lerp(minSlow, maxSlow, energy / 100f);
			bool flag = animSpeed != 0f;
			if (flag)
			{
				targetCreature.animator.speed *= animSpeed / 100f;
				targetCreature.locomotion.SetSpeedModifier(this, animSpeed / 100f, animSpeed / 100f, animSpeed / 100f, animSpeed / 100f, animSpeed / 100f);
			}
			else
			{
				targetCreature.ragdoll.SetState(2);
				targetCreature.brain.AddNoStandUpModifier(this);
				targetCreature.brain.Stop();
			}
			yield return new WaitForSeconds(duration);
			bool flag2 = animSpeed != 0f;
			if (flag2)
			{
				targetCreature.animator.speed = 1f;
				targetCreature.locomotion.ClearSpeedModifiers();
			}
			else
			{
				bool flag3 = !targetCreature.isKilled;
				if (flag3)
				{
					targetCreature.ragdoll.SetState(1);
					targetCreature.brain.RemoveNoStandUpModifier(this);
					targetCreature.brain.Load(targetCreature.brain.instance.id);
				}
			}
			this.effectInstance.Despawn();
			yield break;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E08 File Offset: 0x00001008
		public void UnFreezeCreature(Creature targetCreature)
		{
			bool flag = !targetCreature.isKilled;
			if (flag)
			{
				targetCreature.ragdoll.SetState(1);
				targetCreature.brain.RemoveNoStandUpModifier(this);
				targetCreature.brain.Load(targetCreature.brain.instance.id);
			}
			this.effectInstance.Despawn();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E67 File Offset: 0x00001067
		private IEnumerator SetColorOfMat(Material material, float duration)
		{
			Color defaultColor = material.color;
			material.SetColor("_BaseColor", this.color);
			yield return new WaitForSeconds(duration);
			material.SetColor("_BaseColor", defaultColor);
			yield break;
		}

		// Token: 0x0400001A RID: 26
		private Color color = new Color(0.49f, 0.78f, 1f);

		// Token: 0x0400001B RID: 27
		public EffectInstance effectInstance;
	}
}
