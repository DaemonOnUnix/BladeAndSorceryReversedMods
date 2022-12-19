using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000110 RID: 272
	[Serializable]
	public class SrgsDocument
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x0001FB44 File Offset: 0x0001EB44
		public SrgsDocument()
		{
			this._grammar = new SrgsGrammar();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001FB58 File Offset: 0x0001EB58
		public SrgsDocument(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			using (XmlTextReader xmlTextReader = new XmlTextReader(path))
			{
				this.Load(xmlTextReader);
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001FBA0 File Offset: 0x0001EBA0
		public SrgsDocument(XmlReader srgsGrammar)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			this.Load(srgsGrammar);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001FBBC File Offset: 0x0001EBBC
		public SrgsDocument(GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			this._grammar = new SrgsGrammar();
			this._grammar.Culture = builder.Culture;
			IElementFactory elementFactory = new SrgsElementFactory(this._grammar);
			builder.CreateGrammar(elementFactory);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001FC0C File Offset: 0x0001EC0C
		public SrgsDocument(SrgsRule grammarRootRule)
			: this()
		{
			Helpers.ThrowIfNull(grammarRootRule, "grammarRootRule");
			this.Root = grammarRootRule;
			this.Rules.Add(new SrgsRule[] { grammarRootRule });
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001FC48 File Offset: 0x0001EC48
		public void WriteSrgs(XmlWriter srgsGrammar)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			this._grammar.Validate();
			this._grammar.WriteSrgs(srgsGrammar);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001FC7A File Offset: 0x0001EC7A
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0001FC6C File Offset: 0x0001EC6C
		public Uri XmlBase
		{
			get
			{
				return this._grammar.XmlBase;
			}
			set
			{
				this._grammar.XmlBase = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001FCC5 File Offset: 0x0001ECC5
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001FC87 File Offset: 0x0001EC87
		public CultureInfo Culture
		{
			get
			{
				return this._grammar.Culture;
			}
			set
			{
				Helpers.ThrowIfNull(value, "value");
				if (value.Equals(CultureInfo.InvariantCulture))
				{
					throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "value");
				}
				this._grammar.Culture = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001FCE0 File Offset: 0x0001ECE0
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x0001FCD2 File Offset: 0x0001ECD2
		public SrgsRule Root
		{
			get
			{
				return this._grammar.Root;
			}
			set
			{
				this._grammar.Root = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001FD01 File Offset: 0x0001ED01
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001FCED File Offset: 0x0001ECED
		public SrgsGrammarMode Mode
		{
			get
			{
				if (this._grammar.Mode != GrammarType.VoiceGrammar)
				{
					return SrgsGrammarMode.Dtmf;
				}
				return SrgsGrammarMode.Voice;
			}
			set
			{
				this._grammar.Mode = ((value == SrgsGrammarMode.Voice) ? GrammarType.VoiceGrammar : GrammarType.DtmfGrammar);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001FD2D File Offset: 0x0001ED2D
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0001FD13 File Offset: 0x0001ED13
		public SrgsPhoneticAlphabet PhoneticAlphabet
		{
			get
			{
				return (SrgsPhoneticAlphabet)this._grammar.PhoneticAlphabet;
			}
			set
			{
				this._grammar.PhoneticAlphabet = (AlphabetType)value;
				this._grammar.HasPhoneticAlphabetBeenSet = true;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001FD3A File Offset: 0x0001ED3A
		public SrgsRulesCollection Rules
		{
			get
			{
				return this._grammar.Rules;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001FD55 File Offset: 0x0001ED55
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001FD47 File Offset: 0x0001ED47
		public string Language
		{
			get
			{
				return this._grammar.Language;
			}
			set
			{
				this._grammar.Language = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001FD70 File Offset: 0x0001ED70
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001FD62 File Offset: 0x0001ED62
		public string Namespace
		{
			get
			{
				return this._grammar.Namespace;
			}
			set
			{
				this._grammar.Namespace = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001FD7D File Offset: 0x0001ED7D
		public Collection<string> CodeBehind
		{
			get
			{
				return this._grammar.CodeBehind;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001FD8A File Offset: 0x0001ED8A
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001FD97 File Offset: 0x0001ED97
		public bool Debug
		{
			get
			{
				return this._grammar.Debug;
			}
			set
			{
				this._grammar.Debug = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001FDBE File Offset: 0x0001EDBE
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001FDA5 File Offset: 0x0001EDA5
		public string Script
		{
			get
			{
				return this._grammar.Script;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._grammar.Script = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001FDCB File Offset: 0x0001EDCB
		public Collection<string> ImportNamespaces
		{
			get
			{
				return this._grammar.ImportNamespaces;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001FDD8 File Offset: 0x0001EDD8
		public Collection<string> AssemblyReferences
		{
			get
			{
				return this._grammar.AssemblyReferences;
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001FDE8 File Offset: 0x0001EDE8
		internal void Load(XmlReader srgsGrammar)
		{
			this._grammar = new SrgsGrammar();
			this._grammar.PhoneticAlphabet = AlphabetType.Sapi;
			new XmlParser(srgsGrammar, null)
			{
				ElementFactory = new SrgsElementFactory(this._grammar)
			}.Parse();
			if (!string.IsNullOrEmpty(srgsGrammar.BaseURI))
			{
				this._baseUri = new Uri(srgsGrammar.BaseURI);
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001FE4C File Offset: 0x0001EE4C
		internal static GrammarOptions TagFormat2GrammarOptions(SrgsTagFormat value)
		{
			GrammarOptions grammarOptions = GrammarOptions.KeyValuePairs;
			switch (value)
			{
			case SrgsTagFormat.MssV1:
				grammarOptions = GrammarOptions.MssV1;
				break;
			case SrgsTagFormat.W3cV1:
				grammarOptions = GrammarOptions.W3cV1;
				break;
			case SrgsTagFormat.KeyValuePairs:
				grammarOptions = GrammarOptions.KeyValuePairSrgs;
				break;
			}
			return grammarOptions;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001FE80 File Offset: 0x0001EE80
		internal static SrgsTagFormat GrammarOptions2TagFormat(GrammarOptions value)
		{
			SrgsTagFormat srgsTagFormat = SrgsTagFormat.Default;
			GrammarOptions grammarOptions = value & GrammarOptions.TagFormat;
			switch (grammarOptions)
			{
			case GrammarOptions.KeyValuePairs:
			case GrammarOptions.KeyValuePairSrgs:
				srgsTagFormat = SrgsTagFormat.KeyValuePairs;
				break;
			case GrammarOptions.MssV1:
				srgsTagFormat = SrgsTagFormat.MssV1;
				break;
			default:
				if (grammarOptions == GrammarOptions.W3cV1)
				{
					srgsTagFormat = SrgsTagFormat.W3cV1;
				}
				break;
			}
			return srgsTagFormat;
		}

		// Token: 0x170000FF RID: 255
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0001FEB7 File Offset: 0x0001EEB7
		internal SrgsTagFormat TagFormat
		{
			set
			{
				this._grammar.TagFormat = value;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001FEC5 File Offset: 0x0001EEC5
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001FECD File Offset: 0x0001EECD
		internal SrgsGrammar Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x04000544 RID: 1348
		private SrgsGrammar _grammar;

		// Token: 0x04000545 RID: 1349
		private Uri _baseUri;
	}
}
