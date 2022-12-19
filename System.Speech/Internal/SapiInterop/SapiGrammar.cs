using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000032 RID: 50
	internal class SapiGrammar : IDisposable
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00007F61 File Offset: 0x00006F61
		internal SapiGrammar(ISpRecoGrammar sapiGrammar, SapiProxy thread)
		{
			this._sapiGrammar = sapiGrammar;
			this._sapiProxy = thread;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007F77 File Offset: 0x00006F77
		public void Dispose()
		{
			if (!this._disposed)
			{
				Marshal.ReleaseComObject(this._sapiGrammar);
				GC.SuppressFinalize(this);
				this._disposed = true;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007FBC File Offset: 0x00006FBC
		internal void SetGrammarState(SPGRAMMARSTATE state)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.SetGrammarState(state);
			});
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008028 File Offset: 0x00007028
		internal void SetWordSequenceData(string text, SPTEXTSELECTIONINFO info)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.SetWordSequenceData(text, (uint)text.Length, ref info);
			});
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008090 File Offset: 0x00007090
		internal void LoadCmdFromMemory(IntPtr grammar, SPLOADOPTIONS options)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.LoadCmdFromMemory(grammar, options);
			});
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000080F8 File Offset: 0x000070F8
		internal void LoadDictation(string pszTopicName, SPLOADOPTIONS options)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.LoadDictation(pszTopicName, options);
			});
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000815C File Offset: 0x0000715C
		internal SAPIErrorCodes SetDictationState(SPRULESTATE state)
		{
			return (SAPIErrorCodes)this._sapiProxy.Invoke(() => this._sapiGrammar.SetDictationState(state));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000081CC File Offset: 0x000071CC
		internal SAPIErrorCodes SetRuleState(string name, SPRULESTATE state)
		{
			return (SAPIErrorCodes)this._sapiProxy.Invoke(() => this._sapiGrammar.SetRuleState(name, IntPtr.Zero, state));
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00008210 File Offset: 0x00007210
		internal void SetGrammarLoader(ISpGrammarResourceLoader resourceLoader)
		{
			this.SpRecoGrammar2.SetGrammarLoader(resourceLoader);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000821E File Offset: 0x0000721E
		internal void LoadCmdFromMemory2(IntPtr grammar, SPLOADOPTIONS options, string sharingUri, string baseUri)
		{
			this.SpRecoGrammar2.LoadCmdFromMemory2(grammar, options, sharingUri, baseUri);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008230 File Offset: 0x00007230
		internal void SetRulePriority(string name, uint id, int priority)
		{
			this.SpRecoGrammar2.SetRulePriority(name, id, priority);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008240 File Offset: 0x00007240
		internal void SetRuleWeight(string name, uint id, float weight)
		{
			this.SpRecoGrammar2.SetRuleWeight(name, id, weight);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008250 File Offset: 0x00007250
		internal void SetDictationWeight(float weight)
		{
			this.SpRecoGrammar2.SetDictationWeight(weight);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000825E File Offset: 0x0000725E
		internal ISpRecoGrammar2 SpRecoGrammar2
		{
			get
			{
				if (this._sapiGrammar2 == null)
				{
					this._sapiGrammar2 = (ISpRecoGrammar2)this._sapiGrammar;
				}
				return this._sapiGrammar2;
			}
		}

		// Token: 0x04000113 RID: 275
		private ISpRecoGrammar2 _sapiGrammar2;

		// Token: 0x04000114 RID: 276
		private ISpRecoGrammar _sapiGrammar;

		// Token: 0x04000115 RID: 277
		private SapiProxy _sapiProxy;

		// Token: 0x04000116 RID: 278
		private bool _disposed;
	}
}
