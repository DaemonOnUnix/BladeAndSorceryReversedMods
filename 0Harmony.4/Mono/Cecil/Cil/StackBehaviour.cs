using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C1 RID: 705
	public enum StackBehaviour
	{
		// Token: 0x04000825 RID: 2085
		Pop0,
		// Token: 0x04000826 RID: 2086
		Pop1,
		// Token: 0x04000827 RID: 2087
		Pop1_pop1,
		// Token: 0x04000828 RID: 2088
		Popi,
		// Token: 0x04000829 RID: 2089
		Popi_pop1,
		// Token: 0x0400082A RID: 2090
		Popi_popi,
		// Token: 0x0400082B RID: 2091
		Popi_popi8,
		// Token: 0x0400082C RID: 2092
		Popi_popi_popi,
		// Token: 0x0400082D RID: 2093
		Popi_popr4,
		// Token: 0x0400082E RID: 2094
		Popi_popr8,
		// Token: 0x0400082F RID: 2095
		Popref,
		// Token: 0x04000830 RID: 2096
		Popref_pop1,
		// Token: 0x04000831 RID: 2097
		Popref_popi,
		// Token: 0x04000832 RID: 2098
		Popref_popi_popi,
		// Token: 0x04000833 RID: 2099
		Popref_popi_popi8,
		// Token: 0x04000834 RID: 2100
		Popref_popi_popr4,
		// Token: 0x04000835 RID: 2101
		Popref_popi_popr8,
		// Token: 0x04000836 RID: 2102
		Popref_popi_popref,
		// Token: 0x04000837 RID: 2103
		PopAll,
		// Token: 0x04000838 RID: 2104
		Push0,
		// Token: 0x04000839 RID: 2105
		Push1,
		// Token: 0x0400083A RID: 2106
		Push1_push1,
		// Token: 0x0400083B RID: 2107
		Pushi,
		// Token: 0x0400083C RID: 2108
		Pushi8,
		// Token: 0x0400083D RID: 2109
		Pushr4,
		// Token: 0x0400083E RID: 2110
		Pushr8,
		// Token: 0x0400083F RID: 2111
		Pushref,
		// Token: 0x04000840 RID: 2112
		Varpop,
		// Token: 0x04000841 RID: 2113
		Varpush
	}
}
