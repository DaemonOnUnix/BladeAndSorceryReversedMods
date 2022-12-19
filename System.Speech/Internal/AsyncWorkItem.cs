using System;

namespace System.Speech.Internal
{
	// Token: 0x0200000C RID: 12
	internal class AsyncWorkItem
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002F02 File Offset: 0x00001F02
		internal AsyncWorkItem(Delegate dynamicCallback, params object[] postData)
		{
			this._dynamicCallback = dynamicCallback;
			this._postData = postData;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002F18 File Offset: 0x00001F18
		internal void Invoke()
		{
			if (this._dynamicCallback != null)
			{
				this._dynamicCallback.DynamicInvoke(this._postData);
			}
		}

		// Token: 0x04000071 RID: 113
		private Delegate _dynamicCallback;

		// Token: 0x04000072 RID: 114
		private object[] _postData;
	}
}
