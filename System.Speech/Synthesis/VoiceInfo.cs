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
	// Token: 0x02000155 RID: 341
	[DebuggerDisplay("{(_name != null ? \"'\" + _name + \"' \" : \"\") +  (_culture != null ? \" '\" + _culture.ToString () + \"' \" : \"\") + (_gender != VoiceGender.NotSet ? \" '\" + _gender.ToString () + \"' \" : \"\") + (_age != VoiceAge.NotSet ? \" '\" + _age.ToString () + \"' \" : \"\") + (_variant > 0 ? \" \" + _variant.ToString () : \"\")}")]
	[Serializable]
	public class VoiceInfo
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x00027D79 File Offset: 0x00026D79
		internal VoiceInfo(string name)
		{
			Helpers.ThrowIfEmptyOrNull(name, "name");
			this._name = name;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00027DA8 File Offset: 0x00026DA8
		internal VoiceInfo(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			this._culture = culture;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00027E04 File Offset: 0x00026E04
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
				foreach (string text4 in token.Attributes.GetValueNames())
				{
					string text5;
					if (token.Attributes.TryGetString(text4, out text5))
					{
						this._attributes.InternalDictionary.Add(text4, text5);
					}
				}
			}
			string text6;
			if (token.Attributes != null && token.Attributes.TryGetString("AudioFormats", out text6))
			{
				this._audioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(SapiAttributeParser.GetAudioFormatsFromString(text6));
				return;
			}
			this._audioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(new List<SpeechAudioFormatInfo>());
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00027F63 File Offset: 0x00026F63
		internal VoiceInfo(VoiceGender gender)
		{
			this._gender = gender;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00027F84 File Offset: 0x00026F84
		internal VoiceInfo(VoiceGender gender, VoiceAge age)
		{
			this._gender = gender;
			this._age = age;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00027FAC File Offset: 0x00026FAC
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

		// Token: 0x060008F5 RID: 2293 RVA: 0x00028008 File Offset: 0x00027008
		public override bool Equals(object obj)
		{
			VoiceInfo voiceInfo = obj as VoiceInfo;
			return voiceInfo != null && this._name == voiceInfo._name && (this._age == voiceInfo._age || this._age == VoiceAge.NotSet || voiceInfo._age == VoiceAge.NotSet) && (this._gender == voiceInfo._gender || this._gender == VoiceGender.NotSet || voiceInfo._gender == VoiceGender.NotSet) && (this._culture == null || voiceInfo._culture == null || this._culture.Equals(voiceInfo._culture));
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00028093 File Offset: 0x00027093
		public override int GetHashCode()
		{
			return this._name.GetHashCode();
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x000280A0 File Offset: 0x000270A0
		public VoiceGender Gender
		{
			get
			{
				return this._gender;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x000280A8 File Offset: 0x000270A8
		public VoiceAge Age
		{
			get
			{
				return this._age;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x000280B0 File Offset: 0x000270B0
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x000280B8 File Offset: 0x000270B8
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x000280C0 File Offset: 0x000270C0
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x000280C8 File Offset: 0x000270C8
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

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x000280DE File Offset: 0x000270DE
		[EditorBrowsable(2)]
		public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
		{
			get
			{
				return this._audioFormats;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x000280E6 File Offset: 0x000270E6
		[EditorBrowsable(2)]
		public IDictionary<string, string> AdditionalInfo
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000280EE File Offset: 0x000270EE
		internal static bool ValidateGender(VoiceGender gender)
		{
			return gender == VoiceGender.Female || gender == VoiceGender.Male || gender == VoiceGender.Neutral || gender == VoiceGender.NotSet;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00028102 File Offset: 0x00027102
		internal static bool ValidateAge(VoiceAge age)
		{
			return age == VoiceAge.Adult || age == VoiceAge.Child || age == VoiceAge.NotSet || age == VoiceAge.Senior || age == VoiceAge.Teen;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0002811D File Offset: 0x0002711D
		internal int Variant
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00028125 File Offset: 0x00027125
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002812D File Offset: 0x0002712D
		internal string Clsid
		{
			get
			{
				return this._clsid;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00028135 File Offset: 0x00027135
		internal string RegistryKeyPath
		{
			get
			{
				return this._registryKeyPath;
			}
		}

		// Token: 0x0400067B RID: 1659
		private string _name;

		// Token: 0x0400067C RID: 1660
		private CultureInfo _culture;

		// Token: 0x0400067D RID: 1661
		private VoiceGender _gender;

		// Token: 0x0400067E RID: 1662
		private VoiceAge _age;

		// Token: 0x0400067F RID: 1663
		private int _variant = -1;

		// Token: 0x04000680 RID: 1664
		[NonSerialized]
		private string _id;

		// Token: 0x04000681 RID: 1665
		[NonSerialized]
		private string _registryKeyPath;

		// Token: 0x04000682 RID: 1666
		[NonSerialized]
		private string _assemblyName;

		// Token: 0x04000683 RID: 1667
		[NonSerialized]
		private string _clsid;

		// Token: 0x04000684 RID: 1668
		[NonSerialized]
		private string _description;

		// Token: 0x04000685 RID: 1669
		[NonSerialized]
		private ReadOnlyDictionary<string, string> _attributes = new ReadOnlyDictionary<string, string>();

		// Token: 0x04000686 RID: 1670
		[NonSerialized]
		private ReadOnlyCollection<SpeechAudioFormatInfo> _audioFormats;
	}
}
