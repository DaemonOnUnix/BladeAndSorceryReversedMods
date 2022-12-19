using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000023 RID: 35
	[InterfaceType(1)]
	[Guid("14056589-E16C-11D2-BB90-00C04F8EE6C0")]
	[ComImport]
	internal interface ISpObjectToken : ISpDataKey
	{
		// Token: 0x060000E4 RID: 228
		[PreserveSig]
		int SetData([MarshalAs(21)] string pszValueName, uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] pData);

		// Token: 0x060000E5 RID: 229
		[PreserveSig]
		int GetData([MarshalAs(21)] string pszValueName, ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] pData);

		// Token: 0x060000E6 RID: 230
		[PreserveSig]
		int SetStringValue([MarshalAs(21)] string pszValueName, [MarshalAs(21)] string pszValue);

		// Token: 0x060000E7 RID: 231
		[PreserveSig]
		int GetStringValue([MarshalAs(21)] string pszValueName, [MarshalAs(21)] out string ppszValue);

		// Token: 0x060000E8 RID: 232
		[PreserveSig]
		int SetDWORD([MarshalAs(21)] string pszValueName, uint dwValue);

		// Token: 0x060000E9 RID: 233
		[PreserveSig]
		int GetDWORD([MarshalAs(21)] string pszValueName, ref uint pdwValue);

		// Token: 0x060000EA RID: 234
		[PreserveSig]
		int OpenKey([MarshalAs(21)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x060000EB RID: 235
		[PreserveSig]
		int CreateKey([MarshalAs(21)] string pszSubKey, out ISpDataKey ppSubKey);

		// Token: 0x060000EC RID: 236
		[PreserveSig]
		int DeleteKey([MarshalAs(21)] string pszSubKey);

		// Token: 0x060000ED RID: 237
		[PreserveSig]
		int DeleteValue([MarshalAs(21)] string pszValueName);

		// Token: 0x060000EE RID: 238
		[PreserveSig]
		int EnumKeys(uint Index, [MarshalAs(21)] out string ppszSubKeyName);

		// Token: 0x060000EF RID: 239
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(21)] out string ppszValueName);

		// Token: 0x060000F0 RID: 240
		void SetId([MarshalAs(21)] string pszCategoryId, [MarshalAs(21)] string pszTokenId, [MarshalAs(2)] bool fCreateIfNotExist);

		// Token: 0x060000F1 RID: 241
		void GetId(out IntPtr ppszCoMemTokenId);

		// Token: 0x060000F2 RID: 242
		void Slot15();

		// Token: 0x060000F3 RID: 243
		void Slot16();

		// Token: 0x060000F4 RID: 244
		void Slot17();

		// Token: 0x060000F5 RID: 245
		void Slot18();

		// Token: 0x060000F6 RID: 246
		void Slot19();

		// Token: 0x060000F7 RID: 247
		void Slot20();

		// Token: 0x060000F8 RID: 248
		void Slot21();

		// Token: 0x060000F9 RID: 249
		void MatchesAttributes([MarshalAs(21)] string pszAttributes, [MarshalAs(2)] out bool pfMatches);
	}
}
