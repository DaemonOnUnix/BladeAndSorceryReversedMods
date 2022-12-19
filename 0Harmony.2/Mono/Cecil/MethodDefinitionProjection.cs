using System;

namespace Mono.Cecil
{
	// Token: 0x0200027E RID: 638
	internal sealed class MethodDefinitionProjection
	{
		// Token: 0x06000FF0 RID: 4080 RVA: 0x00030DA6 File Offset: 0x0002EFA6
		public MethodDefinitionProjection(MethodDefinition method, MethodDefinitionTreatment treatment)
		{
			this.Attributes = method.Attributes;
			this.ImplAttributes = method.ImplAttributes;
			this.Name = method.Name;
			this.Treatment = treatment;
		}

		// Token: 0x040005AC RID: 1452
		public readonly MethodAttributes Attributes;

		// Token: 0x040005AD RID: 1453
		public readonly MethodImplAttributes ImplAttributes;

		// Token: 0x040005AE RID: 1454
		public readonly string Name;

		// Token: 0x040005AF RID: 1455
		public readonly MethodDefinitionTreatment Treatment;
	}
}
