using System;

namespace Mono.Cecil
{
	// Token: 0x02000226 RID: 550
	internal sealed class ArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0002925B File Offset: 0x0002745B
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x00029263 File Offset: 0x00027463
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

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0002926C File Offset: 0x0002746C
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x00029274 File Offset: 0x00027474
		public int SizeParameterIndex
		{
			get
			{
				return this.size_parameter_index;
			}
			set
			{
				this.size_parameter_index = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0002927D File Offset: 0x0002747D
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x00029285 File Offset: 0x00027485
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

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0002928E File Offset: 0x0002748E
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x00029296 File Offset: 0x00027496
		public int SizeParameterMultiplier
		{
			get
			{
				return this.size_parameter_multiplier;
			}
			set
			{
				this.size_parameter_multiplier = value;
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002929F File Offset: 0x0002749F
		public ArrayMarshalInfo()
			: base(NativeType.Array)
		{
			this.element_type = NativeType.None;
			this.size_parameter_index = -1;
			this.size = -1;
			this.size_parameter_multiplier = -1;
		}

		// Token: 0x04000350 RID: 848
		internal NativeType element_type;

		// Token: 0x04000351 RID: 849
		internal int size_parameter_index;

		// Token: 0x04000352 RID: 850
		internal int size;

		// Token: 0x04000353 RID: 851
		internal int size_parameter_multiplier;
	}
}
