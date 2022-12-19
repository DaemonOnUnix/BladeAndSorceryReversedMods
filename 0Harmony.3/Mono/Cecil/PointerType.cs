using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000161 RID: 353
	internal sealed class PointerType : TypeSpecification
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00026FCD File Offset: 0x000251CD
		public override string Name
		{
			get
			{
				return base.Name + "*";
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00026FDF File Offset: 0x000251DF
		public override string FullName
		{
			get
			{
				return base.FullName + "*";
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00026FF1 File Offset: 0x000251F1
		public PointerType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Ptr;
		}
	}
}
