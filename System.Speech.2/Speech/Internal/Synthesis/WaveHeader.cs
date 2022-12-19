using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000CE RID: 206
	internal sealed class WaveHeader : IDisposable
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x0001F46B File Offset: 0x0001D66B
		internal WaveHeader(byte[] buffer)
		{
			this._dwBufferLength = buffer.Length;
			this._gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001F48C File Offset: 0x0001D68C
		~WaveHeader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001F4BC File Offset: 0x0001D6BC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001F4CB File Offset: 0x0001D6CB
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ReleaseData();
				if (this._gcHandleWaveHdr.IsAllocated)
				{
					this._gcHandleWaveHdr.Free();
				}
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001F4EE File Offset: 0x0001D6EE
		internal void ReleaseData()
		{
			if (this._gcHandle.IsAllocated)
			{
				this._gcHandle.Free();
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001F508 File Offset: 0x0001D708
		internal GCHandle WAVEHDR
		{
			get
			{
				if (!this._gcHandleWaveHdr.IsAllocated)
				{
					this._waveHdr.lpData = this._gcHandle.AddrOfPinnedObject();
					this._waveHdr.dwBufferLength = (uint)this._dwBufferLength;
					this._waveHdr.dwBytesRecorded = 0U;
					this._waveHdr.dwUser = 0U;
					this._waveHdr.dwFlags = 0U;
					this._waveHdr.dwLoops = 0U;
					this._waveHdr.lpNext = IntPtr.Zero;
					this._gcHandleWaveHdr = GCHandle.Alloc(this._waveHdr, GCHandleType.Pinned);
				}
				return this._gcHandleWaveHdr;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001F5A6 File Offset: 0x0001D7A6
		internal int SizeHDR
		{
			get
			{
				return Marshal.SizeOf(this._waveHdr);
			}
		}

		// Token: 0x0400057F RID: 1407
		internal const int WHDR_DONE = 1;

		// Token: 0x04000580 RID: 1408
		internal const int WHDR_PREPARED = 2;

		// Token: 0x04000581 RID: 1409
		internal const int WHDR_BEGINLOOP = 4;

		// Token: 0x04000582 RID: 1410
		internal const int WHDR_ENDLOOP = 8;

		// Token: 0x04000583 RID: 1411
		internal const int WHDR_INQUEUE = 16;

		// Token: 0x04000584 RID: 1412
		internal const int WAVE_FORMAT_PCM = 1;

		// Token: 0x04000585 RID: 1413
		private GCHandle _gcHandle;

		// Token: 0x04000586 RID: 1414
		private GCHandle _gcHandleWaveHdr;

		// Token: 0x04000587 RID: 1415
		private WAVEHDR _waveHdr;

		// Token: 0x04000588 RID: 1416
		internal int _dwBufferLength;
	}
}
