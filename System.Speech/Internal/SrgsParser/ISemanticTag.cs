using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000B6 RID: 182
	internal interface ISemanticTag : IElement
	{
		// Token: 0x06000404 RID: 1028
		void Content(IElement parent, string value, int line);
	}
}
