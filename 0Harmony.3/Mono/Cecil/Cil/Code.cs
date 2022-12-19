using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001B9 RID: 441
	public enum Code
	{
		// Token: 0x040006A1 RID: 1697
		Nop,
		// Token: 0x040006A2 RID: 1698
		Break,
		// Token: 0x040006A3 RID: 1699
		Ldarg_0,
		// Token: 0x040006A4 RID: 1700
		Ldarg_1,
		// Token: 0x040006A5 RID: 1701
		Ldarg_2,
		// Token: 0x040006A6 RID: 1702
		Ldarg_3,
		// Token: 0x040006A7 RID: 1703
		Ldloc_0,
		// Token: 0x040006A8 RID: 1704
		Ldloc_1,
		// Token: 0x040006A9 RID: 1705
		Ldloc_2,
		// Token: 0x040006AA RID: 1706
		Ldloc_3,
		// Token: 0x040006AB RID: 1707
		Stloc_0,
		// Token: 0x040006AC RID: 1708
		Stloc_1,
		// Token: 0x040006AD RID: 1709
		Stloc_2,
		// Token: 0x040006AE RID: 1710
		Stloc_3,
		// Token: 0x040006AF RID: 1711
		Ldarg_S,
		// Token: 0x040006B0 RID: 1712
		Ldarga_S,
		// Token: 0x040006B1 RID: 1713
		Starg_S,
		// Token: 0x040006B2 RID: 1714
		Ldloc_S,
		// Token: 0x040006B3 RID: 1715
		Ldloca_S,
		// Token: 0x040006B4 RID: 1716
		Stloc_S,
		// Token: 0x040006B5 RID: 1717
		Ldnull,
		// Token: 0x040006B6 RID: 1718
		Ldc_I4_M1,
		// Token: 0x040006B7 RID: 1719
		Ldc_I4_0,
		// Token: 0x040006B8 RID: 1720
		Ldc_I4_1,
		// Token: 0x040006B9 RID: 1721
		Ldc_I4_2,
		// Token: 0x040006BA RID: 1722
		Ldc_I4_3,
		// Token: 0x040006BB RID: 1723
		Ldc_I4_4,
		// Token: 0x040006BC RID: 1724
		Ldc_I4_5,
		// Token: 0x040006BD RID: 1725
		Ldc_I4_6,
		// Token: 0x040006BE RID: 1726
		Ldc_I4_7,
		// Token: 0x040006BF RID: 1727
		Ldc_I4_8,
		// Token: 0x040006C0 RID: 1728
		Ldc_I4_S,
		// Token: 0x040006C1 RID: 1729
		Ldc_I4,
		// Token: 0x040006C2 RID: 1730
		Ldc_I8,
		// Token: 0x040006C3 RID: 1731
		Ldc_R4,
		// Token: 0x040006C4 RID: 1732
		Ldc_R8,
		// Token: 0x040006C5 RID: 1733
		Dup,
		// Token: 0x040006C6 RID: 1734
		Pop,
		// Token: 0x040006C7 RID: 1735
		Jmp,
		// Token: 0x040006C8 RID: 1736
		Call,
		// Token: 0x040006C9 RID: 1737
		Calli,
		// Token: 0x040006CA RID: 1738
		Ret,
		// Token: 0x040006CB RID: 1739
		Br_S,
		// Token: 0x040006CC RID: 1740
		Brfalse_S,
		// Token: 0x040006CD RID: 1741
		Brtrue_S,
		// Token: 0x040006CE RID: 1742
		Beq_S,
		// Token: 0x040006CF RID: 1743
		Bge_S,
		// Token: 0x040006D0 RID: 1744
		Bgt_S,
		// Token: 0x040006D1 RID: 1745
		Ble_S,
		// Token: 0x040006D2 RID: 1746
		Blt_S,
		// Token: 0x040006D3 RID: 1747
		Bne_Un_S,
		// Token: 0x040006D4 RID: 1748
		Bge_Un_S,
		// Token: 0x040006D5 RID: 1749
		Bgt_Un_S,
		// Token: 0x040006D6 RID: 1750
		Ble_Un_S,
		// Token: 0x040006D7 RID: 1751
		Blt_Un_S,
		// Token: 0x040006D8 RID: 1752
		Br,
		// Token: 0x040006D9 RID: 1753
		Brfalse,
		// Token: 0x040006DA RID: 1754
		Brtrue,
		// Token: 0x040006DB RID: 1755
		Beq,
		// Token: 0x040006DC RID: 1756
		Bge,
		// Token: 0x040006DD RID: 1757
		Bgt,
		// Token: 0x040006DE RID: 1758
		Ble,
		// Token: 0x040006DF RID: 1759
		Blt,
		// Token: 0x040006E0 RID: 1760
		Bne_Un,
		// Token: 0x040006E1 RID: 1761
		Bge_Un,
		// Token: 0x040006E2 RID: 1762
		Bgt_Un,
		// Token: 0x040006E3 RID: 1763
		Ble_Un,
		// Token: 0x040006E4 RID: 1764
		Blt_Un,
		// Token: 0x040006E5 RID: 1765
		Switch,
		// Token: 0x040006E6 RID: 1766
		Ldind_I1,
		// Token: 0x040006E7 RID: 1767
		Ldind_U1,
		// Token: 0x040006E8 RID: 1768
		Ldind_I2,
		// Token: 0x040006E9 RID: 1769
		Ldind_U2,
		// Token: 0x040006EA RID: 1770
		Ldind_I4,
		// Token: 0x040006EB RID: 1771
		Ldind_U4,
		// Token: 0x040006EC RID: 1772
		Ldind_I8,
		// Token: 0x040006ED RID: 1773
		Ldind_I,
		// Token: 0x040006EE RID: 1774
		Ldind_R4,
		// Token: 0x040006EF RID: 1775
		Ldind_R8,
		// Token: 0x040006F0 RID: 1776
		Ldind_Ref,
		// Token: 0x040006F1 RID: 1777
		Stind_Ref,
		// Token: 0x040006F2 RID: 1778
		Stind_I1,
		// Token: 0x040006F3 RID: 1779
		Stind_I2,
		// Token: 0x040006F4 RID: 1780
		Stind_I4,
		// Token: 0x040006F5 RID: 1781
		Stind_I8,
		// Token: 0x040006F6 RID: 1782
		Stind_R4,
		// Token: 0x040006F7 RID: 1783
		Stind_R8,
		// Token: 0x040006F8 RID: 1784
		Add,
		// Token: 0x040006F9 RID: 1785
		Sub,
		// Token: 0x040006FA RID: 1786
		Mul,
		// Token: 0x040006FB RID: 1787
		Div,
		// Token: 0x040006FC RID: 1788
		Div_Un,
		// Token: 0x040006FD RID: 1789
		Rem,
		// Token: 0x040006FE RID: 1790
		Rem_Un,
		// Token: 0x040006FF RID: 1791
		And,
		// Token: 0x04000700 RID: 1792
		Or,
		// Token: 0x04000701 RID: 1793
		Xor,
		// Token: 0x04000702 RID: 1794
		Shl,
		// Token: 0x04000703 RID: 1795
		Shr,
		// Token: 0x04000704 RID: 1796
		Shr_Un,
		// Token: 0x04000705 RID: 1797
		Neg,
		// Token: 0x04000706 RID: 1798
		Not,
		// Token: 0x04000707 RID: 1799
		Conv_I1,
		// Token: 0x04000708 RID: 1800
		Conv_I2,
		// Token: 0x04000709 RID: 1801
		Conv_I4,
		// Token: 0x0400070A RID: 1802
		Conv_I8,
		// Token: 0x0400070B RID: 1803
		Conv_R4,
		// Token: 0x0400070C RID: 1804
		Conv_R8,
		// Token: 0x0400070D RID: 1805
		Conv_U4,
		// Token: 0x0400070E RID: 1806
		Conv_U8,
		// Token: 0x0400070F RID: 1807
		Callvirt,
		// Token: 0x04000710 RID: 1808
		Cpobj,
		// Token: 0x04000711 RID: 1809
		Ldobj,
		// Token: 0x04000712 RID: 1810
		Ldstr,
		// Token: 0x04000713 RID: 1811
		Newobj,
		// Token: 0x04000714 RID: 1812
		Castclass,
		// Token: 0x04000715 RID: 1813
		Isinst,
		// Token: 0x04000716 RID: 1814
		Conv_R_Un,
		// Token: 0x04000717 RID: 1815
		Unbox,
		// Token: 0x04000718 RID: 1816
		Throw,
		// Token: 0x04000719 RID: 1817
		Ldfld,
		// Token: 0x0400071A RID: 1818
		Ldflda,
		// Token: 0x0400071B RID: 1819
		Stfld,
		// Token: 0x0400071C RID: 1820
		Ldsfld,
		// Token: 0x0400071D RID: 1821
		Ldsflda,
		// Token: 0x0400071E RID: 1822
		Stsfld,
		// Token: 0x0400071F RID: 1823
		Stobj,
		// Token: 0x04000720 RID: 1824
		Conv_Ovf_I1_Un,
		// Token: 0x04000721 RID: 1825
		Conv_Ovf_I2_Un,
		// Token: 0x04000722 RID: 1826
		Conv_Ovf_I4_Un,
		// Token: 0x04000723 RID: 1827
		Conv_Ovf_I8_Un,
		// Token: 0x04000724 RID: 1828
		Conv_Ovf_U1_Un,
		// Token: 0x04000725 RID: 1829
		Conv_Ovf_U2_Un,
		// Token: 0x04000726 RID: 1830
		Conv_Ovf_U4_Un,
		// Token: 0x04000727 RID: 1831
		Conv_Ovf_U8_Un,
		// Token: 0x04000728 RID: 1832
		Conv_Ovf_I_Un,
		// Token: 0x04000729 RID: 1833
		Conv_Ovf_U_Un,
		// Token: 0x0400072A RID: 1834
		Box,
		// Token: 0x0400072B RID: 1835
		Newarr,
		// Token: 0x0400072C RID: 1836
		Ldlen,
		// Token: 0x0400072D RID: 1837
		Ldelema,
		// Token: 0x0400072E RID: 1838
		Ldelem_I1,
		// Token: 0x0400072F RID: 1839
		Ldelem_U1,
		// Token: 0x04000730 RID: 1840
		Ldelem_I2,
		// Token: 0x04000731 RID: 1841
		Ldelem_U2,
		// Token: 0x04000732 RID: 1842
		Ldelem_I4,
		// Token: 0x04000733 RID: 1843
		Ldelem_U4,
		// Token: 0x04000734 RID: 1844
		Ldelem_I8,
		// Token: 0x04000735 RID: 1845
		Ldelem_I,
		// Token: 0x04000736 RID: 1846
		Ldelem_R4,
		// Token: 0x04000737 RID: 1847
		Ldelem_R8,
		// Token: 0x04000738 RID: 1848
		Ldelem_Ref,
		// Token: 0x04000739 RID: 1849
		Stelem_I,
		// Token: 0x0400073A RID: 1850
		Stelem_I1,
		// Token: 0x0400073B RID: 1851
		Stelem_I2,
		// Token: 0x0400073C RID: 1852
		Stelem_I4,
		// Token: 0x0400073D RID: 1853
		Stelem_I8,
		// Token: 0x0400073E RID: 1854
		Stelem_R4,
		// Token: 0x0400073F RID: 1855
		Stelem_R8,
		// Token: 0x04000740 RID: 1856
		Stelem_Ref,
		// Token: 0x04000741 RID: 1857
		Ldelem_Any,
		// Token: 0x04000742 RID: 1858
		Stelem_Any,
		// Token: 0x04000743 RID: 1859
		Unbox_Any,
		// Token: 0x04000744 RID: 1860
		Conv_Ovf_I1,
		// Token: 0x04000745 RID: 1861
		Conv_Ovf_U1,
		// Token: 0x04000746 RID: 1862
		Conv_Ovf_I2,
		// Token: 0x04000747 RID: 1863
		Conv_Ovf_U2,
		// Token: 0x04000748 RID: 1864
		Conv_Ovf_I4,
		// Token: 0x04000749 RID: 1865
		Conv_Ovf_U4,
		// Token: 0x0400074A RID: 1866
		Conv_Ovf_I8,
		// Token: 0x0400074B RID: 1867
		Conv_Ovf_U8,
		// Token: 0x0400074C RID: 1868
		Refanyval,
		// Token: 0x0400074D RID: 1869
		Ckfinite,
		// Token: 0x0400074E RID: 1870
		Mkrefany,
		// Token: 0x0400074F RID: 1871
		Ldtoken,
		// Token: 0x04000750 RID: 1872
		Conv_U2,
		// Token: 0x04000751 RID: 1873
		Conv_U1,
		// Token: 0x04000752 RID: 1874
		Conv_I,
		// Token: 0x04000753 RID: 1875
		Conv_Ovf_I,
		// Token: 0x04000754 RID: 1876
		Conv_Ovf_U,
		// Token: 0x04000755 RID: 1877
		Add_Ovf,
		// Token: 0x04000756 RID: 1878
		Add_Ovf_Un,
		// Token: 0x04000757 RID: 1879
		Mul_Ovf,
		// Token: 0x04000758 RID: 1880
		Mul_Ovf_Un,
		// Token: 0x04000759 RID: 1881
		Sub_Ovf,
		// Token: 0x0400075A RID: 1882
		Sub_Ovf_Un,
		// Token: 0x0400075B RID: 1883
		Endfinally,
		// Token: 0x0400075C RID: 1884
		Leave,
		// Token: 0x0400075D RID: 1885
		Leave_S,
		// Token: 0x0400075E RID: 1886
		Stind_I,
		// Token: 0x0400075F RID: 1887
		Conv_U,
		// Token: 0x04000760 RID: 1888
		Arglist,
		// Token: 0x04000761 RID: 1889
		Ceq,
		// Token: 0x04000762 RID: 1890
		Cgt,
		// Token: 0x04000763 RID: 1891
		Cgt_Un,
		// Token: 0x04000764 RID: 1892
		Clt,
		// Token: 0x04000765 RID: 1893
		Clt_Un,
		// Token: 0x04000766 RID: 1894
		Ldftn,
		// Token: 0x04000767 RID: 1895
		Ldvirtftn,
		// Token: 0x04000768 RID: 1896
		Ldarg,
		// Token: 0x04000769 RID: 1897
		Ldarga,
		// Token: 0x0400076A RID: 1898
		Starg,
		// Token: 0x0400076B RID: 1899
		Ldloc,
		// Token: 0x0400076C RID: 1900
		Ldloca,
		// Token: 0x0400076D RID: 1901
		Stloc,
		// Token: 0x0400076E RID: 1902
		Localloc,
		// Token: 0x0400076F RID: 1903
		Endfilter,
		// Token: 0x04000770 RID: 1904
		Unaligned,
		// Token: 0x04000771 RID: 1905
		Volatile,
		// Token: 0x04000772 RID: 1906
		Tail,
		// Token: 0x04000773 RID: 1907
		Initobj,
		// Token: 0x04000774 RID: 1908
		Constrained,
		// Token: 0x04000775 RID: 1909
		Cpblk,
		// Token: 0x04000776 RID: 1910
		Initblk,
		// Token: 0x04000777 RID: 1911
		No,
		// Token: 0x04000778 RID: 1912
		Rethrow,
		// Token: 0x04000779 RID: 1913
		Sizeof,
		// Token: 0x0400077A RID: 1914
		Refanytype,
		// Token: 0x0400077B RID: 1915
		Readonly
	}
}
