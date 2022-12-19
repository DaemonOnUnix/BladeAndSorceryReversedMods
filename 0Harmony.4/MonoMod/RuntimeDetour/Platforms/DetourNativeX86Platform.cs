using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200047A RID: 1146
	internal class DetourNativeX86Platform : IDetourNativePlatform
	{
		// Token: 0x060018B2 RID: 6322 RVA: 0x00055AE4 File Offset: 0x00053CE4
		private static bool Is32Bit(long to)
		{
			return (to & 2147483647L) == to;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00055AF4 File Offset: 0x00053CF4
		private unsafe static DetourNativeX86Platform.DetourType GetDetourType(IntPtr from, IntPtr to, ref IntPtr extra)
		{
			long num = (long)to - ((long)from + 5L);
			if ((DetourNativeX86Platform.Is32Bit(num) || DetourNativeX86Platform.Is32Bit(-num)) && ((byte*)(void*)from)[5] != 95)
			{
				return DetourNativeX86Platform.DetourType.Rel32;
			}
			if (DetourNativeX86Platform.Is32Bit((long)to))
			{
				return DetourNativeX86Platform.DetourType.Abs32;
			}
			IDetourRuntimePlatform runtime = DetourHelper.Runtime;
			if (((runtime != null) ? runtime.TryMemAllocScratchCloseTo(from, out extra, 8) : 0U) >= 8U)
			{
				num = (long)extra - ((long)from + 6L);
				if (DetourNativeX86Platform.Is32Bit(num) || DetourNativeX86Platform.Is32Bit(-num))
				{
					return DetourNativeX86Platform.DetourType.Abs64Split;
				}
			}
			return DetourNativeX86Platform.DetourType.Abs64;
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00055B80 File Offset: 0x00053D80
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			NativeDetourData nativeDetourData = new NativeDetourData
			{
				Method = from,
				Target = to
			};
			nativeDetourData.Size = DetourNativeX86Platform.DetourSizes[(int)(nativeDetourData.Type = type ?? ((byte)DetourNativeX86Platform.GetDetourType(from, to, ref nativeDetourData.Extra)))];
			return nativeDetourData;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00055BE1 File Offset: 0x00053DE1
		public void Free(NativeDetourData detour)
		{
			byte type = detour.Type;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00055BEC File Offset: 0x00053DEC
		public void Apply(NativeDetourData detour)
		{
			int num = 0;
			switch (detour.Type)
			{
			case 0:
				detour.Method.Write(ref num, 233);
				detour.Method.Write(ref num, (uint)((int)((long)detour.Target - ((long)detour.Method + (long)num + 4L))));
				return;
			case 1:
				detour.Method.Write(ref num, 104);
				detour.Method.Write(ref num, (uint)(int)detour.Target);
				detour.Method.Write(ref num, 195);
				return;
			case 2:
			case 3:
				detour.Method.Write(ref num, byte.MaxValue);
				detour.Method.Write(ref num, 37);
				if (detour.Type == 3)
				{
					detour.Method.Write(ref num, (uint)((int)((long)detour.Extra - ((long)detour.Method + (long)num + 4L))));
					num = 0;
					detour.Extra.Write(ref num, (ulong)(long)detour.Target);
					return;
				}
				detour.Method.Write(ref num, 0U);
				detour.Method.Write(ref num, (ulong)(long)detour.Target);
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", detour.Type));
			}
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x00055D48 File Offset: 0x00053F48
		public unsafe void Copy(IntPtr src, IntPtr dst, byte type)
		{
			switch (type)
			{
			case 0:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = *(UIntPtr)((long)src + 4L);
				return;
			case 1:
			case 3:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (short)(*(UIntPtr)((long)src + 4L));
				return;
			case 2:
				*(UIntPtr)(long)dst = *(UIntPtr)(long)src;
				*(UIntPtr)((long)dst + 8L) = (int)(*(UIntPtr)((long)src + 8L));
				*(UIntPtr)((long)dst + 12L) = (short)(*(UIntPtr)((long)src + 12L));
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", type));
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00018105 File Offset: 0x00016305
		public void MakeWritable(IntPtr src, uint size)
		{
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00018105 File Offset: 0x00016305
		public void MakeExecutable(IntPtr src, uint size)
		{
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00018105 File Offset: 0x00016305
		public void MakeReadWriteExecutable(IntPtr src, uint size)
		{
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00018105 File Offset: 0x00016305
		public void FlushICache(IntPtr src, uint size)
		{
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000553A9 File Offset: 0x000535A9
		public IntPtr MemAlloc(uint size)
		{
			return Marshal.AllocHGlobal((int)size);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000553B1 File Offset: 0x000535B1
		public void MemFree(IntPtr ptr)
		{
			Marshal.FreeHGlobal(ptr);
		}

		// Token: 0x04001240 RID: 4672
		private static readonly uint[] DetourSizes = new uint[] { 5U, 6U, 14U, 6U };

		// Token: 0x0200047B RID: 1147
		public enum DetourType : byte
		{
			// Token: 0x04001242 RID: 4674
			Rel32,
			// Token: 0x04001243 RID: 4675
			Abs32,
			// Token: 0x04001244 RID: 4676
			Abs64,
			// Token: 0x04001245 RID: 4677
			Abs64Split
		}
	}
}
