using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.Speech.Internal
{
	// Token: 0x02000010 RID: 16
	internal class AlphabetConverter
	{
		// Token: 0x06000037 RID: 55 RVA: 0x0000337C File Offset: 0x0000237C
		internal AlphabetConverter(int langId)
		{
			this._currentLangId = -1;
			this.SetLanguageId(langId);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003393 File Offset: 0x00002393
		internal char[] SapiToIpa(char[] phonemes)
		{
			return this.Convert(phonemes, true);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000339D File Offset: 0x0000239D
		internal char[] IpaToSapi(char[] phonemes)
		{
			return this.Convert(phonemes, false);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000033A7 File Offset: 0x000023A7
		internal bool IsPrefix(string phonemes, bool isSapi)
		{
			return this._phoneMap.IsPrefix(phonemes, isSapi);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000033B6 File Offset: 0x000023B6
		internal bool IsConvertibleUnit(string phonemes, bool isSapi)
		{
			return this._phoneMap.ConvertPhoneme(phonemes, isSapi) != null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000033CC File Offset: 0x000023CC
		internal int SetLanguageId(int langId)
		{
			if (langId < 0)
			{
				throw new ArgumentException(SR.Get(SRID.MustBeGreaterThanZero, new object[0]), "langId");
			}
			if (langId == this._currentLangId)
			{
				return this._currentLangId;
			}
			int currentLangId = this._currentLangId;
			int num = 0;
			while (num < AlphabetConverter._langIds.Length && AlphabetConverter._langIds[num] != langId)
			{
				num++;
			}
			if (num == AlphabetConverter._langIds.Length)
			{
				this._currentLangId = langId;
				this._phoneMap = null;
			}
			else
			{
				lock (AlphabetConverter._staticLock)
				{
					if (AlphabetConverter._phoneMaps[num] == null)
					{
						AlphabetConverter._phoneMaps[num] = this.CreateMap(AlphabetConverter._resourceNames[num]);
					}
					this._phoneMap = AlphabetConverter._phoneMaps[num];
					this._currentLangId = langId;
				}
			}
			return currentLangId;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000349C File Offset: 0x0000249C
		private char[] Convert(char[] phonemes, bool isSapi)
		{
			if (this._phoneMap == null || phonemes.Length == 0)
			{
				return (char[])phonemes.Clone();
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = new string(phonemes);
			string text2 = null;
			int num;
			int i = (num = 0);
			int num2 = -1;
			while (i < text.Length)
			{
				string text3 = text.Substring(num, i - num + 1);
				if (this._phoneMap.IsPrefix(text3, isSapi))
				{
					string text4 = this._phoneMap.ConvertPhoneme(text3, isSapi);
					if (text4 != null)
					{
						text2 = text4;
						num2 = i;
					}
				}
				else
				{
					if (text2 == null)
					{
						break;
					}
					stringBuilder.Append(text2);
					i = num2;
					num = num2 + 1;
					text2 = null;
				}
				i++;
			}
			if (text2 != null && num2 == phonemes.Length - 1)
			{
				stringBuilder.Append(text2);
				return stringBuilder.ToString().ToCharArray();
			}
			return null;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003564 File Offset: 0x00002564
		private AlphabetConverter.PhoneMapData CreateMap(string resourceName)
		{
			Assembly assembly = Assembly.GetAssembly(base.GetType());
			Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
			if (manifestResourceStream == null)
			{
				throw new FileLoadException(SR.Get(SRID.CannotLoadResourceFromManifest, new object[] { resourceName, assembly.FullName }));
			}
			return new AlphabetConverter.PhoneMapData(new BufferedStream(manifestResourceStream));
		}

		// Token: 0x0400007A RID: 122
		private int _currentLangId;

		// Token: 0x0400007B RID: 123
		private AlphabetConverter.PhoneMapData _phoneMap;

		// Token: 0x0400007C RID: 124
		private static int[] _langIds = new int[] { 2052, 1028, 1031, 1033, 1034, 1036, 1041 };

		// Token: 0x0400007D RID: 125
		private static string[] _resourceNames = new string[] { "upstable_chs.upsmap", "upstable_cht.upsmap", "upstable_deu.upsmap", "upstable_enu.upsmap", "upstable_esp.upsmap", "upstable_fra.upsmap", "upstable_jpn.upsmap" };

		// Token: 0x0400007E RID: 126
		private static AlphabetConverter.PhoneMapData[] _phoneMaps = new AlphabetConverter.PhoneMapData[7];

		// Token: 0x0400007F RID: 127
		private static object _staticLock = new object();

		// Token: 0x02000011 RID: 17
		internal class PhoneMapData
		{
			// Token: 0x06000040 RID: 64 RVA: 0x00003654 File Offset: 0x00002654
			internal PhoneMapData(Stream input)
			{
				using (BinaryReader binaryReader = new BinaryReader(input, Encoding.Unicode))
				{
					int num = binaryReader.ReadInt32();
					this.convertTable = new AlphabetConverter.PhoneMapData.ConversionUnit[num];
					for (int i = 0; i < num; i++)
					{
						this.convertTable[i] = new AlphabetConverter.PhoneMapData.ConversionUnit();
						this.convertTable[i].sapi = AlphabetConverter.PhoneMapData.ReadPhoneString(binaryReader);
						this.convertTable[i].ups = AlphabetConverter.PhoneMapData.ReadPhoneString(binaryReader);
						this.convertTable[i].isDefault = binaryReader.ReadInt32() != 0;
					}
					this.prefixSapiTable = this.InitializePrefix(true);
					this.prefixUpsTable = this.InitializePrefix(false);
				}
			}

			// Token: 0x06000041 RID: 65 RVA: 0x00003714 File Offset: 0x00002714
			internal bool IsPrefix(string prefix, bool isSapi)
			{
				if (isSapi)
				{
					return this.prefixSapiTable.ContainsKey(prefix);
				}
				return this.prefixUpsTable.ContainsKey(prefix);
			}

			// Token: 0x06000042 RID: 66 RVA: 0x00003734 File Offset: 0x00002734
			internal string ConvertPhoneme(string phoneme, bool isSapi)
			{
				AlphabetConverter.PhoneMapData.ConversionUnit conversionUnit;
				if (isSapi)
				{
					conversionUnit = (AlphabetConverter.PhoneMapData.ConversionUnit)this.prefixSapiTable[phoneme];
				}
				else
				{
					conversionUnit = (AlphabetConverter.PhoneMapData.ConversionUnit)this.prefixUpsTable[phoneme];
				}
				if (conversionUnit == null)
				{
					return null;
				}
				if (!isSapi)
				{
					return conversionUnit.sapi;
				}
				return conversionUnit.ups;
			}

			// Token: 0x06000043 RID: 67 RVA: 0x00003780 File Offset: 0x00002780
			private Hashtable InitializePrefix(bool isSapi)
			{
				Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
				for (int i = 0; i < this.convertTable.Length; i++)
				{
					string text;
					if (isSapi)
					{
						text = this.convertTable[i].sapi;
					}
					else
					{
						text = this.convertTable[i].ups;
					}
					int num = 0;
					while (num + 1 < text.Length)
					{
						string text2 = text.Substring(0, num + 1);
						if (!hashtable.ContainsKey(text2))
						{
							hashtable[text2] = null;
						}
						num++;
					}
					if (this.convertTable[i].isDefault || hashtable[text] == null)
					{
						hashtable[text] = this.convertTable[i];
					}
				}
				return hashtable;
			}

			// Token: 0x06000044 RID: 68 RVA: 0x0000382C File Offset: 0x0000282C
			private static string ReadPhoneString(BinaryReader reader)
			{
				int num = (int)(reader.ReadInt16() / 2);
				char[] array = reader.ReadChars(num);
				return new string(array, 0, num - 1);
			}

			// Token: 0x04000080 RID: 128
			private Hashtable prefixSapiTable;

			// Token: 0x04000081 RID: 129
			private Hashtable prefixUpsTable;

			// Token: 0x04000082 RID: 130
			private AlphabetConverter.PhoneMapData.ConversionUnit[] convertTable;

			// Token: 0x02000012 RID: 18
			private class ConversionUnit
			{
				// Token: 0x04000083 RID: 131
				public string sapi;

				// Token: 0x04000084 RID: 132
				public string ups;

				// Token: 0x04000085 RID: 133
				public bool isDefault;
			}
		}
	}
}
