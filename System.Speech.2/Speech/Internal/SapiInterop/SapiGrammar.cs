using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010D RID: 269
	internal class SapiGrammar : IDisposable
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0002AC5D File Offset: 0x00028E5D
		internal SapiGrammar(ISpRecoGrammar sapiGrammar, SapiProxy thread)
		{
			this._sapiGrammar = sapiGrammar;
			this._sapiProxy = thread;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002AC73 File Offset: 0x00028E73
		public void Dispose()
		{
			if (!this._disposed)
			{
				Marshal.ReleaseComObject(this._sapiGrammar);
				GC.SuppressFinalize(this);
				this._disposed = true;
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002AC98 File Offset: 0x00028E98
		internal void SetGrammarState(SPGRAMMARSTATE state)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.SetGrammarState(state);
			});
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002ACD0 File Offset: 0x00028ED0
		internal void SetWordSequenceData(string text, SPTEXTSELECTIONINFO info)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.SetWordSequenceData(text, (uint)text.Length, ref info);
			});
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002AD10 File Offset: 0x00028F10
		internal void LoadCmdFromMemory(IntPtr grammar, SPLOADOPTIONS options)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.LoadCmdFromMemory(grammar, options);
			});
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002AD50 File Offset: 0x00028F50
		internal void LoadDictation(string pszTopicName, SPLOADOPTIONS options)
		{
			this._sapiProxy.Invoke2(delegate
			{
				this._sapiGrammar.LoadDictation(pszTopicName, options);
			});
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002AD90 File Offset: 0x00028F90
		internal SAPIErrorCodes SetDictationState(SPRULESTATE state)
		{
			return (SAPIErrorCodes)this._sapiProxy.Invoke(() => this._sapiGrammar.SetDictationState(state));
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002ADD0 File Offset: 0x00028FD0
		internal SAPIErrorCodes SetRuleState(string name, SPRULESTATE state)
		{
			return (SAPIErrorCodes)this._sapiProxy.Invoke(() => this._sapiGrammar.SetRuleState(name, IntPtr.Zero, state));
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002AE14 File Offset: 0x00029014
		internal void SetGrammarLoader(ISpGrammarResourceLoader resourceLoader)
		{
			this.SpRecoGrammar2.SetGrammarLoader(resourceLoader);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002AE22 File Offset: 0x00029022
		internal void LoadCmdFromMemory2(IntPtr grammar, SPLOADOPTIONS options, string sharingUri, string baseUri)
		{
			this.SpRecoGrammar2.LoadCmdFromMemory2(grammar, options, sharingUri, baseUri);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002AE34 File Offset: 0x00029034
		internal void SetRulePriority(string name, uint id, int priority)
		{
			this.SpRecoGrammar2.SetRulePriority(name, id, priority);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002AE44 File Offset: 0x00029044
		internal void SetRuleWeight(string name, uint id, float weight)
		{
			this.SpRecoGrammar2.SetRuleWeight(name, id, weight);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002AE54 File Offset: 0x00029054
		internal void SetDictationWeight(float weight)
		{
			this.SpRecoGrammar2.SetDictationWeight(weight);
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0002AE62 File Offset: 0x00029062
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

		// Token: 0x0400068F RID: 1679
		private ISpRecoGrammar2 _sapiGrammar2;

		// Token: 0x04000690 RID: 1680
		private ISpRecoGrammar _sapiGrammar;

		// Token: 0x04000691 RID: 1681
		private SapiProxy _sapiProxy;

		// Token: 0x04000692 RID: 1682
		private bool _disposed;
	}
}
