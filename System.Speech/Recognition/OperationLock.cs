using System;
using System.Threading;

namespace System.Speech.Recognition
{
	// Token: 0x02000198 RID: 408
	internal class OperationLock : IDisposable
	{
		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002F541 File Offset: 0x0002E541
		public void Dispose()
		{
			this._event.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002F554 File Offset: 0x0002E554
		internal void StartOperation()
		{
			lock (this._thisObjectLock)
			{
				if (this._operationCount == 0U)
				{
					this._event.Reset();
				}
				this._operationCount += 1U;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002F5AC File Offset: 0x0002E5AC
		internal void FinishOperation()
		{
			lock (this._thisObjectLock)
			{
				this._operationCount -= 1U;
				if (this._operationCount == 0U)
				{
					this._event.Set();
				}
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002F604 File Offset: 0x0002E604
		internal void WaitForOperationsToFinish()
		{
			this._event.WaitOne();
		}

		// Token: 0x04000955 RID: 2389
		private ManualResetEvent _event = new ManualResetEvent(true);

		// Token: 0x04000956 RID: 2390
		private uint _operationCount;

		// Token: 0x04000957 RID: 2391
		private object _thisObjectLock = new object();
	}
}
