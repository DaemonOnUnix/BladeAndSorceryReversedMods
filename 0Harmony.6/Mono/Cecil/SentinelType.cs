using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200016E RID: 366
	internal sealed class SentinelType : TypeSpecification
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x000125CE File Offset: 0x000107CE
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsSentinel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000277B7 File Offset: 0x000259B7
		public SentinelType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Sentinel;
		}
	}
}
