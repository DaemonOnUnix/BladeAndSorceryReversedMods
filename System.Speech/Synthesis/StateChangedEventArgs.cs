using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000154 RID: 340
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x00027D53 File Offset: 0x00026D53
		internal StateChangedEventArgs(SynthesizerState state, SynthesizerState previousState)
		{
			this._state = state;
			this._previousState = previousState;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00027D69 File Offset: 0x00026D69
		public SynthesizerState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00027D71 File Offset: 0x00026D71
		public SynthesizerState PreviousState
		{
			get
			{
				return this._previousState;
			}
		}

		// Token: 0x04000679 RID: 1657
		private SynthesizerState _state;

		// Token: 0x0400067A RID: 1658
		private SynthesizerState _previousState;
	}
}
