using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000E1 RID: 225
	internal interface IToken : IElement
	{
		// Token: 0x1700018B RID: 395
		// (set) Token: 0x0600078B RID: 1931
		string Text { set; }

		// Token: 0x1700018C RID: 396
		// (set) Token: 0x0600078C RID: 1932
		string Display { set; }

		// Token: 0x1700018D RID: 397
		// (set) Token: 0x0600078D RID: 1933
		string Pronunciation { set; }
	}
}
