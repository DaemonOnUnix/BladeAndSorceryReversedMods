using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200006B RID: 107
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x060002ED RID: 749 RVA: 0x0000D85B File Offset: 0x0000BA5B
		internal StateChangedEventArgs(RecognizerState recognizerState)
		{
			this._recognizerState = recognizerState;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000D86A File Offset: 0x0000BA6A
		public RecognizerState RecognizerState
		{
			get
			{
				return this._recognizerState;
			}
		}

		// Token: 0x0400039F RID: 927
		private RecognizerState _recognizerState;
	}
}
