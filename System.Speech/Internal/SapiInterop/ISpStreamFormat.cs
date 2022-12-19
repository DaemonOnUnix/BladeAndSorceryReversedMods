using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000047 RID: 71
	[Guid("BED530BE-2606-4F4D-A1C0-54C5CDA5566F")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpStreamFormat : IStream
	{
		// Token: 0x060001BE RID: 446
		void Read([MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x060001BF RID: 447
		void Write([MarshalAs(42, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x060001C0 RID: 448
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x060001C1 RID: 449
		void SetSize(long libNewSize);

		// Token: 0x060001C2 RID: 450
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x060001C3 RID: 451
		void Commit(int grfCommitFlags);

		// Token: 0x060001C4 RID: 452
		void Revert();

		// Token: 0x060001C5 RID: 453
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060001C6 RID: 454
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060001C7 RID: 455
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x060001C8 RID: 456
		void Clone(out IStream ppstm);

		// Token: 0x060001C9 RID: 457
		void GetFormat(out Guid pguidFormatId, out IntPtr ppCoMemWaveFormatEx);
	}
}
