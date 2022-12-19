using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200012A RID: 298
	[Serializable]
	public abstract class RecognitionEventArgs : EventArgs
	{
		// Token: 0x060007E5 RID: 2021 RVA: 0x00022B34 File Offset: 0x00021B34
		internal RecognitionEventArgs(RecognitionResult result)
		{
			this._result = result;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00022B43 File Offset: 0x00021B43
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x0400059E RID: 1438
		private RecognitionResult _result;
	}
}
