using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200011E RID: 286
	[Guid("BED530BE-2606-4F4D-A1C0-54C5CDA5566F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpStreamFormat : IStream
	{
		// Token: 0x060009C0 RID: 2496
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x060009C1 RID: 2497
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x060009C2 RID: 2498
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x060009C3 RID: 2499
		void SetSize(long libNewSize);

		// Token: 0x060009C4 RID: 2500
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x060009C5 RID: 2501
		void Commit(int grfCommitFlags);

		// Token: 0x060009C6 RID: 2502
		void Revert();

		// Token: 0x060009C7 RID: 2503
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060009C8 RID: 2504
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060009C9 RID: 2505
		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

		// Token: 0x060009CA RID: 2506
		void Clone(out IStream ppstm);

		// Token: 0x060009CB RID: 2507
		void GetFormat(out Guid pguidFormatId, out IntPtr ppCoMemWaveFormatEx);
	}
}
