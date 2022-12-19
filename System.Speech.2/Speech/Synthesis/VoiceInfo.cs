using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.ObjectTokens;
using System.Speech.Internal.Synthesis;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001D RID: 29
	[DebuggerDisplay("{(_name != null ? \"'\" + _name + \"' \" : \"\") +  (_culture != null ? \" '\" + _culture.ToString () + \"' \" : \"\") + (_gender != VoiceGender.NotSet ? \" '\" + _gender.ToString () + \"' \" : \"\") + (_age != VoiceAge.NotSet ? \" '\" + _age.ToString () + \"' \" : \"\") + (_variant > 0 ? \" \" + _variant.ToString () : \"\")}")]
	[Serializable]
	public class VoiceInfo
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004550 File Offset: 0x00002750
		internal VoiceInfo(string name)
		{
			Helpers.ThrowIfEmptyOrNull(name, "name");
			this._name = name;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004574 File Offset: 0x00002774
		internal VoiceInfo(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			this._culture = culture;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000045C8 File Offset: 0x000027C8
		internal VoiceInfo(ObjectToken token)
		{
			this._registryKeyPath = token._sKeyId;
			this._id = token.Name;
			this._description = token.Description;
			this._name = token.TokenName();
			SsmlParserHelpers.TryConvertAge(token.Age.ToLowerInvariant(), out this._age);
			SsmlParserHelpers.TryConvertGender(token.Gender.ToLowerInvariant(), out this._gender);
			string text;
			if (token.Attributes.TryGetString("Language", out text))
			{
				this._culture = SapiAttributeParser.GetCultureInfoFromLanguageString(text);
			}
			string text2;
			if (token.TryGetString("Assembly", out text2))
			{
				this._assemblyName = text2;
			}
			string text3;
			if (token.TryGetString("CLSID", out text3))
			{
				this._clsid = text3;
			}
			if (token.Attributes != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (string text4 in token.Attributes.GetValueNames())
				{
					string text5;
					if (token.Attributes.TryGetString(text4, out text5))
					{
						dictionary.Add(text4, text5);
					}
				}
				this._attributes = new ReadOnlyDictionary<string, string>(dictionary);
			}
			string text6;
			if (token.Attributes != null && token.Attributes.TryGetString("AudioFormats", out text6))
			{
				this._audioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(SapiAttributeParser.GetAudioFormatsFromString(text6));
				return;
			}
			this._audioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(new List<SpeechAudioFormatInfo>());
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004729 File Offset: 0x00002929
		internal VoiceInfo(VoiceGender gender)
		{
			this._gender = gender;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000473F File Offset: 0x0000293F
		internal VoiceInfo(VoiceGender gender, VoiceAge age)
		{
			this._gender = gender;
			this._age = age;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000475C File Offset: 0x0000295C
		internal VoiceInfo(VoiceGender gender, VoiceAge age, int voiceAlternate)
		{
			if (voiceAlternate < 0)
			{
				throw new ArgumentOutOfRangeException("voiceAlternate", SR.Get(SRID.PromptBuilderInvalidVariant, new object[0]));
			}
			this._gender = gender;
			this._age = age;
			this._variant = voiceAlternate + 1;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000047AC File Offset: 0x000029AC
		public override bool Equals(object obj)
		{
			VoiceInfo voiceInfo = obj as VoiceInfo;
			return voiceInfo != null && this._name == voiceInfo._name && (this._age == voiceInfo._age || this._age == VoiceAge.NotSet || voiceInfo._age == VoiceAge.NotSet) && (this._gender == voiceInfo._gender || this._gender == VoiceGender.NotSet || voiceInfo._gender == VoiceGender.NotSet) && (this._culture == null || voiceInfo._culture == null || this._culture.Equals(voiceInfo._culture));
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004837 File Offset: 0x00002A37
		public override int GetHashCode()
		{
			return this._name.GetHashCode();
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004844 File Offset: 0x00002A44
		public VoiceGender Gender
		{
			get
			{
				return this._gender;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000484C File Offset: 0x00002A4C
		public VoiceAge Age
		{
			get
			{
				return this._age;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004854 File Offset: 0x00002A54
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000485C File Offset: 0x00002A5C
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004864 File Offset: 0x00002A64
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000486C File Offset: 0x00002A6C
		public string Description
		{
			get
			{
				if (this._description == null)
				{
					return string.Empty;
				}
				return this._description;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004882 File Offset: 0x00002A82
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
		{
			get
			{
				return this._audioFormats;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000488A File Offset: 0x00002A8A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IDictionary<string, string> AdditionalInfo
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(0));
				}
				return this._attributes;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000048AB File Offset: 0x00002AAB
		internal static bool ValidateGender(VoiceGender gender)
		{
			return gender == VoiceGender.Female || gender == VoiceGender.Male || gender == VoiceGender.Neutral || gender == VoiceGender.NotSet;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000048BF File Offset: 0x00002ABF
		internal static bool ValidateAge(VoiceAge age)
		{
			return age == VoiceAge.Adult || age == VoiceAge.Child || age == VoiceAge.NotSet || age == VoiceAge.Senior || age == VoiceAge.Teen;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000048DA File Offset: 0x00002ADA
		internal int Variant
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000048E2 File Offset: 0x00002AE2
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000048EA File Offset: 0x00002AEA
		internal string Clsid
		{
			get
			{
				return this._clsid;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000048F2 File Offset: 0x00002AF2
		internal string RegistryKeyPath
		{
			get
			{
				return this._registryKeyPath;
			}
		}

		// Token: 0x040001F7 RID: 503
		private string _name;

		// Token: 0x040001F8 RID: 504
		private CultureInfo _culture;

		// Token: 0x040001F9 RID: 505
		private VoiceGender _gender;

		// Token: 0x040001FA RID: 506
		private VoiceAge _age;

		// Token: 0x040001FB RID: 507
		private int _variant = -1;

		// Token: 0x040001FC RID: 508
		[NonSerialized]
		private string _id;

		// Token: 0x040001FD RID: 509
		[NonSerialized]
		private string _registryKeyPath;

		// Token: 0x040001FE RID: 510
		[NonSerialized]
		private string _assemblyName;

		// Token: 0x040001FF RID: 511
		[NonSerialized]
		private string _clsid;

		// Token: 0x04000200 RID: 512
		[NonSerialized]
		private string _description;

		// Token: 0x04000201 RID: 513
		[NonSerialized]
		private ReadOnlyDictionary<string, string> _attributes;

		// Token: 0x04000202 RID: 514
		[NonSerialized]
		private ReadOnlyCollection<SpeechAudioFormatInfo> _audioFormats;
	}
}
