using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200003F RID: 63
	internal abstract class SapiProxy : IDisposable
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000834B File Offset: 0x0000734B
		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600018D RID: 397
		internal abstract object Invoke(SapiProxy.ObjectDelegate pfn);

		// Token: 0x0600018E RID: 398
		internal abstract void Invoke2(SapiProxy.VoidDelegate pfn);

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008353 File Offset: 0x00007353
		internal ISpRecognizer Recognizer
		{
			get
			{
				return this._recognizer;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000835B File Offset: 0x0000735B
		internal ISpRecognizer2 Recognizer2
		{
			get
			{
				if (this._recognizer2 == null)
				{
					this._recognizer2 = (ISpRecognizer2)this._recognizer;
				}
				return this._recognizer2;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000837C File Offset: 0x0000737C
		internal ISpeechRecognizer SapiSpeechRecognizer
		{
			get
			{
				if (this._speechRecognizer == null)
				{
					this._speechRecognizer = (ISpeechRecognizer)this._recognizer;
				}
				return this._speechRecognizer;
			}
		}

		// Token: 0x0400013B RID: 315
		protected ISpeechRecognizer _speechRecognizer;

		// Token: 0x0400013C RID: 316
		protected ISpRecognizer2 _recognizer2;

		// Token: 0x0400013D RID: 317
		protected ISpRecognizer _recognizer;

		// Token: 0x02000040 RID: 64
		internal class PassThrough : SapiProxy, IDisposable
		{
			// Token: 0x06000193 RID: 403 RVA: 0x000083A5 File Offset: 0x000073A5
			internal PassThrough(ISpRecognizer recognizer)
			{
				this._recognizer = recognizer;
			}

			// Token: 0x06000194 RID: 404 RVA: 0x000083B4 File Offset: 0x000073B4
			~PassThrough()
			{
				this.Dispose(false);
			}

			// Token: 0x06000195 RID: 405 RVA: 0x000083E4 File Offset: 0x000073E4
			public override void Dispose()
			{
				try
				{
					this.Dispose(true);
				}
				finally
				{
					base.Dispose();
				}
			}

			// Token: 0x06000196 RID: 406 RVA: 0x00008414 File Offset: 0x00007414
			internal override object Invoke(SapiProxy.ObjectDelegate pfn)
			{
				return pfn();
			}

			// Token: 0x06000197 RID: 407 RVA: 0x0000841C File Offset: 0x0000741C
			internal override void Invoke2(SapiProxy.VoidDelegate pfn)
			{
				pfn();
			}

			// Token: 0x06000198 RID: 408 RVA: 0x00008424 File Offset: 0x00007424
			private void Dispose(bool disposing)
			{
				this._recognizer2 = null;
				this._speechRecognizer = null;
				Marshal.ReleaseComObject(this._recognizer);
			}
		}

		// Token: 0x02000041 RID: 65
		internal class MTAThread : SapiProxy, IDisposable
		{
			// Token: 0x06000199 RID: 409 RVA: 0x00008464 File Offset: 0x00007464
			internal MTAThread(SapiRecognizer.RecognizerType type)
			{
				this._mta = new Thread(new ThreadStart(this.SapiMTAThread));
				if (!this._mta.TrySetApartmentState(1))
				{
					throw new InvalidOperationException();
				}
				this._mta.IsBackground = true;
				this._mta.Start();
				if (type == SapiRecognizer.RecognizerType.InProc)
				{
					this.Invoke2(delegate
					{
						this._recognizer = (ISpRecognizer)new SpInprocRecognizer();
					});
					return;
				}
				this.Invoke2(delegate
				{
					this._recognizer = (ISpRecognizer)new SpSharedRecognizer();
				});
			}

			// Token: 0x0600019A RID: 410 RVA: 0x00008508 File Offset: 0x00007508
			~MTAThread()
			{
				this.Dispose(false);
			}

			// Token: 0x0600019B RID: 411 RVA: 0x00008538 File Offset: 0x00007538
			public override void Dispose()
			{
				try
				{
					this.Dispose(true);
				}
				finally
				{
					base.Dispose();
				}
			}

			// Token: 0x0600019C RID: 412 RVA: 0x00008568 File Offset: 0x00007568
			internal override object Invoke(SapiProxy.ObjectDelegate pfn)
			{
				object result;
				lock (this)
				{
					this._doit = pfn;
					this._process.Set();
					this._done.WaitOne();
					if (this._exception != null)
					{
						throw this._exception;
					}
					result = this._result;
				}
				return result;
			}

			// Token: 0x0600019D RID: 413 RVA: 0x000085CC File Offset: 0x000075CC
			internal override void Invoke2(SapiProxy.VoidDelegate pfn)
			{
				lock (this)
				{
					this._doit2 = pfn;
					this._process.Set();
					this._done.WaitOne();
					if (this._exception != null)
					{
						throw this._exception;
					}
				}
			}

			// Token: 0x0600019E RID: 414 RVA: 0x00008638 File Offset: 0x00007638
			private void Dispose(bool disposing)
			{
				lock (this)
				{
					this._recognizer2 = null;
					this._speechRecognizer = null;
					this.Invoke2(delegate
					{
						Marshal.ReleaseComObject(this._recognizer);
					});
					this._process.Dispose();
					this._done.Dispose();
					this._mta.Abort();
				}
				base.Dispose();
			}

			// Token: 0x0600019F RID: 415 RVA: 0x000086B4 File Offset: 0x000076B4
			private void SapiMTAThread()
			{
				for (;;)
				{
					try
					{
						this._process.WaitOne();
						this._exception = null;
						if (this._doit != null)
						{
							this._result = this._doit();
							this._doit = null;
						}
						else
						{
							this._doit2();
							this._doit2 = null;
						}
					}
					catch (Exception ex)
					{
						this._exception = ex;
					}
					this._done.Set();
				}
			}

			// Token: 0x0400013E RID: 318
			private Thread _mta;

			// Token: 0x0400013F RID: 319
			private AutoResetEvent _process = new AutoResetEvent(false);

			// Token: 0x04000140 RID: 320
			private AutoResetEvent _done = new AutoResetEvent(false);

			// Token: 0x04000141 RID: 321
			private SapiProxy.ObjectDelegate _doit;

			// Token: 0x04000142 RID: 322
			private SapiProxy.VoidDelegate _doit2;

			// Token: 0x04000143 RID: 323
			private object _result;

			// Token: 0x04000144 RID: 324
			private Exception _exception;
		}

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x060001A4 RID: 420
		internal delegate object ObjectDelegate();

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x060001A8 RID: 424
		internal delegate void VoidDelegate();
	}
}
