using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Internal.ObjectTokens;
using System.Speech.Recognition;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200011C RID: 284
	internal class SapiRecognizer : IDisposable
	{
		// Token: 0x060009AD RID: 2477 RVA: 0x0002AF60 File Offset: 0x00029160
		internal SapiRecognizer(SapiRecognizer.RecognizerType type)
		{
			ISpRecognizer spRecognizer;
			try
			{
				if (type == SapiRecognizer.RecognizerType.InProc)
				{
					spRecognizer = (ISpRecognizer)new SpInprocRecognizer();
				}
				else
				{
					spRecognizer = (ISpRecognizer)new SpSharedRecognizer();
				}
				this._isSap53 = spRecognizer is ISpRecognizer2;
			}
			catch (COMException ex)
			{
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
			}
			if (!this.IsSapi53 && Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
			{
				Marshal.ReleaseComObject(spRecognizer);
				this._proxy = new SapiProxy.MTAThread(type);
				return;
			}
			this._proxy = new SapiProxy.PassThrough(spRecognizer);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002AFEC File Offset: 0x000291EC
		public void Dispose()
		{
			if (!this._disposed)
			{
				this._proxy.Dispose();
				this._disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002B010 File Offset: 0x00029210
		internal void SetPropertyNum(string name, int value)
		{
			this._proxy.Invoke2(delegate
			{
				SapiRecognizer.SetProperty(this._proxy.Recognizer, name, value);
			});
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0002B050 File Offset: 0x00029250
		internal int GetPropertyNum(string name)
		{
			return (int)this._proxy.Invoke(() => SapiRecognizer.GetProperty(this._proxy.Recognizer, name, true));
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002B090 File Offset: 0x00029290
		internal void SetPropertyString(string name, string value)
		{
			this._proxy.Invoke2(delegate
			{
				SapiRecognizer.SetProperty(this._proxy.Recognizer, name, value);
			});
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002B0D0 File Offset: 0x000292D0
		internal string GetPropertyString(string name)
		{
			return (string)this._proxy.Invoke(() => SapiRecognizer.GetProperty(this._proxy.Recognizer, name, false));
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002B110 File Offset: 0x00029310
		internal void SetRecognizer(ISpObjectToken recognizer)
		{
			try
			{
				this._proxy.Invoke2(delegate
				{
					this._proxy.Recognizer.SetRecognizer(recognizer);
				});
			}
			catch (InvalidCastException)
			{
				throw new PlatformNotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002B170 File Offset: 0x00029370
		internal RecognizerInfo GetRecognizerInfo()
		{
			ISpObjectToken sapiObjectToken;
			return (RecognizerInfo)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.GetRecognizer(out sapiObjectToken);
				RecognizerInfo recognizerInfo;
				try
				{
					IntPtr intPtr;
					sapiObjectToken.GetId(out intPtr);
					string text = Marshal.PtrToStringUni(intPtr);
					recognizerInfo = RecognizerInfo.Create(ObjectToken.Open(null, text, false));
					if (recognizerInfo == null)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerNotFound, new object[0]));
					}
					Marshal.FreeCoTaskMem(intPtr);
				}
				finally
				{
					Marshal.ReleaseComObject(sapiObjectToken);
				}
				return recognizerInfo;
			});
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002B1A8 File Offset: 0x000293A8
		internal void SetInput(object input, bool allowFormatChanges)
		{
			this._proxy.Invoke2(delegate
			{
				this._proxy.Recognizer.SetInput(input, allowFormatChanges);
			});
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002B1E8 File Offset: 0x000293E8
		internal SapiRecoContext CreateRecoContext()
		{
			ISpRecoContext context;
			return (SapiRecoContext)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.CreateRecoContext(out context);
				return new SapiRecoContext(context, this._proxy);
			});
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002B220 File Offset: 0x00029420
		internal SPRECOSTATE GetRecoState()
		{
			SPRECOSTATE state;
			return (SPRECOSTATE)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.GetRecoState(out state);
				return state;
			});
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002B258 File Offset: 0x00029458
		internal void SetRecoState(SPRECOSTATE state)
		{
			this._proxy.Invoke2(delegate
			{
				this._proxy.Recognizer.SetRecoState(state);
			});
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002B290 File Offset: 0x00029490
		internal SPRECOGNIZERSTATUS GetStatus()
		{
			SPRECOGNIZERSTATUS status;
			return (SPRECOGNIZERSTATUS)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.GetStatus(out status);
				return status;
			});
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002B2C8 File Offset: 0x000294C8
		internal IntPtr GetFormat(SPSTREAMFORMATTYPE WaveFormatType)
		{
			return (IntPtr)this._proxy.Invoke(delegate
			{
				Guid guid;
				IntPtr intPtr;
				this._proxy.Recognizer.GetFormat(WaveFormatType, out guid, out intPtr);
				return intPtr;
			});
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002B308 File Offset: 0x00029508
		internal SAPIErrorCodes EmulateRecognition(string phrase)
		{
			object displayAttributes = " ";
			return (SAPIErrorCodes)this._proxy.Invoke(() => this._proxy.SapiSpeechRecognizer.EmulateRecognition(phrase, ref displayAttributes, 0));
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002B350 File Offset: 0x00029550
		internal SAPIErrorCodes EmulateRecognition(ISpPhrase iSpPhrase, uint dwCompareFlags)
		{
			return (SAPIErrorCodes)this._proxy.Invoke(() => this._proxy.Recognizer2.EmulateRecognitionEx(iSpPhrase, dwCompareFlags));
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002B394 File Offset: 0x00029594
		internal bool IsSapi53
		{
			get
			{
				return this._isSap53;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002B39C File Offset: 0x0002959C
		private static void SetProperty(ISpRecognizer sapiRecognizer, string name, object value)
		{
			SAPIErrorCodes sapierrorCodes;
			if (value is int)
			{
				sapierrorCodes = (SAPIErrorCodes)sapiRecognizer.SetPropertyNum(name, (int)value);
			}
			else
			{
				sapierrorCodes = (SAPIErrorCodes)sapiRecognizer.SetPropertyString(name, (string)value);
			}
			if (sapierrorCodes == SAPIErrorCodes.S_FALSE)
			{
				throw new KeyNotFoundException(SR.Get(SRID.RecognizerSettingNotSupported, new object[0]));
			}
			if (sapierrorCodes < SAPIErrorCodes.S_OK)
			{
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(new COMException(SR.Get(SRID.RecognizerSettingUpdateError, new object[0]), (int)sapierrorCodes));
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002B40C File Offset: 0x0002960C
		private static object GetProperty(ISpRecognizer sapiRecognizer, string name, bool integer)
		{
			SAPIErrorCodes sapierrorCodes;
			object obj;
			if (integer)
			{
				int num;
				sapierrorCodes = (SAPIErrorCodes)sapiRecognizer.GetPropertyNum(name, out num);
				obj = num;
			}
			else
			{
				string text;
				sapierrorCodes = (SAPIErrorCodes)sapiRecognizer.GetPropertyString(name, out text);
				obj = text;
			}
			if (sapierrorCodes == SAPIErrorCodes.S_FALSE)
			{
				throw new KeyNotFoundException(SR.Get(SRID.RecognizerSettingNotSupported, new object[0]));
			}
			if (sapierrorCodes < SAPIErrorCodes.S_OK)
			{
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(new COMException(SR.Get(SRID.RecognizerSettingUpdateError, new object[0]), (int)sapierrorCodes));
			}
			return obj;
		}

		// Token: 0x040006BA RID: 1722
		private SapiProxy _proxy;

		// Token: 0x040006BB RID: 1723
		private bool _disposed;

		// Token: 0x040006BC RID: 1724
		private bool _isSap53;

		// Token: 0x020001B7 RID: 439
		internal enum RecognizerType
		{
			// Token: 0x040009C6 RID: 2502
			InProc,
			// Token: 0x040009C7 RID: 2503
			Shared
		}
	}
}
