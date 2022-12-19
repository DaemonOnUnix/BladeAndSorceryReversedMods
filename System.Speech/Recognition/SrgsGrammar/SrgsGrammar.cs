using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000117 RID: 279
	[Serializable]
	internal sealed class SrgsGrammar : IGrammar, IElement
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x000201E0 File Offset: 0x0001F1E0
		internal SrgsGrammar()
		{
			this._rules = new SrgsRulesCollection();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00020254 File Offset: 0x0001F254
		internal void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("grammar", "http://www.w3.org/2001/06/grammar");
			writer.WriteAttributeString("xml", "lang", null, this._culture.ToString());
			if (this._root != null)
			{
				writer.WriteAttributeString("root", this._root.Id);
			}
			this.WriteSTGAttributes(writer);
			if (this._isModeSet)
			{
				switch (this._mode)
				{
				case SrgsGrammarMode.Voice:
					writer.WriteAttributeString("mode", "voice");
					break;
				case SrgsGrammarMode.Dtmf:
					writer.WriteAttributeString("mode", "dtmf");
					break;
				}
			}
			string text = null;
			switch (this._tagFormat)
			{
			case SrgsTagFormat.MssV1:
				text = "semantics-ms/1.0";
				break;
			case SrgsTagFormat.W3cV1:
				text = "semantics/1.0";
				break;
			case SrgsTagFormat.KeyValuePairs:
				text = "properties-ms/1.0";
				break;
			}
			if (text != null)
			{
				writer.WriteAttributeString("tag-format", text);
			}
			if (this._hasPhoneticAlphabetBeenSet || (this._phoneticAlphabet != SrgsPhoneticAlphabet.Sapi && this.HasPronunciation))
			{
				string text2 = ((this._phoneticAlphabet == SrgsPhoneticAlphabet.Ipa) ? "ipa" : ((this._phoneticAlphabet == SrgsPhoneticAlphabet.Ups) ? "x-microsoft-ups" : "x-microsoft-sapi"));
				writer.WriteAttributeString("sapi", "alphabet", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", text2);
			}
			if (this._xmlBase != null)
			{
				writer.WriteAttributeString("xml:base", this._xmlBase.ToString());
			}
			writer.WriteAttributeString("version", "1.0");
			writer.WriteAttributeString("xmlns", "http://www.w3.org/2001/06/grammar");
			if (this._isSapiExtensionUsed)
			{
				writer.WriteAttributeString("xmlns", "sapi", null, "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
			}
			foreach (SrgsRule srgsRule in this._rules)
			{
				srgsRule.Validate(this);
			}
			foreach (string text3 in this._globalTags)
			{
				writer.WriteElementString("tag", text3);
			}
			this.WriteGrammarElements(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002048C File Offset: 0x0001F48C
		internal void Validate()
		{
			this.HasPronunciation = (this.HasSapiExtension = false);
			foreach (SrgsRule srgsRule in this._rules)
			{
				srgsRule.Validate(this);
			}
			this._isSapiExtensionUsed |= this.HasPronunciation;
			this._fContainsCode |= this._language != null || this._script.Length > 0 || this._usings.Count > 0 || this._assemblyReferences.Count > 0 || this._codebehind.Count > 0 || this._namespace != null || this._fDebug;
			this._isSapiExtensionUsed |= this._fContainsCode;
			if (!this.HasPronunciation)
			{
				this.PhoneticAlphabet = AlphabetType.Sapi;
			}
			if (this._root != null && !this._rules.Contains(this._root))
			{
				XmlParser.ThrowSrgsException(SRID.RootNotDefined, new object[] { this._root.Id });
			}
			if (this._globalTags.Count > 0)
			{
				this._tagFormat = SrgsTagFormat.W3cV1;
			}
			if (this._fContainsCode)
			{
				if (this._tagFormat == SrgsTagFormat.Default)
				{
					this._tagFormat = SrgsTagFormat.KeyValuePairs;
				}
				if (this._tagFormat != SrgsTagFormat.KeyValuePairs)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidSemanticProcessingType, new object[0]);
				}
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000205F8 File Offset: 0x0001F5F8
		IRule IGrammar.CreateRule(string id, RulePublic publicRule, RuleDynamic dynamic, bool hasScript)
		{
			SrgsRule srgsRule = new SrgsRule(id);
			if (publicRule != RulePublic.NotSet)
			{
				srgsRule.Scope = ((publicRule == RulePublic.True) ? SrgsRuleScope.Public : SrgsRuleScope.Private);
			}
			srgsRule.Dynamic = dynamic;
			return srgsRule;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00020628 File Offset: 0x0001F628
		void IElement.PostParse(IElement parent)
		{
			if (this._sRoot != null)
			{
				bool flag = false;
				foreach (SrgsRule srgsRule in this.Rules)
				{
					if (srgsRule.Id == this._sRoot)
					{
						this.Root = srgsRule;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					XmlParser.ThrowSrgsException(SRID.RootNotDefined, new object[] { this._sRoot });
				}
			}
			foreach (XmlParser.ForwardReference forwardReference in this._scriptsForwardReference)
			{
				SrgsRule srgsRule2 = this.Rules[forwardReference._name];
				if (srgsRule2 != null)
				{
					srgsRule2.Script += forwardReference._value;
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.InvalidScriptDefinition, new object[0]);
				}
			}
			this.Validate();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00020738 File Offset: 0x0001F738
		internal void AddScript(string rule, string code)
		{
			if (rule == null)
			{
				this._script += code;
				return;
			}
			this._scriptsForwardReference.Add(new XmlParser.ForwardReference(rule, code));
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0002076B File Offset: 0x0001F76B
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00020762 File Offset: 0x0001F762
		string IGrammar.Root
		{
			get
			{
				return this._sRoot;
			}
			set
			{
				this._sRoot = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00020773 File Offset: 0x0001F773
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0002077B File Offset: 0x0001F77B
		public Uri XmlBase
		{
			get
			{
				return this._xmlBase;
			}
			set
			{
				this._xmlBase = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00020784 File Offset: 0x0001F784
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0002078C File Offset: 0x0001F78C
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				Helpers.ThrowIfNull(value, "value");
				this._culture = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000207A0 File Offset: 0x0001F7A0
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x000207AD File Offset: 0x0001F7AD
		public GrammarType Mode
		{
			get
			{
				if (this._mode != SrgsGrammarMode.Voice)
				{
					return GrammarType.DtmfGrammar;
				}
				return GrammarType.VoiceGrammar;
			}
			set
			{
				this._mode = ((value == GrammarType.VoiceGrammar) ? SrgsGrammarMode.Voice : SrgsGrammarMode.Dtmf);
				this._isModeSet = true;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x000207C3 File Offset: 0x0001F7C3
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x000207CB File Offset: 0x0001F7CB
		public AlphabetType PhoneticAlphabet
		{
			get
			{
				return (AlphabetType)this._phoneticAlphabet;
			}
			set
			{
				this._phoneticAlphabet = (SrgsPhoneticAlphabet)value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x000207D4 File Offset: 0x0001F7D4
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x000207DC File Offset: 0x0001F7DC
		public SrgsRule Root
		{
			get
			{
				return this._root;
			}
			set
			{
				this._root = value;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x000207E5 File Offset: 0x0001F7E5
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x000207ED File Offset: 0x0001F7ED
		public SrgsTagFormat TagFormat
		{
			get
			{
				return this._tagFormat;
			}
			set
			{
				this._tagFormat = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x000207F6 File Offset: 0x0001F7F6
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x000207FE File Offset: 0x0001F7FE
		public Collection<string> GlobalTags
		{
			get
			{
				return this._globalTags;
			}
			set
			{
				this._globalTags = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00020807 File Offset: 0x0001F807
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0002080F File Offset: 0x0001F80F
		public string Language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00020818 File Offset: 0x0001F818
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00020820 File Offset: 0x0001F820
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00020830 File Offset: 0x0001F830
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x00020829 File Offset: 0x0001F829
		public Collection<string> CodeBehind
		{
			get
			{
				return this._codebehind;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00020838 File Offset: 0x0001F838
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00020840 File Offset: 0x0001F840
		public bool Debug
		{
			get
			{
				return this._fDebug;
			}
			set
			{
				this._fDebug = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002085D File Offset: 0x0001F85D
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x00020849 File Offset: 0x0001F849
		public string Script
		{
			get
			{
				return this._script;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._script = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002086C File Offset: 0x0001F86C
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x00020865 File Offset: 0x0001F865
		public Collection<string> ImportNamespaces
		{
			get
			{
				return this._usings;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002087B File Offset: 0x0001F87B
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x00020874 File Offset: 0x0001F874
		public Collection<string> AssemblyReferences
		{
			get
			{
				return this._assemblyReferences;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00020883 File Offset: 0x0001F883
		internal SrgsRulesCollection Rules
		{
			get
			{
				return this._rules;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00020894 File Offset: 0x0001F894
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0002088B File Offset: 0x0001F88B
		internal bool HasPronunciation
		{
			get
			{
				return this._hasPronunciation;
			}
			set
			{
				this._hasPronunciation = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0002089C File Offset: 0x0001F89C
		internal bool HasPhoneticAlphabetBeenSet
		{
			set
			{
				this._hasPhoneticAlphabetBeenSet = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x000208AE File Offset: 0x0001F8AE
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x000208A5 File Offset: 0x0001F8A5
		internal bool HasSapiExtension
		{
			get
			{
				return this._isSapiExtensionUsed;
			}
			set
			{
				this._isSapiExtensionUsed = value;
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000208B8 File Offset: 0x0001F8B8
		private void WriteSTGAttributes(XmlWriter writer)
		{
			if (this._language != null)
			{
				writer.WriteAttributeString("sapi", "language", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._language);
			}
			if (this._namespace != null)
			{
				writer.WriteAttributeString("sapi", "namespace", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._namespace);
			}
			foreach (string text in this._codebehind)
			{
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("sapi", "codebehind", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", text);
				}
			}
			if (this._fDebug)
			{
				writer.WriteAttributeString("sapi", "debug", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", "True");
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00020984 File Offset: 0x0001F984
		private void WriteGrammarElements(XmlWriter writer)
		{
			foreach (string text in this._assemblyReferences)
			{
				writer.WriteStartElement("sapi", "assemblyReference", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
				writer.WriteAttributeString("sapi", "assembly", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", text);
				writer.WriteEndElement();
			}
			foreach (string text2 in this._usings)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					writer.WriteStartElement("sapi", "importNamespace", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
					writer.WriteAttributeString("sapi", "namespace", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", text2);
					writer.WriteEndElement();
				}
			}
			this.WriteRules(writer);
			this.WriteGlobalScripts(writer);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00020A78 File Offset: 0x0001FA78
		private void WriteRules(XmlWriter writer)
		{
			foreach (SrgsRule srgsRule in this._rules)
			{
				srgsRule.WriteSrgs(writer);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00020AC8 File Offset: 0x0001FAC8
		private void WriteGlobalScripts(XmlWriter writer)
		{
			if (this._script.Length > 0)
			{
				writer.WriteStartElement("sapi", "script", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
				writer.WriteCData(this._script);
				writer.WriteEndElement();
			}
		}

		// Token: 0x04000550 RID: 1360
		private bool _isSapiExtensionUsed;

		// Token: 0x04000551 RID: 1361
		private Uri _xmlBase;

		// Token: 0x04000552 RID: 1362
		private CultureInfo _culture = CultureInfo.CurrentUICulture;

		// Token: 0x04000553 RID: 1363
		private SrgsGrammarMode _mode;

		// Token: 0x04000554 RID: 1364
		private SrgsPhoneticAlphabet _phoneticAlphabet = SrgsPhoneticAlphabet.Ipa;

		// Token: 0x04000555 RID: 1365
		private bool _hasPhoneticAlphabetBeenSet;

		// Token: 0x04000556 RID: 1366
		private bool _hasPronunciation;

		// Token: 0x04000557 RID: 1367
		private SrgsRule _root;

		// Token: 0x04000558 RID: 1368
		private SrgsTagFormat _tagFormat;

		// Token: 0x04000559 RID: 1369
		private Collection<string> _globalTags = new Collection<string>();

		// Token: 0x0400055A RID: 1370
		private bool _isModeSet;

		// Token: 0x0400055B RID: 1371
		private SrgsRulesCollection _rules;

		// Token: 0x0400055C RID: 1372
		private string _sRoot;

		// Token: 0x0400055D RID: 1373
		internal bool _fContainsCode;

		// Token: 0x0400055E RID: 1374
		private string _language;

		// Token: 0x0400055F RID: 1375
		private Collection<string> _codebehind = new Collection<string>();

		// Token: 0x04000560 RID: 1376
		private string _namespace;

		// Token: 0x04000561 RID: 1377
		internal bool _fDebug;

		// Token: 0x04000562 RID: 1378
		private string _script = string.Empty;

		// Token: 0x04000563 RID: 1379
		private List<XmlParser.ForwardReference> _scriptsForwardReference = new List<XmlParser.ForwardReference>();

		// Token: 0x04000564 RID: 1380
		private Collection<string> _usings = new Collection<string>();

		// Token: 0x04000565 RID: 1381
		private Collection<string> _assemblyReferences = new Collection<string>();
	}
}
