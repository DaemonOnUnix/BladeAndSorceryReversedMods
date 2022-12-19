using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200046C RID: 1132
	internal class DetourNativeLibcPlatform : IDetourNativePlatform
	{
		// Token: 0x06001870 RID: 6256 RVA: 0x00055410 File Offset: 0x00053610
		public DetourNativeLibcPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
			PropertyInfo property = typeof(Environment).GetProperty("SystemPageSize");
			if (property == null)
			{
				throw new NotSupportedException("Unsupported runtime");
			}
			this._Pagesize = (long)((int)property.GetValue(null, new object[0]));
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0005546C File Offset: 0x0005366C
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeLibcPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (DetourNativeLibcPlatform.mprotect((IntPtr)num, (IntPtr)(num2 - num), prot) != 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x000554BD File Offset: 0x000536BD
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000554BD File Offset: 0x000536BD
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000554BD File Offset: 0x000536BD
		public void MakeReadWriteExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000554C9 File Offset: 0x000536C9
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000554D8 File Offset: 0x000536D8
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x000554E8 File Offset: 0x000536E8
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000554F6 File Offset: 0x000536F6
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00055504 File Offset: 0x00053704
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00055514 File Offset: 0x00053714
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00055522 File Offset: 0x00053722
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x0600187C RID: 6268
		[DllImport("libc", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int mprotect(IntPtr start, IntPtr len, DetourNativeLibcPlatform.MmapProts prot);

		// Token: 0x040010A7 RID: 4263
		private readonly IDetourNativePlatform Inner;

		// Token: 0x040010A8 RID: 4264
		private readonly long _Pagesize;

		// Token: 0x0200046D RID: 1133
		[Flags]
		private enum MmapProts
		{
			// Token: 0x040010AA RID: 4266
			PROT_READ = 1,
			// Token: 0x040010AB RID: 4267
			PROT_WRITE = 2,
			// Token: 0x040010AC RID: 4268
			PROT_EXEC = 4,
			// Token: 0x040010AD RID: 4269
			PROT_NONE = 0,
			// Token: 0x040010AE RID: 4270
			PROT_GROWSDOWN = 16777216,
			// Token: 0x040010AF RID: 4271
			PROT_GROWSUP = 33554432
		}
	}
}
