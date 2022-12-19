using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000065 RID: 101
	[Guid("2D3D3845-39AF-4850-BBF9-40B49780011D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpObjectTokenCategory : ISpDataKey
	{
		// Token: 0x060002C9 RID: 713
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x060002CA RID: 714
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x060002CB RID: 715
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x060002CC RID: 716
		[PreserveSig]
		void GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x060002CD RID: 717
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x060002CE RID: 718
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x060002CF RID: 719
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x060002D0 RID: 720
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x060002D1 RID: 721
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x060002D2 RID: 722
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x060002D3 RID: 723
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x060002D4 RID: 724
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x060002D5 RID: 725
		void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist);

		// Token: 0x060002D6 RID: 726
		void GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemCategoryId);

		// Token: 0x060002D7 RID: 727
		void Slot14();

		// Token: 0x060002D8 RID: 728
		void EnumTokens([MarshalAs(UnmanagedType.LPWStr)] string pzsReqAttribs, [MarshalAs(UnmanagedType.LPWStr)] string pszOptAttribs, out IEnumSpObjectTokens ppEnum);

		// Token: 0x060002D9 RID: 729
		void Slot16();

		// Token: 0x060002DA RID: 730
		void GetDefaultTokenId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemTokenId);
	}
}
