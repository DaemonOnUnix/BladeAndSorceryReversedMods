using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000090 RID: 144
	internal class SpStreamWrapper : IStream, IDisposable
	{
		// Token: 0x060002CE RID: 718 RVA: 0x000098E4 File Offset: 0x000088E4
		internal SpStreamWrapper(Stream stream)
		{
			this._stream = stream;
			this._endOfStreamPosition = stream.Length;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009907 File Offset: 0x00008907
		public void Dispose()
		{
			this._stream.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000991C File Offset: 0x0000891C
		public void Read(byte[] pv, int cb, IntPtr pcbRead)
		{
			if (this._endOfStreamPosition >= 0L && this._stream.Position + (long)cb > this._endOfStreamPosition)
			{
				cb = (int)(this._endOfStreamPosition - this._stream.Position);
			}
			int num = 0;
			try
			{
				num = this._stream.Read(pv, 0, cb);
			}
			catch (EndOfStreamException)
			{
				num = 0;
			}
			if (pcbRead != IntPtr.Zero)
			{
				Marshal.WriteIntPtr(pcbRead, new IntPtr(num));
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000099A0 File Offset: 0x000089A0
		public void Write(byte[] pv, int cb, IntPtr pcbWritten)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000099A7 File Offset: 0x000089A7
		public void Seek(long offset, int seekOrigin, IntPtr plibNewPosition)
		{
			this._stream.Seek(offset, seekOrigin);
			if (plibNewPosition != IntPtr.Zero)
			{
				Marshal.WriteIntPtr(plibNewPosition, new IntPtr(this._stream.Position));
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000099DA File Offset: 0x000089DA
		public void SetSize(long libNewSize)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000099E1 File Offset: 0x000089E1
		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000099E8 File Offset: 0x000089E8
		public void Commit(int grfCommitFlags)
		{
			this._stream.Flush();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000099F5 File Offset: 0x000089F5
		public void Revert()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000099FC File Offset: 0x000089FC
		public void LockRegion(long libOffset, long cb, int dwLockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00009A03 File Offset: 0x00008A03
		public void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00009A0A File Offset: 0x00008A0A
		public void Stat(out STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = default(STATSTG);
			pstatstg.cbSize = this._stream.Length;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00009A24 File Offset: 0x00008A24
		public void Clone(out IStream ppstm)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040002AF RID: 687
		private Stream _stream;

		// Token: 0x040002B0 RID: 688
		protected long _endOfStreamPosition = -1L;
	}
}
