using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000CA RID: 202
	// (Invoke) Token: 0x0600045B RID: 1115
	internal delegate IToken CreateTokenCallback(IElement parent, string content, string pronumciation, string display, float reqConfidence);
}
