using System;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x0200013C RID: 316
	[Serializable]
	public sealed class ResolutionException : Exception
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00023299 File Offset: 0x00021499
		public MemberReference Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000232A4 File Offset: 0x000214A4
		public IMetadataScope Scope
		{
			get
			{
				TypeReference typeReference = this.member as TypeReference;
				if (typeReference != null)
				{
					return typeReference.Scope;
				}
				TypeReference declaringType = this.member.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Scope;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x000232E2 File Offset: 0x000214E2
		public ResolutionException(MemberReference member)
			: base("Failed to resolve " + member.FullName)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002330F File Offset: 0x0002150F
		public ResolutionException(MemberReference member, Exception innerException)
			: base("Failed to resolve " + member.FullName, innerException)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002333D File Offset: 0x0002153D
		private ResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0400032F RID: 815
		private readonly MemberReference member;
	}
}
