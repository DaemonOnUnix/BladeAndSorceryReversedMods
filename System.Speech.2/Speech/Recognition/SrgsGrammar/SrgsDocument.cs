using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class SrgsDocument
	{
		// Token: 0x06000394 RID: 916 RVA: 0x0000F40E File Offset: 0x0000D60E
		public SrgsDocument()
		{
			this._grammar = new SrgsGrammar();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000F424 File Offset: 0x0000D624
		public SrgsDocument(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			using (XmlTextReader xmlTextReader = new XmlTextReader(path))
			{
				this.Load(xmlTextReader);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000F46C File Offset: 0x0000D66C
		public SrgsDocument(XmlReader srgsGrammar)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			this.Load(srgsGrammar);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000F488 File Offset: 0x0000D688
		public SrgsDocument(GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			this._grammar = new SrgsGrammar();
			this._grammar.Culture = builder.Culture;
			IElementFactory elementFactory = new SrgsElementFactory(this._grammar);
			builder.CreateGrammar(elementFactory);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000F4D5 File Offset: 0x0000D6D5
		public SrgsDocument(SrgsRule grammarRootRule)
			: this()
		{
			Helpers.ThrowIfNull(grammarRootRule, "grammarRootRule");
			this.Root = grammarRootRule;
			this.Rules.Add(new SrgsRule[] { grammarRootRule });
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000F504 File Offset: 0x0000D704
		public void WriteSrgs(XmlWriter srgsGrammar)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			this._grammar.Validate();
			this._grammar.WriteSrgs(srgsGrammar);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000F536 File Offset: 0x0000D736
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000F528 File Offset: 0x0000D728
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000F581 File Offset: 0x0000D781
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000F543 File Offset: 0x0000D743
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000F59C File Offset: 0x0000D79C
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000F58E File Offset: 0x0000D78E
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000F5BD File Offset: 0x0000D7BD
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000F5A9 File Offset: 0x0000D7A9
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000F5CF File Offset: 0x0000D7CF
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000F5F6 File Offset: 0x0000D7F6
		public SrgsRulesCollection Rules
		{
			get
			{
				return this._grammar.Rules;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000F611 File Offset: 0x0000D811
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000F603 File Offset: 0x0000D803
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000F62C File Offset: 0x0000D82C
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000F61E File Offset: 0x0000D81E
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000F639 File Offset: 0x0000D839
		public Collection<string> CodeBehind
		{
			get
			{
				return this._grammar.CodeBehind;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000F646 File Offset: 0x0000D846
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0000F653 File Offset: 0x0000D853
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

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000F67A File Offset: 0x0000D87A
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000F661 File Offset: 0x0000D861
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000F687 File Offset: 0x0000D887
		public Collection<string> ImportNamespaces
		{
			get
			{
				return this._grammar.ImportNamespaces;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000F694 File Offset: 0x0000D894
		public Collection<string> AssemblyReferences
		{
			get
			{
				return this._grammar.AssemblyReferences;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
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

		// Token: 0x060003B1 RID: 945 RVA: 0x0000F708 File Offset: 0x0000D908
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

		// Token: 0x060003B2 RID: 946 RVA: 0x0000F738 File Offset: 0x0000D938
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

		// Token: 0x170000DD RID: 221
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000F76F File Offset: 0x0000D96F
		internal SrgsTagFormat TagFormat
		{
			set
			{
				this._grammar.TagFormat = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000F77D File Offset: 0x0000D97D
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000F785 File Offset: 0x0000D985
		internal SrgsGrammar Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x040003C1 RID: 961
		private SrgsGrammar _grammar;

		// Token: 0x040003C2 RID: 962
		private Uri _baseUri;
	}
}
