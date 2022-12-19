using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200025A RID: 602
	internal sealed class ByReferenceType : TypeSpecification
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0002DB12 File Offset: 0x0002BD12
		public override string Name
		{
			get
			{
				return base.Name + "&";
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0002DB24 File Offset: 0x0002BD24
		public override string FullName
		{
			get
			{
				return base.FullName + "&";
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsByReference
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0002DB36 File Offset: 0x0002BD36
		public ByReferenceType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.ByRef;
		}
	}
}
