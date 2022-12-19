using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000178 RID: 376
	internal struct TextFragmentInterop
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0002887C File Offset: 0x0002787C
		internal static IntPtr FragmentToPtr(List<TextFragment> textFragments, Collection<IntPtr> memoryBlocks)
		{
			TextFragmentInterop textFragmentInterop = default(TextFragmentInterop);
			int count = textFragments.Count;
			int num = Marshal.SizeOf(textFragmentInterop);
			IntPtr intPtr = Marshal.AllocCoTaskMem(num * count);
			memoryBlocks.Add(intPtr);
			for (int i = 0; i < count; i++)
			{
				textFragmentInterop._state.FragmentStateToPtr(textFragments[i].State, memoryBlocks);
				textFragmentInterop._textToSpeak = textFragments[i].TextToSpeak;
				textFragmentInterop._textOffset = textFragments[i].TextOffset;
				textFragmentInterop._textLength = textFragments[i].TextLength;
				Marshal.StructureToPtr(textFragmentInterop, (IntPtr)((long)intPtr + (long)(i * num)), false);
			}
			return intPtr;
		}

		// Token: 0x04000736 RID: 1846
		internal FragmentStateInterop _state;

		// Token: 0x04000737 RID: 1847
		[MarshalAs(21)]
		internal string _textToSpeak;

		// Token: 0x04000738 RID: 1848
		internal int _textOffset;

		// Token: 0x04000739 RID: 1849
		internal int _textLength;
	}
}
