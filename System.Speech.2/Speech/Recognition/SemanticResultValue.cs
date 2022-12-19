using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x0200006D RID: 109
	[DebuggerDisplay("{_tag.DebugSummary}")]
	public class SemanticResultValue
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0000D93F File Offset: 0x0000BB3F
		public SemanticResultValue(object value)
		{
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(value);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D95E File Offset: 0x0000BB5E
		public SemanticResultValue(string phrase, object value)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(new GrammarBuilderPhrase((string)phrase.Clone()), value);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D998 File Offset: 0x0000BB98
		public SemanticResultValue(GrammarBuilder builder, object value)
		{
			Helpers.ThrowIfNull(builder, "builder");
			Helpers.ThrowIfNull(value, "value");
			this._tag = new TagElement(builder.Clone(), value);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00009A82 File Offset: 0x00007C82
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
		internal TagElement Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x040003A1 RID: 929
		private TagElement _tag;
	}
}
