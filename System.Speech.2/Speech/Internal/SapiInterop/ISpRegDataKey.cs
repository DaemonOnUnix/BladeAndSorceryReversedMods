using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000113 RID: 275
	[Guid("92A66E2B-C830-4149-83DF-6FC2BA1E7A5B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRegDataKey : ISpDataKey
	{
		// Token: 0x0600097B RID: 2427
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x0600097C RID: 2428
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x0600097D RID: 2429
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x0600097E RID: 2430
		[PreserveSig]
		int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x0600097F RID: 2431
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x06000980 RID: 2432
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x06000981 RID: 2433
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000982 RID: 2434
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000983 RID: 2435
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x06000984 RID: 2436
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x06000985 RID: 2437
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x06000986 RID: 2438
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x06000987 RID: 2439
		[PreserveSig]
		int SetKey(IntPtr hkey, bool fReadOnly);
	}
}
