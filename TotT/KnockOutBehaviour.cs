using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000017 RID: 23
	public class KnockOutBehaviour : MonoBehaviour
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000060A1 File Offset: 0x000042A1
		public void Start()
		{
			this.creature = base.GetComponent<Creature>();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000060B0 File Offset: 0x000042B0
		public void Setup(float minutes)
		{
			this.go = false;
			this.creature = base.GetComponent<Creature>();
			this.creature.ragdoll.SetState(0);
			this.creature.brain.AddNoStandUpModifier(this);
			this.Detection = this.creature.brain.instance.GetModule<BrainModuleDetection>(false);
			bool flag = this.Detection != null;
			if (flag)
			{
				this.Detection.canHear = false;
				this.oldSightHFov = this.Detection.sightDetectionHorizontalFov;
				this.oldSightVFov = this.Detection.sightDetectionVerticalFov;
				this.Detection.sightDetectionHorizontalFov = 0f;
				this.Detection.sightDetectionVerticalFov = 0f;
			}
			this.Speak = this.creature.brain.instance.GetModule<BrainModuleSpeak>(false);
			bool flag2 = this.Speak != null;
			if (flag2)
			{
				this.oldSpeakAlertChance = this.Speak.randomSpeakAlertChance;
				this.oldSpeakCombatChance = this.Speak.randomSpeakCombatChance;
				this.oldSpeakIdleChance = this.Speak.randomSpeakIdleChance;
				this.oldSpeakInvestigateChance = this.Speak.randomSpeakInvestigateChance;
				this.Speak.randomSpeakAlertChance = 0f;
				this.Speak.randomSpeakCombatChance = 0f;
				this.Speak.randomSpeakIdleChance = 0f;
				this.Speak.randomSpeakInvestigateChance = 0f;
			}
			this.TimerMax = minutes * 60f;
			this.Timer = this.TimerMax;
			this.go = true;
			base.SendMessage("CheckDominoLink");
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000624C File Offset: 0x0000444C
		public void SetupWithDelay(float minutes, float delay)
		{
			base.StartCoroutine(this.CompleteDelaySetup(minutes, delay));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000625E File Offset: 0x0000445E
		public IEnumerator CompleteDelaySetup(float minutes, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.creature.ragdoll.SetState(0);
			this.creature.brain.AddNoStandUpModifier(this);
			this.Detection = this.creature.brain.instance.GetModule<BrainModuleDetection>(false);
			bool flag = this.Detection != null;
			if (flag)
			{
				this.Detection.canHear = false;
				this.oldSightHFov = this.Detection.sightDetectionHorizontalFov;
				this.oldSightVFov = this.Detection.sightDetectionVerticalFov;
				this.Detection.sightDetectionHorizontalFov = 0f;
				this.Detection.sightDetectionVerticalFov = 0f;
			}
			this.Speak = this.creature.brain.instance.GetModule<BrainModuleSpeak>(false);
			bool flag2 = this.Speak != null;
			if (flag2)
			{
				this.oldSpeakAlertChance = this.Speak.randomSpeakAlertChance;
				this.oldSpeakCombatChance = this.Speak.randomSpeakCombatChance;
				this.oldSpeakIdleChance = this.Speak.randomSpeakIdleChance;
				this.oldSpeakInvestigateChance = this.Speak.randomSpeakInvestigateChance;
				this.Speak.randomSpeakAlertChance = 0f;
				this.Speak.randomSpeakCombatChance = 0f;
				this.Speak.randomSpeakIdleChance = 0f;
				this.Speak.randomSpeakInvestigateChance = 0f;
			}
			this.TimerMax = minutes * 60f;
			this.Timer = this.TimerMax;
			this.go = true;
			base.SendMessage("CheckDominoLink");
			yield break;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000627C File Offset: 0x0000447C
		public void Update()
		{
			bool flag = this.go;
			if (flag)
			{
				this.Timer -= Time.deltaTime;
				bool flag2 = this.Timer <= 0f;
				if (flag2)
				{
					this.go = false;
					this.creature.brain.RemoveNoStandUpModifier(this);
					bool flag3 = this.Detection != null;
					if (flag3)
					{
						this.Detection.canHear = true;
						this.Detection.sightDetectionHorizontalFov = this.oldSightHFov;
						this.Detection.sightDetectionVerticalFov = this.oldSightVFov;
					}
					bool flag4 = this.Speak != null;
					if (flag4)
					{
						this.Speak.randomSpeakAlertChance = this.oldSpeakAlertChance;
						this.Speak.randomSpeakCombatChance = this.oldSpeakCombatChance;
						this.Speak.randomSpeakIdleChance = this.oldSpeakIdleChance;
						this.Speak.randomSpeakInvestigateChance = this.oldSpeakInvestigateChance;
					}
					Object.Destroy(this);
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006374 File Offset: 0x00004574
		public void DominoLinkedTo(Creature creature)
		{
			bool flag = creature.gameObject.GetComponent<KnockOutBehaviour>() == null;
			if (flag)
			{
				KnockOutBehaviour temp = creature.gameObject.AddComponent<KnockOutBehaviour>();
				temp.Setup(this.TimerMax / 60f);
			}
		}

		// Token: 0x0400007A RID: 122
		private Creature creature;

		// Token: 0x0400007B RID: 123
		private float Timer;

		// Token: 0x0400007C RID: 124
		private float TimerMax;

		// Token: 0x0400007D RID: 125
		private BrainModuleSpeak Speak;

		// Token: 0x0400007E RID: 126
		private BrainModuleDetection Detection;

		// Token: 0x0400007F RID: 127
		private float oldSightHFov;

		// Token: 0x04000080 RID: 128
		private float oldSightVFov;

		// Token: 0x04000081 RID: 129
		private float oldSpeakAlertChance;

		// Token: 0x04000082 RID: 130
		private float oldSpeakCombatChance;

		// Token: 0x04000083 RID: 131
		private float oldSpeakIdleChance;

		// Token: 0x04000084 RID: 132
		private float oldSpeakInvestigateChance;

		// Token: 0x04000085 RID: 133
		private bool go = false;
	}
}
