using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000E2 RID: 226
	// (Invoke) Token: 0x0600078F RID: 1935
	internal delegate IToken CreateTokenCallback(IElement parent, string content, string pronumciation, string display, float reqConfidence);
}
