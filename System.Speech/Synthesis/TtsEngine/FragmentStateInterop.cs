using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000179 RID: 377
	internal struct FragmentStateInterop
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x00028938 File Offset: 0x00027938
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
					array[(int)((UIntPtr)num)] = (short)state.Phoneme[(int)((UIntPtr)num)];
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

		// Token: 0x0400073A RID: 1850
		internal TtsEngineAction _action;

		// Token: 0x0400073B RID: 1851
		internal int _langId;

		// Token: 0x0400073C RID: 1852
		internal int _emphasis;

		// Token: 0x0400073D RID: 1853
		internal int _duration;

		// Token: 0x0400073E RID: 1854
		internal IntPtr _sayAs;

		// Token: 0x0400073F RID: 1855
		internal IntPtr _prosody;

		// Token: 0x04000740 RID: 1856
		internal IntPtr _phoneme;
	}
}
