using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CB RID: 459
	public enum StackBehaviour
	{
		// Token: 0x040007E9 RID: 2025
		Pop0,
		// Token: 0x040007EA RID: 2026
		Pop1,
		// Token: 0x040007EB RID: 2027
		Pop1_pop1,
		// Token: 0x040007EC RID: 2028
		Popi,
		// Token: 0x040007ED RID: 2029
		Popi_pop1,
		// Token: 0x040007EE RID: 2030
		Popi_popi,
		// Token: 0x040007EF RID: 2031
		Popi_popi8,
		// Token: 0x040007F0 RID: 2032
		Popi_popi_popi,
		// Token: 0x040007F1 RID: 2033
		Popi_popr4,
		// Token: 0x040007F2 RID: 2034
		Popi_popr8,
		// Token: 0x040007F3 RID: 2035
		Popref,
		// Token: 0x040007F4 RID: 2036
		Popref_pop1,
		// Token: 0x040007F5 RID: 2037
		Popref_popi,
		// Token: 0x040007F6 RID: 2038
		Popref_popi_popi,
		// Token: 0x040007F7 RID: 2039
		Popref_popi_popi8,
		// Token: 0x040007F8 RID: 2040
		Popref_popi_popr4,
		// Token: 0x040007F9 RID: 2041
		Popref_popi_popr8,
		// Token: 0x040007FA RID: 2042
		Popref_popi_popref,
		// Token: 0x040007FB RID: 2043
		PopAll,
		// Token: 0x040007FC RID: 2044
		Push0,
		// Token: 0x040007FD RID: 2045
		Push1,
		// Token: 0x040007FE RID: 2046
		Push1_push1,
		// Token: 0x040007FF RID: 2047
		Pushi,
		// Token: 0x04000800 RID: 2048
		Pushi8,
		// Token: 0x04000801 RID: 2049
		Pushr4,
		// Token: 0x04000802 RID: 2050
		Pushr8,
		// Token: 0x04000803 RID: 2051
		Pushref,
		// Token: 0x04000804 RID: 2052
		Varpop,
		// Token: 0x04000805 RID: 2053
		Varpush
	}
}
