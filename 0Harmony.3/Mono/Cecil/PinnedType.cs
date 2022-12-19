using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200015E RID: 350
	internal sealed class PinnedType : TypeSpecification
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsPinned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00026CC5 File Offset: 0x00024EC5
		public PinnedType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Pinned;
		}
	}
}
