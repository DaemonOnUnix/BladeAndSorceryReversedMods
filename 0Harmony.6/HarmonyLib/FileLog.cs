using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x020000A1 RID: 161
	public static class FileLog
	{
		// Token: 0x0600035C RID: 860 RVA: 0x00010A58 File Offset: 0x0000EC58
		static FileLog()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("HARMONY_LOG_FILE");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				FileLog.logPath = environmentVariable;
				return;
			}
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			Directory.CreateDirectory(folderPath);
			FileLog.logPath = Path.Combine(folderPath, "harmony.log.txt");
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00010ABC File Offset: 0x0000ECBC
		private static string IndentString()
		{
			return new string(FileLog.indentChar, FileLog.indentLevel);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		public static void ChangeIndent(int delta)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.indentLevel = Math.Max(0, FileLog.indentLevel + delta);
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00010B1C File Offset: 0x0000ED1C
		public static void LogBuffered(string str)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.Add(FileLog.IndentString() + str);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00010B6C File Offset: 0x0000ED6C
		public static void LogBuffered(List<string> strings)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.AddRange(strings);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00010BB0 File Offset: 0x0000EDB0
		public static List<string> GetBuffer(bool clear)
		{
			object obj = FileLog.fileLock;
			List<string> list2;
			lock (obj)
			{
				List<string> list = FileLog.buffer;
				if (clear)
				{
					FileLog.buffer = new List<string>();
				}
				list2 = list;
			}
			return list2;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00010C00 File Offset: 0x0000EE00
		public static void SetBuffer(List<string> buffer)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer = buffer;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00010C40 File Offset: 0x0000EE40
		public static void FlushBuffer()
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				if (FileLog.buffer.Count > 0)
				{
					using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
					{
						foreach (string text in FileLog.buffer)
						{
							streamWriter.WriteLine(text);
						}
					}
					FileLog.buffer.Clear();
				}
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00010CF4 File Offset: 0x0000EEF4
		public static void Log(string str)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
				{
					streamWriter.WriteLine(FileLog.IndentString() + str);
				}
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00010D60 File Offset: 0x0000EF60
		public static void Reset()
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				File.Delete(string.Format("{0}{1}harmony.log.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.DirectorySeparatorChar));
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00010DB8 File Offset: 0x0000EFB8
		public unsafe static void LogBytes(long ptr, int len)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				byte* ptr2 = ptr;
				string text = "";
				for (int i = 1; i <= len; i++)
				{
					if (text.Length == 0)
					{
						text = "#  ";
					}
					text += string.Format("{0:X2} ", *ptr2);
					if (i > 1 || len == 1)
					{
						if (i % 8 == 0 || i == len)
						{
							FileLog.Log(text);
							text = "";
						}
						else if (i % 4 == 0)
						{
							text += " ";
						}
					}
					ptr2++;
				}
				byte[] array = new byte[len];
				Marshal.Copy((IntPtr)ptr, array, 0, len);
				byte[] array2 = MD5.Create().ComputeHash(array);
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder.Append(array2[j].ToString("X2"));
				}
				FileLog.Log(string.Format("HASH: {0}", stringBuilder));
			}
		}

		// Token: 0x040001D9 RID: 473
		private static readonly object fileLock = new object();

		// Token: 0x040001DA RID: 474
		public static string logPath;

		// Token: 0x040001DB RID: 475
		public static char indentChar = '\t';

		// Token: 0x040001DC RID: 476
		public static int indentLevel = 0;

		// Token: 0x040001DD RID: 477
		private static List<string> buffer = new List<string>();
	}
}
