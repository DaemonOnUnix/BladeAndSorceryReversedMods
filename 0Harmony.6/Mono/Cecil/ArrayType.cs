using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BC RID: 188
	internal sealed class ArrayType : TypeSpecification
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00014610 File Offset: 0x00012810
		public Collection<ArrayDimension> Dimensions
		{
			get
			{
				if (this.dimensions != null)
				{
					return this.dimensions;
				}
				Interlocked.CompareExchange<Collection<ArrayDimension>>(ref this.dimensions, new Collection<ArrayDimension> { default(ArrayDimension) }, null);
				return this.dimensions;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00014655 File Offset: 0x00012855
		public int Rank
		{
			get
			{
				if (this.dimensions != null)
				{
					return this.dimensions.Count;
				}
				return 1;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0001466C File Offset: 0x0001286C
		public bool IsVector
		{
			get
			{
				return this.dimensions == null || (this.dimensions.Count <= 1 && !this.dimensions[0].IsSized);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000146AA File Offset: 0x000128AA
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000146BD File Offset: 0x000128BD
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000146D0 File Offset: 0x000128D0
		private string Suffix
		{
			get
			{
				if (this.IsVector)
				{
					return "[]";
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("[");
				for (int i = 0; i < this.dimensions.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(this.dimensions[i].ToString());
				}
				stringBuilder.Append("]");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00014756 File Offset: 0x00012956
		public ArrayType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00014770 File Offset: 0x00012970
		public ArrayType(TypeReference type, int rank)
			: this(type)
		{
			Mixin.CheckType(type);
			if (rank == 1)
			{
				return;
			}
			this.dimensions = new Collection<ArrayDimension>(rank);
			for (int i = 0; i < rank; i++)
			{
				this.dimensions.Add(default(ArrayDimension));
			}
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x04000232 RID: 562
		private Collection<ArrayDimension> dimensions;
	}
}
