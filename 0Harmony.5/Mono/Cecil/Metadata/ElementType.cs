using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200029E RID: 670
	internal enum ElementType : byte
	{
		// Token: 0x04000652 RID: 1618
		None,
		// Token: 0x04000653 RID: 1619
		Void,
		// Token: 0x04000654 RID: 1620
		Boolean,
		// Token: 0x04000655 RID: 1621
		Char,
		// Token: 0x04000656 RID: 1622
		I1,
		// Token: 0x04000657 RID: 1623
		U1,
		// Token: 0x04000658 RID: 1624
		I2,
		// Token: 0x04000659 RID: 1625
		U2,
		// Token: 0x0400065A RID: 1626
		I4,
		// Token: 0x0400065B RID: 1627
		U4,
		// Token: 0x0400065C RID: 1628
		I8,
		// Token: 0x0400065D RID: 1629
		U8,
		// Token: 0x0400065E RID: 1630
		R4,
		// Token: 0x0400065F RID: 1631
		R8,
		// Token: 0x04000660 RID: 1632
		String,
		// Token: 0x04000661 RID: 1633
		Ptr,
		// Token: 0x04000662 RID: 1634
		ByRef,
		// Token: 0x04000663 RID: 1635
		ValueType,
		// Token: 0x04000664 RID: 1636
		Class,
		// Token: 0x04000665 RID: 1637
		Var,
		// Token: 0x04000666 RID: 1638
		Array,
		// Token: 0x04000667 RID: 1639
		GenericInst,
		// Token: 0x04000668 RID: 1640
		TypedByRef,
		// Token: 0x04000669 RID: 1641
		I = 24,
		// Token: 0x0400066A RID: 1642
		U,
		// Token: 0x0400066B RID: 1643
		FnPtr = 27,
		// Token: 0x0400066C RID: 1644
		Object,
		// Token: 0x0400066D RID: 1645
		SzArray,
		// Token: 0x0400066E RID: 1646
		MVar,
		// Token: 0x0400066F RID: 1647
		CModReqD,
		// Token: 0x04000670 RID: 1648
		CModOpt,
		// Token: 0x04000671 RID: 1649
		Internal,
		// Token: 0x04000672 RID: 1650
		Modifier = 64,
		// Token: 0x04000673 RID: 1651
		Sentinel,
		// Token: 0x04000674 RID: 1652
		Pinned = 69,
		// Token: 0x04000675 RID: 1653
		Type = 80,
		// Token: 0x04000676 RID: 1654
		Boxed,
		// Token: 0x04000677 RID: 1655
		Enum = 85
	}
}
