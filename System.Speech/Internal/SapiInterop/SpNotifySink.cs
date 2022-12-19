using System;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002A RID: 42
	internal class SpNotifySink : ISpNotifySink
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00007DF2 File Offset: 0x00006DF2
		public SpNotifySink(EventNotify eventNotify)
		{
			this._eventNotifyReference = new WeakReference(eventNotify);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007E08 File Offset: 0x00006E08
		void ISpNotifySink.Notify()
		{
			EventNotify eventNotify = (EventNotify)this._eventNotifyReference.Target;
			if (eventNotify != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(eventNotify.SendNotification));
			}
		}

		// Token: 0x040000D8 RID: 216
		private WeakReference _eventNotifyReference;
	}
}
