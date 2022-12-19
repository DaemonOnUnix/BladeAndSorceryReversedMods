using System;
using System.Threading;

namespace System.Speech.Recognition
{
	// Token: 0x02000064 RID: 100
	internal class OperationLock : IDisposable
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x0000D5FF File Offset: 0x0000B7FF
		public void Dispose()
		{
			this._event.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000D614 File Offset: 0x0000B814
		internal void StartOperation()
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
			{
				if (this._operationCount == 0U)
				{
					this._event.Reset();
				}
				this._operationCount += 1U;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000D670 File Offset: 0x0000B870
		internal void FinishOperation()
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
			{
				this._operationCount -= 1U;
				if (this._operationCount == 0U)
				{
					this._event.Set();
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000D6CC File Offset: 0x0000B8CC
		internal void WaitForOperationsToFinish()
		{
			this._event.WaitOne();
		}

		// Token: 0x04000391 RID: 913
		private ManualResetEvent _event = new ManualResetEvent(true);

		// Token: 0x04000392 RID: 914
		private uint _operationCount;

		// Token: 0x04000393 RID: 915
		private object _thisObjectLock = new object();
	}
}
