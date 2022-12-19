using System;

namespace Sectory
{
	// Token: 0x0200000B RID: 11
	[Serialize("CinematicSettings")]
	[Serializable]
	public class CinematicInfo
	{
		// Token: 0x0400004F RID: 79
		[Range(1, 100)]
		public float cinematicActivationDamage;

		// Token: 0x04000050 RID: 80
		[Range(0, 5)]
		public float parryCinematicTime;

		// Token: 0x04000051 RID: 81
		[Range(0, 5)]
		public float damageCinematicTime;

		// Token: 0x04000052 RID: 82
		public bool damageCinematics;

		// Token: 0x04000053 RID: 83
		public bool parryCinematics;
	}
}
