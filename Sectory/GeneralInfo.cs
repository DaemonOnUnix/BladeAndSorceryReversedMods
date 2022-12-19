using System;
using System.Linq;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200000A RID: 10
	[Serialize("GeneralSettings")]
	[Serializable]
	public class GeneralInfo
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000026D3 File Offset: 0x000008D3
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000026DB File Offset: 0x000008DB
		[Range(1, 250)]
		public float Difficulty
		{
			get
			{
				return this._difficultyPercentage;
			}
			set
			{
				this._difficultyPercentage = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000026E4 File Offset: 0x000008E4
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000026F2 File Offset: 0x000008F2
		[JsonIgnore]
		[MenuIgnore]
		public float ScriptDifficulty
		{
			get
			{
				return this._difficultyPercentage / 100f;
			}
			set
			{
				this._difficultyPercentage = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000026FB File Offset: 0x000008FB
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002704 File Offset: 0x00000904
		[Range(0, 10000)]
		public float CreatureAddHealth
		{
			get
			{
				return this._creatureAddHealth;
			}
			set
			{
				foreach (CreatureData creatureData in from i in Catalog.GetAllID<CreatureData>()
					select Catalog.GetData<CreatureData>(i, true) into s
					where !s.name.Contains("Player")
					select s)
				{
					creatureData.health = (short)Mathf.Clamp((float)Entry.inst.creatureToHealth[creatureData] + value, -32767f, 32767f);
				}
				this._creatureAddHealth = value;
			}
		}

		// Token: 0x04000033 RID: 51
		[Range(0, 100)]
		public float deathFakingChance;

		// Token: 0x04000034 RID: 52
		[Range(0, 100)]
		public float enemyRecoveryRate;

		// Token: 0x04000035 RID: 53
		[Range(0, 100)]
		public float fakingDamageReduction;

		// Token: 0x04000036 RID: 54
		[Range(0, 1)]
		public float elbowSliceDifficulty;

		// Token: 0x04000037 RID: 55
		[Range(0, 1)]
		public float wristSliceDifficulty;

		// Token: 0x04000038 RID: 56
		[Range(0, 1)]
		public float ankleSliceDifficulty;

		// Token: 0x04000039 RID: 57
		[Range(0, 1)]
		public float kneeSliceDifficulty;

		// Token: 0x0400003A RID: 58
		[Range(0, 1)]
		public float neckSliceDifficulty;

		// Token: 0x0400003B RID: 59
		[Range(0, 1)]
		public float slicingDifficultyDamageOverride;

		// Token: 0x0400003C RID: 60
		[Range(0, 10)]
		public float drawDismembermentDamageRequirement;

		// Token: 0x0400003D RID: 61
		[Range(1, 10000)]
		public float playerHealth;

		// Token: 0x0400003E RID: 62
		private float _difficultyPercentage;

		// Token: 0x0400003F RID: 63
		private float _creatureAddHealth;

		// Token: 0x04000040 RID: 64
		[Range(1, 25)]
		public float axeDamageMult;

		// Token: 0x04000041 RID: 65
		[Range(1, 25)]
		public float bluntDamageMult;

		// Token: 0x04000042 RID: 66
		[Range(1, 25)]
		public float rangedDamageMult;

		// Token: 0x04000043 RID: 67
		[Range(1, 25)]
		public float skullPenetrateKillDepth;

		// Token: 0x04000044 RID: 68
		public bool playerEffected;

		// Token: 0x04000045 RID: 69
		public bool cinematicDeath;

		// Token: 0x04000046 RID: 70
		public bool survivableDismemberment;

		// Token: 0x04000047 RID: 71
		public bool deathAnimations;

		// Token: 0x04000048 RID: 72
		public bool lowPhysicsForFPSBoost;

		// Token: 0x04000049 RID: 73
		public bool hardcoreMode;

		// Token: 0x0400004A RID: 74
		public bool deathFaking;

		// Token: 0x0400004B RID: 75
		public bool debugMode;

		// Token: 0x0400004C RID: 76
		public bool liftFromAnywhere;

		// Token: 0x0400004D RID: 77
		public bool pressureDismemberment;

		// Token: 0x0400004E RID: 78
		public bool cleanerDismemberment;
	}
}
