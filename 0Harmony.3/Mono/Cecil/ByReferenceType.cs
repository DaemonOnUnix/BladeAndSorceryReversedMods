using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000166 RID: 358
	internal sealed class ByReferenceType : TypeSpecification
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x000274AA File Offset: 0x000256AA
		public override string Name
		{
			get
			{
				return base.Name + "&";
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x000274BC File Offset: 0x000256BC
		public override string FullName
		{
			get
			{
				return base.FullName + "&";
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000B63 RID: 2915 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsByReference
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000274CE File Offset: 0x000256CE
		public ByReferenceType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.ByRef;
		}
	}
}
