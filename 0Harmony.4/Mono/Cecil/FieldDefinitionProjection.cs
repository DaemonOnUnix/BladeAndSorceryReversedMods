using System;

namespace Mono.Cecil
{
	// Token: 0x0200027F RID: 639
	internal sealed class FieldDefinitionProjection
	{
		// Token: 0x06000FF1 RID: 4081 RVA: 0x00030DD9 File Offset: 0x0002EFD9
		public FieldDefinitionProjection(FieldDefinition field, FieldDefinitionTreatment treatment)
		{
			this.Attributes = field.Attributes;
			this.Treatment = treatment;
		}

		// Token: 0x040005B0 RID: 1456
		public readonly FieldAttributes Attributes;

		// Token: 0x040005B1 RID: 1457
		public readonly FieldDefinitionTreatment Treatment;
	}
}
