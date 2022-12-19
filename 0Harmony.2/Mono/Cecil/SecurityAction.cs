using System;

namespace Mono.Cecil
{
	// Token: 0x0200025D RID: 605
	public enum SecurityAction : ushort
	{
		// Token: 0x040004BB RID: 1211
		Request = 1,
		// Token: 0x040004BC RID: 1212
		Demand,
		// Token: 0x040004BD RID: 1213
		Assert,
		// Token: 0x040004BE RID: 1214
		Deny,
		// Token: 0x040004BF RID: 1215
		PermitOnly,
		// Token: 0x040004C0 RID: 1216
		LinkDemand,
		// Token: 0x040004C1 RID: 1217
		InheritDemand,
		// Token: 0x040004C2 RID: 1218
		RequestMinimum,
		// Token: 0x040004C3 RID: 1219
		RequestOptional,
		// Token: 0x040004C4 RID: 1220
		RequestRefuse,
		// Token: 0x040004C5 RID: 1221
		PreJitGrant,
		// Token: 0x040004C6 RID: 1222
		PreJitDeny,
		// Token: 0x040004C7 RID: 1223
		NonCasDemand,
		// Token: 0x040004C8 RID: 1224
		NonCasLinkDemand,
		// Token: 0x040004C9 RID: 1225
		NonCasInheritance
	}
}
