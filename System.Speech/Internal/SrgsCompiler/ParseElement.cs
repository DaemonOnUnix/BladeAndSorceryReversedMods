using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A6 RID: 166
	internal abstract class ParseElement : IElement
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x0000E788 File Offset: 0x0000D788
		internal ParseElement(Rule rule)
		{
			this._rule = rule;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000E797 File Offset: 0x0000D797
		void IElement.PostParse(IElement parent)
		{
		}

		// Token: 0x04000358 RID: 856
		internal int _confidence;

		// Token: 0x04000359 RID: 857
		internal Rule _rule;
	}
}
