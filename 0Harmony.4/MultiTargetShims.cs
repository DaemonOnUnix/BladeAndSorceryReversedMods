using System;
using Mono.Cecil;

// Token: 0x02000427 RID: 1063
internal static class MultiTargetShims
{
	// Token: 0x06001666 RID: 5734 RVA: 0x000483A0 File Offset: 0x000465A0
	public static string Replace(this string self, string oldValue, string newValue, StringComparison comparison)
	{
		return self.Replace(oldValue, newValue);
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x000483AA File Offset: 0x000465AA
	public static bool Contains(this string self, string value, StringComparison comparison)
	{
		return self.Contains(value);
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x000483B3 File Offset: 0x000465B3
	public static int GetHashCode(this string self, StringComparison comparison)
	{
		return self.GetHashCode();
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x000483BB File Offset: 0x000465BB
	public static int IndexOf(this string self, char value, StringComparison comparison)
	{
		return self.IndexOf(value);
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x000483C4 File Offset: 0x000465C4
	public static int IndexOf(this string self, string value, StringComparison comparison)
	{
		return self.IndexOf(value);
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x000483CD File Offset: 0x000465CD
	public static TypeReference GetConstraintType(this GenericParameterConstraint constraint)
	{
		return constraint.ConstraintType;
	}

	// Token: 0x04000FA0 RID: 4000
	private static readonly object[] _NoArgs = new object[0];
}
