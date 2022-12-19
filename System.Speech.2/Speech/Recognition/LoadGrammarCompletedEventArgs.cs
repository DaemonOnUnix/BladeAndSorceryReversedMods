using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x0200005F RID: 95
	public class LoadGrammarCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000231 RID: 561 RVA: 0x00009C40 File Offset: 0x00007E40
		internal LoadGrammarCompletedEventArgs(Grammar grammar, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._grammar = grammar;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00009C53 File Offset: 0x00007E53
		public Grammar Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x04000345 RID: 837
		private Grammar _grammar;
	}
}
