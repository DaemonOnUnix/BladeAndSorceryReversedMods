using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;

namespace System.Speech.Recognition
{
	// Token: 0x020001A0 RID: 416
	[DebuggerDisplay("{_semanticKey.DebugSummary}")]
	public class SemanticResultKey
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002F7B0 File Offset: 0x0002E7B0
		private SemanticResultKey(string semanticResultKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticResultKey, "semanticResultKey");
			this._semanticKey = new SemanticKeyElement(semanticResultKey);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002F7D0 File Offset: 0x0002E7D0
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

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002F824 File Offset: 0x0002E824
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

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002F873 File Offset: 0x0002E873
		public GrammarBuilder ToGrammarBuilder()
		{
			return new GrammarBuilder(this);
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0002F87B File Offset: 0x0002E87B
		internal SemanticKeyElement SemanticKeyElement
		{
			get
			{
				return this._semanticKey;
			}
		}

		// Token: 0x04000964 RID: 2404
		private readonly SemanticKeyElement _semanticKey;
	}
}
