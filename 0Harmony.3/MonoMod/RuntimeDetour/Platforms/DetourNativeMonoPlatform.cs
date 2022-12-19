using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200036E RID: 878
	internal class DetourNativeMonoPlatform : IDetourNativePlatform
	{
		// Token: 0x060014C7 RID: 5319 RVA: 0x0004C584 File Offset: 0x0004A784
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

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004C5E4 File Offset: 0x0004A7E4
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

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004C642 File Offset: 0x0004A842
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004C642 File Offset: 0x0004A842
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004C64E File Offset: 0x0004A84E
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004C65D File Offset: 0x0004A85D
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004C66D File Offset: 0x0004A86D
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004C67B File Offset: 0x0004A87B
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004C689 File Offset: 0x0004A889
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004C699 File Offset: 0x0004A899
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004C6A7 File Offset: 0x0004A8A7
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x0400104C RID: 4172
		private readonly IDetourNativePlatform Inner;

		// Token: 0x0400104D RID: 4173
		private readonly long _Pagesize;

		// Token: 0x0400104E RID: 4174
		[DynDllImport("mono", new string[] { })]
		private DetourNativeMonoPlatform.d_mono_pagesize mono_pagesize;

		// Token: 0x0400104F RID: 4175
		[DynDllImport("mono", new string[] { })]
		private DetourNativeMonoPlatform.d_mono_mprotect mono_mprotect;

		// Token: 0x0200036F RID: 879
		// (Invoke) Token: 0x060014D3 RID: 5331
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int d_mono_pagesize();

		// Token: 0x02000370 RID: 880
		// (Invoke) Token: 0x060014D7 RID: 5335
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
		private delegate int d_mono_mprotect(IntPtr addr, IntPtr length, int flags);

		// Token: 0x02000371 RID: 881
		[Flags]
		private enum MmapProts
		{
			// Token: 0x04001051 RID: 4177
			PROT_READ = 1,
			// Token: 0x04001052 RID: 4178
			PROT_WRITE = 2,
			// Token: 0x04001053 RID: 4179
			PROT_EXEC = 4,
			// Token: 0x04001054 RID: 4180
			PROT_NONE = 0,
			// Token: 0x04001055 RID: 4181
			PROT_GROWSDOWN = 16777216,
			// Token: 0x04001056 RID: 4182
			PROT_GROWSUP = 33554432
		}
	}
}
