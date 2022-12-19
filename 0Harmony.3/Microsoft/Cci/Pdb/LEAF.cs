﻿using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200024B RID: 587
	internal enum LEAF
	{
		// Token: 0x04000B18 RID: 2840
		LF_VTSHAPE = 10,
		// Token: 0x04000B19 RID: 2841
		LF_COBOL1 = 12,
		// Token: 0x04000B1A RID: 2842
		LF_LABEL = 14,
		// Token: 0x04000B1B RID: 2843
		LF_NULL,
		// Token: 0x04000B1C RID: 2844
		LF_NOTTRAN,
		// Token: 0x04000B1D RID: 2845
		LF_ENDPRECOMP = 20,
		// Token: 0x04000B1E RID: 2846
		LF_TYPESERVER_ST = 22,
		// Token: 0x04000B1F RID: 2847
		LF_LIST = 515,
		// Token: 0x04000B20 RID: 2848
		LF_REFSYM = 524,
		// Token: 0x04000B21 RID: 2849
		LF_ENUMERATE_ST = 1027,
		// Token: 0x04000B22 RID: 2850
		LF_TI16_MAX = 4096,
		// Token: 0x04000B23 RID: 2851
		LF_MODIFIER,
		// Token: 0x04000B24 RID: 2852
		LF_POINTER,
		// Token: 0x04000B25 RID: 2853
		LF_ARRAY_ST,
		// Token: 0x04000B26 RID: 2854
		LF_CLASS_ST,
		// Token: 0x04000B27 RID: 2855
		LF_STRUCTURE_ST,
		// Token: 0x04000B28 RID: 2856
		LF_UNION_ST,
		// Token: 0x04000B29 RID: 2857
		LF_ENUM_ST,
		// Token: 0x04000B2A RID: 2858
		LF_PROCEDURE,
		// Token: 0x04000B2B RID: 2859
		LF_MFUNCTION,
		// Token: 0x04000B2C RID: 2860
		LF_COBOL0,
		// Token: 0x04000B2D RID: 2861
		LF_BARRAY,
		// Token: 0x04000B2E RID: 2862
		LF_DIMARRAY_ST,
		// Token: 0x04000B2F RID: 2863
		LF_VFTPATH,
		// Token: 0x04000B30 RID: 2864
		LF_PRECOMP_ST,
		// Token: 0x04000B31 RID: 2865
		LF_OEM,
		// Token: 0x04000B32 RID: 2866
		LF_ALIAS_ST,
		// Token: 0x04000B33 RID: 2867
		LF_OEM2,
		// Token: 0x04000B34 RID: 2868
		LF_SKIP = 4608,
		// Token: 0x04000B35 RID: 2869
		LF_ARGLIST,
		// Token: 0x04000B36 RID: 2870
		LF_DEFARG_ST,
		// Token: 0x04000B37 RID: 2871
		LF_FIELDLIST,
		// Token: 0x04000B38 RID: 2872
		LF_DERIVED,
		// Token: 0x04000B39 RID: 2873
		LF_BITFIELD,
		// Token: 0x04000B3A RID: 2874
		LF_METHODLIST,
		// Token: 0x04000B3B RID: 2875
		LF_DIMCONU,
		// Token: 0x04000B3C RID: 2876
		LF_DIMCONLU,
		// Token: 0x04000B3D RID: 2877
		LF_DIMVARU,
		// Token: 0x04000B3E RID: 2878
		LF_DIMVARLU,
		// Token: 0x04000B3F RID: 2879
		LF_BCLASS = 5120,
		// Token: 0x04000B40 RID: 2880
		LF_VBCLASS,
		// Token: 0x04000B41 RID: 2881
		LF_IVBCLASS,
		// Token: 0x04000B42 RID: 2882
		LF_FRIENDFCN_ST,
		// Token: 0x04000B43 RID: 2883
		LF_INDEX,
		// Token: 0x04000B44 RID: 2884
		LF_MEMBER_ST,
		// Token: 0x04000B45 RID: 2885
		LF_STMEMBER_ST,
		// Token: 0x04000B46 RID: 2886
		LF_METHOD_ST,
		// Token: 0x04000B47 RID: 2887
		LF_NESTTYPE_ST,
		// Token: 0x04000B48 RID: 2888
		LF_VFUNCTAB,
		// Token: 0x04000B49 RID: 2889
		LF_FRIENDCLS,
		// Token: 0x04000B4A RID: 2890
		LF_ONEMETHOD_ST,
		// Token: 0x04000B4B RID: 2891
		LF_VFUNCOFF,
		// Token: 0x04000B4C RID: 2892
		LF_NESTTYPEEX_ST,
		// Token: 0x04000B4D RID: 2893
		LF_MEMBERMODIFY_ST,
		// Token: 0x04000B4E RID: 2894
		LF_MANAGED_ST,
		// Token: 0x04000B4F RID: 2895
		LF_ST_MAX = 5376,
		// Token: 0x04000B50 RID: 2896
		LF_TYPESERVER,
		// Token: 0x04000B51 RID: 2897
		LF_ENUMERATE,
		// Token: 0x04000B52 RID: 2898
		LF_ARRAY,
		// Token: 0x04000B53 RID: 2899
		LF_CLASS,
		// Token: 0x04000B54 RID: 2900
		LF_STRUCTURE,
		// Token: 0x04000B55 RID: 2901
		LF_UNION,
		// Token: 0x04000B56 RID: 2902
		LF_ENUM,
		// Token: 0x04000B57 RID: 2903
		LF_DIMARRAY,
		// Token: 0x04000B58 RID: 2904
		LF_PRECOMP,
		// Token: 0x04000B59 RID: 2905
		LF_ALIAS,
		// Token: 0x04000B5A RID: 2906
		LF_DEFARG,
		// Token: 0x04000B5B RID: 2907
		LF_FRIENDFCN,
		// Token: 0x04000B5C RID: 2908
		LF_MEMBER,
		// Token: 0x04000B5D RID: 2909
		LF_STMEMBER,
		// Token: 0x04000B5E RID: 2910
		LF_METHOD,
		// Token: 0x04000B5F RID: 2911
		LF_NESTTYPE,
		// Token: 0x04000B60 RID: 2912
		LF_ONEMETHOD,
		// Token: 0x04000B61 RID: 2913
		LF_NESTTYPEEX,
		// Token: 0x04000B62 RID: 2914
		LF_MEMBERMODIFY,
		// Token: 0x04000B63 RID: 2915
		LF_MANAGED,
		// Token: 0x04000B64 RID: 2916
		LF_TYPESERVER2,
		// Token: 0x04000B65 RID: 2917
		LF_NUMERIC = 32768,
		// Token: 0x04000B66 RID: 2918
		LF_CHAR = 32768,
		// Token: 0x04000B67 RID: 2919
		LF_SHORT,
		// Token: 0x04000B68 RID: 2920
		LF_USHORT,
		// Token: 0x04000B69 RID: 2921
		LF_LONG,
		// Token: 0x04000B6A RID: 2922
		LF_ULONG,
		// Token: 0x04000B6B RID: 2923
		LF_REAL32,
		// Token: 0x04000B6C RID: 2924
		LF_REAL64,
		// Token: 0x04000B6D RID: 2925
		LF_REAL80,
		// Token: 0x04000B6E RID: 2926
		LF_REAL128,
		// Token: 0x04000B6F RID: 2927
		LF_QUADWORD,
		// Token: 0x04000B70 RID: 2928
		LF_UQUADWORD,
		// Token: 0x04000B71 RID: 2929
		LF_COMPLEX32 = 32780,
		// Token: 0x04000B72 RID: 2930
		LF_COMPLEX64,
		// Token: 0x04000B73 RID: 2931
		LF_COMPLEX80,
		// Token: 0x04000B74 RID: 2932
		LF_COMPLEX128,
		// Token: 0x04000B75 RID: 2933
		LF_VARSTRING,
		// Token: 0x04000B76 RID: 2934
		LF_OCTWORD = 32791,
		// Token: 0x04000B77 RID: 2935
		LF_UOCTWORD,
		// Token: 0x04000B78 RID: 2936
		LF_DECIMAL,
		// Token: 0x04000B79 RID: 2937
		LF_DATE,
		// Token: 0x04000B7A RID: 2938
		LF_UTF8STRING,
		// Token: 0x04000B7B RID: 2939
		LF_PAD0 = 240,
		// Token: 0x04000B7C RID: 2940
		LF_PAD1,
		// Token: 0x04000B7D RID: 2941
		LF_PAD2,
		// Token: 0x04000B7E RID: 2942
		LF_PAD3,
		// Token: 0x04000B7F RID: 2943
		LF_PAD4,
		// Token: 0x04000B80 RID: 2944
		LF_PAD5,
		// Token: 0x04000B81 RID: 2945
		LF_PAD6,
		// Token: 0x04000B82 RID: 2946
		LF_PAD7,
		// Token: 0x04000B83 RID: 2947
		LF_PAD8,
		// Token: 0x04000B84 RID: 2948
		LF_PAD9,
		// Token: 0x04000B85 RID: 2949
		LF_PAD10,
		// Token: 0x04000B86 RID: 2950
		LF_PAD11,
		// Token: 0x04000B87 RID: 2951
		LF_PAD12,
		// Token: 0x04000B88 RID: 2952
		LF_PAD13,
		// Token: 0x04000B89 RID: 2953
		LF_PAD14,
		// Token: 0x04000B8A RID: 2954
		LF_PAD15
	}
}