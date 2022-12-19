using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Internal.ObjectTokens;
using System.Speech.Recognition;
using System.Threading;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000044 RID: 68
	internal class SapiRecognizer : IDisposable
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00008734 File Offset: 0x00007734
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
			if (!this.IsSapi53 && Thread.CurrentThread.GetApartmentState() == null)
			{
				Marshal.ReleaseComObject(spRecognizer);
				this._proxy = new SapiProxy.MTAThread(type);
				return;
			}
			this._proxy = new SapiProxy.PassThrough(spRecognizer);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000087C0 File Offset: 0x000077C0
		public void Dispose()
		{
			if (!this._disposed)
			{
				this._proxy.Dispose();
				this._disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008814 File Offset: 0x00007814
		internal void SetPropertyNum(string name, int value)
		{
			this._proxy.Invoke2(delegate
			{
				SapiRecognizer.SetProperty(this._proxy.Recognizer, name, value);
			});
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000887C File Offset: 0x0000787C
		internal int GetPropertyNum(string name)
		{
			return (int)this._proxy.Invoke(() => SapiRecognizer.GetProperty(this._proxy.Recognizer, name, true));
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000088E4 File Offset: 0x000078E4
		internal void SetPropertyString(string name, string value)
		{
			this._proxy.Invoke2(delegate
			{
				SapiRecognizer.SetProperty(this._proxy.Recognizer, name, value);
			});
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000894C File Offset: 0x0000794C
		internal string GetPropertyString(string name)
		{
			return (string)this._proxy.Invoke(() => SapiRecognizer.GetProperty(this._proxy.Recognizer, name, false));
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000089B0 File Offset: 0x000079B0
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

		// Token: 0x060001B2 RID: 434 RVA: 0x00008AA8 File Offset: 0x00007AA8
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

		// Token: 0x060001B3 RID: 435 RVA: 0x00008B0C File Offset: 0x00007B0C
		internal void SetInput(object input, bool allowFormatChanges)
		{
			this._proxy.Invoke2(delegate
			{
				this._proxy.Recognizer.SetInput(input, allowFormatChanges);
			});
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008B88 File Offset: 0x00007B88
		internal SapiRecoContext CreateRecoContext()
		{
			ISpRecoContext context;
			return (SapiRecoContext)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.CreateRecoContext(out context);
				return new SapiRecoContext(context, this._proxy);
			});
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008BF0 File Offset: 0x00007BF0
		internal SPRECOSTATE GetRecoState()
		{
			SPRECOSTATE state;
			return (SPRECOSTATE)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.GetRecoState(out state);
				return state;
			});
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008C4C File Offset: 0x00007C4C
		internal void SetRecoState(SPRECOSTATE state)
		{
			this._proxy.Invoke2(delegate
			{
				this._proxy.Recognizer.SetRecoState(state);
			});
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008CB4 File Offset: 0x00007CB4
		internal SPRECOGNIZERSTATUS GetStatus()
		{
			SPRECOGNIZERSTATUS status;
			return (SPRECOGNIZERSTATUS)this._proxy.Invoke(delegate
			{
				this._proxy.Recognizer.GetStatus(out status);
				return status;
			});
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008D28 File Offset: 0x00007D28
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

		// Token: 0x060001B9 RID: 441 RVA: 0x00008D98 File Offset: 0x00007D98
		internal SAPIErrorCodes EmulateRecognition(string phrase)
		{
			object displayAttributes = " ";
			return (SAPIErrorCodes)this._proxy.Invoke(() => this._proxy.SapiSpeechRecognizer.EmulateRecognition(phrase, ref displayAttributes, 0));
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008E10 File Offset: 0x00007E10
		internal SAPIErrorCodes EmulateRecognition(ISpPhrase iSpPhrase, uint dwCompareFlags)
		{
			return (SAPIErrorCodes)this._proxy.Invoke(() => this._proxy.Recognizer2.EmulateRecognitionEx(iSpPhrase, dwCompareFlags));
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00008E54 File Offset: 0x00007E54
		internal bool IsSapi53
		{
			get
			{
				return this._isSap53;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008E5C File Offset: 0x00007E5C
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

		// Token: 0x060001BD RID: 445 RVA: 0x00008ECC File Offset: 0x00007ECC
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

		// Token: 0x04000145 RID: 325
		private SapiProxy _proxy;

		// Token: 0x04000146 RID: 326
		private bool _disposed;

		// Token: 0x04000147 RID: 327
		private bool _isSap53;

		// Token: 0x02000045 RID: 69
		internal enum RecognizerType
		{
			// Token: 0x04000149 RID: 329
			InProc,
			// Token: 0x0400014A RID: 330
			Shared
		}
	}
}
