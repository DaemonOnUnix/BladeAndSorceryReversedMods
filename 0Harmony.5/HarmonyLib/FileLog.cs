using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x02000191 RID: 401
	public static class FileLog
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x00016538 File Offset: 0x00014738
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

		// Token: 0x06000689 RID: 1673 RVA: 0x0001659C File Offset: 0x0001479C
		private static string IndentString()
		{
			return new string(FileLog.indentChar, FileLog.indentLevel);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000165B0 File Offset: 0x000147B0
		public static void ChangeIndent(int delta)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.indentLevel = Math.Max(0, FileLog.indentLevel + delta);
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000165FC File Offset: 0x000147FC
		public static void LogBuffered(string str)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.Add(FileLog.IndentString() + str);
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001664C File Offset: 0x0001484C
		public static void LogBuffered(List<string> strings)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer.AddRange(strings);
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00016690 File Offset: 0x00014890
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

		// Token: 0x0600068E RID: 1678 RVA: 0x000166E0 File Offset: 0x000148E0
		public static void SetBuffer(List<string> buffer)
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				FileLog.buffer = buffer;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00016720 File Offset: 0x00014920
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

		// Token: 0x06000690 RID: 1680 RVA: 0x000167D4 File Offset: 0x000149D4
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

		// Token: 0x06000691 RID: 1681 RVA: 0x00016840 File Offset: 0x00014A40
		public static void Debug(string str)
		{
			if (Harmony.DEBUG)
			{
				FileLog.Log(str);
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00016850 File Offset: 0x00014A50
		public static void Reset()
		{
			object obj = FileLog.fileLock;
			lock (obj)
			{
				File.Delete(string.Format("{0}{1}harmony.log.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.DirectorySeparatorChar));
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000168A8 File Offset: 0x00014AA8
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

		// Token: 0x04000205 RID: 517
		private static readonly object fileLock = new object();

		// Token: 0x04000206 RID: 518
		public static string logPath;

		// Token: 0x04000207 RID: 519
		public static char indentChar = '\t';

		// Token: 0x04000208 RID: 520
		public static int indentLevel = 0;

		// Token: 0x04000209 RID: 521
		private static List<string> buffer = new List<string>();
	}
}
