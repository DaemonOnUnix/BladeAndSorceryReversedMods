using System;

namespace Mono.Cecil
{
	// Token: 0x02000229 RID: 553
	internal sealed class FixedArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00029336 File Offset: 0x00027536
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x0002933E File Offset: 0x0002753E
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

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00029347 File Offset: 0x00027547
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0002934F File Offset: 0x0002754F
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

		// Token: 0x06000C06 RID: 3078 RVA: 0x00029358 File Offset: 0x00027558
		public FixedArrayMarshalInfo()
			: base(NativeType.FixedArray)
		{
			this.element_type = NativeType.None;
		}

		// Token: 0x04000359 RID: 857
		internal NativeType element_type;

		// Token: 0x0400035A RID: 858
		internal int size;
	}
}
