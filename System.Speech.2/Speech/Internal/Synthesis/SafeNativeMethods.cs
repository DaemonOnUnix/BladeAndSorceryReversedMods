using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000BA RID: 186
	internal static class SafeNativeMethods
	{
		// Token: 0x06000649 RID: 1609
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutPrepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600064A RID: 1610
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutWrite(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600064B RID: 1611
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutUnprepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600064C RID: 1612
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutOpen(ref IntPtr phwo, int uDeviceID, byte[] pwfx, SafeNativeMethods.WaveOutProc dwCallback, IntPtr dwInstance, uint fdwOpen);

		// Token: 0x0600064D RID: 1613
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutClose(IntPtr hwo);

		// Token: 0x0600064E RID: 1614
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutReset(IntPtr hwo);

		// Token: 0x0600064F RID: 1615
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutPause(IntPtr hwo);

		// Token: 0x06000650 RID: 1616
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutRestart(IntPtr hwo);

		// Token: 0x06000651 RID: 1617
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutGetDevCaps(IntPtr uDeviceID, ref SafeNativeMethods.WAVEOUTCAPS caps, int cbwoc);

		// Token: 0x06000652 RID: 1618
		[DllImport("winmm.dll")]
		internal static extern int waveOutGetNumDevs();

		// Token: 0x040004C1 RID: 1217
		internal const uint TIME_MS = 1U;

		// Token: 0x040004C2 RID: 1218
		internal const uint TIME_SAMPLES = 2U;

		// Token: 0x040004C3 RID: 1219
		internal const uint TIME_BYTES = 4U;

		// Token: 0x040004C4 RID: 1220
		internal const uint TIME_TICKS = 32U;

		// Token: 0x040004C5 RID: 1221
		internal const uint CALLBACK_WINDOW = 65536U;

		// Token: 0x040004C6 RID: 1222
		internal const uint CALLBACK_NULL = 0U;

		// Token: 0x040004C7 RID: 1223
		internal const uint CALLBACK_FUNCTION = 196608U;

		// Token: 0x02000197 RID: 407
		// (Invoke) Token: 0x06000B94 RID: 2964
		internal delegate void WaveOutProc(IntPtr hwo, MM_MSG uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

		// Token: 0x02000198 RID: 408
		internal struct WAVEOUTCAPS
		{
			// Token: 0x04000942 RID: 2370
			private ushort wMid;

			// Token: 0x04000943 RID: 2371
			private ushort wPid;

			// Token: 0x04000944 RID: 2372
			private uint vDriverVersion;

			// Token: 0x04000945 RID: 2373
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			internal string szPname;

			// Token: 0x04000946 RID: 2374
			private uint dwFormats;

			// Token: 0x04000947 RID: 2375
			private ushort wChannels;

			// Token: 0x04000948 RID: 2376
			private ushort wReserved1;

			// Token: 0x04000949 RID: 2377
			private ushort dwSupport;
		}
	}
}
