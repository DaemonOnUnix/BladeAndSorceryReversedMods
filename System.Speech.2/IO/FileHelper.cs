using System;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x02000002 RID: 2
	internal static class FileHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[SecurityCritical]
		internal static FileStream CreateAndOpenTemporaryFile(out string filePath, FileAccess fileAccess = FileAccess.Write, FileOptions fileOptions = FileOptions.None, string extension = null, string subFolder = "WPF")
		{
			int num = 5;
			filePath = null;
			bool flag = SecurityManager.CurrentThreadRequiresSecurityContextCapture();
			string text = Path.GetTempPath();
			if (!string.IsNullOrEmpty(subFolder))
			{
				string text2 = Path.Combine(text, subFolder);
				if (!Directory.Exists(text2))
				{
					if (!flag)
					{
						Directory.CreateDirectory(text2);
					}
					else
					{
						new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, text).Assert();
						Directory.CreateDirectory(text2);
						CodeAccessPermission.RevertAssert();
					}
				}
				text = text2;
			}
			if (flag)
			{
				new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, text).Assert();
			}
			FileStream fileStream = null;
			while (fileStream == null)
			{
				string text3 = Path.Combine(text, Path.GetRandomFileName());
				if (!string.IsNullOrEmpty(extension))
				{
					text3 = Path.ChangeExtension(text3, extension);
				}
				num--;
				try
				{
					fileStream = new FileStream(text3, FileMode.CreateNew, fileAccess, FileShare.None, 4096, fileOptions);
					filePath = text3;
				}
				catch (Exception ex) when (num > 0 && (ex is IOException || ex is UnauthorizedAccessException))
				{
				}
			}
			return fileStream;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002148 File Offset: 0x00000348
		[SecurityCritical]
		internal static void DeleteTemporaryFile(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				bool flag = SecurityManager.CurrentThreadRequiresSecurityContextCapture();
				if (flag)
				{
					new FileIOPermission(FileIOPermissionAccess.Write, filePath).Assert();
				}
				try
				{
					File.Delete(filePath);
				}
				catch (IOException)
				{
				}
			}
		}
	}
}
