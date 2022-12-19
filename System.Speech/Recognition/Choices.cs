using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x0200018B RID: 395
	[DebuggerDisplay("{_oneOf.DebugSummary}")]
	public class Choices
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x0002AD49 File Offset: 0x00029D49
		public Choices()
		{
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002AD5C File Offset: 0x00029D5C
		public Choices(params string[] phrases)
		{
			Helpers.ThrowIfNull(phrases, "phrases");
			this.Add(phrases);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002AD81 File Offset: 0x00029D81
		public Choices(params GrammarBuilder[] alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			this.Add(alternateChoices);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002ADA8 File Offset: 0x00029DA8
		public void Add(params string[] phrases)
		{
			Helpers.ThrowIfNull(phrases, "phrases");
			foreach (string text in phrases)
			{
				Helpers.ThrowIfEmptyOrNull(text, "phrase");
				this._oneOf.Add(text);
			}
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002ADEC File Offset: 0x00029DEC
		public void Add(params GrammarBuilder[] alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			foreach (GrammarBuilder grammarBuilder in alternateChoices)
			{
				Helpers.ThrowIfNull(grammarBuilder, "alternateChoice");
				this._oneOf.Items.Add(new ItemElement(grammarBuilder));
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002AE39 File Offset: 0x00029E39
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002AE41 File Offset: 0x00029E41
		internal OneOfElement OneOf
		{
			get
			{
				return this._oneOf;
			}
		}

		// Token: 0x040008FF RID: 2303
		private OneOfElement _oneOf = new OneOfElement();
	}
}
