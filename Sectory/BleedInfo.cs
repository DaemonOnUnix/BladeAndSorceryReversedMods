using System;

namespace Sectory
{
	// Token: 0x02000005 RID: 5
	[Serialize("BleedSettings")]
	[Serializable]
	public class BleedInfo
	{
		// Token: 0x04000014 RID: 20
		public PartToMultiplier[] partToBleedMults;

		// Token: 0x04000015 RID: 21
		public float bleedPercentage;

		// Token: 0x04000016 RID: 22
		[Range(1, 120)]
		public float bleedTime;

		// Token: 0x04000017 RID: 23
		[Range(1, 30)]
		public float bleedMinimum;

		// Token: 0x04000018 RID: 24
		[Range(0, 100)]
		public float dismemberBleedHealthAway;

		// Token: 0x04000019 RID: 25
		public bool bleedVFX;

		// Token: 0x0400001A RID: 26
		public bool bleeding;
	}
}
