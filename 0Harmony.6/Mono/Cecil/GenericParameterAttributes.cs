using System;

namespace Mono.Cecil
{
	// Token: 0x0200011B RID: 283
	[Flags]
	public enum GenericParameterAttributes : ushort
	{
		// Token: 0x04000301 RID: 769
		VarianceMask = 3,
		// Token: 0x04000302 RID: 770
		NonVariant = 0,
		// Token: 0x04000303 RID: 771
		Covariant = 1,
		// Token: 0x04000304 RID: 772
		Contravariant = 2,
		// Token: 0x04000305 RID: 773
		SpecialConstraintMask = 28,
		// Token: 0x04000306 RID: 774
		ReferenceTypeConstraint = 4,
		// Token: 0x04000307 RID: 775
		NotNullableValueTypeConstraint = 8,
		// Token: 0x04000308 RID: 776
		DefaultConstructorConstraint = 16
	}
}
