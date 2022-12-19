using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000DE RID: 222
	internal interface ISrgsParser
	{
		// Token: 0x06000789 RID: 1929
		void Parse();

		// Token: 0x1700018A RID: 394
		// (set) Token: 0x0600078A RID: 1930
		IElementFactory ElementFactory { set; }
	}
}
