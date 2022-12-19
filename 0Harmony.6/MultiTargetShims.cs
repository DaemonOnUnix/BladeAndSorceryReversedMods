using System;
using Mono.Cecil;

// Token: 0x0200032F RID: 815
internal static class MultiTargetShims
{
	// Token: 0x060012F5 RID: 4853 RVA: 0x0004045C File Offset: 0x0003E65C
	public static TypeReference GetConstraintType(this GenericParameterConstraint constraint)
	{
		return constraint.ConstraintType;
	}

	// Token: 0x04000F62 RID: 3938
	private static readonly object[] _NoArgs = new object[0];
}
