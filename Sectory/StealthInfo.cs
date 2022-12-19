using System;

namespace Sectory
{
	// Token: 0x0200000C RID: 12
	[Serialize("StealthSettings")]
	[Serializable]
	public class StealthInfo
	{
		// Token: 0x04000054 RID: 84
		public float sightMaxRange;

		// Token: 0x04000055 RID: 85
		[Range(0, 20)]
		public float averageHearRange;

		// Token: 0x04000056 RID: 86
		[Range(0, 360)]
		public float creatureFovInDegrees;

		// Token: 0x04000057 RID: 87
		[Range(0, 10)]
		public float creatureSightHeightMax;

		// Token: 0x04000058 RID: 88
		[Range(0, 120)]
		public float averageInvestigationDuration;

		// Token: 0x04000059 RID: 89
		public bool enemiesCanHear;

		// Token: 0x0400005A RID: 90
		public bool useVanillaStealthSettingsInstead;
	}
}
