using System;

namespace System.Speech.Internal
{
	// Token: 0x0200008E RID: 142
	internal class AsyncWorkItem
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x00012DE6 File Offset: 0x00010FE6
		internal AsyncWorkItem(Delegate dynamicCallback, params object[] postData)
		{
			this._dynamicCallback = dynamicCallback;
			this._postData = postData;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00012DFC File Offset: 0x00010FFC
		internal void Invoke()
		{
			if (this._dynamicCallback != null)
			{
				this._dynamicCallback.DynamicInvoke(this._postData);
			}
		}

		// Token: 0x04000429 RID: 1065
		private Delegate _dynamicCallback;

		// Token: 0x0400042A RID: 1066
		private object[] _postData;
	}
}
