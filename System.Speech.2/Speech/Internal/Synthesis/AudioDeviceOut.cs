using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000AB RID: 171
	internal class AudioDeviceOut : AudioBase, IDisposable
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x00016900 File Offset: 0x00014B00
		internal AudioDeviceOut(int curDevice, IAsyncDispatch asyncDispatch)
		{
			this._delegate = new SafeNativeMethods.WaveOutProc(this.CallBackProc);
			this._asyncDispatch = asyncDispatch;
			this._curDevice = curDevice;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00016960 File Offset: 0x00014B60
		~AudioDeviceOut()
		{
			this.Dispose(false);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00016990 File Offset: 0x00014B90
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001699F File Offset: 0x00014B9F
		private void Dispose(bool disposing)
		{
			if (this._deviceOpen && this._hwo != IntPtr.Zero)
			{
				SafeNativeMethods.waveOutClose(this._hwo);
				this._deviceOpen = false;
			}
			if (disposing)
			{
				((IDisposable)this._evt).Dispose();
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000169DC File Offset: 0x00014BDC
		internal override void Begin(byte[] wfx)
		{
			if (this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			WAVEFORMATEX.AvgBytesPerSec(wfx, out this._nAvgBytesPerSec, out this._blockAlign);
			object noWriteOutLock = this._noWriteOutLock;
			MMSYSERR mmsyserr;
			lock (noWriteOutLock)
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

		// Token: 0x060005B2 RID: 1458 RVA: 0x00016A9C File Offset: 0x00014C9C
		internal override void End()
		{
			if (!this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
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

		// Token: 0x060005B3 RID: 1459 RVA: 0x00016B18 File Offset: 0x00014D18
		internal override void Play(byte[] buffer)
		{
			if (this._deviceOpen)
			{
				int num = buffer.Length;
				this._bytesWritten += num;
				WaveHeader waveHeader = new WaveHeader(buffer);
				GCHandle wavehdr = waveHeader.WAVEHDR;
				MMSYSERR mmsyserr = SafeNativeMethods.waveOutPrepareHeader(this._hwo, wavehdr.AddrOfPinnedObject(), waveHeader.SizeHDR);
				if (mmsyserr != MMSYSERR.NOERROR)
				{
					throw new AudioException(mmsyserr);
				}
				object noWriteOutLock = this._noWriteOutLock;
				lock (noWriteOutLock)
				{
					if (!this._aborted)
					{
						List<AudioDeviceOut.InItem> queueIn = this._queueIn;
						lock (queueIn)
						{
							AudioDeviceOut.InItem inItem = new AudioDeviceOut.InItem(waveHeader);
							this._queueIn.Add(inItem);
							this._evt.Reset();
						}
						mmsyserr = SafeNativeMethods.waveOutWrite(this._hwo, wavehdr.AddrOfPinnedObject(), waveHeader.SizeHDR);
						if (mmsyserr != MMSYSERR.NOERROR)
						{
							List<AudioDeviceOut.InItem> queueIn2 = this._queueIn;
							lock (queueIn2)
							{
								this._queueIn.RemoveAt(this._queueIn.Count - 1);
								throw new AudioException(mmsyserr);
							}
						}
					}
				}
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00016C68 File Offset: 0x00014E68
		internal override void Pause()
		{
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
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

		// Token: 0x060005B5 RID: 1461 RVA: 0x00016CD0 File Offset: 0x00014ED0
		internal override void Resume()
		{
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
			{
				if (!this._aborted && this._fPaused && this._deviceOpen)
				{
					MMSYSERR mmsyserr = SafeNativeMethods.waveOutRestart(this._hwo);
				}
			}
			this._fPaused = false;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00016D38 File Offset: 0x00014F38
		internal override void Abort()
		{
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
			{
				this._aborted = true;
				if (this._queueIn.Count > 0)
				{
					SafeNativeMethods.waveOutReset(this._hwo);
					this._evt.WaitOne();
				}
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00016DA0 File Offset: 0x00014FA0
		internal override void InjectEvent(TTSEvent ttsEvent)
		{
			if (this._asyncDispatch != null && !this._aborted)
			{
				List<AudioDeviceOut.InItem> queueIn = this._queueIn;
				lock (queueIn)
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

		// Token: 0x060005B8 RID: 1464 RVA: 0x00016E18 File Offset: 0x00015018
		internal override void WaitUntilDone()
		{
			if (!this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			this._evt.WaitOne();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00016E34 File Offset: 0x00015034
		internal static int NumDevices()
		{
			return SafeNativeMethods.waveOutGetNumDevs();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00016E3C File Offset: 0x0001503C
		internal static int GetDevicedId(string name)
		{
			for (int i = 0; i < AudioDeviceOut.NumDevices(); i++)
			{
				string text;
				if (AudioDeviceOut.GetDeviceName(i, out text) == MMSYSERR.NOERROR && string.Compare(text, name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00016E70 File Offset: 0x00015070
		internal static MMSYSERR GetDeviceName(int deviceId, [MarshalAs(UnmanagedType.LPWStr)] out string prodName)
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

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00016EB3 File Offset: 0x000150B3
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

		// Token: 0x060005BD RID: 1469 RVA: 0x00016EE0 File Offset: 0x000150E0
		private void CallBackProc(IntPtr hwo, MM_MSG uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
		{
			if (uMsg == MM_MSG.MM_WOM_DONE)
			{
				List<AudioDeviceOut.InItem> queueIn = this._queueIn;
				lock (queueIn)
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

		// Token: 0x060005BE RID: 1470 RVA: 0x00016FBC File Offset: 0x000151BC
		private void ClearBuffers()
		{
			foreach (AudioDeviceOut.InItem inItem in this._queueOut)
			{
				WaveHeader waveHeader = inItem._waveHeader;
				MMSYSERR mmsyserr = SafeNativeMethods.waveOutUnprepareHeader(this._hwo, waveHeader.WAVEHDR.AddrOfPinnedObject(), waveHeader.SizeHDR);
				waveHeader.Dispose();
			}
			this._queueOut.Clear();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00017044 File Offset: 0x00015244
		private void CheckForAbort()
		{
			if (this._aborted)
			{
				List<AudioDeviceOut.InItem> queueIn = this._queueIn;
				lock (queueIn)
				{
					foreach (AudioDeviceOut.InItem inItem in this._queueIn)
					{
						if (inItem._waveHeader != null)
						{
							WaveHeader waveHeader = inItem._waveHeader;
							SafeNativeMethods.waveOutUnprepareHeader(this._hwo, waveHeader.WAVEHDR.AddrOfPinnedObject(), waveHeader.SizeHDR);
							waveHeader.Dispose();
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

		// Token: 0x04000477 RID: 1143
		private List<AudioDeviceOut.InItem> _queueIn = new List<AudioDeviceOut.InItem>();

		// Token: 0x04000478 RID: 1144
		private List<AudioDeviceOut.InItem> _queueOut = new List<AudioDeviceOut.InItem>();

		// Token: 0x04000479 RID: 1145
		private int _blockAlign;

		// Token: 0x0400047A RID: 1146
		private int _bytesWritten;

		// Token: 0x0400047B RID: 1147
		private int _nAvgBytesPerSec;

		// Token: 0x0400047C RID: 1148
		private IntPtr _hwo;

		// Token: 0x0400047D RID: 1149
		private int _curDevice;

		// Token: 0x0400047E RID: 1150
		private ManualResetEvent _evt = new ManualResetEvent(false);

		// Token: 0x0400047F RID: 1151
		private SafeNativeMethods.WaveOutProc _delegate;

		// Token: 0x04000480 RID: 1152
		private IAsyncDispatch _asyncDispatch;

		// Token: 0x04000481 RID: 1153
		private bool _deviceOpen;

		// Token: 0x04000482 RID: 1154
		private object _noWriteOutLock = new object();

		// Token: 0x04000483 RID: 1155
		private bool _fPaused;

		// Token: 0x02000193 RID: 403
		private class InItem : IDisposable
		{
			// Token: 0x06000B8F RID: 2959 RVA: 0x0002DC07 File Offset: 0x0002BE07
			internal InItem(WaveHeader waveHeader)
			{
				this._waveHeader = waveHeader;
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x0002DC16 File Offset: 0x0002BE16
			internal InItem(object userData)
			{
				this._userData = userData;
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x0002DC25 File Offset: 0x0002BE25
			public void Dispose()
			{
				if (this._waveHeader != null)
				{
					this._waveHeader.Dispose();
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x0002DC40 File Offset: 0x0002BE40
			internal void ReleaseData()
			{
				if (this._waveHeader != null)
				{
					this._waveHeader.ReleaseData();
				}
			}

			// Token: 0x04000934 RID: 2356
			internal WaveHeader _waveHeader;

			// Token: 0x04000935 RID: 2357
			internal object _userData;
		}
	}
}
