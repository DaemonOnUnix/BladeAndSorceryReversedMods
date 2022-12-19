using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000125 RID: 293
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsSubset : SrgsElement, ISubset, IElement
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x00022555 File Offset: 0x00021555
		public SrgsSubset(string text)
			: this(text, SubsetMatchingMode.Subsequence)
		{
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00022560 File Offset: 0x00021560
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

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000225D0 File Offset: 0x000215D0
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x000225D8 File Offset: 0x000215D8
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0002260B File Offset: 0x0002160B
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00022613 File Offset: 0x00021613
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

		// Token: 0x060007CA RID: 1994 RVA: 0x00022640 File Offset: 0x00021640
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

		// Token: 0x060007CB RID: 1995 RVA: 0x000226E7 File Offset: 0x000216E7
		internal override void Validate(SrgsGrammar grammar)
		{
			grammar.HasSapiExtension = true;
			base.Validate(grammar);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000226F7 File Offset: 0x000216F7
		internal override string DebuggerDisplayString()
		{
			return this._text + " [" + this._matchMode.ToString() + "]";
		}

		// Token: 0x04000592 RID: 1426
		private SubsetMatchingMode _matchMode;

		// Token: 0x04000593 RID: 1427
		private string _text;
	}
}
