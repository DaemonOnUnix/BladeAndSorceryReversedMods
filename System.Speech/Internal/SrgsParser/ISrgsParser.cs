using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000C7 RID: 199
	internal interface ISrgsParser
	{
		// Token: 0x06000455 RID: 1109
		void Parse();

		// Token: 0x170000A1 RID: 161
		// (set) Token: 0x06000456 RID: 1110
		IElementFactory ElementFactory { set; }
	}
}
