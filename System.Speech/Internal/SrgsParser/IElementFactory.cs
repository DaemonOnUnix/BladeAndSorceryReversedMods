using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000BA RID: 186
	internal interface IElementFactory
	{
		// Token: 0x0600040C RID: 1036
		void RemoveAllRules();

		// Token: 0x0600040D RID: 1037
		IElementText CreateText(IElement parent, string value);

		// Token: 0x0600040E RID: 1038
		IToken CreateToken(IElement parent, string content, string pronumciation, string display, float reqConfidence);

		// Token: 0x0600040F RID: 1039
		IPropertyTag CreatePropertyTag(IElement parent);

		// Token: 0x06000410 RID: 1040
		ISemanticTag CreateSemanticTag(IElement parent);

		// Token: 0x06000411 RID: 1041
		IItem CreateItem(IElement parent, IRule rule, int minRepeat, int maxRepeat, float repeatProbability, float weight);

		// Token: 0x06000412 RID: 1042
		IRuleRef CreateRuleRef(IElement parent, Uri srgsUri);

		// Token: 0x06000413 RID: 1043
		IRuleRef CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters);

		// Token: 0x06000414 RID: 1044
		void InitSpecialRuleRef(IElement parent, IRuleRef special);

		// Token: 0x06000415 RID: 1045
		IOneOf CreateOneOf(IElement parent, IRule rule);

		// Token: 0x06000416 RID: 1046
		ISubset CreateSubset(IElement parent, string text, MatchMode matchMode);

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000417 RID: 1047
		IGrammar Grammar { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000418 RID: 1048
		IRuleRef Null { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000419 RID: 1049
		IRuleRef Void { get; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600041A RID: 1050
		IRuleRef Garbage { get; }

		// Token: 0x0600041B RID: 1051
		string AddScript(IGrammar grammar, string rule, string code, string filename, int line);

		// Token: 0x0600041C RID: 1052
		void AddScript(IGrammar grammar, string script, string filename, int line);

		// Token: 0x0600041D RID: 1053
		void AddScript(IGrammar grammar, string rule, string code);

		// Token: 0x0600041E RID: 1054
		void AddItem(IOneOf oneOf, IItem value);

		// Token: 0x0600041F RID: 1055
		void AddElement(IRule rule, IElement value);

		// Token: 0x06000420 RID: 1056
		void AddElement(IItem item, IElement value);
	}
}
