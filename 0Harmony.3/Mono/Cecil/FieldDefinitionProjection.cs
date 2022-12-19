using System;

namespace Mono.Cecil
{
	// Token: 0x0200018A RID: 394
	internal sealed class FieldDefinitionProjection
	{
		// Token: 0x06000C8F RID: 3215 RVA: 0x00029B15 File Offset: 0x00027D15
		public FieldDefinitionProjection(FieldDefinition field, FieldDefinitionTreatment treatment)
		{
			this.Attributes = field.Attributes;
			this.Treatment = treatment;
		}

		// Token: 0x04000578 RID: 1400
		public readonly FieldAttributes Attributes;

		// Token: 0x04000579 RID: 1401
		public readonly FieldDefinitionTreatment Treatment;
	}
}
