using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009E RID: 158
	internal sealed class GrammarBuilderRuleRef : GrammarBuilderBase
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x000154AE File Offset: 0x000136AE
		internal GrammarBuilderRuleRef(Uri uri, string rule)
		{
			this._uri = uri.OriginalString + ((rule != null) ? ("#" + rule) : "");
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000154DC File Offset: 0x000136DC
		private GrammarBuilderRuleRef(string sgrsUri)
		{
			this._uri = sgrsUri;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000154EC File Offset: 0x000136EC
		public override bool Equals(object obj)
		{
			GrammarBuilderRuleRef grammarBuilderRuleRef = obj as GrammarBuilderRuleRef;
			return grammarBuilderRuleRef != null && this._uri == grammarBuilderRuleRef._uri;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00015516 File Offset: 0x00013716
		public override int GetHashCode()
		{
			return this._uri.GetHashCode();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00015523 File Offset: 0x00013723
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderRuleRef(this._uri);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00015530 File Offset: 0x00013730
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			Uri uri = new Uri(this._uri, UriKind.RelativeOrAbsolute);
			return elementFactory.CreateRuleRef(parent, uri, null, null);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00015554 File Offset: 0x00013754
		internal override string DebugSummary
		{
			get
			{
				return "#" + this._uri;
			}
		}

		// Token: 0x04000453 RID: 1107
		private readonly string _uri;
	}
}
