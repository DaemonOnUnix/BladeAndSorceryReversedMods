using System;

namespace System.Speech.Recognition
{
	// Token: 0x020001A2 RID: 418
	public class SpeechDetectedEventArgs : EventArgs
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x0002F91C File Offset: 0x0002E91C
		internal SpeechDetectedEventArgs(TimeSpan audioPosition)
		{
			this._audioPosition = audioPosition;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002F92B File Offset: 0x0002E92B
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000966 RID: 2406
		private TimeSpan _audioPosition;
	}
}
