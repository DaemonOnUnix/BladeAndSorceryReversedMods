using System;

namespace Mono.Cecil
{
	// Token: 0x0200020D RID: 525
	[Flags]
	public enum GenericParameterAttributes : ushort
	{
		// Token: 0x04000333 RID: 819
		VarianceMask = 3,
		// Token: 0x04000334 RID: 820
		NonVariant = 0,
		// Token: 0x04000335 RID: 821
		Covariant = 1,
		// Token: 0x04000336 RID: 822
		Contravariant = 2,
		// Token: 0x04000337 RID: 823
		SpecialConstraintMask = 28,
		// Token: 0x04000338 RID: 824
		ReferenceTypeConstraint = 4,
		// Token: 0x04000339 RID: 825
		NotNullableValueTypeConstraint = 8,
		// Token: 0x0400033A RID: 826
		DefaultConstructorConstraint = 16
	}
}
