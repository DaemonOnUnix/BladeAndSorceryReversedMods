using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000109 RID: 265
	internal sealed class WaveHeader : IDisposable
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x0001E5D0 File Offset: 0x0001D5D0
		internal WaveHeader(byte[] buffer)
		{
			this._dwBufferLength = buffer.Length;
			this._gcHandle = GCHandle.Alloc(buffer, 3);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001E620 File Offset: 0x0001D620
		~WaveHeader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001E650 File Offset: 0x0001D650
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001E65F File Offset: 0x0001D65F
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

		// Token: 0x06000689 RID: 1673 RVA: 0x0001E682 File Offset: 0x0001D682
		internal void ReleaseData()
		{
			if (this._gcHandle.IsAllocated)
			{
				this._gcHandle.Free();
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001E69C File Offset: 0x0001D69C
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
					this._gcHandleWaveHdr = GCHandle.Alloc(this._waveHdr, 3);
				}
				return this._gcHandleWaveHdr;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001E73A File Offset: 0x0001D73A
		internal int SizeHDR
		{
			get
			{
				return Marshal.SizeOf(this._waveHdr);
			}
		}

		// Token: 0x04000511 RID: 1297
		internal const int WHDR_DONE = 1;

		// Token: 0x04000512 RID: 1298
		internal const int WHDR_PREPARED = 2;

		// Token: 0x04000513 RID: 1299
		internal const int WHDR_BEGINLOOP = 4;

		// Token: 0x04000514 RID: 1300
		internal const int WHDR_ENDLOOP = 8;

		// Token: 0x04000515 RID: 1301
		internal const int WHDR_INQUEUE = 16;

		// Token: 0x04000516 RID: 1302
		internal const int WAVE_FORMAT_PCM = 1;

		// Token: 0x04000517 RID: 1303
		private GCHandle _gcHandle = default(GCHandle);

		// Token: 0x04000518 RID: 1304
		private GCHandle _gcHandleWaveHdr = default(GCHandle);

		// Token: 0x04000519 RID: 1305
		private WAVEHDR _waveHdr = default(WAVEHDR);

		// Token: 0x0400051A RID: 1306
		internal int _dwBufferLength;
	}
}
