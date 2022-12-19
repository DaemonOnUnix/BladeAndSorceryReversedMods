using System;

namespace Mono.Cecil
{
	// Token: 0x0200018B RID: 395
	internal sealed class CustomAttributeValueProjection
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x00029B30 File Offset: 0x00027D30
		public CustomAttributeValueProjection(AttributeTargets targets, CustomAttributeValueTreatment treatment)
		{
			this.Targets = targets;
			this.Treatment = treatment;
		}

		// Token: 0x0400057A RID: 1402
		public readonly AttributeTargets Targets;

		// Token: 0x0400057B RID: 1403
		public readonly CustomAttributeValueTreatment Treatment;
	}
}
