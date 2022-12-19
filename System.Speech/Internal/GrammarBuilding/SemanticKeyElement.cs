using System;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AF RID: 431
	internal sealed class SemanticKeyElement : BuilderElements
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003194C File Offset: 0x0003094C
		internal SemanticKeyElement(string semanticKey)
		{
			this._semanticKey = semanticKey;
			RuleElement ruleElement = new RuleElement(semanticKey);
			this._ruleRef = new RuleRefElement(ruleElement, this._semanticKey);
			base.Items.Add(ruleElement);
			base.Items.Add(this._ruleRef);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0003199C File Offset: 0x0003099C
		public override bool Equals(object obj)
		{
			SemanticKeyElement semanticKeyElement = obj as SemanticKeyElement;
			return semanticKeyElement != null && base.Equals(obj) && this._semanticKey == semanticKeyElement._semanticKey;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000319D1 File Offset: 0x000309D1
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000319D9 File Offset: 0x000309D9
		internal new void Add(string phrase)
		{
			this._ruleRef.Add(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000319EC File Offset: 0x000309EC
		internal new void Add(GrammarBuilder builder)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builder.InternalBuilder.Items)
			{
				this._ruleRef.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00031A4C File Offset: 0x00030A4C
		internal override GrammarBuilderBase Clone()
		{
			SemanticKeyElement semanticKeyElement = new SemanticKeyElement(this._semanticKey);
			semanticKeyElement._ruleRef.CloneItems(this._ruleRef);
			return semanticKeyElement;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00031A78 File Offset: 0x00030A78
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			this._ruleRef.Rule.CreateElement(elementFactory, parent, rule, ruleIds);
			return this._ruleRef.CreateElement(elementFactory, parent, rule, ruleIds);
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00031AAD File Offset: 0x00030AAD
		internal override string DebugSummary
		{
			get
			{
				return this._ruleRef.Rule.DebugSummary;
			}
		}

		// Token: 0x04000992 RID: 2450
		private readonly string _semanticKey;

		// Token: 0x04000993 RID: 2451
		private readonly RuleRefElement _ruleRef;
	}
}
