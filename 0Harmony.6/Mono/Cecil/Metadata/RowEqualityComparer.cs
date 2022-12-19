using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B3 RID: 435
	internal sealed class RowEqualityComparer : IEqualityComparer<Row<string, string>>, IEqualityComparer<Row<uint, uint>>, IEqualityComparer<Row<uint, uint, uint>>
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002F45A File Offset: 0x0002D65A
		public bool Equals(Row<string, string> x, Row<string, string> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002F484 File Offset: 0x0002D684
		public int GetHashCode(Row<string, string> obj)
		{
			string col = obj.Col1;
			string col2 = obj.Col2;
			return ((col != null) ? col.GetHashCode() : 0) ^ ((col2 != null) ? col2.GetHashCode() : 0);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002F4B8 File Offset: 0x0002D6B8
		public bool Equals(Row<uint, uint> x, Row<uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002F4D8 File Offset: 0x0002D6D8
		public int GetHashCode(Row<uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002F4E7 File Offset: 0x0002D6E7
		public bool Equals(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return x.Col1 == y.Col1 && x.Col2 == y.Col2 && x.Col3 == y.Col3;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002F515 File Offset: 0x0002D715
		public int GetHashCode(Row<uint, uint, uint> obj)
		{
			return (int)(obj.Col1 ^ obj.Col2 ^ obj.Col3);
		}
	}
}
