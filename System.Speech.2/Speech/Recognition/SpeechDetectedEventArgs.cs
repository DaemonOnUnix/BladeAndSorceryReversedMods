using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200006E RID: 110
	public class SpeechDetectedEventArgs : EventArgs
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
		internal SpeechDetectedEventArgs(TimeSpan audioPosition)
		{
			this._audioPosition = audioPosition;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000D9DF File Offset: 0x0000BBDF
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x040003A2 RID: 930
		private TimeSpan _audioPosition;
	}
}
