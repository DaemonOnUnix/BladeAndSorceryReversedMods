using System;

namespace Mono.Cecil
{
	// Token: 0x02000189 RID: 393
	internal sealed class MethodDefinitionProjection
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x00029AE2 File Offset: 0x00027CE2
		public MethodDefinitionProjection(MethodDefinition method, MethodDefinitionTreatment treatment)
		{
			this.Attributes = method.Attributes;
			this.ImplAttributes = method.ImplAttributes;
			this.Name = method.Name;
			this.Treatment = treatment;
		}

		// Token: 0x04000574 RID: 1396
		public readonly MethodAttributes Attributes;

		// Token: 0x04000575 RID: 1397
		public readonly MethodImplAttributes ImplAttributes;

		// Token: 0x04000576 RID: 1398
		public readonly string Name;

		// Token: 0x04000577 RID: 1399
		public readonly MethodDefinitionTreatment Treatment;
	}
}
