using System;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x0200022F RID: 559
	[Serializable]
	public sealed class ResolutionException : Exception
	{
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00029565 File Offset: 0x00027765
		public MemberReference Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00029570 File Offset: 0x00027770
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

		// Token: 0x06000C2C RID: 3116 RVA: 0x000295AE File Offset: 0x000277AE
		public ResolutionException(MemberReference member)
			: base("Failed to resolve " + member.FullName)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000295DB File Offset: 0x000277DB
		public ResolutionException(MemberReference member, Exception innerException)
			: base("Failed to resolve " + member.FullName, innerException)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00029609 File Offset: 0x00027809
		private ResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000361 RID: 865
		private readonly MemberReference member;
	}
}
