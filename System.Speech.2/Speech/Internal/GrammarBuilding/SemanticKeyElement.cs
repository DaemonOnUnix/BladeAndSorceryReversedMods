using System;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A5 RID: 165
	internal sealed class SemanticKeyElement : BuilderElements
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x00015B60 File Offset: 0x00013D60
		internal SemanticKeyElement(string semanticKey)
		{
			this._semanticKey = semanticKey;
			RuleElement ruleElement = new RuleElement(semanticKey);
			this._ruleRef = new RuleRefElement(ruleElement, this._semanticKey);
			base.Items.Add(ruleElement);
			base.Items.Add(this._ruleRef);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00015BB0 File Offset: 0x00013DB0
		public override bool Equals(object obj)
		{
			SemanticKeyElement semanticKeyElement = obj as SemanticKeyElement;
			return semanticKeyElement != null && base.Equals(obj) && this._semanticKey == semanticKeyElement._semanticKey;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001579E File Offset: 0x0001399E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00015BE5 File Offset: 0x00013DE5
		internal new void Add(string phrase)
		{
			this._ruleRef.Add(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00015BF8 File Offset: 0x00013DF8
		internal new void Add(GrammarBuilder builder)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builder.InternalBuilder.Items)
			{
				this._ruleRef.Add(grammarBuilderBase);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00015C58 File Offset: 0x00013E58
		internal override GrammarBuilderBase Clone()
		{
			SemanticKeyElement semanticKeyElement = new SemanticKeyElement(this._semanticKey);
			semanticKeyElement._ruleRef.CloneItems(this._ruleRef);
			return semanticKeyElement;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00015C84 File Offset: 0x00013E84
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			this._ruleRef.Rule.CreateElement(elementFactory, parent, rule, ruleIds);
			return this._ruleRef.CreateElement(elementFactory, parent, rule, ruleIds);
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00015CB9 File Offset: 0x00013EB9
		internal override string DebugSummary
		{
			get
			{
				return this._ruleRef.Rule.DebugSummary;
			}
		}

		// Token: 0x0400045C RID: 1116
		private readonly string _semanticKey;

		// Token: 0x0400045D RID: 1117
		private readonly RuleRefElement _ruleRef;
	}
}
