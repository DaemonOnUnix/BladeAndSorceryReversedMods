using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A1 RID: 673
	internal sealed class PdbHeap : Heap
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x0003644F File Offset: 0x0003464F
		public PdbHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00036D7E File Offset: 0x00034F7E
		public bool HasTable(Table table)
		{
			return (this.TypeSystemTables & (1L << (int)table)) != 0L;
		}

		// Token: 0x0400067A RID: 1658
		public byte[] Id;

		// Token: 0x0400067B RID: 1659
		public uint EntryPoint;

		// Token: 0x0400067C RID: 1660
		public long TypeSystemTables;

		// Token: 0x0400067D RID: 1661
		public uint[] TypeSystemTableRows;
	}
}
