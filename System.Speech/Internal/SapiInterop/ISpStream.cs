using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000048 RID: 72
	[Guid("BED530BE-2606-4F4D-A1C0-54C5CDA5566F")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpStream : ISpStreamFormat, IStream
	{
		// Token: 0x060001CA RID: 458
		void Read([MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x060001CB RID: 459
		void Write([MarshalAs(42, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x060001CC RID: 460
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x060001CD RID: 461
		void SetSize(long libNewSize);

		// Token: 0x060001CE RID: 462
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x060001CF RID: 463
		void Commit(int grfCommitFlags);

		// Token: 0x060001D0 RID: 464
		void Revert();

		// Token: 0x060001D1 RID: 465
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060001D2 RID: 466
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060001D3 RID: 467
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x060001D4 RID: 468
		void Clone(out IStream ppstm);

		// Token: 0x060001D5 RID: 469
		void GetFormat(out Guid pguidFormatId, out IntPtr ppCoMemWaveFormatEx);

		// Token: 0x060001D6 RID: 470
		void SetBaseStream(IStream pStream, ref Guid rguidFormat, IntPtr pWaveFormatEx);

		// Token: 0x060001D7 RID: 471
		void Slot14();

		// Token: 0x060001D8 RID: 472
		void BindToFile(string pszFileName, SPFILEMODE eMode, ref Guid pFormatId, IntPtr pWaveFormatEx, ulong ullEventInterest);

		// Token: 0x060001D9 RID: 473
		void Close();
	}
}
