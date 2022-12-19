using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020002F4 RID: 756
[CompilerGenerated]
internal sealed class <8cf76e39-9470-4183-92b8-5ceb117e9190><PrivateImplementationDetails>
{
	// Token: 0x06001315 RID: 4885 RVA: 0x0003D298 File Offset: 0x0003B498
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

	// Token: 0x040009A7 RID: 2471 RVA: 0x000582A4 File Offset: 0x000564A4
	internal static readonly long 1B960802B155541DF3837ADE50790DA7E91762D14B8E011FA8223424FF75ACDB;

	// Token: 0x040009A8 RID: 2472 RVA: 0x000582AC File Offset: 0x000564AC
	internal static readonly <8cf76e39-9470-4183-92b8-5ceb117e9190><PrivateImplementationDetails>.__StaticArrayInitTypeSize=1790 2EF0065A03764C27AE8D5DC3002E10F0426E43BDFA7D8ECFFF633E45DD32376B;

	// Token: 0x040009A9 RID: 2473 RVA: 0x000589AA File Offset: 0x00056BAA
	internal static readonly <8cf76e39-9470-4183-92b8-5ceb117e9190><PrivateImplementationDetails>.__StaticArrayInitTypeSize=160 933598639CBAA1DE502F80D2FD1DB78F13C8D7BB64A5FDC1BC73AC0B5CE4F5CA;

	// Token: 0x040009AA RID: 2474 RVA: 0x00058A4A File Offset: 0x00056C4A
	internal static readonly long 971150DD73DC318E68A98CCE9B91AC7DEA2D43C562B4F5A9A2F4272C7E29477E;

	// Token: 0x040009AB RID: 2475 RVA: 0x00058A52 File Offset: 0x00056C52
	internal static readonly <8cf76e39-9470-4183-92b8-5ceb117e9190><PrivateImplementationDetails>.__StaticArrayInitTypeSize=128 BFDF5E72651B4EC588BD5FC6A9F17E9E0972248146BBACC10478F48D72F29B81;

	// Token: 0x020002F5 RID: 757
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 128)]
	private struct __StaticArrayInitTypeSize=128
	{
	}

	// Token: 0x020002F6 RID: 758
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 160)]
	private struct __StaticArrayInitTypeSize=160
	{
	}

	// Token: 0x020002F7 RID: 759
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1790)]
	private struct __StaticArrayInitTypeSize=1790
	{
	}
}
