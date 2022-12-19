using System;

namespace Sectory
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class Range : Attribute
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003440 File Offset: 0x00001640
		public Range(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0400008B RID: 139
		public int min;

		// Token: 0x0400008C RID: 140
		public int max;
	}
}
