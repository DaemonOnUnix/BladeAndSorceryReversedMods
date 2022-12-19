using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009C RID: 156
	internal sealed class GrammarBuilderDictation : GrammarBuilderBase
	{
		// Token: 0x06000535 RID: 1333 RVA: 0x00015228 File Offset: 0x00013428
		internal GrammarBuilderDictation()
			: this(null)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00015231 File Offset: 0x00013431
		internal GrammarBuilderDictation(string category)
		{
			this._category = category;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00015240 File Offset: 0x00013440
		public override bool Equals(object obj)
		{
			GrammarBuilderDictation grammarBuilderDictation = obj as GrammarBuilderDictation;
			return grammarBuilderDictation != null && this._category == grammarBuilderDictation._category;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001526A File Offset: 0x0001346A
		public override int GetHashCode()
		{
			if (this._category != null)
			{
				return this._category.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00015281 File Offset: 0x00013481
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderDictation(this._category);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001528E File Offset: 0x0001348E
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return this.CreateRuleRefToDictation(elementFactory, parent);
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00015298 File Offset: 0x00013498
		internal override string DebugSummary
		{
			get
			{
				string text = ((this._category != null) ? (":" + this._category) : string.Empty);
				return "dictation" + text;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000152D0 File Offset: 0x000134D0
		private IRuleRef CreateRuleRefToDictation(IElementFactory elementFactory, IElement parent)
		{
			Uri uri;
			if (!string.IsNullOrEmpty(this._category) && this._category == "spelling")
			{
				uri = new Uri("grammar:dictation#spelling", UriKind.RelativeOrAbsolute);
			}
			else
			{
				uri = new Uri("grammar:dictation", UriKind.RelativeOrAbsolute);
			}
			return elementFactory.CreateRuleRef(parent, uri, null, null);
		}

		// Token: 0x0400044F RID: 1103
		private readonly string _category;
	}
}
