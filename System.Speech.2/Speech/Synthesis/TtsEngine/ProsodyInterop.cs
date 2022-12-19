using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000045 RID: 69
	internal struct ProsodyInterop
	{
		// Token: 0x06000136 RID: 310 RVA: 0x000052C8 File Offset: 0x000034C8
		internal static IntPtr ProsodyToPtr(Prosody prosody, Collection<IntPtr> memoryBlocks)
		{
			if (prosody == null)
			{
				return IntPtr.Zero;
			}
			ProsodyInterop prosodyInterop = default(ProsodyInterop);
			prosodyInterop._pitch = prosody.Pitch;
			prosodyInterop._range = prosody.Range;
			prosodyInterop._rate = prosody.Rate;
			prosodyInterop._duration = prosody.Duration;
			prosodyInterop._volume = prosody.Volume;
			ContourPoint[] contourPoints = prosody.GetContourPoints();
			if (contourPoints != null)
			{
				int num = Marshal.SizeOf(contourPoints[0]);
				prosodyInterop._contourPoints = Marshal.AllocCoTaskMem(contourPoints.Length * num);
				memoryBlocks.Add(prosodyInterop._contourPoints);
				uint num2 = 0U;
				while ((ulong)num2 < (ulong)((long)contourPoints.Length))
				{
					Marshal.StructureToPtr(contourPoints[(int)num2], (IntPtr)((long)prosodyInterop._contourPoints + (long)num * (long)((ulong)num2)), false);
					num2 += 1U;
				}
			}
			else
			{
				prosodyInterop._contourPoints = IntPtr.Zero;
			}
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(prosodyInterop));
			memoryBlocks.Add(intPtr);
			Marshal.StructureToPtr(prosodyInterop, intPtr, false);
			return intPtr;
		}

		// Token: 0x040002C7 RID: 711
		internal ProsodyNumber _pitch;

		// Token: 0x040002C8 RID: 712
		internal ProsodyNumber _range;

		// Token: 0x040002C9 RID: 713
		internal ProsodyNumber _rate;

		// Token: 0x040002CA RID: 714
		internal int _duration;

		// Token: 0x040002CB RID: 715
		internal ProsodyNumber _volume;

		// Token: 0x040002CC RID: 716
		internal IntPtr _contourPoints;
	}
}
