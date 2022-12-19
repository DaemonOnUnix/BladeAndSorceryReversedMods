using System;

namespace Mono.Cecil
{
	// Token: 0x0200027D RID: 637
	internal sealed class TypeReferenceProjection
	{
		// Token: 0x06000FEF RID: 4079 RVA: 0x00030D73 File Offset: 0x0002EF73
		public TypeReferenceProjection(TypeReference type, TypeReferenceTreatment treatment)
		{
			this.Name = type.Name;
			this.Namespace = type.Namespace;
			this.Scope = type.Scope;
			this.Treatment = treatment;
		}

		// Token: 0x040005A8 RID: 1448
		public readonly string Name;

		// Token: 0x040005A9 RID: 1449
		public readonly string Namespace;

		// Token: 0x040005AA RID: 1450
		public readonly IMetadataScope Scope;

		// Token: 0x040005AB RID: 1451
		public readonly TypeReferenceTreatment Treatment;
	}
}
