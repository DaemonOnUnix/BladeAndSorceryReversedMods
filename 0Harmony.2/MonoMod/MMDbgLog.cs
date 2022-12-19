using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace MonoMod
{
	// Token: 0x02000429 RID: 1065
	internal static class MMDbgLog
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x000483FC File Offset: 0x000465FC
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
					string text = environmentVariable.ToLower(CultureInfo.InvariantCulture);
					flag = ((text != null) ? new bool?(text.Contains(MMDbgLog.Tag.ToLower(CultureInfo.InvariantCulture), StringComparison.Ordinal)) : null);
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

		// Token: 0x06001670 RID: 5744 RVA: 0x0004849B File Offset: 0x0004669B
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

		// Token: 0x06001671 RID: 5745 RVA: 0x000484C0 File Offset: 0x000466C0
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

		// Token: 0x06001672 RID: 5746 RVA: 0x0004858C File Offset: 0x0004678C
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

		// Token: 0x06001673 RID: 5747 RVA: 0x000485B0 File Offset: 0x000467B0
		public static T Log<T>(string str, T value)
		{
			TextWriter writer = MMDbgLog.Writer;
			if (writer == null)
			{
				return value;
			}
			writer.WriteLine(string.Format(CultureInfo.InvariantCulture, str, new object[] { value }));
			writer.Flush();
			return value;
		}

		// Token: 0x04000FA2 RID: 4002
		public static readonly string Tag = typeof(MMDbgLog).Assembly.GetName().Name;

		// Token: 0x04000FA3 RID: 4003
		public static TextWriter Writer;

		// Token: 0x04000FA4 RID: 4004
		public static bool Debugging;
	}
}
