using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001AE RID: 430
	internal sealed class ArrayType : TypeSpecification
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0001A49C File Offset: 0x0001869C
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001A4E1 File Offset: 0x000186E1
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

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001A4F8 File Offset: 0x000186F8
		public bool IsVector
		{
			get
			{
				return this.dimensions == null || (this.dimensions.Count <= 1 && !this.dimensions[0].IsSized);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0001A536 File Offset: 0x00018736
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001A549 File Offset: 0x00018749
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001A55C File Offset: 0x0001875C
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

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001A5E2 File Offset: 0x000187E2
		public ArrayType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001A5FC File Offset: 0x000187FC
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

		// Token: 0x04000260 RID: 608
		private Collection<ArrayDimension> dimensions;
	}
}
