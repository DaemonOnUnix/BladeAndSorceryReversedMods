using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001C RID: 28
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x06000096 RID: 150 RVA: 0x0000452A File Offset: 0x0000272A
		internal StateChangedEventArgs(SynthesizerState state, SynthesizerState previousState)
		{
			this._state = state;
			this._previousState = previousState;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004540 File Offset: 0x00002740
		public SynthesizerState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004548 File Offset: 0x00002748
		public SynthesizerState PreviousState
		{
			get
			{
				return this._previousState;
			}
		}

		// Token: 0x040001F5 RID: 501
		private SynthesizerState _state;

		// Token: 0x040001F6 RID: 502
		private SynthesizerState _previousState;
	}
}
