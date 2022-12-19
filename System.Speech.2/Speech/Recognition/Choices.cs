using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x0200005B RID: 91
	[DebuggerDisplay("{_oneOf.DebugSummary}")]
	public class Choices
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x0000941A File Offset: 0x0000761A
		public Choices()
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000942D File Offset: 0x0000762D
		public Choices(params string[] phrases)
		{
			Helpers.ThrowIfNull(phrases, "phrases");
			this.Add(phrases);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009452 File Offset: 0x00007652
		public Choices(params GrammarBuilder[] alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			this.Add(alternateChoices);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009478 File Offset: 0x00007678
		public void Add(params string[] phrases)
		{
			Helpers.ThrowIfNull(phrases, "phrases");
			foreach (string text in phrases)
			{
				Helpers.ThrowIfEmptyOrNull(text, "phrase");
				this._oneOf.Add(text);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000094BC File Offset: 0x000076BC
		public void Add(params GrammarBuilder[] alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			foreach (GrammarBuilder grammarBuilder in alternateChoices)
			{
				Helpers.ThrowIfNull(grammarBuilder, "alternateChoice");
				this._oneOf.Items.Add(new ItemElement(grammarBuilder));
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009509 File Offset: 0x00007709
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00009511 File Offset: 0x00007711
		internal OneOfElement OneOf
		{
			get
			{
				return this._oneOf;
			}
		}

		// Token: 0x04000340 RID: 832
		private OneOfElement _oneOf = new OneOfElement();
	}
}
