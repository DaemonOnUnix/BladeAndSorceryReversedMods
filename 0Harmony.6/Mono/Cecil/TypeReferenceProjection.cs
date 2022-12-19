using System;

namespace Mono.Cecil
{
	// Token: 0x02000188 RID: 392
	internal sealed class TypeReferenceProjection
	{
		// Token: 0x06000C8D RID: 3213 RVA: 0x00029AAF File Offset: 0x00027CAF
		public TypeReferenceProjection(TypeReference type, TypeReferenceTreatment treatment)
		{
			this.Name = type.Name;
			this.Namespace = type.Namespace;
			this.Scope = type.Scope;
			this.Treatment = treatment;
		}

		// Token: 0x04000570 RID: 1392
		public readonly string Name;

		// Token: 0x04000571 RID: 1393
		public readonly string Namespace;

		// Token: 0x04000572 RID: 1394
		public readonly IMetadataScope Scope;

		// Token: 0x04000573 RID: 1395
		public readonly TypeReferenceTreatment Treatment;
	}
}
