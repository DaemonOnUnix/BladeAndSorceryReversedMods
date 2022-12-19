using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000B7 RID: 183
	internal sealed class SemanticTag : ParseElement, ISemanticTag, IElement
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x0000FD2D File Offset: 0x0000ED2D
		internal SemanticTag(ParseElement parent, Backend backend)
			: base(parent._rule)
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000FD48 File Offset: 0x0000ED48
		void ISemanticTag.Content(IElement parentElement, string sTag, int iLine)
		{
			sTag = sTag.Trim(Helpers._achTrimChars);
			if (string.IsNullOrEmpty(sTag))
			{
				return;
			}
			this._propInfo._ulId = (uint)iLine;
			this._propInfo._comValue = sTag;
			ParseElementCollection parseElementCollection = (ParseElementCollection)parentElement;
			parseElementCollection.AddSemanticInterpretationTag(this._propInfo);
		}

		// Token: 0x04000380 RID: 896
		private CfgGrammar.CfgProperty _propInfo = new CfgGrammar.CfgProperty();
	}
}
