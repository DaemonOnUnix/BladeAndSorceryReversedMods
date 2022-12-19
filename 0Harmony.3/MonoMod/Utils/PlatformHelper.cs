using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MonoMod.Utils
{
	// Token: 0x02000351 RID: 849
	internal static class PlatformHelper
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x000487CC File Offset: 0x000469CC
		static PlatformHelper()
		{
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
			text = text.ToLowerInvariant();
			if (text.Contains("win"))
			{
				PlatformHelper.Current = Platform.Windows;
			}
			else if (text.Contains("mac") || text.Contains("osx"))
			{
				PlatformHelper.Current = Platform.MacOS;
			}
			else if (text.Contains("lin") || text.Contains("unix"))
			{
				PlatformHelper.Current = Platform.Linux;
			}
			if (PlatformHelper.Is(Platform.Linux) && Directory.Exists("/data") && File.Exists("/system/build.prop"))
			{
				PlatformHelper.Current = Platform.Android;
			}
			else if (PlatformHelper.Is(Platform.Unix) && Directory.Exists("/Applications") && Directory.Exists("/System"))
			{
				PlatformHelper.Current = Platform.iOS;
			}
			PropertyInfo property2 = typeof(Environment).GetProperty("Is64BitOperatingSystem");
			MethodInfo methodInfo = ((property2 != null) ? property2.GetGetMethod() : null);
			if (methodInfo != null)
			{
				PlatformHelper.Current |= (((bool)methodInfo.Invoke(null, new object[0])) ? Platform.Bits64 : ((Platform)0));
			}
			else
			{
				PlatformHelper.Current |= ((IntPtr.Size >= 8) ? Platform.Bits64 : ((Platform)0));
			}
			if ((PlatformHelper.Is(Platform.Unix) || PlatformHelper.Is(Platform.Unknown)) && Type.GetType("Mono.Runtime") != null)
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
					if (text2.StartsWith("aarch") || text2.StartsWith("arm"))
					{
						PlatformHelper.Current |= Platform.ARM;
					}
					goto IL_246;
				}
				catch (Exception)
				{
					goto IL_246;
				}
			}
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			typeof(object).Module.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			if (imageFileMachine == ImageFileMachine.ARM)
			{
				PlatformHelper.Current |= Platform.ARM;
			}
			IL_246:
			PlatformHelper.LibrarySuffix = (PlatformHelper.Is(Platform.MacOS) ? "dylib" : (PlatformHelper.Is(Platform.Unix) ? "so" : "dll"));
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x00048A64 File Offset: 0x00046C64
		// (set) Token: 0x060013D4 RID: 5076 RVA: 0x00048A6B File Offset: 0x00046C6B
		public static Platform Current { get; private set; } = Platform.Unknown;

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00048A73 File Offset: 0x00046C73
		// (set) Token: 0x060013D6 RID: 5078 RVA: 0x00048A7A File Offset: 0x00046C7A
		public static string LibrarySuffix { get; private set; }

		// Token: 0x060013D7 RID: 5079 RVA: 0x00048A82 File Offset: 0x00046C82
		public static bool Is(Platform platform)
		{
			return (PlatformHelper.Current & platform) == platform;
		}
	}
}
