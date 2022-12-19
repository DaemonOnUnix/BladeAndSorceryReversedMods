using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public abstract class RecognitionEventArgs : EventArgs
	{
		// Token: 0x06000185 RID: 389 RVA: 0x000067E4 File Offset: 0x000049E4
		internal RecognitionEventArgs(RecognitionResult result)
		{
			this._result = result;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000067F3 File Offset: 0x000049F3
		public RecognitionResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x040002F4 RID: 756
		private RecognitionResult _result;
	}
}
