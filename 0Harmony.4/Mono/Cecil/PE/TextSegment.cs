using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000290 RID: 656
	internal enum TextSegment
	{
		// Token: 0x04000623 RID: 1571
		ImportAddressTable,
		// Token: 0x04000624 RID: 1572
		CLIHeader,
		// Token: 0x04000625 RID: 1573
		Code,
		// Token: 0x04000626 RID: 1574
		Resources,
		// Token: 0x04000627 RID: 1575
		Data,
		// Token: 0x04000628 RID: 1576
		StrongNameSignature,
		// Token: 0x04000629 RID: 1577
		MetadataHeader,
		// Token: 0x0400062A RID: 1578
		TableHeap,
		// Token: 0x0400062B RID: 1579
		StringHeap,
		// Token: 0x0400062C RID: 1580
		UserStringHeap,
		// Token: 0x0400062D RID: 1581
		GuidHeap,
		// Token: 0x0400062E RID: 1582
		BlobHeap,
		// Token: 0x0400062F RID: 1583
		PdbHeap,
		// Token: 0x04000630 RID: 1584
		DebugDirectory,
		// Token: 0x04000631 RID: 1585
		ImportDirectory,
		// Token: 0x04000632 RID: 1586
		ImportHintNameTable,
		// Token: 0x04000633 RID: 1587
		StartupStub
	}
}
