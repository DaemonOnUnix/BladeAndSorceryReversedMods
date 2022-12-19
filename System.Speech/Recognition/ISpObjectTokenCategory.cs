using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000199 RID: 409
	[Guid("2D3D3845-39AF-4850-BBF9-40B49780011D")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpObjectTokenCategory : ISpDataKey
	{
		// Token: 0x06000ABB RID: 2747
		[PreserveSig]
		int SetData([MarshalAs(21)] string valueName, uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] data);

		// Token: 0x06000ABC RID: 2748
		[PreserveSig]
		int GetData([MarshalAs(21)] string valueName, ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x06000ABD RID: 2749
		[PreserveSig]
		int SetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] string value);

		// Token: 0x06000ABE RID: 2750
		[PreserveSig]
		void GetStringValue([MarshalAs(21)] string pszValueName, [MarshalAs(21)] out string ppszValue);

		// Token: 0x06000ABF RID: 2751
		[PreserveSig]
		int SetDWORD([MarshalAs(21)] string valueName, uint dwValue);

		// Token: 0x06000AC0 RID: 2752
		[PreserveSig]
		int GetDWORD([MarshalAs(21)] string pszValueName, ref uint pdwValue);

		// Token: 0x06000AC1 RID: 2753
		[PreserveSig]
		int OpenKey([MarshalAs(21)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000AC2 RID: 2754
		[PreserveSig]
		int CreateKey([MarshalAs(21)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000AC3 RID: 2755
		[PreserveSig]
		int DeleteKey([MarshalAs(21)] string subKey);

		// Token: 0x06000AC4 RID: 2756
		[PreserveSig]
		int DeleteValue([MarshalAs(21)] string valueName);

		// Token: 0x06000AC5 RID: 2757
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(21)] out string ppszSubKeyName);

		// Token: 0x06000AC6 RID: 2758
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(21)] out string ppszValueName);

		// Token: 0x06000AC7 RID: 2759
		void SetId([MarshalAs(21)] string pszCategoryId, [MarshalAs(2)] bool fCreateIfNotExist);

		// Token: 0x06000AC8 RID: 2760
		void GetId([MarshalAs(21)] out string ppszCoMemCategoryId);

		// Token: 0x06000AC9 RID: 2761
		void Slot14();

		// Token: 0x06000ACA RID: 2762
		void EnumTokens([MarshalAs(21)] string pzsReqAttribs, [MarshalAs(21)] string pszOptAttribs, out IEnumSpObjectTokens ppEnum);

		// Token: 0x06000ACB RID: 2763
		void Slot16();

		// Token: 0x06000ACC RID: 2764
		void GetDefaultTokenId([MarshalAs(21)] out string ppszCoMemTokenId);
	}
}
