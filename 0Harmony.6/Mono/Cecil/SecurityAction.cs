using System;

namespace Mono.Cecil
{
	// Token: 0x02000169 RID: 361
	public enum SecurityAction : ushort
	{
		// Token: 0x04000486 RID: 1158
		Request = 1,
		// Token: 0x04000487 RID: 1159
		Demand,
		// Token: 0x04000488 RID: 1160
		Assert,
		// Token: 0x04000489 RID: 1161
		Deny,
		// Token: 0x0400048A RID: 1162
		PermitOnly,
		// Token: 0x0400048B RID: 1163
		LinkDemand,
		// Token: 0x0400048C RID: 1164
		InheritDemand,
		// Token: 0x0400048D RID: 1165
		RequestMinimum,
		// Token: 0x0400048E RID: 1166
		RequestOptional,
		// Token: 0x0400048F RID: 1167
		RequestRefuse,
		// Token: 0x04000490 RID: 1168
		PreJitGrant,
		// Token: 0x04000491 RID: 1169
		PreJitDeny,
		// Token: 0x04000492 RID: 1170
		NonCasDemand,
		// Token: 0x04000493 RID: 1171
		NonCasLinkDemand,
		// Token: 0x04000494 RID: 1172
		NonCasInheritance
	}
}
