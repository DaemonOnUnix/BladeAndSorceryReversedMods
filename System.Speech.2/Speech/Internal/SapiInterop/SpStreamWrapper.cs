using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200016A RID: 362
	internal class SpStreamWrapper : IStream, IDisposable
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x0002C11B File Offset: 0x0002A31B
		internal SpStreamWrapper(Stream stream)
		{
			this._stream = stream;
			this._endOfStreamPosition = stream.Length;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002C13E File Offset: 0x0002A33E
		public void Dispose()
		{
			this._stream.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002C154 File Offset: 0x0002A354
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

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void Write(byte[] pv, int cb, IntPtr pcbWritten)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002C1DF File Offset: 0x0002A3DF
		public void Seek(long offset, int seekOrigin, IntPtr plibNewPosition)
		{
			this._stream.Seek(offset, (SeekOrigin)seekOrigin);
			if (plibNewPosition != IntPtr.Zero)
			{
				Marshal.WriteIntPtr(plibNewPosition, new IntPtr(this._stream.Position));
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void SetSize(long libNewSize)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002C212 File Offset: 0x0002A412
		public void Commit(int grfCommitFlags)
		{
			this._stream.Flush();
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void Revert()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void LockRegion(long libOffset, long cb, int dwLockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002C21F File Offset: 0x0002A41F
		public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = default(System.Runtime.InteropServices.ComTypes.STATSTG);
			pstatstg.cbSize = this._stream.Length;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public void Clone(out IStream ppstm)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000834 RID: 2100
		private Stream _stream;

		// Token: 0x04000835 RID: 2101
		protected long _endOfStreamPosition = -1L;
	}
}
