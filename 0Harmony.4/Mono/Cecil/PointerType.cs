using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000255 RID: 597
	internal sealed class PointerType : TypeSpecification
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0002D635 File Offset: 0x0002B835
		public override string Name
		{
			get
			{
				return base.Name + "*";
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0002D647 File Offset: 0x0002B847
		public override string FullName
		{
			get
			{
				return base.FullName + "*";
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0002D659 File Offset: 0x0002B859
		public PointerType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Ptr;
		}
	}
}
