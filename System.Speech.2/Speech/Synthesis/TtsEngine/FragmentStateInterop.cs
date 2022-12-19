using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000044 RID: 68
	internal struct FragmentStateInterop
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00005184 File Offset: 0x00003384
		internal void FragmentStateToPtr(FragmentState state, Collection<IntPtr> memoryBlocks)
		{
			this._action = state.Action;
			this._langId = state.LangId;
			this._emphasis = state.Emphasis;
			this._duration = state.Duration;
			if (state.SayAs != null)
			{
				this._sayAs = Marshal.AllocCoTaskMem(Marshal.SizeOf(state.SayAs));
				memoryBlocks.Add(this._sayAs);
				Marshal.StructureToPtr(state.SayAs, this._sayAs, false);
			}
			else
			{
				this._sayAs = IntPtr.Zero;
			}
			if (state.Phoneme != null)
			{
				short[] array = new short[state.Phoneme.Length + 1];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)state.Phoneme.Length))
				{
					array[(int)num] = (short)state.Phoneme[(int)num];
					num += 1U;
				}
				array[state.Phoneme.Length] = 0;
				int num2 = Marshal.SizeOf(array[0]);
				this._phoneme = Marshal.AllocCoTaskMem(num2 * array.Length);
				memoryBlocks.Add(this._phoneme);
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)array.Length))
				{
					Marshal.Copy(array, 0, this._phoneme, array.Length);
					num3 += 1U;
				}
			}
			else
			{
				this._phoneme = IntPtr.Zero;
			}
			this._prosody = ProsodyInterop.ProsodyToPtr(state.Prosody, memoryBlocks);
		}

		// Token: 0x040002C0 RID: 704
		internal TtsEngineAction _action;

		// Token: 0x040002C1 RID: 705
		internal int _langId;

		// Token: 0x040002C2 RID: 706
		internal int _emphasis;

		// Token: 0x040002C3 RID: 707
		internal int _duration;

		// Token: 0x040002C4 RID: 708
		internal IntPtr _sayAs;

		// Token: 0x040002C5 RID: 709
		internal IntPtr _prosody;

		// Token: 0x040002C6 RID: 710
		internal IntPtr _phoneme;
	}
}
