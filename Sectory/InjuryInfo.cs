using System;

namespace Sectory
{
	// Token: 0x02000004 RID: 4
	[Serialize("InjurySettings")]
	[Serializable]
	public class InjuryInfo
	{
		// Token: 0x0400000A RID: 10
		public PartToMultiplier[] partToInjuryMults;

		// Token: 0x0400000B RID: 11
		[Range(0, 100)]
		public float minStamina;

		// Token: 0x0400000C RID: 12
		[Range(0, 20)]
		public float injuryMinAtk;

		// Token: 0x0400000D RID: 13
		[Range(-1, 1)]
		public float neckSnapTwistEase;

		// Token: 0x0400000E RID: 14
		[Range(1, 1000)]
		public float maxInjury;

		// Token: 0x0400000F RID: 15
		public bool injurySystem;

		// Token: 0x04000010 RID: 16
		public bool neckBreaking;

		// Token: 0x04000011 RID: 17
		public bool necksnapTwist;

		// Token: 0x04000012 RID: 18
		public bool staminaSystem;

		// Token: 0x04000013 RID: 19
		public bool injurySounds;
	}
}
