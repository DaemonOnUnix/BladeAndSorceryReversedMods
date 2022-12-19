using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000C9 RID: 201
	internal interface IToken : IElement
	{
		// Token: 0x170000A2 RID: 162
		// (set) Token: 0x06000457 RID: 1111
		string Text { set; }

		// Token: 0x170000A3 RID: 163
		// (set) Token: 0x06000458 RID: 1112
		string Display { set; }

		// Token: 0x170000A4 RID: 164
		// (set) Token: 0x06000459 RID: 1113
		string Pronunciation { set; }
	}
}
