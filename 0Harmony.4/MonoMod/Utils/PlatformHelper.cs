using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonoMod.Utils
{
	// Token: 0x0200044C RID: 1100
	internal static class PlatformHelper
	{
		// Token: 0x06001760 RID: 5984 RVA: 0x00050DEC File Offset: 0x0004EFEC
		private static void DeterminePlatform()
		{
			PlatformHelper._current = Platform.Unknown;
			PropertyInfo property = typeof(Environment).GetProperty("Platform", BindingFlags.Static | BindingFlags.NonPublic);
			string text;
			if (property != null)
			{
				text = property.GetValue(null, new object[0]).ToString();
			}
			else
			{
				text = Environment.OSVersion.Platform.ToString();
			}
			text = text.ToLower(CultureInfo.InvariantCulture);
			if (text.Contains("win"))
			{
				PlatformHelper._current = Platform.Windows;
			}
			else if (text.Contains("mac") || text.Contains("osx"))
			{
				PlatformHelper._current = Platform.MacOS;
			}
			else if (text.Contains("lin") || text.Contains("unix"))
			{
				PlatformHelper._current = Platform.Linux;
			}
			if (PlatformHelper._current != Platform.Unknown)
			{
				if (PlatformHelper.Is(Platform.Linux) && Directory.Exists("/data") && File.Exists("/system/build.prop"))
				{
					PlatformHelper._current = Platform.Android;
				}
				else if (PlatformHelper.Is(Platform.Unix) && Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/User") && !Directory.Exists("/Users"))
				{
					PlatformHelper._current = Platform.iOS;
				}
				else if (PlatformHelper.Is(Platform.Windows) && PlatformHelper.CheckWine())
				{
					PlatformHelper._current |= Platform.Wine;
				}
			}
			PropertyInfo property2 = typeof(Environment).GetProperty("Is64BitOperatingSystem");
			MethodInfo methodInfo = ((property2 != null) ? property2.GetGetMethod() : null);
			if (methodInfo != null)
			{
				PlatformHelper._current |= (((bool)methodInfo.Invoke(null, new object[0])) ? Platform.Bits64 : ((Platform)0));
			}
			else
			{
				PlatformHelper._current |= ((IntPtr.Size >= 8) ? Platform.Bits64 : ((Platform)0));
			}
			if (PlatformHelper._current != Platform.Unknown && (PlatformHelper.Is(Platform.Unix) || PlatformHelper.Is(Platform.Unknown)) && ReflectionHelper.IsMono)
			{
				try
				{
					string text2;
					using (Process process = Process.Start(new ProcessStartInfo("uname", "-m")
					{
						UseShellExecute = false,
						RedirectStandardOutput = true
					}))
					{
						text2 = process.StandardOutput.ReadLine().Trim();
					}
					if (text2.StartsWith("aarch", StringComparison.Ordinal) || text2.StartsWith("arm", StringComparison.Ordinal))
					{
						PlatformHelper._current |= Platform.ARM;
					}
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			typeof(object).Module.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			if (imageFileMachine == ImageFileMachine.ARM)
			{
				PlatformHelper._current |= Platform.ARM;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x000510A8 File Offset: 0x0004F2A8
		// (set) Token: 0x06001762 RID: 5986 RVA: 0x000510CA File Offset: 0x0004F2CA
		public static Platform Current
		{
			get
			{
				if (!PlatformHelper._currentLocked)
				{
					if (PlatformHelper._current == Platform.Unknown)
					{
						PlatformHelper.DeterminePlatform();
					}
					PlatformHelper._currentLocked = true;
				}
				return PlatformHelper._current;
			}
			set
			{
				if (PlatformHelper._currentLocked)
				{
					throw new InvalidOperationException("Cannot set the value of PlatformHelper.Current once it has been accessed.");
				}
				PlatformHelper._current = value;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x000510E4 File Offset: 0x0004F2E4
		public static string LibrarySuffix
		{
			get
			{
				if (PlatformHelper._librarySuffix == null)
				{
					PlatformHelper._librarySuffix = (PlatformHelper.Is(Platform.MacOS) ? "dylib" : (PlatformHelper.Is(Platform.Unix) ? "so" : "dll"));
				}
				return PlatformHelper._librarySuffix;
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0005111B File Offset: 0x0004F31B
		public static bool Is(Platform platform)
		{
			return (PlatformHelper.Current & platform) == platform;
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00051128 File Offset: 0x0004F328
		private static bool CheckWine()
		{
			string text = Environment.GetEnvironmentVariable("MONOMOD_WINE");
			if (text == "1")
			{
				return true;
			}
			if (text == "0")
			{
				return false;
			}
			string environmentVariable = Environment.GetEnvironmentVariable("XL_WINEONLINUX");
			text = ((environmentVariable != null) ? environmentVariable.ToLower(CultureInfo.InvariantCulture) : null);
			if (text == "true")
			{
				return true;
			}
			if (text == "false")
			{
				return false;
			}
			IntPtr moduleHandle = PlatformHelper.GetModuleHandle("ntdll.dll");
			return moduleHandle != IntPtr.Zero && PlatformHelper.GetProcAddress(moduleHandle, "wine_get_version") != IntPtr.Zero;
		}

		// Token: 0x06001766 RID: 5990
		[DllImport("kernel32", SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06001767 RID: 5991
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x04001028 RID: 4136
		private static Platform _current = Platform.Unknown;

		// Token: 0x04001029 RID: 4137
		private static bool _currentLocked = false;

		// Token: 0x0400102A RID: 4138
		private static string _librarySuffix;
	}
}
