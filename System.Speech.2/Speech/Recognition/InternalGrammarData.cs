using System;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000063 RID: 99
	internal class InternalGrammarData
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000D5D2 File Offset: 0x0000B7D2
		internal InternalGrammarData(ulong grammarId, SapiGrammar sapiGrammar, bool enabled, float weight, int priority)
		{
			this._grammarId = grammarId;
			this._sapiGrammar = sapiGrammar;
			this._grammarEnabled = enabled;
			this._grammarWeight = weight;
			this._grammarPriority = priority;
		}

		// Token: 0x0400038C RID: 908
		internal ulong _grammarId;

		// Token: 0x0400038D RID: 909
		internal SapiGrammar _sapiGrammar;

		// Token: 0x0400038E RID: 910
		internal bool _grammarEnabled;

		// Token: 0x0400038F RID: 911
		internal float _grammarWeight;

		// Token: 0x04000390 RID: 912
		internal int _grammarPriority;
	}
}
