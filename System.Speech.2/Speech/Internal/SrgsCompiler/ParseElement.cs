using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F3 RID: 243
	internal abstract class ParseElement : IElement
	{
		// Token: 0x06000897 RID: 2199 RVA: 0x00026D16 File Offset: 0x00024F16
		internal ParseElement(Rule rule)
		{
			this._rule = rule;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElement.PostParse(IElement parent)
		{
		}

		// Token: 0x04000615 RID: 1557
		internal int _confidence;

		// Token: 0x04000616 RID: 1558
		internal Rule _rule;
	}
}
