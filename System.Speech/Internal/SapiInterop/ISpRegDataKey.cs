using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000036 RID: 54
	[Guid("92A66E2B-C830-4149-83DF-6FC2BA1E7A5B")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRegDataKey : ISpDataKey
	{
		// Token: 0x0600015E RID: 350
		[PreserveSig]
		int SetData([MarshalAs(21)] string valueName, uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] data);

		// Token: 0x0600015F RID: 351
		[PreserveSig]
		int GetData([MarshalAs(21)] string valueName, ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x06000160 RID: 352
		[PreserveSig]
		int SetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] string value);

		// Token: 0x06000161 RID: 353
		[PreserveSig]
		int GetStringValue([MarshalAs(21)] string pszValueName, [MarshalAs(21)] out string ppszValue);

		// Token: 0x06000162 RID: 354
		[PreserveSig]
		int SetDWORD([MarshalAs(21)] string valueName, uint dwValue);

		// Token: 0x06000163 RID: 355
		[PreserveSig]
		int GetDWORD([MarshalAs(21)] string pszValueName, ref uint pdwValue);

		// Token: 0x06000164 RID: 356
		[PreserveSig]
		int OpenKey([MarshalAs(21)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000165 RID: 357
		[PreserveSig]
		int CreateKey([MarshalAs(21)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000166 RID: 358
		[PreserveSig]
		int DeleteKey([MarshalAs(21)] string subKey);

		// Token: 0x06000167 RID: 359
		[PreserveSig]
		int DeleteValue([MarshalAs(21)] string valueName);

		// Token: 0x06000168 RID: 360
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(21)] out string ppszSubKeyName);

		// Token: 0x06000169 RID: 361
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(21)] out string ppszValueName);

		// Token: 0x0600016A RID: 362
		[PreserveSig]
		int SetKey(IntPtr hkey, bool fReadOnly);
	}
}
