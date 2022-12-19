using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000A8 RID: 168
	[TypeLibType(16)]
	internal struct WAVEFORMATEX
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001626C File Offset: 0x0001446C
		internal static WAVEFORMATEX ToWaveHeader(byte[] waveHeader)
		{
			GCHandle gchandle = GCHandle.Alloc(waveHeader, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			WAVEFORMATEX waveformatex = new WAVEFORMATEX
			{
				wFormatTag = Marshal.ReadInt16(intPtr),
				nChannels = Marshal.ReadInt16(intPtr, 2),
				nSamplesPerSec = Marshal.ReadInt32(intPtr, 4),
				nAvgBytesPerSec = Marshal.ReadInt32(intPtr, 8),
				nBlockAlign = Marshal.ReadInt16(intPtr, 12),
				wBitsPerSample = Marshal.ReadInt16(intPtr, 14),
				cbSize = Marshal.ReadInt16(intPtr, 16)
			};
			if (waveformatex.cbSize != 0)
			{
				throw new InvalidOperationException();
			}
			gchandle.Free();
			return waveformatex;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001630C File Offset: 0x0001450C
		internal static void AvgBytesPerSec(byte[] waveHeader, out int avgBytesPerSec, out int nBlockAlign)
		{
			GCHandle gchandle = GCHandle.Alloc(waveHeader, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			avgBytesPerSec = Marshal.ReadInt32(intPtr, 8);
			nBlockAlign = (int)Marshal.ReadInt16(intPtr, 12);
			gchandle.Free();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00016344 File Offset: 0x00014544
		internal byte[] ToBytes()
		{
			GCHandle gchandle = GCHandle.Alloc(this, GCHandleType.Pinned);
			byte[] array = WAVEFORMATEX.ToBytes(gchandle.AddrOfPinnedObject());
			gchandle.Free();
			return array;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00016378 File Offset: 0x00014578
		internal static byte[] ToBytes(IntPtr waveHeader)
		{
			int num = (int)Marshal.ReadInt16(waveHeader, 16);
			byte[] array = new byte[18 + num];
			Marshal.Copy(waveHeader, array, 0, 18 + num);
			return array;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x000163A8 File Offset: 0x000145A8
		internal static WAVEFORMATEX Default
		{
			get
			{
				return new WAVEFORMATEX
				{
					wFormatTag = 1,
					nChannels = 1,
					nSamplesPerSec = 22050,
					nAvgBytesPerSec = 44100,
					nBlockAlign = 2,
					wBitsPerSample = 16,
					cbSize = 0
				};
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x000163FF File Offset: 0x000145FF
		internal int Length
		{
			get
			{
				return (int)(18 + this.cbSize);
			}
		}

		// Token: 0x04000464 RID: 1124
		internal short wFormatTag;

		// Token: 0x04000465 RID: 1125
		internal short nChannels;

		// Token: 0x04000466 RID: 1126
		internal int nSamplesPerSec;

		// Token: 0x04000467 RID: 1127
		internal int nAvgBytesPerSec;

		// Token: 0x04000468 RID: 1128
		internal short nBlockAlign;

		// Token: 0x04000469 RID: 1129
		internal short wBitsPerSample;

		// Token: 0x0400046A RID: 1130
		internal short cbSize;
	}
}
