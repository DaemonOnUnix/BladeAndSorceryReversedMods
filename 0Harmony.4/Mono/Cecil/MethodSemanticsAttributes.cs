using System;

namespace Mono.Cecil
{
	// Token: 0x0200023B RID: 571
	[Flags]
	public enum MethodSemanticsAttributes : ushort
	{
		// Token: 0x040003D1 RID: 977
		None = 0,
		// Token: 0x040003D2 RID: 978
		Setter = 1,
		// Token: 0x040003D3 RID: 979
		Getter = 2,
		// Token: 0x040003D4 RID: 980
		Other = 4,
		// Token: 0x040003D5 RID: 981
		AddOn = 8,
		// Token: 0x040003D6 RID: 982
		RemoveOn = 16,
		// Token: 0x040003D7 RID: 983
		Fire = 32
	}
}
