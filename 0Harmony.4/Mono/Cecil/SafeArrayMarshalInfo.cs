using System;

namespace Mono.Cecil
{
	// Token: 0x02000228 RID: 552
	internal sealed class SafeArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00029314 File Offset: 0x00027514
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x0002931C File Offset: 0x0002751C
		public VariantType ElementType
		{
			get
			{
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00029325 File Offset: 0x00027525
		public SafeArrayMarshalInfo()
			: base(NativeType.SafeArray)
		{
			this.element_type = VariantType.None;
		}

		// Token: 0x04000358 RID: 856
		internal VariantType element_type;
	}
}
