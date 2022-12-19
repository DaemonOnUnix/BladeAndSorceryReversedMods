using System;

namespace Sectory
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class PartToMultiplier
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000025F7 File Offset: 0x000007F7
		public PartToMultiplier(string partName, float multiplier, float multiplierB, float multiplierP, float multiplierS, float multiplierE)
		{
			this.partName = partName;
			this.multiplierBlunt = multiplierB;
			this.multiplierPierce = multiplierP;
			this.multiplierSlash = multiplierS;
			this.multiplierEnergy = multiplierE;
			this.multiplier = multiplier;
		}

		// Token: 0x04000026 RID: 38
		public string partName;

		// Token: 0x04000027 RID: 39
		public float multiplierBlunt;

		// Token: 0x04000028 RID: 40
		public float multiplierPierce;

		// Token: 0x04000029 RID: 41
		public float multiplierSlash;

		// Token: 0x0400002A RID: 42
		public float multiplierEnergy;

		// Token: 0x0400002B RID: 43
		public float multiplier;
	}
}
