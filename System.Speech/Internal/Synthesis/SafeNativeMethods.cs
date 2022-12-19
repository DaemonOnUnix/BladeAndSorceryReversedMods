using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000EC RID: 236
	internal static class SafeNativeMethods
	{
		// Token: 0x06000569 RID: 1385
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutPrepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600056A RID: 1386
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutWrite(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600056B RID: 1387
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutUnprepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

		// Token: 0x0600056C RID: 1388
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutOpen(ref IntPtr phwo, int uDeviceID, byte[] pwfx, SafeNativeMethods.WaveOutProc dwCallback, IntPtr dwInstance, uint fdwOpen);

		// Token: 0x0600056D RID: 1389
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutClose(IntPtr hwo);

		// Token: 0x0600056E RID: 1390
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutReset(IntPtr hwo);

		// Token: 0x0600056F RID: 1391
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutPause(IntPtr hwo);

		// Token: 0x06000570 RID: 1392
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutRestart(IntPtr hwo);

		// Token: 0x06000571 RID: 1393
		[DllImport("winmm.dll")]
		internal static extern MMSYSERR waveOutGetDevCaps(IntPtr uDeviceID, ref SafeNativeMethods.WAVEOUTCAPS caps, int cbwoc);

		// Token: 0x06000572 RID: 1394
		[DllImport("winmm.dll")]
		internal static extern int waveOutGetNumDevs();

		// Token: 0x0400043A RID: 1082
		internal const uint TIME_MS = 1U;

		// Token: 0x0400043B RID: 1083
		internal const uint TIME_SAMPLES = 2U;

		// Token: 0x0400043C RID: 1084
		internal const uint TIME_BYTES = 4U;

		// Token: 0x0400043D RID: 1085
		internal const uint TIME_TICKS = 32U;

		// Token: 0x0400043E RID: 1086
		internal const uint CALLBACK_WINDOW = 65536U;

		// Token: 0x0400043F RID: 1087
		internal const uint CALLBACK_NULL = 0U;

		// Token: 0x04000440 RID: 1088
		internal const uint CALLBACK_FUNCTION = 196608U;

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x06000574 RID: 1396
		internal delegate void WaveOutProc(IntPtr hwo, MM_MSG uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

		// Token: 0x020000EE RID: 238
		internal struct WAVEOUTCAPS
		{
			// Token: 0x04000441 RID: 1089
			private ushort wMid;

			// Token: 0x04000442 RID: 1090
			private ushort wPid;

			// Token: 0x04000443 RID: 1091
			private uint vDriverVersion;

			// Token: 0x04000444 RID: 1092
			[MarshalAs(23, SizeConst = 32)]
			internal string szPname;

			// Token: 0x04000445 RID: 1093
			private uint dwFormats;

			// Token: 0x04000446 RID: 1094
			private ushort wChannels;

			// Token: 0x04000447 RID: 1095
			private ushort wReserved1;

			// Token: 0x04000448 RID: 1096
			private ushort dwSupport;
		}
	}
}
