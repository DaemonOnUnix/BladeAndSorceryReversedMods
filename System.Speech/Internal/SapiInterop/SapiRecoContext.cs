using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200008F RID: 143
	internal class SapiRecoContext : IDisposable
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x000094C7 File Offset: 0x000084C7
		internal SapiRecoContext(ISpRecoContext recoContext, SapiProxy proxy)
		{
			this._recoContext = recoContext;
			this._proxy = proxy;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000094EC File Offset: 0x000084EC
		public void Dispose()
		{
			if (!this._disposed)
			{
				this._proxy.Invoke2(delegate
				{
					Marshal.ReleaseComObject(this._recoContext);
				});
				this._disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00009554 File Offset: 0x00008554
		internal void SetInterest(ulong eventInterest, ulong queuedInterest)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetInterest(eventInterest, queuedInterest);
			});
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000095D0 File Offset: 0x000085D0
		internal SapiGrammar CreateGrammar(ulong id)
		{
			ISpRecoGrammar sapiGrammar;
			return (SapiGrammar)this._proxy.Invoke(delegate
			{
				this._recoContext.CreateGrammar(id, out sapiGrammar);
				return new SapiGrammar(sapiGrammar, this._proxy);
			});
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00009630 File Offset: 0x00008630
		internal void SetMaxAlternates(uint count)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetMaxAlternates(count);
			});
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00009694 File Offset: 0x00008694
		internal void SetAudioOptions(SPAUDIOOPTIONS options, IntPtr audioFormatId, IntPtr waveFormatEx)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetAudioOptions(options, audioFormatId, waveFormatEx);
			});
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00009708 File Offset: 0x00008708
		internal void Bookmark(SPBOOKMARKOPTIONS options, ulong position, IntPtr lparam)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.Bookmark(options, position, lparam);
			});
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000975C File Offset: 0x0000875C
		internal void Resume()
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.Resume(0U);
			});
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00009798 File Offset: 0x00008798
		internal void SetContextState(SPCONTEXTSTATE state)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetContextState(state);
			});
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000097F8 File Offset: 0x000087F8
		internal EventNotify CreateEventNotify(AsyncSerializedWorker asyncWorker, bool supportsSapi53)
		{
			return (EventNotify)this._proxy.Invoke(() => new EventNotify(this._recoContext, asyncWorker, supportsSapi53));
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009854 File Offset: 0x00008854
		internal void DisposeEventNotify(EventNotify eventNotify)
		{
			this._proxy.Invoke2(delegate
			{
				eventNotify.Dispose();
			});
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000098AC File Offset: 0x000088AC
		internal void SetGrammarOptions(SPGRAMMAROPTIONS options)
		{
			this._proxy.Invoke2(delegate
			{
				((ISpRecoContext2)this._recoContext).SetGrammarOptions(options);
			});
		}

		// Token: 0x040002AC RID: 684
		private ISpRecoContext _recoContext;

		// Token: 0x040002AD RID: 685
		private SapiProxy _proxy;

		// Token: 0x040002AE RID: 686
		private bool _disposed;
	}
}
