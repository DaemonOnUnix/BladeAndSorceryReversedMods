using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000D2 RID: 210
	[TypeLibType(16)]
	internal struct WAVEFORMATEX
	{
		// Token: 0x060004A4 RID: 1188 RVA: 0x000140E4 File Offset: 0x000130E4
		internal static WAVEFORMATEX ToWaveHeader(byte[] waveHeader)
		{
			GCHandle gchandle = GCHandle.Alloc(waveHeader, 3);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			WAVEFORMATEX waveformatex = default(WAVEFORMATEX);
			waveformatex.wFormatTag = Marshal.ReadInt16(intPtr);
			waveformatex.nChannels = Marshal.ReadInt16(intPtr, 2);
			waveformatex.nSamplesPerSec = Marshal.ReadInt32(intPtr, 4);
			waveformatex.nAvgBytesPerSec = Marshal.ReadInt32(intPtr, 8);
			waveformatex.nBlockAlign = Marshal.ReadInt16(intPtr, 12);
			waveformatex.wBitsPerSample = Marshal.ReadInt16(intPtr, 14);
			waveformatex.cbSize = Marshal.ReadInt16(intPtr, 16);
			if (waveformatex.cbSize != 0)
			{
				throw new InvalidOperationException();
			}
			gchandle.Free();
			return waveformatex;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00014184 File Offset: 0x00013184
		internal static void AvgBytesPerSec(byte[] waveHeader, out int avgBytesPerSec, out int nBlockAlign)
		{
			GCHandle gchandle = GCHandle.Alloc(waveHeader, 3);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			avgBytesPerSec = Marshal.ReadInt32(intPtr, 8);
			nBlockAlign = (int)Marshal.ReadInt16(intPtr, 12);
			gchandle.Free();
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000141BC File Offset: 0x000131BC
		internal byte[] ToBytes()
		{
			GCHandle gchandle = GCHandle.Alloc(this, 3);
			byte[] array = WAVEFORMATEX.ToBytes(gchandle.AddrOfPinnedObject());
			gchandle.Free();
			return array;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000141F0 File Offset: 0x000131F0
		internal static byte[] ToBytes(IntPtr waveHeader)
		{
			int num = (int)Marshal.ReadInt16(waveHeader, 16);
			byte[] array = new byte[18 + num];
			Marshal.Copy(waveHeader, array, 0, 18 + num);
			return array;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00014220 File Offset: 0x00013220
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00014277 File Offset: 0x00013277
		internal int Length
		{
			get
			{
				return (int)(18 + this.cbSize);
			}
		}

		// Token: 0x040003CB RID: 971
		internal short wFormatTag;

		// Token: 0x040003CC RID: 972
		internal short nChannels;

		// Token: 0x040003CD RID: 973
		internal int nSamplesPerSec;

		// Token: 0x040003CE RID: 974
		internal int nAvgBytesPerSec;

		// Token: 0x040003CF RID: 975
		internal short nBlockAlign;

		// Token: 0x040003D0 RID: 976
		internal short wBitsPerSample;

		// Token: 0x040003D1 RID: 977
		internal short cbSize;
	}
}
