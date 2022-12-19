using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Internal.Synthesis;
using Microsoft.Win32;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x0200016F RID: 367
	internal static class SAPICategories
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x0002CC6C File Offset: 0x0002AE6C
		internal static ObjectToken DefaultToken(string category)
		{
			Helpers.ThrowIfEmptyOrNull(category, "category");
			ObjectToken objectToken = SAPICategories.DefaultToken("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Speech\\" + category, "DefaultTokenId");
			if (objectToken == null)
			{
				objectToken = SAPICategories.DefaultToken("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\" + category, "DefaultTokenId");
			}
			return objectToken;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002CCB8 File Offset: 0x0002AEB8
		internal static int DefaultDeviceOut()
		{
			int num = -1;
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Speech\\AudioOutput"))
			{
				string text;
				if (objectTokenCategory != null && objectTokenCategory.TryGetString("DefaultTokenId", out text))
				{
					int num2 = text.IndexOf('\\');
					if (num2 > 0 && num2 < text.Length)
					{
						using (RegistryDataKey registryDataKey = RegistryDataKey.Create(text.Substring(num2 + 1), Registry.LocalMachine))
						{
							if (registryDataKey != null)
							{
								num = AudioDeviceOut.GetDevicedId(registryDataKey.Name);
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002CD58 File Offset: 0x0002AF58
		private static ObjectToken DefaultToken(string category, string defaultTokenIdValueName)
		{
			ObjectToken objectToken = SAPICategories.GetPreference(category, defaultTokenIdValueName);
			if (objectToken != null)
			{
				using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create(category))
				{
					if (objectTokenCategory != null)
					{
						if (objectToken != null)
						{
							using (IEnumerator<ObjectToken> enumerator = ((IEnumerable<ObjectToken>)objectTokenCategory).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									ObjectToken objectToken2 = enumerator.Current;
									objectToken = SAPICategories.GetHighestTokenVersion(objectToken, objectToken2, SAPICategories.asVersionDefault);
								}
								return objectToken;
							}
						}
						string[] array = new string[] { string.Format(CultureInfo.InvariantCulture, "{0:x}", new object[] { CultureInfo.CurrentUICulture.LCID }) };
						foreach (ObjectToken objectToken3 in ((IEnumerable<ObjectToken>)objectTokenCategory))
						{
							if (objectToken3.MatchesAttributes(array))
							{
								objectToken = objectToken3;
								break;
							}
						}
						if (objectToken == null)
						{
							using (IEnumerator<ObjectToken> enumerator3 = ((IEnumerable<ObjectToken>)objectTokenCategory).GetEnumerator())
							{
								if (enumerator3.MoveNext())
								{
									ObjectToken objectToken4 = enumerator3.Current;
									objectToken = objectToken4;
								}
							}
						}
					}
				}
			}
			return objectToken;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002CE98 File Offset: 0x0002B098
		private static ObjectToken GetPreference(string category, string defaultLocation)
		{
			ObjectToken objectToken = null;
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create(category))
			{
				string text;
				if (objectTokenCategory != null && objectTokenCategory.TryGetString(defaultLocation, out text))
				{
					objectToken = objectTokenCategory.OpenToken(text);
				}
			}
			return objectToken;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002CEE4 File Offset: 0x0002B0E4
		private static int CompareTokenVersions(ObjectToken token1, ObjectToken token2, out bool pfDidCompare)
		{
			pfDidCompare = false;
			RegistryDataKey attributes = token1.Attributes;
			RegistryDataKey attributes2 = token2.Attributes;
			if (attributes == null)
			{
				return -1;
			}
			string text;
			attributes.TryGetString("Vendor", out text);
			string text2;
			attributes.TryGetString("ProductLine", out text2);
			string text3;
			attributes.TryGetString("Version", out text3);
			string text4;
			attributes.TryGetString("Language", out text4);
			if (attributes2 == null)
			{
				return 1;
			}
			string text5;
			attributes2.TryGetString("Vendor", out text5);
			string text6;
			attributes2.TryGetString("ProductLine", out text6);
			string text7;
			attributes2.TryGetString("Version", out text7);
			string text8;
			attributes2.TryGetString("Language", out text8);
			if (((string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text5)) || (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text5) && text == text5)) && ((string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text6)) || (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text6) && text2 == text6)) && ((string.IsNullOrEmpty(text4) && string.IsNullOrEmpty(text8)) || (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text8) && text4 == text8)))
			{
				pfDidCompare = true;
				return SAPICategories.CompareVersions(text3, text7);
			}
			return -1;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002D01C File Offset: 0x0002B21C
		private static int CompareVersions(string sV1, string sV2)
		{
			ushort[] array = new ushort[4];
			ushort[] array2 = new ushort[4];
			bool flag = SAPICategories.ParseVersion(sV1, array);
			bool flag2 = SAPICategories.ParseVersion(sV2, array2);
			if (!flag && !flag2)
			{
				return 0;
			}
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag && flag2)
			{
				return -1;
			}
			for (int i = 0; i < 4; i++)
			{
				if (array[i] > array2[i])
				{
					return 1;
				}
				if (array[i] < array2[i])
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002D08C File Offset: 0x0002B28C
		private static bool ParseVersion(string s, ushort[] Version)
		{
			bool flag = true;
			Version[0] = (Version[1] = (Version[2] = (Version[3] = 0)));
			if (string.IsNullOrEmpty(s))
			{
				flag = false;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				while (num2 < 4 && num < s.Length)
				{
					int num3 = s.IndexOf('.', num);
					string text = s.Substring(num, num3);
					ushort num4;
					if (!ushort.TryParse(text, out num4) || num4 > 9999)
					{
						flag = false;
						break;
					}
					Version[num2] = num4;
					num = num3 + 1;
					num2++;
				}
				if (flag && num != s.Length)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002D11C File Offset: 0x0002B31C
		private static ObjectToken GetHighestTokenVersion(ObjectToken token, ObjectToken tokenSeed, string[] criterias)
		{
			bool flag = tokenSeed.MatchesAttributes(criterias);
			if (flag)
			{
				bool flag2;
				int num = SAPICategories.CompareTokenVersions(tokenSeed, token, out flag2);
				if (flag2 && num > 0)
				{
					token = tokenSeed;
				}
			}
			return token;
		}

		// Token: 0x04000841 RID: 2113
		private const string SpeechRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\";

		// Token: 0x04000842 RID: 2114
		internal const string CurrentUserVoices = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Speech\\Voices";

		// Token: 0x04000843 RID: 2115
		internal const string Recognizers = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Recognizers";

		// Token: 0x04000844 RID: 2116
		internal const string Voices = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices";

		// Token: 0x04000845 RID: 2117
		internal const string AudioIn = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\AudioInput";

		// Token: 0x04000846 RID: 2118
		private const string _defaultTokenIdValueName = "DefaultTokenId";

		// Token: 0x04000847 RID: 2119
		private static readonly string[] asVersionDefault = new string[] { "VersionDefault" };
	}
}
