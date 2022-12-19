using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200011B RID: 283
	internal abstract class SapiProxy : IDisposable
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x0002AF0C File Offset: 0x0002910C
		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009A7 RID: 2471
		internal abstract object Invoke(SapiProxy.ObjectDelegate pfn);

		// Token: 0x060009A8 RID: 2472
		internal abstract void Invoke2(SapiProxy.VoidDelegate pfn);

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0002AF14 File Offset: 0x00029114
		internal ISpRecognizer Recognizer
		{
			get
			{
				return this._recognizer;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0002AF1C File Offset: 0x0002911C
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0002AF3D File Offset: 0x0002913D
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

		// Token: 0x040006B7 RID: 1719
		protected ISpeechRecognizer _speechRecognizer;

		// Token: 0x040006B8 RID: 1720
		protected ISpRecognizer2 _recognizer2;

		// Token: 0x040006B9 RID: 1721
		protected ISpRecognizer _recognizer;

		// Token: 0x020001B3 RID: 435
		internal class PassThrough : SapiProxy, IDisposable
		{
			// Token: 0x06000BC9 RID: 3017 RVA: 0x0002E2BA File Offset: 0x0002C4BA
			internal PassThrough(ISpRecognizer recognizer)
			{
				this._recognizer = recognizer;
			}

			// Token: 0x06000BCA RID: 3018 RVA: 0x0002E2CC File Offset: 0x0002C4CC
			~PassThrough()
			{
				this.Dispose(false);
			}

			// Token: 0x06000BCB RID: 3019 RVA: 0x0002E2FC File Offset: 0x0002C4FC
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

			// Token: 0x06000BCC RID: 3020 RVA: 0x0002E32C File Offset: 0x0002C52C
			internal override object Invoke(SapiProxy.ObjectDelegate pfn)
			{
				return pfn();
			}

			// Token: 0x06000BCD RID: 3021 RVA: 0x0002E334 File Offset: 0x0002C534
			internal override void Invoke2(SapiProxy.VoidDelegate pfn)
			{
				pfn();
			}

			// Token: 0x06000BCE RID: 3022 RVA: 0x0002E33C File Offset: 0x0002C53C
			private void Dispose(bool disposing)
			{
				this._recognizer2 = null;
				this._speechRecognizer = null;
				Marshal.ReleaseComObject(this._recognizer);
			}
		}

		// Token: 0x020001B4 RID: 436
		internal class MTAThread : SapiProxy, IDisposable
		{
			// Token: 0x06000BCF RID: 3023 RVA: 0x0002E358 File Offset: 0x0002C558
			internal MTAThread(SapiRecognizer.RecognizerType type)
			{
				this._mta = new Thread(new ThreadStart(this.SapiMTAThread));
				if (!this._mta.TrySetApartmentState(ApartmentState.MTA))
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

			// Token: 0x06000BD0 RID: 3024 RVA: 0x0002E3F0 File Offset: 0x0002C5F0
			~MTAThread()
			{
				this.Dispose(false);
			}

			// Token: 0x06000BD1 RID: 3025 RVA: 0x0002E420 File Offset: 0x0002C620
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

			// Token: 0x06000BD2 RID: 3026 RVA: 0x0002E450 File Offset: 0x0002C650
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

			// Token: 0x06000BD3 RID: 3027 RVA: 0x0002E4BC File Offset: 0x0002C6BC
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

			// Token: 0x06000BD4 RID: 3028 RVA: 0x0002E520 File Offset: 0x0002C720
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
					((IDisposable)this._process).Dispose();
					((IDisposable)this._done).Dispose();
					this._mta.Abort();
				}
				base.Dispose();
			}

			// Token: 0x06000BD5 RID: 3029 RVA: 0x0002E59C File Offset: 0x0002C79C
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

			// Token: 0x040009BE RID: 2494
			private Thread _mta;

			// Token: 0x040009BF RID: 2495
			private AutoResetEvent _process = new AutoResetEvent(false);

			// Token: 0x040009C0 RID: 2496
			private AutoResetEvent _done = new AutoResetEvent(false);

			// Token: 0x040009C1 RID: 2497
			private SapiProxy.ObjectDelegate _doit;

			// Token: 0x040009C2 RID: 2498
			private SapiProxy.VoidDelegate _doit2;

			// Token: 0x040009C3 RID: 2499
			private object _result;

			// Token: 0x040009C4 RID: 2500
			private Exception _exception;
		}

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06000BDA RID: 3034
		internal delegate object ObjectDelegate();

		// Token: 0x020001B6 RID: 438
		// (Invoke) Token: 0x06000BDE RID: 3038
		internal delegate void VoidDelegate();
	}
}
