using System;

namespace Mono.Cecil
{
	// Token: 0x02000280 RID: 640
	internal sealed class CustomAttributeValueProjection
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x00030DF4 File Offset: 0x0002EFF4
		public CustomAttributeValueProjection(AttributeTargets targets, CustomAttributeValueTreatment treatment)
		{
			this.Targets = targets;
			this.Treatment = treatment;
		}

		// Token: 0x040005B2 RID: 1458
		public readonly AttributeTargets Targets;

		// Token: 0x040005B3 RID: 1459
		public readonly CustomAttributeValueTreatment Treatment;
	}
}
