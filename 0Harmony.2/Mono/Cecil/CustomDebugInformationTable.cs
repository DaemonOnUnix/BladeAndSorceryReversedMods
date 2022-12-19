using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001EB RID: 491
	internal sealed class CustomDebugInformationTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x000219F0 File Offset: 0x0001FBF0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomDebugInformation);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00020C8E File Offset: 0x0001EE8E
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
