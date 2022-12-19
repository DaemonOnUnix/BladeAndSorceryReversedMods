using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000043 RID: 67
	internal struct TextFragmentInterop
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000050C8 File Offset: 0x000032C8
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

		// Token: 0x040002BC RID: 700
		internal FragmentStateInterop _state;

		// Token: 0x040002BD RID: 701
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string _textToSpeak;

		// Token: 0x040002BE RID: 702
		internal int _textOffset;

		// Token: 0x040002BF RID: 703
		internal int _textLength;
	}
}
