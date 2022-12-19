using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020001FE RID: 510
[CompilerGenerated]
internal sealed class <5c122140-1889-445a-a95b-56bc787f60ce><PrivateImplementationDetails>
{
	// Token: 0x06000FA5 RID: 4005 RVA: 0x0003534C File Offset: 0x0003354C
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s[i] ^ num) * 16777619U;
			}
		}
		return num;
	}

	// Token: 0x04000968 RID: 2408 RVA: 0x0004DC4C File Offset: 0x0004BE4C
	internal static readonly long 1B960802B155541DF3837ADE50790DA7E91762D14B8E011FA8223424FF75ACDB;

	// Token: 0x04000969 RID: 2409 RVA: 0x0004DC54 File Offset: 0x0004BE54
	internal static readonly <5c122140-1889-445a-a95b-56bc787f60ce><PrivateImplementationDetails>.__StaticArrayInitTypeSize=1790 2EF0065A03764C27AE8D5DC3002E10F0426E43BDFA7D8ECFFF633E45DD32376B;

	// Token: 0x0400096A RID: 2410 RVA: 0x0004E352 File Offset: 0x0004C552
	internal static readonly <5c122140-1889-445a-a95b-56bc787f60ce><PrivateImplementationDetails>.__StaticArrayInitTypeSize=160 933598639CBAA1DE502F80D2FD1DB78F13C8D7BB64A5FDC1BC73AC0B5CE4F5CA;

	// Token: 0x0400096B RID: 2411 RVA: 0x0004E3F2 File Offset: 0x0004C5F2
	internal static readonly long 971150DD73DC318E68A98CCE9B91AC7DEA2D43C562B4F5A9A2F4272C7E29477E;

	// Token: 0x0400096C RID: 2412 RVA: 0x0004E3FA File Offset: 0x0004C5FA
	internal static readonly <5c122140-1889-445a-a95b-56bc787f60ce><PrivateImplementationDetails>.__StaticArrayInitTypeSize=128 BFDF5E72651B4EC588BD5FC6A9F17E9E0972248146BBACC10478F48D72F29B81;

	// Token: 0x020001FF RID: 511
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 128)]
	private struct __StaticArrayInitTypeSize=128
	{
	}

	// Token: 0x02000200 RID: 512
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 160)]
	private struct __StaticArrayInitTypeSize=160
	{
	}

	// Token: 0x02000201 RID: 513
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1790)]
	private struct __StaticArrayInitTypeSize=1790
	{
	}
}
