using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001A6 RID: 422
	internal sealed class GrammarBuilderDictation : GrammarBuilderBase
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x00030FFA File Offset: 0x0002FFFA
		internal GrammarBuilderDictation()
			: this(null)
		{
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00031003 File Offset: 0x00030003
		internal GrammarBuilderDictation(string category)
		{
			this._category = category;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00031014 File Offset: 0x00030014
		public override bool Equals(object obj)
		{
			GrammarBuilderDictation grammarBuilderDictation = obj as GrammarBuilderDictation;
			return grammarBuilderDictation != null && this._category == grammarBuilderDictation._category;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0003103E File Offset: 0x0003003E
		public override int GetHashCode()
		{
			if (this._category != null)
			{
				return this._category.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00031055 File Offset: 0x00030055
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderDictation(this._category);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00031062 File Offset: 0x00030062
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return this.CreateRuleRefToDictation(elementFactory, parent);
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0003106C File Offset: 0x0003006C
		internal override string DebugSummary
		{
			get
			{
				string text = ((this._category != null) ? (":" + this._category) : string.Empty);
				return "dictation" + text;
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000310A4 File Offset: 0x000300A4
		private IRuleRef CreateRuleRefToDictation(IElementFactory elementFactory, IElement parent)
		{
			Uri uri;
			if (!string.IsNullOrEmpty(this._category) && this._category == "spelling")
			{
				uri = new Uri("grammar:dictation#spelling", 0);
			}
			else
			{
				uri = new Uri("grammar:dictation", 0);
			}
			return elementFactory.CreateRuleRef(parent, uri, null, null);
		}

		// Token: 0x04000985 RID: 2437
		private readonly string _category;
	}
}
