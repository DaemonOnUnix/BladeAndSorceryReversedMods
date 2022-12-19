using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200019F RID: 415
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002F799 File Offset: 0x0002E799
		internal StateChangedEventArgs(RecognizerState recognizerState)
		{
			this._recognizerState = recognizerState;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0002F7A8 File Offset: 0x0002E7A8
		public RecognizerState RecognizerState
		{
			get
			{
				return this._recognizerState;
			}
		}

		// Token: 0x04000963 RID: 2403
		private RecognizerState _recognizerState;
	}
}
