using System;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000197 RID: 407
	internal class InternalGrammarData
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002F514 File Offset: 0x0002E514
		internal InternalGrammarData(ulong grammarId, SapiGrammar sapiGrammar, bool enabled, float weight, int priority)
		{
			this._grammarId = grammarId;
			this._sapiGrammar = sapiGrammar;
			this._grammarEnabled = enabled;
			this._grammarWeight = weight;
			this._grammarPriority = priority;
		}

		// Token: 0x04000950 RID: 2384
		internal ulong _grammarId;

		// Token: 0x04000951 RID: 2385
		internal SapiGrammar _sapiGrammar;

		// Token: 0x04000952 RID: 2386
		internal bool _grammarEnabled;

		// Token: 0x04000953 RID: 2387
		internal float _grammarWeight;

		// Token: 0x04000954 RID: 2388
		internal int _grammarPriority;
	}
}
