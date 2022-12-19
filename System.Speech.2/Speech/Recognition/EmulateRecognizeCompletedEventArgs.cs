using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x0200005D RID: 93
	public class EmulateRecognizeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000201 RID: 513 RVA: 0x00009579 File Offset: 0x00007779
		internal EmulateRecognizeCompletedEventArgs(RecognitionResult result, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._result = result;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000958C File Offset: 0x0000778C
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x04000342 RID: 834
		private RecognitionResult _result;
	}
}
