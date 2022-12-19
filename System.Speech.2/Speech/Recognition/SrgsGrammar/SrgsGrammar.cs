using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	internal sealed class SrgsGrammar : IGrammar, IElement
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		internal SrgsGrammar()
		{
			this._rules = new SrgsRulesCollection();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000FAE0 File Offset: 0x0000DCE0
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
				SrgsGrammarMode mode = this._mode;
				if (mode != SrgsGrammarMode.Voice)
				{
					if (mode == SrgsGrammarMode.Dtmf)
					{
						writer.WriteAttributeString("mode", "dtmf");
					}
				}
				else
				{
					writer.WriteAttributeString("mode", "voice");
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

		// Token: 0x060003D7 RID: 983 RVA: 0x0000FD10 File Offset: 0x0000DF10
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

		// Token: 0x060003D8 RID: 984 RVA: 0x0000FE78 File Offset: 0x0000E078
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

		// Token: 0x060003D9 RID: 985 RVA: 0x0000FEA8 File Offset: 0x0000E0A8
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

		// Token: 0x060003DA RID: 986 RVA: 0x0000FFB4 File Offset: 0x0000E1B4
		internal void AddScript(string rule, string code)
		{
			if (rule == null)
			{
				this._script += code;
				return;
			}
			this._scriptsForwardReference.Add(new XmlParser.ForwardReference(rule, code));
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000FFE7 File Offset: 0x0000E1E7
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000FFDE File Offset: 0x0000E1DE
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000FFEF File Offset: 0x0000E1EF
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000FFF7 File Offset: 0x0000E1F7
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00010000 File Offset: 0x0000E200
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00010008 File Offset: 0x0000E208
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001001C File Offset: 0x0000E21C
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00010029 File Offset: 0x0000E229
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001003F File Offset: 0x0000E23F
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x00010047 File Offset: 0x0000E247
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00010050 File Offset: 0x0000E250
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00010058 File Offset: 0x0000E258
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00010061 File Offset: 0x0000E261
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00010069 File Offset: 0x0000E269
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00010072 File Offset: 0x0000E272
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x0001007A File Offset: 0x0000E27A
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00010083 File Offset: 0x0000E283
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0001008B File Offset: 0x0000E28B
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00010094 File Offset: 0x0000E294
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001009C File Offset: 0x0000E29C
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000100AC File Offset: 0x0000E2AC
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x000100A5 File Offset: 0x0000E2A5
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000100B4 File Offset: 0x0000E2B4
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000100BC File Offset: 0x0000E2BC
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000100D9 File Offset: 0x0000E2D9
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000100C5 File Offset: 0x0000E2C5
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000100E1 File Offset: 0x0000E2E1
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x000100A5 File Offset: 0x0000E2A5
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x000100E9 File Offset: 0x0000E2E9
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x000100A5 File Offset: 0x0000E2A5
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

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000100F1 File Offset: 0x0000E2F1
		internal SrgsRulesCollection Rules
		{
			get
			{
				return this._rules;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00010102 File Offset: 0x0000E302
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x000100F9 File Offset: 0x0000E2F9
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

		// Token: 0x170000F6 RID: 246
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0001010A File Offset: 0x0000E30A
		internal bool HasPhoneticAlphabetBeenSet
		{
			set
			{
				this._hasPhoneticAlphabetBeenSet = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001011C File Offset: 0x0000E31C
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00010113 File Offset: 0x0000E313
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

		// Token: 0x060003FF RID: 1023 RVA: 0x00010124 File Offset: 0x0000E324
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

		// Token: 0x06000400 RID: 1024 RVA: 0x000101F0 File Offset: 0x0000E3F0
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

		// Token: 0x06000401 RID: 1025 RVA: 0x000102E4 File Offset: 0x0000E4E4
		private void WriteRules(XmlWriter writer)
		{
			foreach (SrgsRule srgsRule in this._rules)
			{
				srgsRule.WriteSrgs(writer);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00010334 File Offset: 0x0000E534
		private void WriteGlobalScripts(XmlWriter writer)
		{
			if (this._script.Length > 0)
			{
				writer.WriteStartElement("sapi", "script", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
				writer.WriteCData(this._script);
				writer.WriteEndElement();
			}
		}

		// Token: 0x040003CC RID: 972
		private bool _isSapiExtensionUsed;

		// Token: 0x040003CD RID: 973
		private Uri _xmlBase;

		// Token: 0x040003CE RID: 974
		private CultureInfo _culture = CultureInfo.CurrentUICulture;

		// Token: 0x040003CF RID: 975
		private SrgsGrammarMode _mode;

		// Token: 0x040003D0 RID: 976
		private SrgsPhoneticAlphabet _phoneticAlphabet = SrgsPhoneticAlphabet.Ipa;

		// Token: 0x040003D1 RID: 977
		private bool _hasPhoneticAlphabetBeenSet;

		// Token: 0x040003D2 RID: 978
		private bool _hasPronunciation;

		// Token: 0x040003D3 RID: 979
		private SrgsRule _root;

		// Token: 0x040003D4 RID: 980
		private SrgsTagFormat _tagFormat;

		// Token: 0x040003D5 RID: 981
		private Collection<string> _globalTags = new Collection<string>();

		// Token: 0x040003D6 RID: 982
		private bool _isModeSet;

		// Token: 0x040003D7 RID: 983
		private SrgsRulesCollection _rules;

		// Token: 0x040003D8 RID: 984
		private string _sRoot;

		// Token: 0x040003D9 RID: 985
		internal bool _fContainsCode;

		// Token: 0x040003DA RID: 986
		private string _language;

		// Token: 0x040003DB RID: 987
		private Collection<string> _codebehind = new Collection<string>();

		// Token: 0x040003DC RID: 988
		private string _namespace;

		// Token: 0x040003DD RID: 989
		internal bool _fDebug;

		// Token: 0x040003DE RID: 990
		private string _script = string.Empty;

		// Token: 0x040003DF RID: 991
		private List<XmlParser.ForwardReference> _scriptsForwardReference = new List<XmlParser.ForwardReference>();

		// Token: 0x040003E0 RID: 992
		private Collection<string> _usings = new Collection<string>();

		// Token: 0x040003E1 RID: 993
		private Collection<string> _assemblyReferences = new Collection<string>();
	}
}
