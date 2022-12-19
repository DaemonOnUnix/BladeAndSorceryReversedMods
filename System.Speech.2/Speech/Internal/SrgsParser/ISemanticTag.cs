using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000DB RID: 219
	internal interface ISemanticTag : IElement
	{
		// Token: 0x06000787 RID: 1927
		void Content(IElement parent, string value, int line);
	}
}
