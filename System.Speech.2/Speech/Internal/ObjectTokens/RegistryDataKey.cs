using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;
using Microsoft.Win32;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x0200016E RID: 366
	internal class RegistryDataKey : ISpDataKey, IEnumerable<RegistryDataKey>, IEnumerable, IDisposable
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x0002C738 File Offset: 0x0002A938
		protected RegistryDataKey(string fullPath, IntPtr regHandle)
		{
			ISpRegDataKey spRegDataKey = (ISpRegDataKey)new SpDataKey();
			RegistryDataKey.SAPIErrorCodes sapierrorCodes = (RegistryDataKey.SAPIErrorCodes)spRegDataKey.SetKey(regHandle, false);
			if (sapierrorCodes != RegistryDataKey.SAPIErrorCodes.S_OK && sapierrorCodes != RegistryDataKey.SAPIErrorCodes.SPERR_ALREADY_INITIALIZED)
			{
				throw new InvalidOperationException();
			}
			this._sapiRegKey = spRegDataKey;
			this._sKeyId = fullPath;
			this._disposeSapiKey = true;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002C785 File Offset: 0x0002A985
		protected RegistryDataKey(string fullPath, RegistryKey managedRegKey)
			: this(fullPath, RegistryDataKey.HKEYfromRegKey(managedRegKey))
		{
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002C794 File Offset: 0x0002A994
		protected RegistryDataKey(string fullPath, RegistryDataKey copyKey)
		{
			this._sKeyId = fullPath;
			this._sapiRegKey = copyKey._sapiRegKey;
			this._disposeSapiKey = copyKey._disposeSapiKey;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002C7BB File Offset: 0x0002A9BB
		protected RegistryDataKey(string fullPath, ISpDataKey copyKey, bool shouldDispose)
		{
			this._sKeyId = fullPath;
			this._sapiRegKey = copyKey;
			this._disposeSapiKey = shouldDispose;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002C7D8 File Offset: 0x0002A9D8
		protected RegistryDataKey(ISpObjectToken sapiToken)
			: this(RegistryDataKey.GetTokenIdFromToken(sapiToken), sapiToken, false)
		{
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002C7E8 File Offset: 0x0002A9E8
		internal static RegistryDataKey Open(string registryPath, bool fCreateIfNotExist)
		{
			if (string.IsNullOrEmpty(registryPath))
			{
				return null;
			}
			registryPath = registryPath.Trim(new char[] { '\\' });
			string firstKeyAndParseRemainder = RegistryDataKey.GetFirstKeyAndParseRemainder(ref registryPath);
			IntPtr intPtr = RegistryDataKey.RootHKEYFromRegPath(firstKeyAndParseRemainder);
			if (IntPtr.Zero == intPtr)
			{
				return null;
			}
			RegistryDataKey registryDataKey = new RegistryDataKey(firstKeyAndParseRemainder, intPtr);
			if (string.IsNullOrEmpty(registryPath))
			{
				return registryDataKey;
			}
			return RegistryDataKey.OpenSubKey(registryDataKey, registryPath, fCreateIfNotExist);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002C84C File Offset: 0x0002AA4C
		internal static RegistryDataKey Create(string keyId, RegistryKey hkey)
		{
			return new RegistryDataKey(keyId, hkey);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002C858 File Offset: 0x0002AA58
		private static RegistryDataKey OpenSubKey(RegistryDataKey baseKey, string registryPath, bool createIfNotExist)
		{
			if (string.IsNullOrEmpty(registryPath) || baseKey == null)
			{
				return null;
			}
			string firstKeyAndParseRemainder = RegistryDataKey.GetFirstKeyAndParseRemainder(ref registryPath);
			RegistryDataKey registryDataKey = (createIfNotExist ? baseKey.CreateKey(firstKeyAndParseRemainder) : baseKey.OpenKey(firstKeyAndParseRemainder));
			if (string.IsNullOrEmpty(registryPath))
			{
				return registryDataKey;
			}
			return RegistryDataKey.OpenSubKey(registryDataKey, registryPath, createIfNotExist);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002C8A4 File Offset: 0x0002AAA4
		private static string GetTokenIdFromToken(ISpObjectToken sapiToken)
		{
			IntPtr zero = IntPtr.Zero;
			string text;
			try
			{
				sapiToken.GetId(out zero);
				text = Marshal.PtrToStringUni(zero);
			}
			finally
			{
				Marshal.FreeCoTaskMem(zero);
			}
			return text;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002C8E0 File Offset: 0x0002AAE0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002C8EF File Offset: 0x0002AAEF
		[PreserveSig]
		public int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.SysUInt)] uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data)
		{
			return this._sapiRegKey.SetData(valueName, cbData, data);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002C8FF File Offset: 0x0002AAFF
		[PreserveSig]
		public int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.SysUInt)] ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data)
		{
			return this._sapiRegKey.GetData(valueName, ref pcbData, data);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002C90F File Offset: 0x0002AB0F
		[PreserveSig]
		public int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value)
		{
			return this._sapiRegKey.SetStringValue(valueName, value);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002C91E File Offset: 0x0002AB1E
		[PreserveSig]
		public int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] out string value)
		{
			return this._sapiRegKey.GetStringValue(valueName, out value);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002C92D File Offset: 0x0002AB2D
		[PreserveSig]
		public int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.SysUInt)] uint value)
		{
			return this._sapiRegKey.SetDWORD(valueName, value);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002C93C File Offset: 0x0002AB3C
		[PreserveSig]
		public int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pdwValue)
		{
			return this._sapiRegKey.GetDWORD(valueName, ref pdwValue);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002C94B File Offset: 0x0002AB4B
		[PreserveSig]
		public int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string subKeyName, out ISpDataKey ppSubKey)
		{
			return this._sapiRegKey.OpenKey(subKeyName, out ppSubKey);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002C95A File Offset: 0x0002AB5A
		[PreserveSig]
		public int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKeyName, out ISpDataKey ppSubKey)
		{
			return this._sapiRegKey.CreateKey(subKeyName, out ppSubKey);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002C969 File Offset: 0x0002AB69
		[PreserveSig]
		public int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKeyName)
		{
			return this._sapiRegKey.DeleteKey(subKeyName);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002C977 File Offset: 0x0002AB77
		[PreserveSig]
		public int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName)
		{
			return this._sapiRegKey.DeleteValue(valueName);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002C985 File Offset: 0x0002AB85
		[PreserveSig]
		public int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName)
		{
			return this._sapiRegKey.EnumKeys(index, out ppszSubKeyName);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002C994 File Offset: 0x0002AB94
		[PreserveSig]
		public int EnumValues(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string valueName)
		{
			return this._sapiRegKey.EnumValues(index, out valueName);
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0002C9A3 File Offset: 0x0002ABA3
		internal string Id
		{
			get
			{
				return (string)this._sKeyId.Clone();
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0002C9B8 File Offset: 0x0002ABB8
		internal string Name
		{
			get
			{
				int num = this._sKeyId.LastIndexOf('\\');
				return this._sKeyId.Substring(num + 1);
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002C9E1 File Offset: 0x0002ABE1
		internal bool TryGetString(string valueName, out string value)
		{
			if (valueName == null)
			{
				valueName = string.Empty;
			}
			return this.GetStringValue(valueName, out value) == 0;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002C9F8 File Offset: 0x0002ABF8
		internal bool HasValue(string valueName)
		{
			uint num = 0U;
			byte[] array = new byte[0];
			string text;
			return this._sapiRegKey.GetStringValue(valueName, out text) == 0 || this._sapiRegKey.GetDWORD(valueName, ref num) == 0 || this._sapiRegKey.GetData(valueName, ref num, array) == 0;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002CA42 File Offset: 0x0002AC42
		internal bool TryGetDWORD(string valueName, ref uint value)
		{
			return !string.IsNullOrEmpty(valueName) && this._sapiRegKey.GetDWORD(valueName, ref value) == 0;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0002CA60 File Offset: 0x0002AC60
		internal RegistryDataKey OpenKey(string keyName)
		{
			Helpers.ThrowIfEmptyOrNull(keyName, "keyName");
			ISpDataKey spDataKey;
			if (this._sapiRegKey.OpenKey(keyName, out spDataKey) != 0)
			{
				return null;
			}
			return new RegistryDataKey(this._sKeyId + "\\" + keyName, spDataKey, true);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002CAA4 File Offset: 0x0002ACA4
		internal RegistryDataKey CreateKey(string keyName)
		{
			Helpers.ThrowIfEmptyOrNull(keyName, "keyName");
			ISpDataKey spDataKey;
			if (this._sapiRegKey.CreateKey(keyName, out spDataKey) != 0)
			{
				return null;
			}
			return new RegistryDataKey(this._sKeyId + "\\" + keyName, spDataKey, true);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002CAE8 File Offset: 0x0002ACE8
		internal string[] GetValueNames()
		{
			List<string> list = new List<string>();
			uint num = 0U;
			string text;
			while (this._sapiRegKey.EnumValues(num, out text) == 0)
			{
				list.Add(text);
				num += 1U;
			}
			return list.ToArray();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002CB20 File Offset: 0x0002AD20
		IEnumerator<RegistryDataKey> IEnumerable<RegistryDataKey>.GetEnumerator()
		{
			string empty = string.Empty;
			uint i = 0U;
			while (this._sapiRegKey.EnumKeys(i, out empty) == 0)
			{
				yield return this.CreateKey(empty);
				uint num = i;
				i = num + 1U;
			}
			yield break;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002CB2F File Offset: 0x0002AD2F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RegistryDataKey>)this).GetEnumerator();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002CB37 File Offset: 0x0002AD37
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this._sapiRegKey != null && this._disposeSapiKey)
			{
				Marshal.ReleaseComObject(this._sapiRegKey);
				this._sapiRegKey = null;
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002CB60 File Offset: 0x0002AD60
		private static IntPtr HKEYfromRegKey(RegistryKey regKey)
		{
			Type typeFromHandle = typeof(RegistryKey);
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			FieldInfo field = typeFromHandle.GetField("hkey", bindingFlags);
			SafeHandle safeHandle = (SafeHandle)field.GetValue(regKey);
			return safeHandle.DangerousGetHandle();
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002CB9C File Offset: 0x0002AD9C
		private static IntPtr RootHKEYFromRegPath(string rootPath)
		{
			RegistryKey registryKey = RegistryDataKey.RegKeyFromRootPath(rootPath);
			IntPtr intPtr;
			if (registryKey == null)
			{
				intPtr = IntPtr.Zero;
			}
			else
			{
				intPtr = RegistryDataKey.HKEYfromRegKey(registryKey);
			}
			return intPtr;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002CBC4 File Offset: 0x0002ADC4
		private static string GetFirstKeyAndParseRemainder(ref string registryPath)
		{
			int num = registryPath.IndexOf('\\');
			string text;
			if (num >= 0)
			{
				text = registryPath.Substring(0, num);
				registryPath = registryPath.Substring(num + 1, registryPath.Length - num - 1);
			}
			else
			{
				text = registryPath;
				registryPath = string.Empty;
			}
			return text;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002CC10 File Offset: 0x0002AE10
		private static RegistryKey RegKeyFromRootPath(string rootPath)
		{
			RegistryKey[] array = new RegistryKey[]
			{
				Registry.ClassesRoot,
				Registry.LocalMachine,
				Registry.CurrentUser,
				Registry.CurrentConfig
			};
			foreach (RegistryKey registryKey in array)
			{
				if (registryKey.Name.Equals(rootPath, StringComparison.OrdinalIgnoreCase))
				{
					return registryKey;
				}
			}
			return null;
		}

		// Token: 0x0400083E RID: 2110
		internal string _sKeyId;

		// Token: 0x0400083F RID: 2111
		internal ISpDataKey _sapiRegKey;

		// Token: 0x04000840 RID: 2112
		internal bool _disposeSapiKey;

		// Token: 0x020001D4 RID: 468
		internal enum SAPIErrorCodes
		{
			// Token: 0x04000A0D RID: 2573
			STG_E_FILENOTFOUND = -2147287038,
			// Token: 0x04000A0E RID: 2574
			SPERR_ALREADY_INITIALIZED = -2147201022,
			// Token: 0x04000A0F RID: 2575
			SPERR_UNSUPPORTED_FORMAT,
			// Token: 0x04000A10 RID: 2576
			SPERR_DEVICE_BUSY = -2147201018,
			// Token: 0x04000A11 RID: 2577
			SPERR_DEVICE_NOT_SUPPORTED,
			// Token: 0x04000A12 RID: 2578
			SPERR_DEVICE_NOT_ENABLED,
			// Token: 0x04000A13 RID: 2579
			SPERR_NO_DRIVER,
			// Token: 0x04000A14 RID: 2580
			SPERR_TOO_MANY_GRAMMARS = -2147200990,
			// Token: 0x04000A15 RID: 2581
			SPERR_INVALID_IMPORT = -2147200988,
			// Token: 0x04000A16 RID: 2582
			SPERR_AUDIO_BUFFER_OVERFLOW = -2147200977,
			// Token: 0x04000A17 RID: 2583
			SPERR_NO_AUDIO_DATA,
			// Token: 0x04000A18 RID: 2584
			SPERR_NO_MORE_ITEMS = -2147200967,
			// Token: 0x04000A19 RID: 2585
			SPERR_NOT_FOUND,
			// Token: 0x04000A1A RID: 2586
			SPERR_GENERIC_MMSYS_ERROR = -2147200964,
			// Token: 0x04000A1B RID: 2587
			SPERR_NOT_TOPLEVEL_RULE = -2147200940,
			// Token: 0x04000A1C RID: 2588
			SPERR_NOT_ACTIVE_SESSION = -2147200925,
			// Token: 0x04000A1D RID: 2589
			SPERR_SML_GENERATION_FAIL = -2147200921,
			// Token: 0x04000A1E RID: 2590
			SPERR_SHARED_ENGINE_DISABLED = -2147200906,
			// Token: 0x04000A1F RID: 2591
			SPERR_RECOGNIZER_NOT_FOUND,
			// Token: 0x04000A20 RID: 2592
			SPERR_AUDIO_NOT_FOUND,
			// Token: 0x04000A21 RID: 2593
			S_OK = 0,
			// Token: 0x04000A22 RID: 2594
			S_FALSE,
			// Token: 0x04000A23 RID: 2595
			E_INVALIDARG = -2147024809,
			// Token: 0x04000A24 RID: 2596
			SP_NO_RULES_TO_ACTIVATE = 282747,
			// Token: 0x04000A25 RID: 2597
			ERROR_MORE_DATA = 20714
		}
	}
}
