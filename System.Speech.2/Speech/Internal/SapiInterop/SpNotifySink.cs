using System;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000104 RID: 260
	internal class SpNotifySink : ISpNotifySink
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x0002AAD9 File Offset: 0x00028CD9
		public SpNotifySink(EventNotify eventNotify)
		{
			this._eventNotifyReference = new WeakReference(eventNotify);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002AAF0 File Offset: 0x00028CF0
		void ISpNotifySink.Notify()
		{
			EventNotify eventNotify = (EventNotify)this._eventNotifyReference.Target;
			if (eventNotify != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(eventNotify.SendNotification));
			}
		}

		// Token: 0x04000654 RID: 1620
		private WeakReference _eventNotifyReference;
	}
}
