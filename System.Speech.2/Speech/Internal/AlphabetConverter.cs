using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.Speech.Internal
{
	// Token: 0x02000092 RID: 146
	internal class AlphabetConverter
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x00013060 File Offset: 0x00011260
		internal AlphabetConverter(int langId)
		{
			this._currentLangId = -1;
			this.SetLanguageId(langId);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00013077 File Offset: 0x00011277
		internal char[] SapiToIpa(char[] phonemes)
		{
			return this.Convert(phonemes, true);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00013081 File Offset: 0x00011281
		internal char[] IpaToSapi(char[] phonemes)
		{
			return this.Convert(phonemes, false);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001308B File Offset: 0x0001128B
		internal bool IsPrefix(string phonemes, bool isSapi)
		{
			return this._phoneMap.IsPrefix(phonemes, isSapi);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001309A File Offset: 0x0001129A
		internal bool IsConvertibleUnit(string phonemes, bool isSapi)
		{
			return this._phoneMap.ConvertPhoneme(phonemes, isSapi) != null;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000130AC File Offset: 0x000112AC
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
				object staticLock = AlphabetConverter._staticLock;
				lock (staticLock)
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

		// Token: 0x060004C3 RID: 1219 RVA: 0x00013180 File Offset: 0x00011380
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

		// Token: 0x060004C4 RID: 1220 RVA: 0x00013248 File Offset: 0x00011448
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

		// Token: 0x04000432 RID: 1074
		private int _currentLangId;

		// Token: 0x04000433 RID: 1075
		private AlphabetConverter.PhoneMapData _phoneMap;

		// Token: 0x04000434 RID: 1076
		private static int[] _langIds = new int[] { 2052, 1028, 1031, 1033, 1034, 1036, 1041 };

		// Token: 0x04000435 RID: 1077
		private static string[] _resourceNames = new string[] { "upstable_chs.upsmap", "upstable_cht.upsmap", "upstable_deu.upsmap", "upstable_enu.upsmap", "upstable_esp.upsmap", "upstable_fra.upsmap", "upstable_jpn.upsmap" };

		// Token: 0x04000436 RID: 1078
		private static AlphabetConverter.PhoneMapData[] _phoneMaps = new AlphabetConverter.PhoneMapData[7];

		// Token: 0x04000437 RID: 1079
		private static object _staticLock = new object();

		// Token: 0x02000186 RID: 390
		internal class PhoneMapData
		{
			// Token: 0x06000B6B RID: 2923 RVA: 0x0002D85C File Offset: 0x0002BA5C
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

			// Token: 0x06000B6C RID: 2924 RVA: 0x0002D91C File Offset: 0x0002BB1C
			internal bool IsPrefix(string prefix, bool isSapi)
			{
				if (isSapi)
				{
					return this.prefixSapiTable.ContainsKey(prefix);
				}
				return this.prefixUpsTable.ContainsKey(prefix);
			}

			// Token: 0x06000B6D RID: 2925 RVA: 0x0002D93C File Offset: 0x0002BB3C
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

			// Token: 0x06000B6E RID: 2926 RVA: 0x0002D988 File Offset: 0x0002BB88
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

			// Token: 0x06000B6F RID: 2927 RVA: 0x0002DA34 File Offset: 0x0002BC34
			private static string ReadPhoneString(BinaryReader reader)
			{
				int num = (int)(reader.ReadInt16() / 2);
				char[] array = reader.ReadChars(num);
				return new string(array, 0, num - 1);
			}

			// Token: 0x04000913 RID: 2323
			private Hashtable prefixSapiTable;

			// Token: 0x04000914 RID: 2324
			private Hashtable prefixUpsTable;

			// Token: 0x04000915 RID: 2325
			private AlphabetConverter.PhoneMapData.ConversionUnit[] convertTable;

			// Token: 0x020001F2 RID: 498
			private class ConversionUnit
			{
				// Token: 0x04000A2A RID: 2602
				public string sapi;

				// Token: 0x04000A2B RID: 2603
				public string ups;

				// Token: 0x04000A2C RID: 2604
				public bool isDefault;
			}
		}
	}
}
