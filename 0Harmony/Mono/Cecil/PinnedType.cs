using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000252 RID: 594
	internal sealed class PinnedType : TypeSpecification
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsPinned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0002D32D File Offset: 0x0002B52D
		public PinnedType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Pinned;
		}
	}
}
