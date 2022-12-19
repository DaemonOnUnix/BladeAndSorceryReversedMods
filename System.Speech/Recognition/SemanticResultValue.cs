using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x020001A1 RID: 417
	[DebuggerDisplay("{_tag.DebugSummary}")]
	public class SemanticResultValue
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002F883 File Offset: 0x0002E883
		public SemanticResultValue(object value)
		{
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(value);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002F8A2 File Offset: 0x0002E8A2
		public SemanticResultValue(string phrase, object value)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(new GrammarBuilderPhrase((string)phrase.Clone()), value);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002F8DC File Offset: 0x0002E8DC
		public SemanticResultValue(GrammarBuilder builder, object value)
		{
			Helpers.ThrowIfNull(builder, "builder");
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(builder.Clone(), value);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002F90C File Offset: 0x0002E90C
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002F914 File Offset: 0x0002E914
		internal TagElement Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x04000965 RID: 2405
		private TagElement _tag;
	}
}
