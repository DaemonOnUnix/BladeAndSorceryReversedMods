using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.ObjectTokens;

namespace System.Speech.Recognition
{
	// Token: 0x0200019D RID: 413
	public class RecognizerInfo : IDisposable
	{
		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002F634 File Offset: 0x0002E634
		private RecognizerInfo(ObjectToken token, CultureInfo culture)
		{
			this._id = token.Name;
			this._description = token.Description;
			this._sapiObjectTokenId = token.Id;
			this._name = token.TokenName();
			this._culture = culture;
			foreach (string text in token.Attributes.GetValueNames())
			{
				string text2;
				if (token.Attributes.TryGetString(text, out text2))
				{
					this._attributes.InternalDictionary[text] = text2;
				}
			}
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

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002F70C File Offset: 0x0002E70C
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

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002F74C File Offset: 0x0002E74C
		internal ObjectToken GetObjectToken()
		{
			return this._objectToken;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002F754 File Offset: 0x0002E754
		public void Dispose()
		{
			this._objectToken.Dispose();
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002F761 File Offset: 0x0002E761
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0002F769 File Offset: 0x0002E769
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002F771 File Offset: 0x0002E771
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002F779 File Offset: 0x0002E779
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002F781 File Offset: 0x0002E781
		public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
		{
			get
			{
				return this._supportedAudioFormats;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0002F789 File Offset: 0x0002E789
		public IDictionary<string, string> AdditionalInfo
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0002F791 File Offset: 0x0002E791
		internal string SapiObjectTokenId
		{
			get
			{
				return this._sapiObjectTokenId;
			}
		}

		// Token: 0x04000958 RID: 2392
		private ReadOnlyDictionary<string, string> _attributes = new ReadOnlyDictionary<string, string>();

		// Token: 0x04000959 RID: 2393
		private string _id;

		// Token: 0x0400095A RID: 2394
		private string _name;

		// Token: 0x0400095B RID: 2395
		private string _description;

		// Token: 0x0400095C RID: 2396
		private string _sapiObjectTokenId;

		// Token: 0x0400095D RID: 2397
		private CultureInfo _culture;

		// Token: 0x0400095E RID: 2398
		private ReadOnlyCollection<SpeechAudioFormatInfo> _supportedAudioFormats;

		// Token: 0x0400095F RID: 2399
		private ObjectToken _objectToken;
	}
}
