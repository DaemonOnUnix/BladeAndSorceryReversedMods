﻿using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200024A RID: 586
	internal enum TYPE_ENUM
	{
		// Token: 0x04000AA1 RID: 2721
		T_NOTYPE,
		// Token: 0x04000AA2 RID: 2722
		T_ABS,
		// Token: 0x04000AA3 RID: 2723
		T_SEGMENT,
		// Token: 0x04000AA4 RID: 2724
		T_VOID,
		// Token: 0x04000AA5 RID: 2725
		T_HRESULT = 8,
		// Token: 0x04000AA6 RID: 2726
		T_32PHRESULT = 1032,
		// Token: 0x04000AA7 RID: 2727
		T_64PHRESULT = 1544,
		// Token: 0x04000AA8 RID: 2728
		T_PVOID = 259,
		// Token: 0x04000AA9 RID: 2729
		T_PFVOID = 515,
		// Token: 0x04000AAA RID: 2730
		T_PHVOID = 771,
		// Token: 0x04000AAB RID: 2731
		T_32PVOID = 1027,
		// Token: 0x04000AAC RID: 2732
		T_64PVOID = 1539,
		// Token: 0x04000AAD RID: 2733
		T_CURRENCY = 4,
		// Token: 0x04000AAE RID: 2734
		T_NOTTRANS = 7,
		// Token: 0x04000AAF RID: 2735
		T_BIT = 96,
		// Token: 0x04000AB0 RID: 2736
		T_PASCHAR,
		// Token: 0x04000AB1 RID: 2737
		T_CHAR = 16,
		// Token: 0x04000AB2 RID: 2738
		T_32PCHAR = 1040,
		// Token: 0x04000AB3 RID: 2739
		T_64PCHAR = 1552,
		// Token: 0x04000AB4 RID: 2740
		T_UCHAR = 32,
		// Token: 0x04000AB5 RID: 2741
		T_32PUCHAR = 1056,
		// Token: 0x04000AB6 RID: 2742
		T_64PUCHAR = 1568,
		// Token: 0x04000AB7 RID: 2743
		T_RCHAR = 112,
		// Token: 0x04000AB8 RID: 2744
		T_32PRCHAR = 1136,
		// Token: 0x04000AB9 RID: 2745
		T_64PRCHAR = 1648,
		// Token: 0x04000ABA RID: 2746
		T_WCHAR = 113,
		// Token: 0x04000ABB RID: 2747
		T_32PWCHAR = 1137,
		// Token: 0x04000ABC RID: 2748
		T_64PWCHAR = 1649,
		// Token: 0x04000ABD RID: 2749
		T_INT1 = 104,
		// Token: 0x04000ABE RID: 2750
		T_32PINT1 = 1128,
		// Token: 0x04000ABF RID: 2751
		T_64PINT1 = 1640,
		// Token: 0x04000AC0 RID: 2752
		T_UINT1 = 105,
		// Token: 0x04000AC1 RID: 2753
		T_32PUINT1 = 1129,
		// Token: 0x04000AC2 RID: 2754
		T_64PUINT1 = 1641,
		// Token: 0x04000AC3 RID: 2755
		T_SHORT = 17,
		// Token: 0x04000AC4 RID: 2756
		T_32PSHORT = 1041,
		// Token: 0x04000AC5 RID: 2757
		T_64PSHORT = 1553,
		// Token: 0x04000AC6 RID: 2758
		T_USHORT = 33,
		// Token: 0x04000AC7 RID: 2759
		T_32PUSHORT = 1057,
		// Token: 0x04000AC8 RID: 2760
		T_64PUSHORT = 1569,
		// Token: 0x04000AC9 RID: 2761
		T_INT2 = 114,
		// Token: 0x04000ACA RID: 2762
		T_32PINT2 = 1138,
		// Token: 0x04000ACB RID: 2763
		T_64PINT2 = 1650,
		// Token: 0x04000ACC RID: 2764
		T_UINT2 = 115,
		// Token: 0x04000ACD RID: 2765
		T_32PUINT2 = 1139,
		// Token: 0x04000ACE RID: 2766
		T_64PUINT2 = 1651,
		// Token: 0x04000ACF RID: 2767
		T_LONG = 18,
		// Token: 0x04000AD0 RID: 2768
		T_ULONG = 34,
		// Token: 0x04000AD1 RID: 2769
		T_32PLONG = 1042,
		// Token: 0x04000AD2 RID: 2770
		T_32PULONG = 1058,
		// Token: 0x04000AD3 RID: 2771
		T_64PLONG = 1554,
		// Token: 0x04000AD4 RID: 2772
		T_64PULONG = 1570,
		// Token: 0x04000AD5 RID: 2773
		T_INT4 = 116,
		// Token: 0x04000AD6 RID: 2774
		T_32PINT4 = 1140,
		// Token: 0x04000AD7 RID: 2775
		T_64PINT4 = 1652,
		// Token: 0x04000AD8 RID: 2776
		T_UINT4 = 117,
		// Token: 0x04000AD9 RID: 2777
		T_32PUINT4 = 1141,
		// Token: 0x04000ADA RID: 2778
		T_64PUINT4 = 1653,
		// Token: 0x04000ADB RID: 2779
		T_QUAD = 19,
		// Token: 0x04000ADC RID: 2780
		T_32PQUAD = 1043,
		// Token: 0x04000ADD RID: 2781
		T_64PQUAD = 1555,
		// Token: 0x04000ADE RID: 2782
		T_UQUAD = 35,
		// Token: 0x04000ADF RID: 2783
		T_32PUQUAD = 1059,
		// Token: 0x04000AE0 RID: 2784
		T_64PUQUAD = 1571,
		// Token: 0x04000AE1 RID: 2785
		T_INT8 = 118,
		// Token: 0x04000AE2 RID: 2786
		T_32PINT8 = 1142,
		// Token: 0x04000AE3 RID: 2787
		T_64PINT8 = 1654,
		// Token: 0x04000AE4 RID: 2788
		T_UINT8 = 119,
		// Token: 0x04000AE5 RID: 2789
		T_32PUINT8 = 1143,
		// Token: 0x04000AE6 RID: 2790
		T_64PUINT8 = 1655,
		// Token: 0x04000AE7 RID: 2791
		T_OCT = 20,
		// Token: 0x04000AE8 RID: 2792
		T_32POCT = 1044,
		// Token: 0x04000AE9 RID: 2793
		T_64POCT = 1556,
		// Token: 0x04000AEA RID: 2794
		T_UOCT = 36,
		// Token: 0x04000AEB RID: 2795
		T_32PUOCT = 1060,
		// Token: 0x04000AEC RID: 2796
		T_64PUOCT = 1572,
		// Token: 0x04000AED RID: 2797
		T_INT16 = 120,
		// Token: 0x04000AEE RID: 2798
		T_32PINT16 = 1144,
		// Token: 0x04000AEF RID: 2799
		T_64PINT16 = 1656,
		// Token: 0x04000AF0 RID: 2800
		T_UINT16 = 121,
		// Token: 0x04000AF1 RID: 2801
		T_32PUINT16 = 1145,
		// Token: 0x04000AF2 RID: 2802
		T_64PUINT16 = 1657,
		// Token: 0x04000AF3 RID: 2803
		T_REAL32 = 64,
		// Token: 0x04000AF4 RID: 2804
		T_32PREAL32 = 1088,
		// Token: 0x04000AF5 RID: 2805
		T_64PREAL32 = 1600,
		// Token: 0x04000AF6 RID: 2806
		T_REAL64 = 65,
		// Token: 0x04000AF7 RID: 2807
		T_32PREAL64 = 1089,
		// Token: 0x04000AF8 RID: 2808
		T_64PREAL64 = 1601,
		// Token: 0x04000AF9 RID: 2809
		T_REAL80 = 66,
		// Token: 0x04000AFA RID: 2810
		T_32PREAL80 = 1090,
		// Token: 0x04000AFB RID: 2811
		T_64PREAL80 = 1602,
		// Token: 0x04000AFC RID: 2812
		T_REAL128 = 67,
		// Token: 0x04000AFD RID: 2813
		T_32PREAL128 = 1091,
		// Token: 0x04000AFE RID: 2814
		T_64PREAL128 = 1603,
		// Token: 0x04000AFF RID: 2815
		T_CPLX32 = 80,
		// Token: 0x04000B00 RID: 2816
		T_32PCPLX32 = 1104,
		// Token: 0x04000B01 RID: 2817
		T_64PCPLX32 = 1616,
		// Token: 0x04000B02 RID: 2818
		T_CPLX64 = 81,
		// Token: 0x04000B03 RID: 2819
		T_32PCPLX64 = 1105,
		// Token: 0x04000B04 RID: 2820
		T_64PCPLX64 = 1617,
		// Token: 0x04000B05 RID: 2821
		T_CPLX80 = 82,
		// Token: 0x04000B06 RID: 2822
		T_32PCPLX80 = 1106,
		// Token: 0x04000B07 RID: 2823
		T_64PCPLX80 = 1618,
		// Token: 0x04000B08 RID: 2824
		T_CPLX128 = 83,
		// Token: 0x04000B09 RID: 2825
		T_32PCPLX128 = 1107,
		// Token: 0x04000B0A RID: 2826
		T_64PCPLX128 = 1619,
		// Token: 0x04000B0B RID: 2827
		T_BOOL08 = 48,
		// Token: 0x04000B0C RID: 2828
		T_32PBOOL08 = 1072,
		// Token: 0x04000B0D RID: 2829
		T_64PBOOL08 = 1584,
		// Token: 0x04000B0E RID: 2830
		T_BOOL16 = 49,
		// Token: 0x04000B0F RID: 2831
		T_32PBOOL16 = 1073,
		// Token: 0x04000B10 RID: 2832
		T_64PBOOL16 = 1585,
		// Token: 0x04000B11 RID: 2833
		T_BOOL32 = 50,
		// Token: 0x04000B12 RID: 2834
		T_32PBOOL32 = 1074,
		// Token: 0x04000B13 RID: 2835
		T_64PBOOL32 = 1586,
		// Token: 0x04000B14 RID: 2836
		T_BOOL64 = 51,
		// Token: 0x04000B15 RID: 2837
		T_32PBOOL64 = 1075,
		// Token: 0x04000B16 RID: 2838
		T_64PBOOL64 = 1587
	}
}