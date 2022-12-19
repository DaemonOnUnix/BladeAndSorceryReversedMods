using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x0200006C RID: 108
	[DebuggerDisplay("{_semanticKey.DebugSummary}")]
	public class SemanticResultKey
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000D872 File Offset: 0x0000BA72
		private SemanticResultKey(string semanticResultKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticResultKey, "semanticResultKey");
			this._semanticKey = new SemanticKeyElement(semanticResultKey);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D894 File Offset: 0x0000BA94
		public SemanticResultKey(string semanticResultKey, params string[] phrases)
			: this(semanticResultKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticResultKey, "semanticResultKey");
			Helpers.ThrowIfNull(phrases, "phrases");
			foreach (string text in phrases)
			{
				this._semanticKey.Add((string)text.Clone());
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public SemanticResultKey(string semanticResultKey, params GrammarBuilder[] builders)
			: this(semanticResultKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticResultKey, "semanticResultKey");
			Helpers.ThrowIfNull(builders, "phrases");
			foreach (GrammarBuilder grammarBuilder in builders)
			{
				this._semanticKey.Add(grammarBuilder.Clone());
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009A7A File Offset: 0x00007C7A
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000D937 File Offset: 0x0000BB37
		internal SemanticKeyElement SemanticKeyElement
		{
			get
			{
				return this._semanticKey;
			}
		}

		// Token: 0x040003A0 RID: 928
		private readonly SemanticKeyElement _semanticKey;
	}
}
