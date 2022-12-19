using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000114 RID: 276
	[Guid("2D3D3845-39AF-4850-BBF9-40B49780011D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpObjectTokenCategory : ISpDataKey
	{
		// Token: 0x06000988 RID: 2440
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x06000989 RID: 2441
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x0600098A RID: 2442
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x0600098B RID: 2443
		[PreserveSig]
		void GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x0600098C RID: 2444
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x0600098D RID: 2445
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x0600098E RID: 2446
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x0600098F RID: 2447
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000990 RID: 2448
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x06000991 RID: 2449
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x06000992 RID: 2450
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x06000993 RID: 2451
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x06000994 RID: 2452
		void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist);

		// Token: 0x06000995 RID: 2453
		void GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemCategoryId);

		// Token: 0x06000996 RID: 2454
		void Slot14();

		// Token: 0x06000997 RID: 2455
		void EnumTokens([MarshalAs(UnmanagedType.LPWStr)] string pzsReqAttribs, [MarshalAs(UnmanagedType.LPWStr)] string pszOptAttribs, out IEnumSpObjectTokens ppEnum);

		// Token: 0x06000998 RID: 2456
		void Slot16();

		// Token: 0x06000999 RID: 2457
		void GetDefaultTokenId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemTokenId);
	}
}
