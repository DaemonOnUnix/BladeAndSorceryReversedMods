using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A9 RID: 425
	internal enum ElementType : byte
	{
		// Token: 0x0400061A RID: 1562
		None,
		// Token: 0x0400061B RID: 1563
		Void,
		// Token: 0x0400061C RID: 1564
		Boolean,
		// Token: 0x0400061D RID: 1565
		Char,
		// Token: 0x0400061E RID: 1566
		I1,
		// Token: 0x0400061F RID: 1567
		U1,
		// Token: 0x04000620 RID: 1568
		I2,
		// Token: 0x04000621 RID: 1569
		U2,
		// Token: 0x04000622 RID: 1570
		I4,
		// Token: 0x04000623 RID: 1571
		U4,
		// Token: 0x04000624 RID: 1572
		I8,
		// Token: 0x04000625 RID: 1573
		U8,
		// Token: 0x04000626 RID: 1574
		R4,
		// Token: 0x04000627 RID: 1575
		R8,
		// Token: 0x04000628 RID: 1576
		String,
		// Token: 0x04000629 RID: 1577
		Ptr,
		// Token: 0x0400062A RID: 1578
		ByRef,
		// Token: 0x0400062B RID: 1579
		ValueType,
		// Token: 0x0400062C RID: 1580
		Class,
		// Token: 0x0400062D RID: 1581
		Var,
		// Token: 0x0400062E RID: 1582
		Array,
		// Token: 0x0400062F RID: 1583
		GenericInst,
		// Token: 0x04000630 RID: 1584
		TypedByRef,
		// Token: 0x04000631 RID: 1585
		I = 24,
		// Token: 0x04000632 RID: 1586
		U,
		// Token: 0x04000633 RID: 1587
		FnPtr = 27,
		// Token: 0x04000634 RID: 1588
		Object,
		// Token: 0x04000635 RID: 1589
		SzArray,
		// Token: 0x04000636 RID: 1590
		MVar,
		// Token: 0x04000637 RID: 1591
		CModReqD,
		// Token: 0x04000638 RID: 1592
		CModOpt,
		// Token: 0x04000639 RID: 1593
		Internal,
		// Token: 0x0400063A RID: 1594
		Modifier = 64,
		// Token: 0x0400063B RID: 1595
		Sentinel,
		// Token: 0x0400063C RID: 1596
		Pinned = 69,
		// Token: 0x0400063D RID: 1597
		Type = 80,
		// Token: 0x0400063E RID: 1598
		Boxed,
		// Token: 0x0400063F RID: 1599
		Enum = 85
	}
}
