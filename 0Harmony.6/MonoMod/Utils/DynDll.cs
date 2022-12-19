using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonoMod.Utils
{
	// Token: 0x02000343 RID: 835
	internal static class DynDll
	{
		// Token: 0x06001352 RID: 4946
		[DllImport("kernel32", SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06001353 RID: 4947
		[DllImport("kernel32", SetLastError = true)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x06001354 RID: 4948
		[DllImport("kernel32", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hLibModule);

		// Token: 0x06001355 RID: 4949
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x06001356 RID: 4950
		[DllImport("dl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlopen(string filename, int flags);

		// Token: 0x06001357 RID: 4951
		[DllImport("dl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool dlclose(IntPtr handle);

		// Token: 0x06001358 RID: 4952
		[DllImport("dl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlsym(IntPtr handle, string symbol);

		// Token: 0x06001359 RID: 4953
		[DllImport("dl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr dlerror();

		// Token: 0x0600135A RID: 4954 RVA: 0x00044094 File Offset: 0x00042294
		private static bool CheckError(out Exception exception)
		{
			if (PlatformHelper.Is(Platform.Windows))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 0)
				{
					exception = new Win32Exception(lastWin32Error);
					return false;
				}
			}
			else
			{
				IntPtr intPtr = DynDll.dlerror();
				if (intPtr != IntPtr.Zero)
				{
					exception = new Win32Exception(Marshal.PtrToStringAnsi(intPtr));
					return false;
				}
			}
			exception = null;
			return true;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x000440E4 File Offset: 0x000422E4
		public static IntPtr OpenLibrary(string name, bool skipMapping = false, int? flags = null)
		{
			IntPtr intPtr;
			if (!DynDll.InternalTryOpenLibrary(name, out intPtr, skipMapping, flags))
			{
				throw new DllNotFoundException("Unable to load library '" + name + "'");
			}
			Exception ex;
			if (!DynDll.CheckError(out ex))
			{
				throw ex;
			}
			return intPtr;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00044120 File Offset: 0x00042320
		public static bool TryOpenLibrary(string name, out IntPtr libraryPtr, bool skipMapping = false, int? flags = null)
		{
			Exception ex;
			return DynDll.InternalTryOpenLibrary(name, out libraryPtr, skipMapping, flags) && DynDll.CheckError(out ex);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00044148 File Offset: 0x00042348
		private static bool InternalTryOpenLibrary(string name, out IntPtr libraryPtr, bool skipMapping, int? flags)
		{
			List<DynDllMapping> list;
			if (name != null && !skipMapping && DynDll.Mappings.TryGetValue(name, out list))
			{
				foreach (DynDllMapping dynDllMapping in list)
				{
					if (DynDll.InternalTryOpenLibrary(dynDllMapping.LibraryName, out libraryPtr, true, dynDllMapping.Flags))
					{
						return true;
					}
				}
				libraryPtr = IntPtr.Zero;
				return true;
			}
			if (PlatformHelper.Is(Platform.Windows))
			{
				libraryPtr = ((name == null) ? DynDll.GetModuleHandle(name) : DynDll.LoadLibrary(name));
			}
			else
			{
				int num = flags ?? 258;
				libraryPtr = DynDll.dlopen(name, num);
				if (libraryPtr == IntPtr.Zero && File.Exists(name))
				{
					libraryPtr = DynDll.dlopen(Path.GetFullPath(name), num);
				}
			}
			return libraryPtr != IntPtr.Zero;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00044240 File Offset: 0x00042440
		public static bool CloseLibrary(IntPtr lib)
		{
			if (PlatformHelper.Is(Platform.Windows))
			{
				DynDll.CloseLibrary(lib);
			}
			else
			{
				DynDll.dlclose(lib);
			}
			Exception ex;
			return DynDll.CheckError(out ex);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00044270 File Offset: 0x00042470
		public static IntPtr GetFunction(this IntPtr libraryPtr, string name)
		{
			IntPtr intPtr;
			if (!DynDll.InternalTryGetFunction(libraryPtr, name, out intPtr))
			{
				throw new MissingMethodException("Unable to load function '" + name + "'");
			}
			Exception ex;
			if (!DynDll.CheckError(out ex))
			{
				throw ex;
			}
			return intPtr;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x000442AC File Offset: 0x000424AC
		public static bool TryGetFunction(this IntPtr libraryPtr, string name, out IntPtr functionPtr)
		{
			Exception ex;
			return DynDll.InternalTryGetFunction(libraryPtr, name, out functionPtr) && DynDll.CheckError(out ex);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x000442D4 File Offset: 0x000424D4
		private static bool InternalTryGetFunction(IntPtr libraryPtr, string name, out IntPtr functionPtr)
		{
			if (libraryPtr == IntPtr.Zero)
			{
				throw new ArgumentNullException("libraryPtr");
			}
			functionPtr = (PlatformHelper.Is(Platform.Windows) ? DynDll.GetProcAddress(libraryPtr, name) : DynDll.dlsym(libraryPtr, name));
			return functionPtr != IntPtr.Zero;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00044320 File Offset: 0x00042520
		public static T AsDelegate<T>(this IntPtr s) where T : class
		{
			return Marshal.GetDelegateForFunctionPointer(s, typeof(T)) as T;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0004433C File Offset: 0x0004253C
		public static void ResolveDynDllImports(this Type type, Dictionary<string, List<DynDllMapping>> mappings = null)
		{
			DynDll.InternalResolveDynDllImports(type, null, mappings);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00044346 File Offset: 0x00042546
		public static void ResolveDynDllImports(object instance, Dictionary<string, List<DynDllMapping>> mappings = null)
		{
			DynDll.InternalResolveDynDllImports(instance.GetType(), instance, mappings);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00044358 File Offset: 0x00042558
		private static void InternalResolveDynDllImports(Type type, object instance, Dictionary<string, List<DynDllMapping>> mappings)
		{
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
			if (instance == null)
			{
				bindingFlags |= BindingFlags.Static;
			}
			else
			{
				bindingFlags |= BindingFlags.Instance;
			}
			foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
			{
				bool flag = true;
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DynDllImportAttribute), true);
				int j = 0;
				while (j < customAttributes.Length)
				{
					DynDllImportAttribute dynDllImportAttribute = (DynDllImportAttribute)customAttributes[j];
					flag = false;
					IntPtr zero = IntPtr.Zero;
					List<DynDllMapping> list;
					if (mappings != null && mappings.TryGetValue(dynDllImportAttribute.LibraryName, out list))
					{
						bool flag2 = false;
						foreach (DynDllMapping dynDllMapping in list)
						{
							if (DynDll.TryOpenLibrary(dynDllMapping.LibraryName, out zero, true, dynDllMapping.Flags))
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							goto IL_DC;
						}
					}
					else if (DynDll.TryOpenLibrary(dynDllImportAttribute.LibraryName, out zero, false, null))
					{
						goto IL_DC;
					}
					IL_158:
					j++;
					continue;
					IL_DC:
					foreach (string text in dynDllImportAttribute.EntryPoints.Concat(new string[]
					{
						fieldInfo.Name,
						fieldInfo.FieldType.Name
					}))
					{
						IntPtr intPtr;
						if (zero.TryGetFunction(text, out intPtr))
						{
							fieldInfo.SetValue(instance, Marshal.GetDelegateForFunctionPointer(intPtr, fieldInfo.FieldType));
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						goto IL_158;
					}
					break;
				}
				if (!flag)
				{
					throw new EntryPointNotFoundException("No matching entry point found for " + fieldInfo.Name + " in " + fieldInfo.DeclaringType.FullName);
				}
			}
		}

		// Token: 0x04000FAB RID: 4011
		public static Dictionary<string, List<DynDllMapping>> Mappings = new Dictionary<string, List<DynDllMapping>>();

		// Token: 0x02000344 RID: 836
		public static class DlopenFlags
		{
			// Token: 0x04000FAC RID: 4012
			public const int RTLD_LAZY = 1;

			// Token: 0x04000FAD RID: 4013
			public const int RTLD_NOW = 2;

			// Token: 0x04000FAE RID: 4014
			public const int RTLD_LOCAL = 0;

			// Token: 0x04000FAF RID: 4015
			public const int RTLD_GLOBAL = 256;
		}
	}
}
