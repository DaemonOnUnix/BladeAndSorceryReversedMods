using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000110 RID: 272
	[Guid("14056589-E16C-11D2-BB90-00C04F8EE6C0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpObjectToken : ISpDataKey
	{
		// Token: 0x06000957 RID: 2391
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pData);

		// Token: 0x06000958 RID: 2392
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pData);

		// Token: 0x06000959 RID: 2393
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] string pszValue);

		// Token: 0x0600095A RID: 2394
		[PreserveSig]
		int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x0600095B RID: 2395
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, uint dwValue);

		// Token: 0x0600095C RID: 2396
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x0600095D RID: 2397
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x0600095E RID: 2398
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKey, out ISpDataKey ppSubKey);

		// Token: 0x0600095F RID: 2399
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKey);

		// Token: 0x06000960 RID: 2400
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName);

		// Token: 0x06000961 RID: 2401
		[PreserveSig]
		int EnumKeys(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x06000962 RID: 2402
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x06000963 RID: 2403
		void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.LPWStr)] string pszTokenId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist);

		// Token: 0x06000964 RID: 2404
		void GetId(out IntPtr ppszCoMemTokenId);

		// Token: 0x06000965 RID: 2405
		void Slot15();

		// Token: 0x06000966 RID: 2406
		void Slot16();

		// Token: 0x06000967 RID: 2407
		void Slot17();

		// Token: 0x06000968 RID: 2408
		void Slot18();

		// Token: 0x06000969 RID: 2409
		void Slot19();

		// Token: 0x0600096A RID: 2410
		void Slot20();

		// Token: 0x0600096B RID: 2411
		void Slot21();

		// Token: 0x0600096C RID: 2412
		void MatchesAttributes([MarshalAs(UnmanagedType.LPWStr)] string pszAttributes, [MarshalAs(UnmanagedType.Bool)] out bool pfMatches);
	}
}
