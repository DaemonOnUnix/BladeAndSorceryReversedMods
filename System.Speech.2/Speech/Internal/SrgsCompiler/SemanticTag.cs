using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F8 RID: 248
	internal sealed class SemanticTag : ParseElement, ISemanticTag, IElement
	{
		// Token: 0x060008BE RID: 2238 RVA: 0x00027A21 File Offset: 0x00025C21
		internal SemanticTag(ParseElement parent, Backend backend)
			: base(parent._rule)
		{
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00027A3C File Offset: 0x00025C3C
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

		// Token: 0x0400062A RID: 1578
		private CfgGrammar.CfgProperty _propInfo = new CfgGrammar.CfgProperty();
	}
}
