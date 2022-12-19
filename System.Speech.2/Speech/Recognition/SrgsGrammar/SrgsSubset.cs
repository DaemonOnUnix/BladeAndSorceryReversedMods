using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000082 RID: 130
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsSubset : SrgsElement, ISubset, IElement
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x00011AB1 File Offset: 0x0000FCB1
		public SrgsSubset(string text)
			: this(text, SubsetMatchingMode.Subsequence)
		{
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011ABC File Offset: 0x0000FCBC
		public SrgsSubset(string text, SubsetMatchingMode matchingMode)
		{
			Helpers.ThrowIfEmptyOrNull(text, "text");
			if (matchingMode != SubsetMatchingMode.OrderedSubset && matchingMode != SubsetMatchingMode.Subsequence && matchingMode != SubsetMatchingMode.OrderedSubsetContentRequired && matchingMode != SubsetMatchingMode.SubsequenceContentRequired)
			{
				throw new ArgumentException(SR.Get(SRID.InvalidSubsetAttribute, new object[0]), "matchingMode");
			}
			this._matchMode = matchingMode;
			this._text = text.Trim(Helpers._achTrimChars);
			Helpers.ThrowIfEmptyOrNull(this._text, "text");
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00011B2C File Offset: 0x0000FD2C
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00011B34 File Offset: 0x0000FD34
		public SubsetMatchingMode MatchingMode
		{
			get
			{
				return this._matchMode;
			}
			set
			{
				if (value != SubsetMatchingMode.OrderedSubset && value != SubsetMatchingMode.Subsequence && value != SubsetMatchingMode.OrderedSubsetContentRequired && value != SubsetMatchingMode.SubsequenceContentRequired)
				{
					throw new ArgumentException(SR.Get(SRID.InvalidSubsetAttribute, new object[0]), "value");
				}
				this._matchMode = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00011B67 File Offset: 0x0000FD67
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00011B6F File Offset: 0x0000FD6F
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				value = value.Trim(Helpers._achTrimChars);
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._text = value;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011B9C File Offset: 0x0000FD9C
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("sapi", "subset", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
			if (this._matchMode != SubsetMatchingMode.Subsequence)
			{
				string text = null;
				switch (this._matchMode)
				{
				case SubsetMatchingMode.Subsequence:
					text = "subsequence";
					break;
				case SubsetMatchingMode.OrderedSubset:
					text = "ordered-subset";
					break;
				case SubsetMatchingMode.SubsequenceContentRequired:
					text = "subsequence-content-required";
					break;
				case SubsetMatchingMode.OrderedSubsetContentRequired:
					text = "ordered-subset-content-required";
					break;
				}
				writer.WriteAttributeString("sapi", "match", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", text);
			}
			if (this._text != null && this._text.Length > 0)
			{
				writer.WriteString(this._text);
			}
			writer.WriteEndElement();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011C43 File Offset: 0x0000FE43
		internal override void Validate(SrgsGrammar grammar)
		{
			grammar.HasSapiExtension = true;
			base.Validate(grammar);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011C53 File Offset: 0x0000FE53
		internal override string DebuggerDisplayString()
		{
			return this._text + " [" + this._matchMode.ToString() + "]";
		}

		// Token: 0x04000403 RID: 1027
		private SubsetMatchingMode _matchMode;

		// Token: 0x04000404 RID: 1028
		private string _text;
	}
}
