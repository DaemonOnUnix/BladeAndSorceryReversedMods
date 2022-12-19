using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000EC RID: 236
	[Flags]
	internal enum SPCFGRULEATTRIBUTES
	{
		// Token: 0x040005ED RID: 1517
		SPRAF_TopLevel = 1,
		// Token: 0x040005EE RID: 1518
		SPRAF_Active = 2,
		// Token: 0x040005EF RID: 1519
		SPRAF_Export = 4,
		// Token: 0x040005F0 RID: 1520
		SPRAF_Import = 8,
		// Token: 0x040005F1 RID: 1521
		SPRAF_Interpreter = 16,
		// Token: 0x040005F2 RID: 1522
		SPRAF_Dynamic = 32,
		// Token: 0x040005F3 RID: 1523
		SPRAF_Root = 64,
		// Token: 0x040005F4 RID: 1524
		SPRAF_AutoPause = 65536,
		// Token: 0x040005F5 RID: 1525
		SPRAF_UserDelimited = 131072
	}
}
