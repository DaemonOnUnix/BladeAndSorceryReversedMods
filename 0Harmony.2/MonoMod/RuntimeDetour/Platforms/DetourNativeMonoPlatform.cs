using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200046E RID: 1134
	internal class DetourNativeMonoPlatform : IDetourNativePlatform
	{
		// Token: 0x0600187D RID: 6269 RVA: 0x00055530 File Offset: 0x00053730
		public DetourNativeMonoPlatform(IDetourNativePlatform inner, string libmono)
		{
			this.Inner = inner;
			Dictionary<string, List<DynDllMapping>> dictionary = new Dictionary<string, List<DynDllMapping>>();
			if (!string.IsNullOrEmpty(libmono))
			{
				dictionary.Add("mono", new List<DynDllMapping> { libmono });
			}
			DynDll.ResolveDynDllImports(this, dictionary);
			this._Pagesize = (long)this.mono_pagesize();
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00055590 File Offset: 0x00053790
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeMonoPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (this.mono_mprotect((IntPtr)num, (IntPtr)(num2 - num), (int)prot) != 0 && Marshal.GetLastWin32Error() != 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000555EE File Offset: 0x000537EE
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x000555EE File Offset: 0x000537EE
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x000555EE File Offset: 0x000537EE
		public void MakeReadWriteExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x000555FA File Offset: 0x000537FA
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00055609 File Offset: 0x00053809
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00055619 File Offset: 0x00053819
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00055627 File Offset: 0x00053827
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00055635 File Offset: 0x00053835
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00055645 File Offset: 0x00053845
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00055653 File Offset: 0x00053853
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x040010B0 RID: 4272
		private readonly IDetourNativePlatform Inner;

		// Token: 0x040010B1 RID: 4273
		private readonly long _Pagesize;

		// Token: 0x040010B2 RID: 4274
		[DynDllImport("mono", new string[] { })]
		private DetourNativeMonoPlatform.d_mono_pagesize mono_pagesize;

		// Token: 0x040010B3 RID: 4275
		[DynDllImport("mono", new string[] { })]
		private DetourNativeMonoPlatform.d_mono_mprotect mono_mprotect;

		// Token: 0x0200046F RID: 1135
		// (Invoke) Token: 0x0600188A RID: 6282
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int d_mono_pagesize();

		// Token: 0x02000470 RID: 1136
		// (Invoke) Token: 0x0600188E RID: 6286
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
		private delegate int d_mono_mprotect(IntPtr addr, IntPtr length, int flags);

		// Token: 0x02000471 RID: 1137
		[Flags]
		private enum MmapProts
		{
			// Token: 0x040010B5 RID: 4277
			PROT_READ = 1,
			// Token: 0x040010B6 RID: 4278
			PROT_WRITE = 2,
			// Token: 0x040010B7 RID: 4279
			PROT_EXEC = 4,
			// Token: 0x040010B8 RID: 4280
			PROT_NONE = 0,
			// Token: 0x040010B9 RID: 4281
			PROT_GROWSDOWN = 16777216,
			// Token: 0x040010BA RID: 4282
			PROT_GROWSUP = 33554432
		}
	}
}
