using System;

namespace Mono.Cecil
{
	// Token: 0x02000136 RID: 310
	internal sealed class FixedArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0002305E File Offset: 0x0002125E
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x00023066 File Offset: 0x00021266
		public NativeType ElementType
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0002306F File Offset: 0x0002126F
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x00023077 File Offset: 0x00021277
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00023080 File Offset: 0x00021280
		public FixedArrayMarshalInfo()
			: base(NativeType.FixedArray)
		{
			this.element_type = NativeType.None;
		}

		// Token: 0x04000327 RID: 807
		internal NativeType element_type;

		// Token: 0x04000328 RID: 808
		internal int size;
	}
}
