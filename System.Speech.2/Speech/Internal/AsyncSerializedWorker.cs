using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;

namespace System.Speech.Internal
{
	// Token: 0x0200008D RID: 141
	internal class AsyncSerializedWorker : IAsyncDispatch
	{
		// Token: 0x0600049C RID: 1180 RVA: 0x000128A9 File Offset: 0x00010AA9
		internal AsyncSerializedWorker(WaitCallback defaultCallback, AsyncOperation asyncOperation)
		{
			this._asyncOperation = asyncOperation;
			this._workerPostCallback = new SendOrPostCallback(this.WorkerProc);
			this.Initialize(defaultCallback);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000128D1 File Offset: 0x00010AD1
		private void Initialize(WaitCallback defaultCallback)
		{
			this._queue = new Queue();
			this._hasPendingPost = false;
			this._workerCallback = new WaitCallback(this.WorkerProc);
			this._defaultCallback = defaultCallback;
			this._isAsyncMode = true;
			this._isEnabled = true;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001290C File Offset: 0x00010B0C
		public void Post(object evt)
		{
			this.AddItem(new AsyncWorkItem(this.DefaultCallback, new object[] { evt }));
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001292C File Offset: 0x00010B2C
		public void Post(object[] evt)
		{
			object syncRoot = this._queue.SyncRoot;
			lock (syncRoot)
			{
				if (this.Enabled)
				{
					for (int i = 0; i < evt.Length; i++)
					{
						this.AddItem(new AsyncWorkItem(this.DefaultCallback, new object[] { evt[i] }));
					}
				}
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000129A0 File Offset: 0x00010BA0
		public void PostOperation(Delegate callback, params object[] parameters)
		{
			this.AddItem(new AsyncWorkItem(callback, parameters));
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000129B0 File Offset: 0x00010BB0
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x000129F8 File Offset: 0x00010BF8
		internal bool Enabled
		{
			get
			{
				object syncRoot = this._queue.SyncRoot;
				bool isEnabled;
				lock (syncRoot)
				{
					isEnabled = this._isEnabled;
				}
				return isEnabled;
			}
			set
			{
				object syncRoot = this._queue.SyncRoot;
				lock (syncRoot)
				{
					this._isEnabled = value;
				}
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00012A40 File Offset: 0x00010C40
		internal void Purge()
		{
			object syncRoot = this._queue.SyncRoot;
			lock (syncRoot)
			{
				this._queue.Clear();
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00012A8C File Offset: 0x00010C8C
		internal WaitCallback DefaultCallback
		{
			get
			{
				object syncRoot = this._queue.SyncRoot;
				WaitCallback defaultCallback;
				lock (syncRoot)
				{
					defaultCallback = this._defaultCallback;
				}
				return defaultCallback;
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00012AD4 File Offset: 0x00010CD4
		internal AsyncWorkItem NextWorkItem()
		{
			object syncRoot = this._queue.SyncRoot;
			AsyncWorkItem asyncWorkItem;
			lock (syncRoot)
			{
				if (this._queue.Count == 0)
				{
					asyncWorkItem = null;
				}
				else
				{
					AsyncWorkItem asyncWorkItem2 = (AsyncWorkItem)this._queue.Dequeue();
					if (this._queue.Count == 0)
					{
						this._hasPendingPost = false;
					}
					asyncWorkItem = asyncWorkItem2;
				}
			}
			return asyncWorkItem;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00012B4C File Offset: 0x00010D4C
		internal void ConsumeQueue()
		{
			AsyncWorkItem asyncWorkItem;
			while ((asyncWorkItem = this.NextWorkItem()) != null)
			{
				asyncWorkItem.Invoke();
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00012BD8 File Offset: 0x00010DD8
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x00012B6C File Offset: 0x00010D6C
		internal bool AsyncMode
		{
			get
			{
				object syncRoot = this._queue.SyncRoot;
				bool isAsyncMode;
				lock (syncRoot)
				{
					isAsyncMode = this._isAsyncMode;
				}
				return isAsyncMode;
			}
			set
			{
				bool flag = false;
				object syncRoot = this._queue.SyncRoot;
				lock (syncRoot)
				{
					if (this._isAsyncMode != value)
					{
						this._isAsyncMode = value;
						if (this._queue.Count > 0)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.OnWorkItemPending();
				}
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060004A9 RID: 1193 RVA: 0x00012C20 File Offset: 0x00010E20
		// (remove) Token: 0x060004AA RID: 1194 RVA: 0x00012C58 File Offset: 0x00010E58
		internal event WaitCallback WorkItemPending;

		// Token: 0x060004AB RID: 1195 RVA: 0x00012C90 File Offset: 0x00010E90
		private void AddItem(AsyncWorkItem item)
		{
			bool flag = true;
			object syncRoot = this._queue.SyncRoot;
			lock (syncRoot)
			{
				if (this.Enabled)
				{
					this._queue.Enqueue(item);
					if (!this._hasPendingPost || !this._isAsyncMode)
					{
						flag = false;
						this._hasPendingPost = true;
					}
				}
			}
			if (!flag)
			{
				this.OnWorkItemPending();
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00012D08 File Offset: 0x00010F08
		private void WorkerProc(object ignored)
		{
			for (;;)
			{
				object syncRoot = this._queue.SyncRoot;
				AsyncWorkItem asyncWorkItem;
				lock (syncRoot)
				{
					if (this._queue.Count <= 0 || !this._isAsyncMode)
					{
						if (this._queue.Count == 0)
						{
							this._hasPendingPost = false;
						}
						break;
					}
					asyncWorkItem = (AsyncWorkItem)this._queue.Dequeue();
				}
				asyncWorkItem.Invoke();
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00012D8C File Offset: 0x00010F8C
		private void OnWorkItemPending()
		{
			if (this._hasPendingPost)
			{
				if (this.AsyncMode)
				{
					if (this._asyncOperation == null)
					{
						ThreadPool.QueueUserWorkItem(this._workerCallback, null);
						return;
					}
					this._asyncOperation.Post(this._workerPostCallback, null);
					return;
				}
				else if (this.WorkItemPending != null)
				{
					this.WorkItemPending(null);
				}
			}
		}

		// Token: 0x04000421 RID: 1057
		private AsyncOperation _asyncOperation;

		// Token: 0x04000422 RID: 1058
		private SendOrPostCallback _workerPostCallback;

		// Token: 0x04000423 RID: 1059
		private Queue _queue;

		// Token: 0x04000424 RID: 1060
		private bool _hasPendingPost;

		// Token: 0x04000425 RID: 1061
		private bool _isAsyncMode;

		// Token: 0x04000426 RID: 1062
		private WaitCallback _workerCallback;

		// Token: 0x04000427 RID: 1063
		private WaitCallback _defaultCallback;

		// Token: 0x04000428 RID: 1064
		private bool _isEnabled;
	}
}
