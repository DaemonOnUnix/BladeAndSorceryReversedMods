using System;

namespace Mono.Cecil
{
	// Token: 0x02000135 RID: 309
	internal sealed class SafeArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0002303C File Offset: 0x0002123C
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00023044 File Offset: 0x00021244
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

		// Token: 0x060008BC RID: 2236 RVA: 0x0002304D File Offset: 0x0002124D
		public SafeArrayMarshalInfo()
			: base(NativeType.SafeArray)
		{
			this.element_type = VariantType.None;
		}

		// Token: 0x04000326 RID: 806
		internal VariantType element_type;
	}
}
