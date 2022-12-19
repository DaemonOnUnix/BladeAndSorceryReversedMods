using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;

namespace System.Speech.Internal
{
	// Token: 0x0200000B RID: 11
	internal class AsyncSerializedWorker : IAsyncDispatch
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002A36 File Offset: 0x00001A36
		internal AsyncSerializedWorker(WaitCallback defaultCallback, AsyncOperation asyncOperation)
		{
			this._asyncOperation = asyncOperation;
			this._workerPostCallback = new SendOrPostCallback(this.WorkerProc);
			this.Initialize(defaultCallback);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A5E File Offset: 0x00001A5E
		private void Initialize(WaitCallback defaultCallback)
		{
			this._queue = new Queue();
			this._hasPendingPost = false;
			this._workerCallback = new WaitCallback(this.WorkerProc);
			this._defaultCallback = defaultCallback;
			this._isAsyncMode = true;
			this._isEnabled = true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A9C File Offset: 0x00001A9C
		public void Post(object evt)
		{
			this.AddItem(new AsyncWorkItem(this.DefaultCallback, new object[] { evt }));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002AC8 File Offset: 0x00001AC8
		public void Post(object[] evt)
		{
			lock (this._queue.SyncRoot)
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

		// Token: 0x06000019 RID: 25 RVA: 0x00002B38 File Offset: 0x00001B38
		public void PostOperation(Delegate callback, params object[] parameters)
		{
			this.AddItem(new AsyncWorkItem(callback, parameters));
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002B48 File Offset: 0x00001B48
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002B88 File Offset: 0x00001B88
		internal bool Enabled
		{
			get
			{
				bool isEnabled;
				lock (this._queue.SyncRoot)
				{
					isEnabled = this._isEnabled;
				}
				return isEnabled;
			}
			set
			{
				lock (this._queue.SyncRoot)
				{
					this._isEnabled = value;
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BC8 File Offset: 0x00001BC8
		internal void Purge()
		{
			lock (this._queue.SyncRoot)
			{
				this._queue.Clear();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002C0C File Offset: 0x00001C0C
		internal WaitCallback DefaultCallback
		{
			get
			{
				WaitCallback defaultCallback;
				lock (this._queue.SyncRoot)
				{
					defaultCallback = this._defaultCallback;
				}
				return defaultCallback;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002C4C File Offset: 0x00001C4C
		internal AsyncWorkItem NextWorkItem()
		{
			AsyncWorkItem asyncWorkItem;
			lock (this._queue.SyncRoot)
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

		// Token: 0x0600001F RID: 31 RVA: 0x00002CC0 File Offset: 0x00001CC0
		internal void ConsumeQueue()
		{
			AsyncWorkItem asyncWorkItem;
			while ((asyncWorkItem = this.NextWorkItem()) != null)
			{
				asyncWorkItem.Invoke();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002D44 File Offset: 0x00001D44
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002CE0 File Offset: 0x00001CE0
		internal bool AsyncMode
		{
			get
			{
				bool isAsyncMode;
				lock (this._queue.SyncRoot)
				{
					isAsyncMode = this._isAsyncMode;
				}
				return isAsyncMode;
			}
			set
			{
				bool flag = false;
				lock (this._queue.SyncRoot)
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000022 RID: 34 RVA: 0x00002D84 File Offset: 0x00001D84
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x00002D9D File Offset: 0x00001D9D
		internal event WaitCallback WorkItemPending;

		// Token: 0x06000024 RID: 36 RVA: 0x00002DB8 File Offset: 0x00001DB8
		private void AddItem(AsyncWorkItem item)
		{
			bool flag = true;
			lock (this._queue.SyncRoot)
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

		// Token: 0x06000025 RID: 37 RVA: 0x00002E28 File Offset: 0x00001E28
		private void WorkerProc(object ignored)
		{
			for (;;)
			{
				AsyncWorkItem asyncWorkItem;
				lock (this._queue.SyncRoot)
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

		// Token: 0x06000026 RID: 38 RVA: 0x00002EA8 File Offset: 0x00001EA8
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
					this.WorkItemPending.Invoke(null);
				}
			}
		}

		// Token: 0x04000069 RID: 105
		private AsyncOperation _asyncOperation;

		// Token: 0x0400006A RID: 106
		private SendOrPostCallback _workerPostCallback;

		// Token: 0x0400006B RID: 107
		private Queue _queue;

		// Token: 0x0400006C RID: 108
		private bool _hasPendingPost;

		// Token: 0x0400006D RID: 109
		private bool _isAsyncMode;

		// Token: 0x0400006E RID: 110
		private WaitCallback _workerCallback;

		// Token: 0x0400006F RID: 111
		private WaitCallback _defaultCallback;

		// Token: 0x04000070 RID: 112
		private bool _isEnabled;
	}
}
