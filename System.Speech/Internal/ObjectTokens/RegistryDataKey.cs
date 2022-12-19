using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;
using Microsoft.Win32;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x02000021 RID: 33
	internal class RegistryDataKey : ISpDataKey, IEnumerable<RegistryDataKey>, IEnumerable, IDisposable
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00006A88 File Offset: 0x00005A88
		protected RegistryDataKey(string fullPath, IntPtr regHandle)
		{
			ISpRegDataKey spRegDataKey = (ISpRegDataKey)new SpDataKey();
			spRegDataKey.SetKey(regHandle, false);
			this._sapiRegKey = spRegDataKey;
			this._sKeyId = fullPath;
			this._disposeSapiKey = true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006AC4 File Offset: 0x00005AC4
		protected RegistryDataKey(string fullPath, RegistryKey managedRegKey)
			: this(fullPath, RegistryDataKey.HKEYfromRegKey(managedRegKey))
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006AD3 File Offset: 0x00005AD3
		protected RegistryDataKey(string fullPath, RegistryDataKey copyKey)
		{
			this._sKeyId = fullPath;
			this._sapiRegKey = copyKey._sapiRegKey;
			this._disposeSapiKey = copyKey._disposeSapiKey;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006AFA File Offset: 0x00005AFA
		protected RegistryDataKey(string fullPath, ISpDataKey copyKey, bool shouldDispose)
		{
			this._sKeyId = fullPath;
			this._sapiRegKey = copyKey;
			this._disposeSapiKey = shouldDispose;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006B17 File Offset: 0x00005B17
		protected RegistryDataKey(ISpObjectToken sapiToken)
			: this(RegistryDataKey.GetTokenIdFromToken(sapiToken), sapiToken, false)
		{
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006B28 File Offset: 0x00005B28
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00006B91 File Offset: 0x00005B91
		internal static RegistryDataKey Create(string keyId, RegistryKey hkey)
		{
			return new RegistryDataKey(keyId, hkey);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006B9C File Offset: 0x00005B9C
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

		// Token: 0x060000C3 RID: 195 RVA: 0x00006BE8 File Offset: 0x00005BE8
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

		// Token: 0x060000C4 RID: 196 RVA: 0x00006C24 File Offset: 0x00005C24
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006C33 File Offset: 0x00005C33
		[PreserveSig]
		public int SetData([MarshalAs(21)] string valueName, [MarshalAs(32)] uint cbData, [MarshalAs(42, SizeParamIndex = 1)] byte[] data)
		{
			return this._sapiRegKey.SetData(valueName, cbData, data);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006C43 File Offset: 0x00005C43
		[PreserveSig]
		public int GetData([MarshalAs(21)] string valueName, [MarshalAs(32)] ref uint pcbData, [MarshalAs(42, SizeParamIndex = 1)] [Out] byte[] data)
		{
			return this._sapiRegKey.GetData(valueName, ref pcbData, data);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006C53 File Offset: 0x00005C53
		[PreserveSig]
		public int SetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] string value)
		{
			return this._sapiRegKey.SetStringValue(valueName, value);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006C62 File Offset: 0x00005C62
		[PreserveSig]
		public int GetStringValue([MarshalAs(21)] string valueName, [MarshalAs(21)] out string value)
		{
			return this._sapiRegKey.GetStringValue(valueName, out value);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006C71 File Offset: 0x00005C71
		[PreserveSig]
		public int SetDWORD([MarshalAs(21)] string valueName, [MarshalAs(32)] uint value)
		{
			return this._sapiRegKey.SetDWORD(valueName, value);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006C80 File Offset: 0x00005C80
		[PreserveSig]
		public int GetDWORD([MarshalAs(21)] string valueName, ref uint pdwValue)
		{
			return this._sapiRegKey.GetDWORD(valueName, ref pdwValue);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006C8F File Offset: 0x00005C8F
		[PreserveSig]
		public int OpenKey([MarshalAs(21)] string subKeyName, out ISpDataKey ppSubKey)
		{
			return this._sapiRegKey.OpenKey(subKeyName, out ppSubKey);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006C9E File Offset: 0x00005C9E
		[PreserveSig]
		public int CreateKey([MarshalAs(21)] string subKeyName, out ISpDataKey ppSubKey)
		{
			return this._sapiRegKey.CreateKey(subKeyName, out ppSubKey);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006CAD File Offset: 0x00005CAD
		[PreserveSig]
		public int DeleteKey([MarshalAs(21)] string subKeyName)
		{
			return this._sapiRegKey.DeleteKey(subKeyName);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006CBB File Offset: 0x00005CBB
		[PreserveSig]
		public int DeleteValue([MarshalAs(21)] string valueName)
		{
			return this._sapiRegKey.DeleteValue(valueName);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006CC9 File Offset: 0x00005CC9
		[PreserveSig]
		public int EnumKeys(uint index, [MarshalAs(21)] out string ppszSubKeyName)
		{
			return this._sapiRegKey.EnumKeys(index, out ppszSubKeyName);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006CD8 File Offset: 0x00005CD8
		[PreserveSig]
		public int EnumValues(uint index, [MarshalAs(21)] out string valueName)
		{
			return this._sapiRegKey.EnumValues(index, out valueName);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00006CE7 File Offset: 0x00005CE7
		internal string Id
		{
			get
			{
				return (string)this._sKeyId.Clone();
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006CFC File Offset: 0x00005CFC
		internal string Name
		{
			get
			{
				int num = this._sKeyId.LastIndexOf('\\');
				return this._sKeyId.Substring(num + 1);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006D25 File Offset: 0x00005D25
		internal bool SetString(string valueName, string sValue)
		{
			return 0 == this.SetStringValue(valueName, sValue);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006D32 File Offset: 0x00005D32
		internal bool TryOpenKey(string keyName, out RegistryDataKey subKey)
		{
			if (string.IsNullOrEmpty(keyName))
			{
				subKey = null;
				return false;
			}
			subKey = this.OpenKey(keyName);
			return null != subKey;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006D52 File Offset: 0x00005D52
		internal bool TryGetString(string valueName, out string value)
		{
			if (valueName == null)
			{
				valueName = string.Empty;
			}
			return 0 == this.GetStringValue(valueName, out value);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006D6C File Offset: 0x00005D6C
		internal bool HasValue(string valueName)
		{
			uint num = 0U;
			byte[] array = new byte[0];
			string text;
			return this._sapiRegKey.GetStringValue(valueName, out text) == 0 || this._sapiRegKey.GetDWORD(valueName, ref num) == 0 || 0 == this._sapiRegKey.GetData(valueName, ref num, array);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006DB6 File Offset: 0x00005DB6
		internal bool TryGetDWORD(string valueName, ref uint value)
		{
			return !string.IsNullOrEmpty(valueName) && 0 == this._sapiRegKey.GetDWORD(valueName, ref value);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006DD4 File Offset: 0x00005DD4
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

		// Token: 0x060000D9 RID: 217 RVA: 0x00006E18 File Offset: 0x00005E18
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

		// Token: 0x060000DA RID: 218 RVA: 0x00006E5C File Offset: 0x00005E5C
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

		// Token: 0x060000DB RID: 219 RVA: 0x00006F50 File Offset: 0x00005F50
		IEnumerator<RegistryDataKey> IEnumerable<RegistryDataKey>.GetEnumerator()
		{
			string childKeyName = string.Empty;
			uint i = 0U;
			while (this._sapiRegKey.EnumKeys(i, out childKeyName) == 0)
			{
				yield return this.CreateKey(childKeyName);
				i += 1U;
			}
			yield break;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006F6C File Offset: 0x00005F6C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006F74 File Offset: 0x00005F74
		internal bool DeleteSubKeyTree(string childKeyName)
		{
			using (RegistryDataKey registryDataKey = this.OpenKey(childKeyName))
			{
				if (registryDataKey == null)
				{
					return true;
				}
				if (!registryDataKey.DeleteSubKeys())
				{
					return false;
				}
			}
			return 0 == this.DeleteKey(childKeyName);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006FC4 File Offset: 0x00005FC4
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this._sapiRegKey != null && this._disposeSapiKey)
			{
				Marshal.ReleaseComObject(this._sapiRegKey);
				this._sapiRegKey = null;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006FEC File Offset: 0x00005FEC
		private static IntPtr HKEYfromRegKey(RegistryKey regKey)
		{
			Type typeFromHandle = typeof(RegistryKey);
			BindingFlags bindingFlags = 36;
			FieldInfo field = typeFromHandle.GetField("hkey", bindingFlags);
			SafeHandle safeHandle = (SafeHandle)field.GetValue(regKey);
			return safeHandle.DangerousGetHandle();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007028 File Offset: 0x00006028
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

		// Token: 0x060000E1 RID: 225 RVA: 0x00007050 File Offset: 0x00006050
		private static string GetFirstKeyAndParseRemainder(ref string registryPath)
		{
			registryPath.Trim(new char[] { '\\' });
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

		// Token: 0x060000E2 RID: 226 RVA: 0x000070B0 File Offset: 0x000060B0
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
				if (registryKey.Name.Equals(rootPath, 3))
				{
					return registryKey;
				}
			}
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007118 File Offset: 0x00006118
		private bool DeleteSubKeys()
		{
			string text;
			while (this.EnumKeys(0U, out text) == 0)
			{
				using (RegistryDataKey registryDataKey = this.OpenKey(text))
				{
					if (registryDataKey == null)
					{
						return false;
					}
					if (!registryDataKey.DeleteSubKeys())
					{
						return false;
					}
				}
				if (this.DeleteKey(text) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000AC RID: 172
		internal string _sKeyId;

		// Token: 0x040000AD RID: 173
		internal ISpDataKey _sapiRegKey;

		// Token: 0x040000AE RID: 174
		internal bool _disposeSapiKey;

		// Token: 0x02000022 RID: 34
		internal enum SAPIErrorCodes
		{
			// Token: 0x040000B0 RID: 176
			STG_E_FILENOTFOUND = -2147287038,
			// Token: 0x040000B1 RID: 177
			SPERR_UNSUPPORTED_FORMAT = -2147201021,
			// Token: 0x040000B2 RID: 178
			SPERR_DEVICE_BUSY = -2147201018,
			// Token: 0x040000B3 RID: 179
			SPERR_DEVICE_NOT_SUPPORTED,
			// Token: 0x040000B4 RID: 180
			SPERR_DEVICE_NOT_ENABLED,
			// Token: 0x040000B5 RID: 181
			SPERR_NO_DRIVER,
			// Token: 0x040000B6 RID: 182
			SPERR_TOO_MANY_GRAMMARS = -2147200990,
			// Token: 0x040000B7 RID: 183
			SPERR_INVALID_IMPORT = -2147200988,
			// Token: 0x040000B8 RID: 184
			SPERR_AUDIO_BUFFER_OVERFLOW = -2147200977,
			// Token: 0x040000B9 RID: 185
			SPERR_NO_AUDIO_DATA,
			// Token: 0x040000BA RID: 186
			SPERR_NO_MORE_ITEMS = -2147200967,
			// Token: 0x040000BB RID: 187
			SPERR_NOT_FOUND,
			// Token: 0x040000BC RID: 188
			SPERR_GENERIC_MMSYS_ERROR = -2147200964,
			// Token: 0x040000BD RID: 189
			SPERR_NOT_TOPLEVEL_RULE = -2147200940,
			// Token: 0x040000BE RID: 190
			SPERR_NOT_ACTIVE_SESSION = -2147200925,
			// Token: 0x040000BF RID: 191
			SPERR_SML_GENERATION_FAIL = -2147200921,
			// Token: 0x040000C0 RID: 192
			SPERR_SHARED_ENGINE_DISABLED = -2147200906,
			// Token: 0x040000C1 RID: 193
			SPERR_RECOGNIZER_NOT_FOUND,
			// Token: 0x040000C2 RID: 194
			SPERR_AUDIO_NOT_FOUND,
			// Token: 0x040000C3 RID: 195
			S_OK = 0,
			// Token: 0x040000C4 RID: 196
			S_FALSE,
			// Token: 0x040000C5 RID: 197
			E_INVALIDARG = -2147024809,
			// Token: 0x040000C6 RID: 198
			SP_NO_RULES_TO_ACTIVATE = 282747,
			// Token: 0x040000C7 RID: 199
			ERROR_MORE_DATA = 20714
		}
	}
}
