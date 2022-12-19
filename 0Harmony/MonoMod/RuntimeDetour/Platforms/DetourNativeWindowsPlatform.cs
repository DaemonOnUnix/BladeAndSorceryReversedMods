using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000476 RID: 1142
	internal class DetourNativeWindowsPlatform : IDetourNativePlatform
	{
		// Token: 0x060018A2 RID: 6306 RVA: 0x00055789 File Offset: 0x00053989
		public DetourNativeWindowsPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00055798 File Offset: 0x00053998
		public void MakeWritable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.PAGE page;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.PAGE.EXECUTE_READWRITE, out page))
			{
				throw this.LogAllSections(Marshal.GetLastWin32Error(), "MakeWriteable", src, size);
			}
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x000557CC File Offset: 0x000539CC
		public void MakeExecutable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.PAGE page;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.PAGE.EXECUTE_READWRITE, out page))
			{
				throw this.LogAllSections(Marshal.GetLastWin32Error(), "MakeExecutable", src, size);
			}
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00055800 File Offset: 0x00053A00
		public void MakeReadWriteExecutable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.PAGE page;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.PAGE.EXECUTE_READWRITE, out page))
			{
				throw this.LogAllSections(Marshal.GetLastWin32Error(), "MakeExecutable", src, size);
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00055833 File Offset: 0x00053A33
		public void FlushICache(IntPtr src, uint size)
		{
			if (!DetourNativeWindowsPlatform.FlushInstructionCache(DetourNativeWindowsPlatform.GetCurrentProcess(), src, (UIntPtr)size))
			{
				throw this.LogAllSections(Marshal.GetLastWin32Error(), "FlushICache", src, size);
			}
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0005585C File Offset: 0x00053A5C
		private unsafe Exception LogAllSections(int error, string from, IntPtr src, uint size)
		{
			Exception ex = new Win32Exception(error);
			if (MMDbgLog.Writer == null)
			{
				return ex;
			}
			MMDbgLog.Log(string.Format("{0} failed for 0x{1:X16} + {2} - logging all memory sections", from, (long)src, size));
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
					try
					{
						IntPtr intPtr2 = intPtr;
						intPtr = (IntPtr)((long)memory_BASIC_INFORMATION.BaseAddress + (long)memory_BASIC_INFORMATION.RegionSize);
						if ((long)intPtr > (long)intPtr2)
						{
							continue;
						}
					}
					catch (OverflowException)
					{
						MMDbgLog.Log("overflow");
					}
					break;
				}
			}
			catch
			{
				throw ex;
			}
			return ex;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00055A8C File Offset: 0x00053C8C
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00055A9C File Offset: 0x00053C9C
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00055AAA File Offset: 0x00053CAA
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00055AB8 File Offset: 0x00053CB8
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00055AC8 File Offset: 0x00053CC8
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00055AD6 File Offset: 0x00053CD6
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060018AE RID: 6318
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, DetourNativeWindowsPlatform.PAGE flNewProtect, out DetourNativeWindowsPlatform.PAGE lpflOldProtect);

		// Token: 0x060018AF RID: 6319
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x060018B0 RID: 6320
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

		// Token: 0x060018B1 RID: 6321
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out DetourNativeWindowsPlatform.MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

		// Token: 0x04001223 RID: 4643
		private readonly IDetourNativePlatform Inner;

		// Token: 0x02000477 RID: 1143
		[Flags]
		private enum PAGE : uint
		{
			// Token: 0x04001225 RID: 4645
			UNSET = 0U,
			// Token: 0x04001226 RID: 4646
			NOACCESS = 1U,
			// Token: 0x04001227 RID: 4647
			READONLY = 2U,
			// Token: 0x04001228 RID: 4648
			READWRITE = 4U,
			// Token: 0x04001229 RID: 4649
			WRITECOPY = 8U,
			// Token: 0x0400122A RID: 4650
			EXECUTE = 16U,
			// Token: 0x0400122B RID: 4651
			EXECUTE_READ = 32U,
			// Token: 0x0400122C RID: 4652
			EXECUTE_READWRITE = 64U,
			// Token: 0x0400122D RID: 4653
			EXECUTE_WRITECOPY = 128U,
			// Token: 0x0400122E RID: 4654
			GUARD = 256U,
			// Token: 0x0400122F RID: 4655
			NOCACHE = 512U,
			// Token: 0x04001230 RID: 4656
			WRITECOMBINE = 1024U
		}

		// Token: 0x02000478 RID: 1144
		private enum MEM : uint
		{
			// Token: 0x04001232 RID: 4658
			UNSET,
			// Token: 0x04001233 RID: 4659
			MEM_COMMIT = 4096U,
			// Token: 0x04001234 RID: 4660
			MEM_RESERVE = 8192U,
			// Token: 0x04001235 RID: 4661
			MEM_FREE = 65536U,
			// Token: 0x04001236 RID: 4662
			MEM_PRIVATE = 131072U,
			// Token: 0x04001237 RID: 4663
			MEM_MAPPED = 262144U,
			// Token: 0x04001238 RID: 4664
			MEM_IMAGE = 16777216U
		}

		// Token: 0x02000479 RID: 1145
		private struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x04001239 RID: 4665
			public IntPtr BaseAddress;

			// Token: 0x0400123A RID: 4666
			public IntPtr AllocationBase;

			// Token: 0x0400123B RID: 4667
			public DetourNativeWindowsPlatform.PAGE AllocationProtect;

			// Token: 0x0400123C RID: 4668
			public IntPtr RegionSize;

			// Token: 0x0400123D RID: 4669
			public DetourNativeWindowsPlatform.MEM State;

			// Token: 0x0400123E RID: 4670
			public DetourNativeWindowsPlatform.PAGE Protect;

			// Token: 0x0400123F RID: 4671
			public DetourNativeWindowsPlatform.MEM Type;
		}
	}
}
