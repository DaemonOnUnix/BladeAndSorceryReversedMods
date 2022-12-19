using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000112 RID: 274
	[Guid("14056581-E16C-11D2-BB90-00C04F8EE6C0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpDataKey
	{
		// Token: 0x0600096F RID: 2415
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x06000970 RID: 2416
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x06000971 RID: 2417
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x06000972 RID: 2418
		[PreserveSig]
		int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] out string value);

		// Token: 0x06000973 RID: 2419
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x06000974 RID: 2420
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pdwValue);

		// Token: 0x06000975 RID: 2421
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string subKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000976 RID: 2422
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000977 RID: 2423
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x06000978 RID: 2424
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x06000979 RID: 2425
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x0600097A RID: 2426
		[PreserveSig]
		int EnumValues(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string valueName);
	}
}
