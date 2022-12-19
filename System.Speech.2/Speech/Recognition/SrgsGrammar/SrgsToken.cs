using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000086 RID: 134
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsToken : SrgsElement, IToken, IElement
	{
		// Token: 0x0600047F RID: 1151 RVA: 0x00011E1E File Offset: 0x0001001E
		public SrgsToken(string text)
		{
			Helpers.ThrowIfEmptyOrNull(text, "text");
			this.Text = text;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00011E43 File Offset: 0x00010043
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00011E4C File Offset: 0x0001004C
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

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00011EA2 File Offset: 0x000100A2
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x00011EAA File Offset: 0x000100AA
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00011EBE File Offset: 0x000100BE
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00011EC6 File Offset: 0x000100C6
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

		// Token: 0x06000486 RID: 1158 RVA: 0x00011EDC File Offset: 0x000100DC
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

		// Token: 0x06000487 RID: 1159 RVA: 0x00011F64 File Offset: 0x00010164
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

		// Token: 0x06000488 RID: 1160 RVA: 0x0001202C File Offset: 0x0001022C
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

		// Token: 0x0400040C RID: 1036
		private string _text = string.Empty;

		// Token: 0x0400040D RID: 1037
		private string _pronunciation;

		// Token: 0x0400040E RID: 1038
		private string _display;
	}
}
