using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000D8 RID: 216
	internal class AudioDeviceOut : AudioBase, IDisposable
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x000155A0 File Offset: 0x000145A0
		internal AudioDeviceOut(int curDevice, IAsyncDispatch asyncDispatch)
		{
			this._delegate = new SafeNativeMethods.WaveOutProc(this.CallBackProc);
			this._asyncDispatch = asyncDispatch;
			this._curDevice = curDevice;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00015600 File Offset: 0x00014600
		~AudioDeviceOut()
		{
			this.Dispose(false);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00015630 File Offset: 0x00014630
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001563F File Offset: 0x0001463F
		private void Dispose(bool disposing)
		{
			if (this._deviceOpen && this._hwo != IntPtr.Zero)
			{
				SafeNativeMethods.waveOutClose(this._hwo);
				this._deviceOpen = false;
			}
			if (disposing)
			{
				this._evt.Dispose();
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001567C File Offset: 0x0001467C
		internal override void Begin(byte[] wfx)
		{
			if (this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			WAVEFORMATEX.AvgBytesPerSec(wfx, out this._nAvgBytesPerSec, out this._blockAlign);
			MMSYSERR mmsyserr;
			lock (this._noWriteOutLock)
			{
				mmsyserr = SafeNativeMethods.waveOutOpen(ref this._hwo, this._curDevice, wfx, this._delegate, IntPtr.Zero, 196608U);
				if (this._fPaused && mmsyserr == MMSYSERR.NOERROR)
				{
					mmsyserr = SafeNativeMethods.waveOutPause(this._hwo);
				}
				this._aborted = false;
				this._deviceOpen = true;
			}
			if (mmsyserr != MMSYSERR.NOERROR)
			{
				throw new AudioException(mmsyserr);
			}
			this._bytesWritten = 0;
			this._evt.Set();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00015734 File Offset: 0x00014734
		internal override void End()
		{
			if (!this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			lock (this._noWriteOutLock)
			{
				this._deviceOpen = false;
				this.CheckForAbort();
				if (this._queueIn.Count != 0)
				{
					SafeNativeMethods.waveOutReset(this._hwo);
				}
				MMSYSERR mmsyserr = SafeNativeMethods.waveOutClose(this._hwo);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000157AC File Offset: 0x000147AC
		internal override void Play(byte[] buffer)
		{
			if (!this._deviceOpen)
			{
				return;
			}
			int num = buffer.Length;
			this._bytesWritten += num;
			WaveHeader waveHeader = new WaveHeader(buffer);
			GCHandle wavehdr = waveHeader.WAVEHDR;
			MMSYSERR mmsyserr = SafeNativeMethods.waveOutPrepareHeader(this._hwo, wavehdr.AddrOfPinnedObject(), waveHeader.SizeHDR);
			if (mmsyserr != MMSYSERR.NOERROR)
			{
				throw new AudioException(mmsyserr);
			}
			lock (this._noWriteOutLock)
			{
				if (!this._aborted)
				{
					lock (this._queueIn)
					{
						AudioDeviceOut.InItem inItem = new AudioDeviceOut.InItem(waveHeader);
						this._queueIn.Add(inItem);
						this._evt.Reset();
					}
					mmsyserr = SafeNativeMethods.waveOutWrite(this._hwo, wavehdr.AddrOfPinnedObject(), waveHeader.SizeHDR);
					if (mmsyserr != MMSYSERR.NOERROR)
					{
						lock (this._queueIn)
						{
							this._queueIn.RemoveAt(this._queueIn.Count - 1);
							throw new AudioException(mmsyserr);
						}
					}
				}
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000158DC File Offset: 0x000148DC
		internal override void Pause()
		{
			lock (this._noWriteOutLock)
			{
				if (!this._aborted && !this._fPaused)
				{
					if (this._deviceOpen)
					{
						MMSYSERR mmsyserr = SafeNativeMethods.waveOutPause(this._hwo);
					}
					this._fPaused = true;
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001593C File Offset: 0x0001493C
		internal override void Resume()
		{
			lock (this._noWriteOutLock)
			{
				if (!this._aborted && this._fPaused && this._deviceOpen)
				{
					MMSYSERR mmsyserr = SafeNativeMethods.waveOutRestart(this._hwo);
				}
			}
			this._fPaused = false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001599C File Offset: 0x0001499C
		internal override void Abort()
		{
			lock (this._noWriteOutLock)
			{
				this._aborted = true;
				if (this._queueIn.Count > 0)
				{
					SafeNativeMethods.waveOutReset(this._hwo);
					this._evt.WaitOne();
				}
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000159FC File Offset: 0x000149FC
		internal override void InjectEvent(TTSEvent ttsEvent)
		{
			if (this._asyncDispatch != null && !this._aborted)
			{
				lock (this._queueIn)
				{
					if (this._queueIn.Count == 0)
					{
						this._asyncDispatch.Post(ttsEvent);
					}
					else
					{
						this._queueIn.Add(new AudioDeviceOut.InItem(ttsEvent));
					}
				}
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00015A6C File Offset: 0x00014A6C
		internal override void WaitUntilDone()
		{
			if (!this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			this._evt.WaitOne();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00015A88 File Offset: 0x00014A88
		internal static int NumDevices()
		{
			return SafeNativeMethods.waveOutGetNumDevs();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00015A90 File Offset: 0x00014A90
		internal static int GetDevicedId(string name)
		{
			for (int i = 0; i < AudioDeviceOut.NumDevices(); i++)
			{
				string text;
				if (AudioDeviceOut.GetDeviceName(i, out text) == MMSYSERR.NOERROR && string.Compare(text, name, 5) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00015AC4 File Offset: 0x00014AC4
		internal static MMSYSERR GetDeviceName(int deviceId, [MarshalAs(21)] out string prodName)
		{
			prodName = string.Empty;
			SafeNativeMethods.WAVEOUTCAPS waveoutcaps = default(SafeNativeMethods.WAVEOUTCAPS);
			MMSYSERR mmsyserr = SafeNativeMethods.waveOutGetDevCaps((IntPtr)deviceId, ref waveoutcaps, Marshal.SizeOf(waveoutcaps));
			if (mmsyserr != MMSYSERR.NOERROR)
			{
				return mmsyserr;
			}
			prodName = waveoutcaps.szPname;
			return MMSYSERR.NOERROR;
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00015B08 File Offset: 0x00014B08
		internal override TimeSpan Duration
		{
			get
			{
				if (this._nAvgBytesPerSec == 0)
				{
					return new TimeSpan(0L);
				}
				return new TimeSpan((long)this._bytesWritten * 10000000L / (long)this._nAvgBytesPerSec);
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00015B38 File Offset: 0x00014B38
		private void CallBackProc(IntPtr hwo, MM_MSG uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
		{
			if (uMsg == MM_MSG.MM_WOM_DONE)
			{
				lock (this._queueIn)
				{
					AudioDeviceOut.InItem inItem = this._queueIn[0];
					inItem.ReleaseData();
					this._queueIn.RemoveAt(0);
					this._queueOut.Add(inItem);
					while (this._queueIn.Count > 0)
					{
						inItem = this._queueIn[0];
						if (inItem._waveHeader != null)
						{
							break;
						}
						if (this._asyncDispatch != null && !this._aborted)
						{
							this._asyncDispatch.Post(inItem._userData);
						}
						this._queueIn.RemoveAt(0);
					}
				}
				if (this._queueIn.Count == 0)
				{
					this._evt.Set();
				}
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00015C0C File Offset: 0x00014C0C
		private void ClearBuffers()
		{
			foreach (AudioDeviceOut.InItem inItem in this._queueOut)
			{
				WaveHeader waveHeader = inItem._waveHeader;
				MMSYSERR mmsyserr = SafeNativeMethods.waveOutUnprepareHeader(this._hwo, waveHeader.WAVEHDR.AddrOfPinnedObject(), waveHeader.SizeHDR);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00015C84 File Offset: 0x00014C84
		private void CheckForAbort()
		{
			if (this._aborted)
			{
				lock (this._queueIn)
				{
					foreach (AudioDeviceOut.InItem inItem in this._queueIn)
					{
						if (inItem._waveHeader != null)
						{
							WaveHeader waveHeader = inItem._waveHeader;
							SafeNativeMethods.waveOutUnprepareHeader(this._hwo, waveHeader.WAVEHDR.AddrOfPinnedObject(), waveHeader.SizeHDR);
						}
						else
						{
							this._asyncDispatch.Post(inItem._userData);
						}
					}
					this._queueIn.Clear();
					this._evt.Set();
				}
			}
			this.ClearBuffers();
		}

		// Token: 0x040003E2 RID: 994
		private List<AudioDeviceOut.InItem> _queueIn = new List<AudioDeviceOut.InItem>();

		// Token: 0x040003E3 RID: 995
		private List<AudioDeviceOut.InItem> _queueOut = new List<AudioDeviceOut.InItem>();

		// Token: 0x040003E4 RID: 996
		private int _blockAlign;

		// Token: 0x040003E5 RID: 997
		private int _bytesWritten;

		// Token: 0x040003E6 RID: 998
		private int _nAvgBytesPerSec;

		// Token: 0x040003E7 RID: 999
		private IntPtr _hwo;

		// Token: 0x040003E8 RID: 1000
		private int _curDevice;

		// Token: 0x040003E9 RID: 1001
		private ManualResetEvent _evt = new ManualResetEvent(false);

		// Token: 0x040003EA RID: 1002
		private SafeNativeMethods.WaveOutProc _delegate;

		// Token: 0x040003EB RID: 1003
		private IAsyncDispatch _asyncDispatch;

		// Token: 0x040003EC RID: 1004
		private bool _deviceOpen;

		// Token: 0x040003ED RID: 1005
		private object _noWriteOutLock = new object();

		// Token: 0x040003EE RID: 1006
		private bool _fPaused;

		// Token: 0x020000D9 RID: 217
		private class InItem : IDisposable
		{
			// Token: 0x060004D3 RID: 1235 RVA: 0x00015D60 File Offset: 0x00014D60
			internal InItem(WaveHeader waveHeader)
			{
				this._waveHeader = waveHeader;
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x00015D6F File Offset: 0x00014D6F
			internal InItem(object userData)
			{
				this._userData = userData;
			}

			// Token: 0x060004D5 RID: 1237 RVA: 0x00015D7E File Offset: 0x00014D7E
			public void Dispose()
			{
				if (this._waveHeader != null)
				{
					this._waveHeader.Dispose();
				}
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x00015D93 File Offset: 0x00014D93
			internal void ReleaseData()
			{
				if (this._waveHeader != null)
				{
					this._waveHeader.ReleaseData();
				}
			}

			// Token: 0x040003EF RID: 1007
			internal WaveHeader _waveHeader;

			// Token: 0x040003F0 RID: 1008
			internal object _userData;
		}
	}
}
