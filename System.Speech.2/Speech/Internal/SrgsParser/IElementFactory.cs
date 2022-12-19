using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000D0 RID: 208
	internal interface IElementFactory
	{
		// Token: 0x06000758 RID: 1880
		void RemoveAllRules();

		// Token: 0x06000759 RID: 1881
		IElementText CreateText(IElement parent, string value);

		// Token: 0x0600075A RID: 1882
		IToken CreateToken(IElement parent, string content, string pronumciation, string display, float reqConfidence);

		// Token: 0x0600075B RID: 1883
		IPropertyTag CreatePropertyTag(IElement parent);

		// Token: 0x0600075C RID: 1884
		ISemanticTag CreateSemanticTag(IElement parent);

		// Token: 0x0600075D RID: 1885
		IItem CreateItem(IElement parent, IRule rule, int minRepeat, int maxRepeat, float repeatProbability, float weight);

		// Token: 0x0600075E RID: 1886
		IRuleRef CreateRuleRef(IElement parent, Uri srgsUri);

		// Token: 0x0600075F RID: 1887
		IRuleRef CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters);

		// Token: 0x06000760 RID: 1888
		void InitSpecialRuleRef(IElement parent, IRuleRef special);

		// Token: 0x06000761 RID: 1889
		IOneOf CreateOneOf(IElement parent, IRule rule);

		// Token: 0x06000762 RID: 1890
		ISubset CreateSubset(IElement parent, string text, MatchMode matchMode);

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000763 RID: 1891
		IGrammar Grammar { get; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000764 RID: 1892
		IRuleRef Null { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000765 RID: 1893
		IRuleRef Void { get; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000766 RID: 1894
		IRuleRef Garbage { get; }

		// Token: 0x06000767 RID: 1895
		string AddScript(IGrammar grammar, string rule, string code, string filename, int line);

		// Token: 0x06000768 RID: 1896
		void AddScript(IGrammar grammar, string script, string filename, int line);

		// Token: 0x06000769 RID: 1897
		void AddScript(IGrammar grammar, string rule, string code);

		// Token: 0x0600076A RID: 1898
		void AddItem(IOneOf oneOf, IItem value);

		// Token: 0x0600076B RID: 1899
		void AddElement(IRule rule, IElement value);

		// Token: 0x0600076C RID: 1900
		void AddElement(IItem item, IElement value);
	}
}
