using System;
using System.ComponentModel;

namespace System.Speech.Recognition
{
	// Token: 0x0200018D RID: 397
	public class EmulateRecognizeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x0002AEA9 File Offset: 0x00029EA9
		internal EmulateRecognizeCompletedEventArgs(RecognitionResult result, Exception error, bool cancelled, object userState)
			: base(error, cancelled, userState)
		{
			this._result = result;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002AEBC File Offset: 0x00029EBC
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x04000901 RID: 2305
		private RecognitionResult _result;
	}
}
