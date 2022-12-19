using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200011F RID: 287
	[Guid("BED530BE-2606-4F4D-A1C0-54C5CDA5566F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpStream : ISpStreamFormat, IStream
	{
		// Token: 0x060009CC RID: 2508
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x060009CD RID: 2509
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x060009CE RID: 2510
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x060009CF RID: 2511
		void SetSize(long libNewSize);

		// Token: 0x060009D0 RID: 2512
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x060009D1 RID: 2513
		void Commit(int grfCommitFlags);

		// Token: 0x060009D2 RID: 2514
		void Revert();

		// Token: 0x060009D3 RID: 2515
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060009D4 RID: 2516
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060009D5 RID: 2517
		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

		// Token: 0x060009D6 RID: 2518
		void Clone(out IStream ppstm);

		// Token: 0x060009D7 RID: 2519
		void GetFormat(out Guid pguidFormatId, out IntPtr ppCoMemWaveFormatEx);

		// Token: 0x060009D8 RID: 2520
		void SetBaseStream(IStream pStream, ref Guid rguidFormat, IntPtr pWaveFormatEx);

		// Token: 0x060009D9 RID: 2521
		void Slot14();

		// Token: 0x060009DA RID: 2522
		void BindToFile(string pszFileName, SPFILEMODE eMode, ref Guid pFormatId, IntPtr pWaveFormatEx, ulong ullEventInterest);

		// Token: 0x060009DB RID: 2523
		void Close();
	}
}
