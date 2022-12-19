using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200017A RID: 378
	internal struct ProsodyInterop
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x00028A80 File Offset: 0x00027A80
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
					Marshal.StructureToPtr(contourPoints[(int)((UIntPtr)num2)], (IntPtr)((long)prosodyInterop._contourPoints + (long)num * (long)((ulong)num2)), false);
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

		// Token: 0x04000741 RID: 1857
		internal ProsodyNumber _pitch;

		// Token: 0x04000742 RID: 1858
		internal ProsodyNumber _range;

		// Token: 0x04000743 RID: 1859
		internal ProsodyNumber _rate;

		// Token: 0x04000744 RID: 1860
		internal int _duration;

		// Token: 0x04000745 RID: 1861
		internal ProsodyNumber _volume;

		// Token: 0x04000746 RID: 1862
		internal IntPtr _contourPoints;
	}
}
