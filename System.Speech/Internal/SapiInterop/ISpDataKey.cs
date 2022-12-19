using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000020 RID: 32
	[InterfaceType(1)]
	[Guid("14056581-E16C-11D2-BB90-00C04F8EE6C0")]
	[ComImport]
	internal interface ISpDataKey
	{
		// Token: 0x060000AF RID: 175
		[PreserveSig]
		int SetData([MarshalAs(21)] string valueName, uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] data);

		// Token: 0x060000B0 RID: 176
		[PreserveSig]
		int GetData([MarshalAs(21)] string valueName, ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x060000B1 RID: 177
		[PreserveSig]
		int SetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] string value);

		// Token: 0x060000B2 RID: 178
		[PreserveSig]
		int GetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] out string value);

		// Token: 0x060000B3 RID: 179
		[PreserveSig]
		int SetDWORD([MarshalAs(21)] string valueName, uint dwValue);

		// Token: 0x060000B4 RID: 180
		[PreserveSig]
		int GetDWORD([MarshalAs(21)] string valueName, ref uint pdwValue);

		// Token: 0x060000B5 RID: 181
		[PreserveSig]
		int OpenKey([MarshalAs(21)] string subKeyName, out ISpDataKey ppSubKey);

		// Token: 0x060000B6 RID: 182
		[PreserveSig]
		int CreateKey([MarshalAs(21)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x060000B7 RID: 183
		[PreserveSig]
		int DeleteKey([MarshalAs(21)] string subKey);

		// Token: 0x060000B8 RID: 184
		[PreserveSig]
		int DeleteValue([MarshalAs(21)] string valueName);

		// Token: 0x060000B9 RID: 185
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(21)] out string ppszSubKeyName);

		// Token: 0x060000BA RID: 186
		[PreserveSig]
		int EnumValues(uint index, [MarshalAs(21)] out string valueName);
	}
}
