﻿using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000341 RID: 833
	internal enum LEAF
	{
		// Token: 0x04000B57 RID: 2903
		LF_VTSHAPE = 10,
		// Token: 0x04000B58 RID: 2904
		LF_COBOL1 = 12,
		// Token: 0x04000B59 RID: 2905
		LF_LABEL = 14,
		// Token: 0x04000B5A RID: 2906
		LF_NULL,
		// Token: 0x04000B5B RID: 2907
		LF_NOTTRAN,
		// Token: 0x04000B5C RID: 2908
		LF_ENDPRECOMP = 20,
		// Token: 0x04000B5D RID: 2909
		LF_TYPESERVER_ST = 22,
		// Token: 0x04000B5E RID: 2910
		LF_LIST = 515,
		// Token: 0x04000B5F RID: 2911
		LF_REFSYM = 524,
		// Token: 0x04000B60 RID: 2912
		LF_ENUMERATE_ST = 1027,
		// Token: 0x04000B61 RID: 2913
		LF_TI16_MAX = 4096,
		// Token: 0x04000B62 RID: 2914
		LF_MODIFIER,
		// Token: 0x04000B63 RID: 2915
		LF_POINTER,
		// Token: 0x04000B64 RID: 2916
		LF_ARRAY_ST,
		// Token: 0x04000B65 RID: 2917
		LF_CLASS_ST,
		// Token: 0x04000B66 RID: 2918
		LF_STRUCTURE_ST,
		// Token: 0x04000B67 RID: 2919
		LF_UNION_ST,
		// Token: 0x04000B68 RID: 2920
		LF_ENUM_ST,
		// Token: 0x04000B69 RID: 2921
		LF_PROCEDURE,
		// Token: 0x04000B6A RID: 2922
		LF_MFUNCTION,
		// Token: 0x04000B6B RID: 2923
		LF_COBOL0,
		// Token: 0x04000B6C RID: 2924
		LF_BARRAY,
		// Token: 0x04000B6D RID: 2925
		LF_DIMARRAY_ST,
		// Token: 0x04000B6E RID: 2926
		LF_VFTPATH,
		// Token: 0x04000B6F RID: 2927
		LF_PRECOMP_ST,
		// Token: 0x04000B70 RID: 2928
		LF_OEM,
		// Token: 0x04000B71 RID: 2929
		LF_ALIAS_ST,
		// Token: 0x04000B72 RID: 2930
		LF_OEM2,
		// Token: 0x04000B73 RID: 2931
		LF_SKIP = 4608,
		// Token: 0x04000B74 RID: 2932
		LF_ARGLIST,
		// Token: 0x04000B75 RID: 2933
		LF_DEFARG_ST,
		// Token: 0x04000B76 RID: 2934
		LF_FIELDLIST,
		// Token: 0x04000B77 RID: 2935
		LF_DERIVED,
		// Token: 0x04000B78 RID: 2936
		LF_BITFIELD,
		// Token: 0x04000B79 RID: 2937
		LF_METHODLIST,
		// Token: 0x04000B7A RID: 2938
		LF_DIMCONU,
		// Token: 0x04000B7B RID: 2939
		LF_DIMCONLU,
		// Token: 0x04000B7C RID: 2940
		LF_DIMVARU,
		// Token: 0x04000B7D RID: 2941
		LF_DIMVARLU,
		// Token: 0x04000B7E RID: 2942
		LF_BCLASS = 5120,
		// Token: 0x04000B7F RID: 2943
		LF_VBCLASS,
		// Token: 0x04000B80 RID: 2944
		LF_IVBCLASS,
		// Token: 0x04000B81 RID: 2945
		LF_FRIENDFCN_ST,
		// Token: 0x04000B82 RID: 2946
		LF_INDEX,
		// Token: 0x04000B83 RID: 2947
		LF_MEMBER_ST,
		// Token: 0x04000B84 RID: 2948
		LF_STMEMBER_ST,
		// Token: 0x04000B85 RID: 2949
		LF_METHOD_ST,
		// Token: 0x04000B86 RID: 2950
		LF_NESTTYPE_ST,
		// Token: 0x04000B87 RID: 2951
		LF_VFUNCTAB,
		// Token: 0x04000B88 RID: 2952
		LF_FRIENDCLS,
		// Token: 0x04000B89 RID: 2953
		LF_ONEMETHOD_ST,
		// Token: 0x04000B8A RID: 2954
		LF_VFUNCOFF,
		// Token: 0x04000B8B RID: 2955
		LF_NESTTYPEEX_ST,
		// Token: 0x04000B8C RID: 2956
		LF_MEMBERMODIFY_ST,
		// Token: 0x04000B8D RID: 2957
		LF_MANAGED_ST,
		// Token: 0x04000B8E RID: 2958
		LF_ST_MAX = 5376,
		// Token: 0x04000B8F RID: 2959
		LF_TYPESERVER,
		// Token: 0x04000B90 RID: 2960
		LF_ENUMERATE,
		// Token: 0x04000B91 RID: 2961
		LF_ARRAY,
		// Token: 0x04000B92 RID: 2962
		LF_CLASS,
		// Token: 0x04000B93 RID: 2963
		LF_STRUCTURE,
		// Token: 0x04000B94 RID: 2964
		LF_UNION,
		// Token: 0x04000B95 RID: 2965
		LF_ENUM,
		// Token: 0x04000B96 RID: 2966
		LF_DIMARRAY,
		// Token: 0x04000B97 RID: 2967
		LF_PRECOMP,
		// Token: 0x04000B98 RID: 2968
		LF_ALIAS,
		// Token: 0x04000B99 RID: 2969
		LF_DEFARG,
		// Token: 0x04000B9A RID: 2970
		LF_FRIENDFCN,
		// Token: 0x04000B9B RID: 2971
		LF_MEMBER,
		// Token: 0x04000B9C RID: 2972
		LF_STMEMBER,
		// Token: 0x04000B9D RID: 2973
		LF_METHOD,
		// Token: 0x04000B9E RID: 2974
		LF_NESTTYPE,
		// Token: 0x04000B9F RID: 2975
		LF_ONEMETHOD,
		// Token: 0x04000BA0 RID: 2976
		LF_NESTTYPEEX,
		// Token: 0x04000BA1 RID: 2977
		LF_MEMBERMODIFY,
		// Token: 0x04000BA2 RID: 2978
		LF_MANAGED,
		// Token: 0x04000BA3 RID: 2979
		LF_TYPESERVER2,
		// Token: 0x04000BA4 RID: 2980
		LF_NUMERIC = 32768,
		// Token: 0x04000BA5 RID: 2981
		LF_CHAR = 32768,
		// Token: 0x04000BA6 RID: 2982
		LF_SHORT,
		// Token: 0x04000BA7 RID: 2983
		LF_USHORT,
		// Token: 0x04000BA8 RID: 2984
		LF_LONG,
		// Token: 0x04000BA9 RID: 2985
		LF_ULONG,
		// Token: 0x04000BAA RID: 2986
		LF_REAL32,
		// Token: 0x04000BAB RID: 2987
		LF_REAL64,
		// Token: 0x04000BAC RID: 2988
		LF_REAL80,
		// Token: 0x04000BAD RID: 2989
		LF_REAL128,
		// Token: 0x04000BAE RID: 2990
		LF_QUADWORD,
		// Token: 0x04000BAF RID: 2991
		LF_UQUADWORD,
		// Token: 0x04000BB0 RID: 2992
		LF_COMPLEX32 = 32780,
		// Token: 0x04000BB1 RID: 2993
		LF_COMPLEX64,
		// Token: 0x04000BB2 RID: 2994
		LF_COMPLEX80,
		// Token: 0x04000BB3 RID: 2995
		LF_COMPLEX128,
		// Token: 0x04000BB4 RID: 2996
		LF_VARSTRING,
		// Token: 0x04000BB5 RID: 2997
		LF_OCTWORD = 32791,
		// Token: 0x04000BB6 RID: 2998
		LF_UOCTWORD,
		// Token: 0x04000BB7 RID: 2999
		LF_DECIMAL,
		// Token: 0x04000BB8 RID: 3000
		LF_DATE,
		// Token: 0x04000BB9 RID: 3001
		LF_UTF8STRING,
		// Token: 0x04000BBA RID: 3002
		LF_PAD0 = 240,
		// Token: 0x04000BBB RID: 3003
		LF_PAD1,
		// Token: 0x04000BBC RID: 3004
		LF_PAD2,
		// Token: 0x04000BBD RID: 3005
		LF_PAD3,
		// Token: 0x04000BBE RID: 3006
		LF_PAD4,
		// Token: 0x04000BBF RID: 3007
		LF_PAD5,
		// Token: 0x04000BC0 RID: 3008
		LF_PAD6,
		// Token: 0x04000BC1 RID: 3009
		LF_PAD7,
		// Token: 0x04000BC2 RID: 3010
		LF_PAD8,
		// Token: 0x04000BC3 RID: 3011
		LF_PAD9,
		// Token: 0x04000BC4 RID: 3012
		LF_PAD10,
		// Token: 0x04000BC5 RID: 3013
		LF_PAD11,
		// Token: 0x04000BC6 RID: 3014
		LF_PAD12,
		// Token: 0x04000BC7 RID: 3015
		LF_PAD13,
		// Token: 0x04000BC8 RID: 3016
		LF_PAD14,
		// Token: 0x04000BC9 RID: 3017
		LF_PAD15
	}
}
