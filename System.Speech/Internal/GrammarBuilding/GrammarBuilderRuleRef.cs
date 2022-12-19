using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001A8 RID: 424
	internal sealed class GrammarBuilderRuleRef : GrammarBuilderBase
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x00031282 File Offset: 0x00030282
		internal GrammarBuilderRuleRef(Uri uri, string rule)
		{
			this._uri = uri.OriginalString + ((rule != null) ? ("#" + rule) : "");
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x000312B0 File Offset: 0x000302B0
		private GrammarBuilderRuleRef(string sgrsUri)
		{
			this._uri = sgrsUri;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000312C0 File Offset: 0x000302C0
		public override bool Equals(object obj)
		{
			GrammarBuilderRuleRef grammarBuilderRuleRef = obj as GrammarBuilderRuleRef;
			return grammarBuilderRuleRef != null && this._uri == grammarBuilderRuleRef._uri;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000312EA File Offset: 0x000302EA
		public override int GetHashCode()
		{
			return this._uri.GetHashCode();
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000312F7 File Offset: 0x000302F7
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderRuleRef(this._uri);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00031304 File Offset: 0x00030304
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			Uri uri = new Uri(this._uri, 0);
			return elementFactory.CreateRuleRef(parent, uri, null, null);
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00031328 File Offset: 0x00030328
		internal override string DebugSummary
		{
			get
			{
				return "#" + this._uri;
			}
		}

		// Token: 0x04000989 RID: 2441
		private readonly string _uri;
	}
}
