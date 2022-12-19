using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AC RID: 428
	internal sealed class PdbHeap : Heap
	{
		// Token: 0x06000DAB RID: 3499 RVA: 0x0002EA27 File Offset: 0x0002CC27
		public PdbHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002F356 File Offset: 0x0002D556
		public bool HasTable(Table table)
		{
			return (this.TypeSystemTables & (1L << (int)table)) != 0L;
		}

		// Token: 0x04000642 RID: 1602
		public byte[] Id;

		// Token: 0x04000643 RID: 1603
		public uint EntryPoint;

		// Token: 0x04000644 RID: 1604
		public long TypeSystemTables;

		// Token: 0x04000645 RID: 1605
		public uint[] TypeSystemTableRows;
	}
}
