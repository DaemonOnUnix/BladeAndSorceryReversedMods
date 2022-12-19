using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200036C RID: 876
	internal class DetourNativeLibcPlatform : IDetourNativePlatform
	{
		// Token: 0x060014BB RID: 5307 RVA: 0x0004C464 File Offset: 0x0004A664
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

		// Token: 0x060014BC RID: 5308 RVA: 0x0004C4C0 File Offset: 0x0004A6C0
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

		// Token: 0x060014BD RID: 5309 RVA: 0x0004C511 File Offset: 0x0004A711
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004C511 File Offset: 0x0004A711
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004C51D File Offset: 0x0004A71D
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004C52C File Offset: 0x0004A72C
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004C53C File Offset: 0x0004A73C
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004C54A File Offset: 0x0004A74A
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004C558 File Offset: 0x0004A758
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004C568 File Offset: 0x0004A768
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004C576 File Offset: 0x0004A776
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060014C6 RID: 5318
		[DllImport("libc", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int mprotect(IntPtr start, IntPtr len, DetourNativeLibcPlatform.MmapProts prot);

		// Token: 0x04001043 RID: 4163
		private readonly IDetourNativePlatform Inner;

		// Token: 0x04001044 RID: 4164
		private readonly long _Pagesize;

		// Token: 0x0200036D RID: 877
		[Flags]
		private enum MmapProts
		{
			// Token: 0x04001046 RID: 4166
			PROT_READ = 1,
			// Token: 0x04001047 RID: 4167
			PROT_WRITE = 2,
			// Token: 0x04001048 RID: 4168
			PROT_EXEC = 4,
			// Token: 0x04001049 RID: 4169
			PROT_NONE = 0,
			// Token: 0x0400104A RID: 4170
			PROT_GROWSDOWN = 16777216,
			// Token: 0x0400104B RID: 4171
			PROT_GROWSUP = 33554432
		}
	}
}
