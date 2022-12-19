using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000166 RID: 358
	internal class SapiRecoContext : IDisposable
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002B997 File Offset: 0x00029B97
		internal SapiRecoContext(ISpRecoContext recoContext, SapiProxy proxy)
		{
			this._recoContext = recoContext;
			this._proxy = proxy;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002B9AD File Offset: 0x00029BAD
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

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002B9DC File Offset: 0x00029BDC
		internal void SetInterest(ulong eventInterest, ulong queuedInterest)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetInterest(eventInterest, queuedInterest);
			});
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002BA1C File Offset: 0x00029C1C
		internal SapiGrammar CreateGrammar(ulong id)
		{
			ISpRecoGrammar sapiGrammar;
			return (SapiGrammar)this._proxy.Invoke(delegate
			{
				this._recoContext.CreateGrammar(id, out sapiGrammar);
				return new SapiGrammar(sapiGrammar, this._proxy);
			});
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002BA5C File Offset: 0x00029C5C
		internal void SetMaxAlternates(uint count)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetMaxAlternates(count);
			});
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002BA94 File Offset: 0x00029C94
		internal void SetAudioOptions(SPAUDIOOPTIONS options, IntPtr audioFormatId, IntPtr waveFormatEx)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetAudioOptions(options, audioFormatId, waveFormatEx);
			});
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002BADC File Offset: 0x00029CDC
		internal void Bookmark(SPBOOKMARKOPTIONS options, ulong position, IntPtr lparam)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.Bookmark(options, position, lparam);
			});
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002BB22 File Offset: 0x00029D22
		internal void Resume()
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.Resume(0U);
			});
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002BB3C File Offset: 0x00029D3C
		internal void SetContextState(SPCONTEXTSTATE state)
		{
			this._proxy.Invoke2(delegate
			{
				this._recoContext.SetContextState(state);
			});
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002BB74 File Offset: 0x00029D74
		internal EventNotify CreateEventNotify(AsyncSerializedWorker asyncWorker, bool supportsSapi53)
		{
			return (EventNotify)this._proxy.Invoke(() => new EventNotify(this._recoContext, asyncWorker, supportsSapi53));
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002BBB8 File Offset: 0x00029DB8
		internal void DisposeEventNotify(EventNotify eventNotify)
		{
			this._proxy.Invoke2(delegate
			{
				eventNotify.Dispose();
			});
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002BBEC File Offset: 0x00029DEC
		internal void SetGrammarOptions(SPGRAMMAROPTIONS options)
		{
			this._proxy.Invoke2(delegate
			{
				((ISpRecoContext2)this._recoContext).SetGrammarOptions(options);
			});
		}

		// Token: 0x0400081E RID: 2078
		private ISpRecoContext _recoContext;

		// Token: 0x0400081F RID: 2079
		private SapiProxy _proxy;

		// Token: 0x04000820 RID: 2080
		private bool _disposed;
	}
}
