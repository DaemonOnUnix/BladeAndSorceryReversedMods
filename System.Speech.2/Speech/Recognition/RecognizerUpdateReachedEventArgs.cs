using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000071 RID: 113
	public class RecognizerUpdateReachedEventArgs : EventArgs
	{
		// Token: 0x06000391 RID: 913 RVA: 0x0000F3E8 File Offset: 0x0000D5E8
		internal RecognizerUpdateReachedEventArgs(object userToken, TimeSpan audioPosition)
		{
			this._userToken = userToken;
			this._audioPosition = audioPosition;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000F3FE File Offset: 0x0000D5FE
		public object UserToken
		{
			get
			{
				return this._userToken;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000F406 File Offset: 0x0000D606
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x040003BF RID: 959
		private object _userToken;

		// Token: 0x040003C0 RID: 960
		private TimeSpan _audioPosition;
	}
}
