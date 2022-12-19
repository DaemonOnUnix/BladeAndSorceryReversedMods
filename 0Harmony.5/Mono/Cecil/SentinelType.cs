using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000262 RID: 610
	internal sealed class SentinelType : TypeSpecification
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsSentinel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002DE1F File Offset: 0x0002C01F
		public SentinelType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Sentinel;
		}
	}
}
