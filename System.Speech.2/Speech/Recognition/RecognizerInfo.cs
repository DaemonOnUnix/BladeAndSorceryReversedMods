using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.ObjectTokens;

namespace System.Speech.Recognition
{
	// Token: 0x02000069 RID: 105
	public class RecognizerInfo : IDisposable
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000D6FC File Offset: 0x0000B8FC
		private RecognizerInfo(ObjectToken token, CultureInfo culture)
		{
			this._id = token.Name;
			this._description = token.Description;
			this._sapiObjectTokenId = token.Id;
			this._name = token.TokenName();
			this._culture = culture;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in token.Attributes.GetValueNames())
			{
				string text2;
				if (token.Attributes.TryGetString(text, out text2))
				{
					dictionary[text] = text2;
				}
			}
			this._attributes = new ReadOnlyDictionary<string, string>(dictionary);
			string text3;
			if (token.Attributes.TryGetString("AudioFormats", out text3))
			{
				this._supportedAudioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(SapiAttributeParser.GetAudioFormatsFromString(text3));
			}
			else
			{
				this._supportedAudioFormats = new ReadOnlyCollection<SpeechAudioFormatInfo>(new List<SpeechAudioFormatInfo>());
			}
			this._objectToken = token;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
		internal static RecognizerInfo Create(ObjectToken token)
		{
			if (token.Attributes == null)
			{
				return null;
			}
			string text;
			if (!token.Attributes.TryGetString("Language", out text))
			{
				return null;
			}
			CultureInfo cultureInfoFromLanguageString = SapiAttributeParser.GetCultureInfoFromLanguageString(text);
			if (cultureInfoFromLanguageString != null)
			{
				return new RecognizerInfo(token, cultureInfoFromLanguageString);
			}
			return null;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D810 File Offset: 0x0000BA10
		internal ObjectToken GetObjectToken()
		{
			return this._objectToken;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000D818 File Offset: 0x0000BA18
		public void Dispose()
		{
			this._objectToken.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000D82B File Offset: 0x0000BA2B
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000D833 File Offset: 0x0000BA33
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000D83B File Offset: 0x0000BA3B
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000D843 File Offset: 0x0000BA43
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000D84B File Offset: 0x0000BA4B
		public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
		{
			get
			{
				return this._supportedAudioFormats;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000D853 File Offset: 0x0000BA53
		public IDictionary<string, string> AdditionalInfo
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x04000394 RID: 916
		private ReadOnlyDictionary<string, string> _attributes;

		// Token: 0x04000395 RID: 917
		private string _id;

		// Token: 0x04000396 RID: 918
		private string _name;

		// Token: 0x04000397 RID: 919
		private string _description;

		// Token: 0x04000398 RID: 920
		private string _sapiObjectTokenId;

		// Token: 0x04000399 RID: 921
		private CultureInfo _culture;

		// Token: 0x0400039A RID: 922
		private ReadOnlyCollection<SpeechAudioFormatInfo> _supportedAudioFormats;

		// Token: 0x0400039B RID: 923
		private ObjectToken _objectToken;
	}
}
