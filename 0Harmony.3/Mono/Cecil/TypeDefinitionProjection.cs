using System;

namespace Mono.Cecil
{
	// Token: 0x02000187 RID: 391
	internal sealed class TypeDefinitionProjection
	{
		// Token: 0x06000C8C RID: 3212 RVA: 0x00029A88 File Offset: 0x00027C88
		public TypeDefinitionProjection(TypeDefinition type, TypeDefinitionTreatment treatment)
		{
			this.Attributes = type.Attributes;
			this.Name = type.Name;
			this.Treatment = treatment;
		}

		// Token: 0x0400056D RID: 1389
		public readonly TypeAttributes Attributes;

		// Token: 0x0400056E RID: 1390
		public readonly string Name;

		// Token: 0x0400056F RID: 1391
		public readonly TypeDefinitionTreatment Treatment;
	}
}
