using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000376 RID: 886
	internal class DetourNativeWindowsPlatform : IDetourNativePlatform
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x0004C7DD File Offset: 0x0004A9DD
		public DetourNativeWindowsPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0004C7EC File Offset: 0x0004A9EC
		public void MakeWritable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.PAGE page;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.PAGE.EXECUTE_READWRITE, out page))
			{
				this.LogAllSections("MakeWriteable", src, size);
				throw new Win32Exception();
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0004C820 File Offset: 0x0004AA20
		public void MakeExecutable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.PAGE page;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.PAGE.EXECUTE_READWRITE, out page))
			{
				this.LogAllSections("MakeExecutable", src, size);
				throw new Win32Exception();
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0004C853 File Offset: 0x0004AA53
		public void FlushICache(IntPtr src, uint size)
		{
			if (!DetourNativeWindowsPlatform.FlushInstructionCache(DetourNativeWindowsPlatform.GetCurrentProcess(), src, (UIntPtr)size))
			{
				this.LogAllSections("FlushICache", src, size);
				throw new Win32Exception();
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004C87C File Offset: 0x0004AA7C
		private unsafe void LogAllSections(string from, IntPtr src, uint size)
		{
			MMDbgLog.Log(string.Format("{0} failed for 0x{1:X16} + {2} - logging all memory sections", from, (long)src, size));
			Exception ex = new Win32Exception();
			MMDbgLog.Log("reason: " + ex.Message);
			try
			{
				IntPtr currentProcess = DetourNativeWindowsPlatform.GetCurrentProcess();
				IntPtr intPtr = (IntPtr)65536;
				int num = 0;
				DetourNativeWindowsPlatform.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION;
				while (DetourNativeWindowsPlatform.VirtualQueryEx(currentProcess, intPtr, out memory_BASIC_INFORMATION, sizeof(DetourNativeWindowsPlatform.MEMORY_BASIC_INFORMATION)) != 0)
				{
					ulong num2 = (ulong)(long)src;
					ulong num3 = num2 + (ulong)size;
					long num4 = (long)memory_BASIC_INFORMATION.BaseAddress;
					ulong num5 = (ulong)(num4 + (long)memory_BASIC_INFORMATION.RegionSize);
					bool flag = num4 <= (long)num3 && num2 <= num5;
					MMDbgLog.Log(string.Format("{0} #{1}", flag ? "*" : "-", num++));
					MMDbgLog.Log(string.Format("addr: 0x{0:X16}", (long)memory_BASIC_INFORMATION.BaseAddress));
					MMDbgLog.Log(string.Format("size: 0x{0:X16}", (long)memory_BASIC_INFORMATION.RegionSize));
					MMDbgLog.Log(string.Format("aaddr: 0x{0:X16}", (long)memory_BASIC_INFORMATION.AllocationBase));
					MMDbgLog.Log(string.Format("state: {0}", memory_BASIC_INFORMATION.State));
					MMDbgLog.Log(string.Format("type: {0}", memory_BASIC_INFORMATION.Type));
					MMDbgLog.Log(string.Format("protect: {0}", memory_BASIC_INFORMATION.Protect));
					MMDbgLog.Log(string.Format("aprotect: {0}", memory_BASIC_INFORMATION.AllocationProtect));
					long num6 = (long)memory_BASIC_INFORMATION.RegionSize;
					if (num6 > 0L && (long)((int)num6) == num6)
					{
						goto IL_1EC;
					}
					if (IntPtr.Size == 8)
					{
						try
						{
							intPtr = (IntPtr)((long)memory_BASIC_INFORMATION.BaseAddress + (long)memory_BASIC_INFORMATION.RegionSize);
							continue;
						}
						catch (OverflowException)
						{
							MMDbgLog.Log("overflow");
							goto IL_223;
						}
						goto IL_1EC;
					}
					goto IL_21F;
					IL_21F:
					goto IL_223;
					IL_1EC:
					try
					{
						intPtr = (IntPtr)((long)((ulong)((int)memory_BASIC_INFORMATION.BaseAddress + (int)memory_BASIC_INFORMATION.RegionSize)));
						continue;
					}
					catch (OverflowException)
					{
						MMDbgLog.Log("overflow");
						goto IL_223;
					}
					goto IL_21F;
				}
				goto IL_223;
			}
			finally
			{
				throw ex;
			}
			goto IL_223;
			for (;;)
			{
				IL_223:
				goto IL_223;
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0004CAFC File Offset: 0x0004ACFC
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0004CB0C File Offset: 0x0004AD0C
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004CB1A File Offset: 0x0004AD1A
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0004CB28 File Offset: 0x0004AD28
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0004CB38 File Offset: 0x0004AD38
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0004CB46 File Offset: 0x0004AD46
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060014F5 RID: 5365
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, DetourNativeWindowsPlatform.PAGE flNewProtect, out DetourNativeWindowsPlatform.PAGE lpflOldProtect);

		// Token: 0x060014F6 RID: 5366
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x060014F7 RID: 5367
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

		// Token: 0x060014F8 RID: 5368
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out DetourNativeWindowsPlatform.MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

		// Token: 0x040011BF RID: 4543
		private readonly IDetourNativePlatform Inner;

		// Token: 0x02000377 RID: 887
		[Flags]
		private enum PAGE : uint
		{
			// Token: 0x040011C1 RID: 4545
			UNSET = 0U,
			// Token: 0x040011C2 RID: 4546
			NOACCESS = 1U,
			// Token: 0x040011C3 RID: 4547
			READONLY = 2U,
			// Token: 0x040011C4 RID: 4548
			READWRITE = 4U,
			// Token: 0x040011C5 RID: 4549
			WRITECOPY = 8U,
			// Token: 0x040011C6 RID: 4550
			EXECUTE = 16U,
			// Token: 0x040011C7 RID: 4551
			EXECUTE_READ = 32U,
			// Token: 0x040011C8 RID: 4552
			EXECUTE_READWRITE = 64U,
			// Token: 0x040011C9 RID: 4553
			EXECUTE_WRITECOPY = 128U,
			// Token: 0x040011CA RID: 4554
			GUARD = 256U,
			// Token: 0x040011CB RID: 4555
			NOCACHE = 512U,
			// Token: 0x040011CC RID: 4556
			WRITECOMBINE = 1024U
		}

		// Token: 0x02000378 RID: 888
		private enum MEM : uint
		{
			// Token: 0x040011CE RID: 4558
			UNSET,
			// Token: 0x040011CF RID: 4559
			MEM_COMMIT = 4096U,
			// Token: 0x040011D0 RID: 4560
			MEM_RESERVE = 8192U,
			// Token: 0x040011D1 RID: 4561
			MEM_FREE = 65536U,
			// Token: 0x040011D2 RID: 4562
			MEM_PRIVATE = 131072U,
			// Token: 0x040011D3 RID: 4563
			MEM_MAPPED = 262144U,
			// Token: 0x040011D4 RID: 4564
			MEM_IMAGE = 16777216U
		}

		// Token: 0x02000379 RID: 889
		private struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x040011D5 RID: 4565
			public IntPtr BaseAddress;

			// Token: 0x040011D6 RID: 4566
			public IntPtr AllocationBase;

			// Token: 0x040011D7 RID: 4567
			public DetourNativeWindowsPlatform.PAGE AllocationProtect;

			// Token: 0x040011D8 RID: 4568
			public IntPtr RegionSize;

			// Token: 0x040011D9 RID: 4569
			public DetourNativeWindowsPlatform.MEM State;

			// Token: 0x040011DA RID: 4570
			public DetourNativeWindowsPlatform.PAGE Protect;

			// Token: 0x040011DB RID: 4571
			public DetourNativeWindowsPlatform.MEM Type;
		}
	}
}
