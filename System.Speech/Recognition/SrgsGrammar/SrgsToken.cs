using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000129 RID: 297
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsToken : SrgsElement, IToken, IElement
	{
		// Token: 0x060007DB RID: 2011 RVA: 0x000228C2 File Offset: 0x000218C2
		public SrgsToken(string text)
		{
			Helpers.ThrowIfEmptyOrNull(text, "text");
			this.Text = text;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x000228E7 File Offset: 0x000218E7
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x000228F0 File Offset: 0x000218F0
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				string text = value.Trim(Helpers._achTrimChars);
				if (string.IsNullOrEmpty(text) || text.IndexOf('"') >= 0)
				{
					throw new ArgumentException(SR.Get(SRID.InvalidTokenString, new object[0]), "value");
				}
				this._text = text;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00022946 File Offset: 0x00021946
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0002294E File Offset: 0x0002194E
		public string Pronunciation
		{
			get
			{
				return this._pronunciation;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._pronunciation = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00022962 File Offset: 0x00021962
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x0002296A File Offset: 0x0002196A
		public string Display
		{
			get
			{
				return this._display;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._display = value;
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00022980 File Offset: 0x00021980
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("token");
			if (this._display != null)
			{
				writer.WriteAttributeString("sapi", "display", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._display);
			}
			if (this._pronunciation != null)
			{
				writer.WriteAttributeString("sapi", "pron", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._pronunciation);
			}
			if (this._text != null && this._text.Length > 0)
			{
				writer.WriteString(this._text);
			}
			writer.WriteEndElement();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00022A08 File Offset: 0x00021A08
		internal override void Validate(SrgsGrammar grammar)
		{
			if (this._pronunciation != null || this._display != null)
			{
				grammar.HasPronunciation = true;
			}
			if (this._pronunciation != null)
			{
				int num;
				for (int i = 0; i < this._pronunciation.Length; i = num + 1)
				{
					num = this._pronunciation.IndexOf(';', i);
					if (num == -1)
					{
						num = this._pronunciation.Length;
					}
					string text = this._pronunciation.Substring(i, num - i);
					switch (grammar.PhoneticAlphabet)
					{
					case AlphabetType.Sapi:
						PhonemeConverter.ConvertPronToId(text, grammar.Culture.LCID);
						break;
					case AlphabetType.Ipa:
						PhonemeConverter.ValidateUpsIds(text.ToCharArray());
						break;
					case AlphabetType.Ups:
						PhonemeConverter.UpsConverter.ConvertPronToId(text);
						break;
					}
				}
			}
			base.Validate(grammar);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00022AD0 File Offset: 0x00021AD0
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("Token '");
			stringBuilder.Append(this._text);
			stringBuilder.Append("'");
			if (this._pronunciation != null)
			{
				stringBuilder.Append(" Pronunciation '");
				stringBuilder.Append(this._pronunciation);
				stringBuilder.Append("'");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400059B RID: 1435
		private string _text = string.Empty;

		// Token: 0x0400059C RID: 1436
		private string _pronunciation;

		// Token: 0x0400059D RID: 1437
		private string _display;
	}
}
