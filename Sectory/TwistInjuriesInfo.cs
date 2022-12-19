using System;

namespace Sectory
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class TwistInjuriesInfo
	{
		// Token: 0x0400006B RID: 107
		public TwistInjuriesInfo.PartToTwist[] partToTwists;

		// Token: 0x02000028 RID: 40
		[Serializable]
		public class PartToTwist
		{
			// Token: 0x0600008F RID: 143 RVA: 0x000076FA File Offset: 0x000058FA
			public PartToTwist(string partName, bool twistSnappingAllowed, float twistSpeedToSnap)
			{
				this.partName = partName;
				this.twistSnappingAllowed = twistSnappingAllowed;
				this.twistSpeedToSnap = twistSpeedToSnap;
			}

			// Token: 0x040000E1 RID: 225
			public string partName;

			// Token: 0x040000E2 RID: 226
			public bool twistSnappingAllowed;

			// Token: 0x040000E3 RID: 227
			public float twistSpeedToSnap;
		}
	}
}
