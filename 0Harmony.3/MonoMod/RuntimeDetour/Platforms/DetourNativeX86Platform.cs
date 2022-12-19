using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200037A RID: 890
	internal class DetourNativeX86Platform : IDetourNativePlatform
	{
		// Token: 0x060014F9 RID: 5369 RVA: 0x0004CB54 File Offset: 0x0004AD54
		private static bool Is32Bit(long to)
		{
			return (to & 2147483647L) == to;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004CB64 File Offset: 0x0004AD64
		private static DetourNativeX86Platform.DetourType GetDetourType(IntPtr from, IntPtr to)
		{
			long num = (long)to - ((long)from + 5L);
			if (DetourNativeX86Platform.Is32Bit(num) || DetourNativeX86Platform.Is32Bit(-num))
			{
				return DetourNativeX86Platform.DetourType.Rel32;
			}
			if (DetourNativeX86Platform.Is32Bit((long)to))
			{
				return DetourNativeX86Platform.DetourType.Abs32;
			}
			return DetourNativeX86Platform.DetourType.Abs64;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004CBA8 File Offset: 0x0004ADA8
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			NativeDetourData nativeDetourData = new NativeDetourData
			{
				Method = from,
				Target = to
			};
			nativeDetourData.Size = DetourNativeX86Platform.DetourSizes[(int)(nativeDetourData.Type = type ?? ((byte)DetourNativeX86Platform.GetDetourType(from, to)))];
			return nativeDetourData;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00012279 File Offset: 0x00010479
		public void Free(NativeDetourData detour)
		{
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004CC04 File Offset: 0x0004AE04
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
				detour.Method.Write(ref num, byte.MaxValue);
				detour.Method.Write(ref num, 37);
				detour.Method.Write(ref num, 0U);
				detour.Method.Write(ref num, (ulong)(long)detour.Target);
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", detour.Type));
			}
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004CD0C File Offset: 0x0004AF0C
		public unsafe void Copy(IntPtr src, IntPtr dst, byte type)
		{
			switch (type)
			{
			case 0:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = *(UIntPtr)((long)src + 4L);
				return;
			case 1:
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

		// Token: 0x060014FF RID: 5375 RVA: 0x00012279 File Offset: 0x00010479
		public void MakeWritable(IntPtr src, uint size)
		{
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00012279 File Offset: 0x00010479
		public void MakeExecutable(IntPtr src, uint size)
		{
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00012279 File Offset: 0x00010479
		public void FlushICache(IntPtr src, uint size)
		{
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0004C3FD File Offset: 0x0004A5FD
		public IntPtr MemAlloc(uint size)
		{
			return Marshal.AllocHGlobal((int)size);
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0004C405 File Offset: 0x0004A605
		public void MemFree(IntPtr ptr)
		{
			Marshal.FreeHGlobal(ptr);
		}

		// Token: 0x040011DC RID: 4572
		private static readonly uint[] DetourSizes = new uint[] { 5U, 6U, 14U };

		// Token: 0x0200037B RID: 891
		public enum DetourType : byte
		{
			// Token: 0x040011DE RID: 4574
			Rel32,
			// Token: 0x040011DF RID: 4575
			Abs32,
			// Token: 0x040011E0 RID: 4576
			Abs64
		}
	}
}
