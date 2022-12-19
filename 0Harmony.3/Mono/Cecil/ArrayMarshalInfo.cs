using System;

namespace Mono.Cecil
{
	// Token: 0x02000133 RID: 307
	internal sealed class ArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00022F83 File Offset: 0x00021183
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00022F8B File Offset: 0x0002118B
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00022F94 File Offset: 0x00021194
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00022F9C File Offset: 0x0002119C
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00022FA5 File Offset: 0x000211A5
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00022FAD File Offset: 0x000211AD
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00022FB6 File Offset: 0x000211B6
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00022FBE File Offset: 0x000211BE
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

		// Token: 0x060008B0 RID: 2224 RVA: 0x00022FC7 File Offset: 0x000211C7
		public ArrayMarshalInfo()
			: base(NativeType.Array)
		{
			this.element_type = NativeType.None;
			this.size_parameter_index = -1;
			this.size = -1;
			this.size_parameter_multiplier = -1;
		}

		// Token: 0x0400031E RID: 798
		internal NativeType element_type;

		// Token: 0x0400031F RID: 799
		internal int size_parameter_index;

		// Token: 0x04000320 RID: 800
		internal int size;

		// Token: 0x04000321 RID: 801
		internal int size_parameter_multiplier;
	}
}
