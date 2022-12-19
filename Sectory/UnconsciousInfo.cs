using System;

namespace Sectory
{
	// Token: 0x02000006 RID: 6
	[Serialize("UnconsciousSettings")]
	[Serializable]
	public class UnconsciousInfo
	{
		// Token: 0x0400001B RID: 27
		public PartToMultiplier[] partToPainMults;

		// Token: 0x0400001C RID: 28
		[Range(5, 100)]
		public float bloodLossKnockoutPercentage;

		// Token: 0x0400001D RID: 29
		[Range(5, 100)]
		public float averageBodyPainShutDownPercentage;

		// Token: 0x0400001E RID: 30
		[Range(30, 150)]
		public float maxAdrenaline;

		// Token: 0x0400001F RID: 31
		[Range(30, 150)]
		public float maxLimbPain;

		// Token: 0x04000020 RID: 32
		[Range(0, 1)]
		public float bleedingInfluencePainMult;

		// Token: 0x04000021 RID: 33
		[Range(0, 1)]
		public float damageInfluenceBloodLossMult;

		// Token: 0x04000022 RID: 34
		[Range(0, 1)]
		public float adrenalineGainPercentageHit;

		// Token: 0x04000023 RID: 35
		[Range(0, 1)]
		public float negativeAdrenalineSleepAmount;

		// Token: 0x04000024 RID: 36
		public bool unconsciousEnabled;

		// Token: 0x04000025 RID: 37
		public bool holdWeaponsWhileAsleep;
	}
}
