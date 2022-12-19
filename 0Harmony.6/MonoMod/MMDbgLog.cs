using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace MonoMod
{
	// Token: 0x02000331 RID: 817
	internal static class MMDbgLog
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x00040488 File Offset: 0x0003E688
		static MMDbgLog()
		{
			bool flag3;
			if (!(Environment.GetEnvironmentVariable("MONOMOD_DBGLOG") == "1"))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("MONOMOD_DBGLOG");
				bool? flag;
				if (environmentVariable == null)
				{
					flag = null;
				}
				else
				{
					string text = environmentVariable.ToLowerInvariant();
					flag = ((text != null) ? new bool?(text.Contains(MMDbgLog.Tag.ToLowerInvariant())) : null);
				}
				bool? flag2 = flag;
				flag3 = flag2.GetValueOrDefault();
			}
			else
			{
				flag3 = true;
			}
			if (flag3)
			{
				MMDbgLog.Start();
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0004051C File Offset: 0x0003E71C
		public static void WaitForDebugger()
		{
			if (!MMDbgLog.Debugging)
			{
				MMDbgLog.Debugging = true;
				Debugger.Launch();
				Thread.Sleep(6000);
				Debugger.Break();
			}
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00040540 File Offset: 0x0003E740
		public static void Start()
		{
			if (MMDbgLog.Writer != null)
			{
				return;
			}
			string text = Environment.GetEnvironmentVariable("MONOMOD_DBGLOG_PATH");
			if (text == "-")
			{
				MMDbgLog.Writer = Console.Out;
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "mmdbglog.txt";
			}
			text = Path.GetFullPath(Path.GetFileNameWithoutExtension(text) + "-" + MMDbgLog.Tag + Path.GetExtension(text));
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch
			{
			}
			try
			{
				string directoryName = Path.GetDirectoryName(text);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				MMDbgLog.Writer = new StreamWriter(new FileStream(text, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete), Encoding.UTF8);
			}
			catch
			{
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0004060C File Offset: 0x0003E80C
		public static void Log(string str)
		{
			TextWriter writer = MMDbgLog.Writer;
			if (writer == null)
			{
				return;
			}
			writer.WriteLine(str);
			writer.Flush();
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00040630 File Offset: 0x0003E830
		public static T Log<T>(string str, T value)
		{
			TextWriter writer = MMDbgLog.Writer;
			if (writer == null)
			{
				return value;
			}
			writer.WriteLine(string.Format(str, value));
			writer.Flush();
			return value;
		}

		// Token: 0x04000F64 RID: 3940
		public static readonly string Tag = typeof(MMDbgLog).Assembly.GetName().Name;

		// Token: 0x04000F65 RID: 3941
		public static TextWriter Writer;

		// Token: 0x04000F66 RID: 3942
		public static bool Debugging;
	}
}
