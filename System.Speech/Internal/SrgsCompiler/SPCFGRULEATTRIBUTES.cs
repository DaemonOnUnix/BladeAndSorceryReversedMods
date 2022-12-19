using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A1 RID: 161
	[Flags]
	internal enum SPCFGRULEATTRIBUTES
	{
		// Token: 0x0400033C RID: 828
		SPRAF_TopLevel = 1,
		// Token: 0x0400033D RID: 829
		SPRAF_Active = 2,
		// Token: 0x0400033E RID: 830
		SPRAF_Export = 4,
		// Token: 0x0400033F RID: 831
		SPRAF_Import = 8,
		// Token: 0x04000340 RID: 832
		SPRAF_Interpreter = 16,
		// Token: 0x04000341 RID: 833
		SPRAF_Dynamic = 32,
		// Token: 0x04000342 RID: 834
		SPRAF_Root = 64,
		// Token: 0x04000343 RID: 835
		SPRAF_AutoPause = 65536,
		// Token: 0x04000344 RID: 836
		SPRAF_UserDelimited = 131072
	}
}
