using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000037 RID: 55
	[Guid("2D3D3845-39AF-4850-BBF9-40B49780011D")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpObjectTokenCategory : ISpDataKey
	{
		// Token: 0x0600016B RID: 363
		[PreserveSig]
		int SetData([MarshalAs(21)] string valueName, uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] data);

		// Token: 0x0600016C RID: 364
		[PreserveSig]
		int GetData([MarshalAs(21)] string valueName, ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x0600016D RID: 365
		[PreserveSig]
		int SetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] string value);

		// Token: 0x0600016E RID: 366
		[PreserveSig]
		void GetStringValue([MarshalAs(21)] string pszValueName, [MarshalAs(21)] out string ppszValue);

		// Token: 0x0600016F RID: 367
		[PreserveSig]
		int SetDWORD([MarshalAs(21)] string valueName, uint dwValue);

		// Token: 0x06000170 RID: 368
		[PreserveSig]
		int GetDWORD([MarshalAs(21)] string pszValueName, ref uint pdwValue);

		// Token: 0x06000171 RID: 369
		[PreserveSig]
		int OpenKey([MarshalAs(21)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000172 RID: 370
		[PreserveSig]
		int CreateKey([MarshalAs(21)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000173 RID: 371
		[PreserveSig]
		int DeleteKey([MarshalAs(21)] string subKey);

		// Token: 0x06000174 RID: 372
		[PreserveSig]
		int DeleteValue([MarshalAs(21)] string valueName);

		// Token: 0x06000175 RID: 373
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(21)] out string ppszSubKeyName);

		// Token: 0x06000176 RID: 374
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(21)] out string ppszValueName);

		// Token: 0x06000177 RID: 375
		void SetId([MarshalAs(21)] string pszCategoryId, [MarshalAs(2)] bool fCreateIfNotExist);

		// Token: 0x06000178 RID: 376
		void GetId([MarshalAs(21)] out string ppszCoMemCategoryId);

		// Token: 0x06000179 RID: 377
		void Slot14();

		// Token: 0x0600017A RID: 378
		void EnumTokens([MarshalAs(21)] string pzsReqAttribs, [MarshalAs(21)] string pszOptAttribs, out IEnumSpObjectTokens ppEnum);

		// Token: 0x0600017B RID: 379
		void Slot16();

		// Token: 0x0600017C RID: 380
		void GetDefaultTokenId([MarshalAs(21)] out string ppszCoMemTokenId);
	}
}
