using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x02000192 RID: 402
	public class LoadGrammarCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x0002BE04 File Offset: 0x0002AE04
		internal LoadGrammarCompletedEventArgs(Grammar grammar, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._grammar = grammar;
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0002BE17 File Offset: 0x0002AE17
		public Grammar Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x04000908 RID: 2312
		private Grammar _grammar;
	}
}
