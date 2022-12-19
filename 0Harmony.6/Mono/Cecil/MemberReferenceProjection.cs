using System;

namespace Mono.Cecil
{
	// Token: 0x02000186 RID: 390
	internal sealed class MemberReferenceProjection
	{
		// Token: 0x06000C8B RID: 3211 RVA: 0x00029A6D File Offset: 0x00027C6D
		public MemberReferenceProjection(MemberReference member, MemberReferenceTreatment treatment)
		{
			this.Name = member.Name;
			this.Treatment = treatment;
		}

		// Token: 0x0400056B RID: 1387
		public readonly string Name;

		// Token: 0x0400056C RID: 1388
		public readonly MemberReferenceTreatment Treatment;
	}
}
