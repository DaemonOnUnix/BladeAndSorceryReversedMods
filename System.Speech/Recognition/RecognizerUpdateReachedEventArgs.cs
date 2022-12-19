using System;

namespace System.Speech.Recognition
{
	// Token: 0x020001A5 RID: 421
	public class RecognizerUpdateReachedEventArgs : EventArgs
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x00030FD4 File Offset: 0x0002FFD4
		internal RecognizerUpdateReachedEventArgs(object userToken, TimeSpan audioPosition)
		{
			this._userToken = userToken;
			this._audioPosition = audioPosition;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00030FEA File Offset: 0x0002FFEA
		public object UserToken
		{
			get
			{
				return this._userToken;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00030FF2 File Offset: 0x0002FFF2
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000983 RID: 2435
		private object _userToken;

		// Token: 0x04000984 RID: 2436
		private TimeSpan _audioPosition;
	}
}
