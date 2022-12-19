using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A8 RID: 680
	internal sealed class RowEqualityComparer : IEqualityComparer<Row<string, string>>, IEqualityComparer<Row<uint, uint>>, IEqualityComparer<Row<uint, uint, uint>>
	{
		// Token: 0x06001116 RID: 4374 RVA: 0x00036E82 File Offset: 0x00035082
		public bool Equals(Row<string, string> x, Row<string, string> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00036EAC File Offset: 0x000350AC
		public int GetHashCode(Row<string, string> obj)
		{
			string col = obj.Col1;
			string col2 = obj.Col2;
			return ((col != null) ? col.GetHashCode() : 0) ^ ((col2 != null) ? col2.GetHashCode() : 0);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00036EE0 File Offset: 0x000350E0
		public bool Equals(Row<uint, uint> x, Row<uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00036F00 File Offset: 0x00035100
		public int GetHashCode(Row<uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00036F0F File Offset: 0x0003510F
		public bool Equals(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2 && x.Col3 == y.Col3;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00036F3D File Offset: 0x0003513D
		public int GetHashCode(Row<uint, uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2 ^ obj.Col3);
		}
	}
}
