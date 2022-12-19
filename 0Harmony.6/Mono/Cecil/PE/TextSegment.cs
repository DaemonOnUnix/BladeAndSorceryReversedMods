using System;

namespace Mono.Cecil.PE
{
	// Token: 0x0200019B RID: 411
	internal enum TextSegment
	{
		// Token: 0x040005EB RID: 1515
		ImportAddressTable,
		// Token: 0x040005EC RID: 1516
		CLIHeader,
		// Token: 0x040005ED RID: 1517
		Code,
		// Token: 0x040005EE RID: 1518
		Resources,
		// Token: 0x040005EF RID: 1519
		Data,
		// Token: 0x040005F0 RID: 1520
		StrongNameSignature,
		// Token: 0x040005F1 RID: 1521
		MetadataHeader,
		// Token: 0x040005F2 RID: 1522
		TableHeap,
		// Token: 0x040005F3 RID: 1523
		StringHeap,
		// Token: 0x040005F4 RID: 1524
		UserStringHeap,
		// Token: 0x040005F5 RID: 1525
		GuidHeap,
		// Token: 0x040005F6 RID: 1526
		BlobHeap,
		// Token: 0x040005F7 RID: 1527
		PdbHeap,
		// Token: 0x040005F8 RID: 1528
		DebugDirectory,
		// Token: 0x040005F9 RID: 1529
		ImportDirectory,
		// Token: 0x040005FA RID: 1530
		ImportHintNameTable,
		// Token: 0x040005FB RID: 1531
		StartupStub
	}
}
