using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x02000193 RID: 403
	public class RecognizeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000A20 RID: 2592 RVA: 0x0002BE1F File Offset: 0x0002AE1F
		internal RecognizeCompletedEventArgs(RecognitionResult result, bool initialSilenceTimeout, bool babbleTimeout, bool inputStreamEnded, TimeSpan audioPosition, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._result = result;
			this._initialSilenceTimeout = initialSilenceTimeout;
			this._babbleTimeout = babbleTimeout;
			this._inputStreamEnded = inputStreamEnded;
			this._audioPosition = audioPosition;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0002BE52 File Offset: 0x0002AE52
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002BE5A File Offset: 0x0002AE5A
		public bool InitialSilenceTimeout
		{
			get
			{
				return this._initialSilenceTimeout;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0002BE62 File Offset: 0x0002AE62
		public bool BabbleTimeout
		{
			get
			{
				return this._babbleTimeout;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002BE6A File Offset: 0x0002AE6A
		public bool InputStreamEnded
		{
			get
			{
				return this._inputStreamEnded;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0002BE72 File Offset: 0x0002AE72
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000909 RID: 2313
		private RecognitionResult _result;

		// Token: 0x0400090A RID: 2314
		private bool _initialSilenceTimeout;

		// Token: 0x0400090B RID: 2315
		private bool _babbleTimeout;

		// Token: 0x0400090C RID: 2316
		private bool _inputStreamEnded;

		// Token: 0x0400090D RID: 2317
		private TimeSpan _audioPosition;
	}
}
