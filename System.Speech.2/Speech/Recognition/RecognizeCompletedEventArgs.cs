using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x02000060 RID: 96
	public class RecognizeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00009C5B File Offset: 0x00007E5B
		internal RecognizeCompletedEventArgs(RecognitionResult result, bool initialSilenceTimeout, bool babbleTimeout, bool inputStreamEnded, TimeSpan audioPosition, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._result = result;
			this._initialSilenceTimeout = initialSilenceTimeout;
			this._babbleTimeout = babbleTimeout;
			this._inputStreamEnded = inputStreamEnded;
			this._audioPosition = audioPosition;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00009C8E File Offset: 0x00007E8E
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00009C96 File Offset: 0x00007E96
		public bool InitialSilenceTimeout
		{
			get
			{
				return this._initialSilenceTimeout;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00009C9E File Offset: 0x00007E9E
		public bool BabbleTimeout
		{
			get
			{
				return this._babbleTimeout;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00009CA6 File Offset: 0x00007EA6
		public bool InputStreamEnded
		{
			get
			{
				return this._inputStreamEnded;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00009CAE File Offset: 0x00007EAE
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000346 RID: 838
		private RecognitionResult _result;

		// Token: 0x04000347 RID: 839
		private bool _initialSilenceTimeout;

		// Token: 0x04000348 RID: 840
		private bool _babbleTimeout;

		// Token: 0x04000349 RID: 841
		private bool _inputStreamEnded;

		// Token: 0x0400034A RID: 842
		private TimeSpan _audioPosition;
	}
}
