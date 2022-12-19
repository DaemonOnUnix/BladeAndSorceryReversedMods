using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002AA RID: 682
	internal enum Table : byte
	{
		// Token: 0x0400069D RID: 1693
		Module,
		// Token: 0x0400069E RID: 1694
		TypeRef,
		// Token: 0x0400069F RID: 1695
		TypeDef,
		// Token: 0x040006A0 RID: 1696
		FieldPtr,
		// Token: 0x040006A1 RID: 1697
		Field,
		// Token: 0x040006A2 RID: 1698
		MethodPtr,
		// Token: 0x040006A3 RID: 1699
		Method,
		// Token: 0x040006A4 RID: 1700
		ParamPtr,
		// Token: 0x040006A5 RID: 1701
		Param,
		// Token: 0x040006A6 RID: 1702
		InterfaceImpl,
		// Token: 0x040006A7 RID: 1703
		MemberRef,
		// Token: 0x040006A8 RID: 1704
		Constant,
		// Token: 0x040006A9 RID: 1705
		CustomAttribute,
		// Token: 0x040006AA RID: 1706
		FieldMarshal,
		// Token: 0x040006AB RID: 1707
		DeclSecurity,
		// Token: 0x040006AC RID: 1708
		ClassLayout,
		// Token: 0x040006AD RID: 1709
		FieldLayout,
		// Token: 0x040006AE RID: 1710
		StandAloneSig,
		// Token: 0x040006AF RID: 1711
		EventMap,
		// Token: 0x040006B0 RID: 1712
		EventPtr,
		// Token: 0x040006B1 RID: 1713
		Event,
		// Token: 0x040006B2 RID: 1714
		PropertyMap,
		// Token: 0x040006B3 RID: 1715
		PropertyPtr,
		// Token: 0x040006B4 RID: 1716
		Property,
		// Token: 0x040006B5 RID: 1717
		MethodSemantics,
		// Token: 0x040006B6 RID: 1718
		MethodImpl,
		// Token: 0x040006B7 RID: 1719
		ModuleRef,
		// Token: 0x040006B8 RID: 1720
		TypeSpec,
		// Token: 0x040006B9 RID: 1721
		ImplMap,
		// Token: 0x040006BA RID: 1722
		FieldRVA,
		// Token: 0x040006BB RID: 1723
		EncLog,
		// Token: 0x040006BC RID: 1724
		EncMap,
		// Token: 0x040006BD RID: 1725
		Assembly,
		// Token: 0x040006BE RID: 1726
		AssemblyProcessor,
		// Token: 0x040006BF RID: 1727
		AssemblyOS,
		// Token: 0x040006C0 RID: 1728
		AssemblyRef,
		// Token: 0x040006C1 RID: 1729
		AssemblyRefProcessor,
		// Token: 0x040006C2 RID: 1730
		AssemblyRefOS,
		// Token: 0x040006C3 RID: 1731
		File,
		// Token: 0x040006C4 RID: 1732
		ExportedType,
		// Token: 0x040006C5 RID: 1733
		ManifestResource,
		// Token: 0x040006C6 RID: 1734
		NestedClass,
		// Token: 0x040006C7 RID: 1735
		GenericParam,
		// Token: 0x040006C8 RID: 1736
		MethodSpec,
		// Token: 0x040006C9 RID: 1737
		GenericParamConstraint,
		// Token: 0x040006CA RID: 1738
		Document = 48,
		// Token: 0x040006CB RID: 1739
		MethodDebugInformation,
		// Token: 0x040006CC RID: 1740
		LocalScope,
		// Token: 0x040006CD RID: 1741
		LocalVariable,
		// Token: 0x040006CE RID: 1742
		LocalConstant,
		// Token: 0x040006CF RID: 1743
		ImportScope,
		// Token: 0x040006D0 RID: 1744
		StateMachineMethod,
		// Token: 0x040006D1 RID: 1745
		CustomDebugInformation
	}
}
