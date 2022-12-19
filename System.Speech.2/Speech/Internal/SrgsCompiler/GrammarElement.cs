using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F1 RID: 241
	internal class GrammarElement : ParseElement, IGrammar, IElement
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x000267D4 File Offset: 0x000249D4
		internal GrammarElement(Backend backend, CustomGrammar cg)
			: base(null)
		{
			this._cg = cg;
			this._backend = backend;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0002680A File Offset: 0x00024A0A
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x00026801 File Offset: 0x00024A01
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

		// Token: 0x0600087A RID: 2170 RVA: 0x00026814 File Offset: 0x00024A14
		IRule IGrammar.CreateRule(string id, RulePublic publicRule, RuleDynamic dynamic, bool hasScript)
		{
			SPCFGRULEATTRIBUTES spcfgruleattributes = (SPCFGRULEATTRIBUTES)0;
			if (id == this._sRoot)
			{
				spcfgruleattributes |= SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Active | SPCFGRULEATTRIBUTES.SPRAF_Root;
				this._hasRoot = true;
			}
			if (publicRule == RulePublic.True)
			{
				spcfgruleattributes |= SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Export;
			}
			if (dynamic == RuleDynamic.True)
			{
				spcfgruleattributes |= SPCFGRULEATTRIBUTES.SPRAF_Dynamic;
			}
			Rule rule = this.GetRule(id, spcfgruleattributes);
			if (publicRule == RulePublic.True || id == this._sRoot || hasScript)
			{
				this._cg._rules.Add(rule);
			}
			return rule;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00026880 File Offset: 0x00024A80
		void IElement.PostParse(IElement parent)
		{
			if (this._sRoot != null && !this._hasRoot)
			{
				XmlParser.ThrowSrgsException(SRID.RootNotDefined, new object[] { this._sRoot });
			}
			if (this._undefRules.Count > 0)
			{
				Rule rule = this._undefRules[0];
				XmlParser.ThrowSrgsException(SRID.UndefRuleRef, new object[] { rule.Name });
			}
			bool flag = ((IGrammar)this).CodeBehind.Count > 0 || ((IGrammar)this).ImportNamespaces.Count > 0 || ((IGrammar)this).AssemblyReferences.Count > 0 || this.CustomGrammar._scriptRefs.Count > 0;
			if (flag && ((IGrammar)this).TagFormat != SrgsTagFormat.KeyValuePairs)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidSemanticProcessingType, new object[0]);
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00026940 File Offset: 0x00024B40
		internal void AddScript(string name, string code)
		{
			foreach (Rule rule in this._cg._rules)
			{
				if (rule.Name == name)
				{
					rule.Script.Append(code);
					break;
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x000269B0 File Offset: 0x00024BB0
		Uri IGrammar.XmlBase
		{
			set
			{
				if (value != null)
				{
					this._backend.SetBasePath(value.ToString());
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x000269CC File Offset: 0x00024BCC
		CultureInfo IGrammar.Culture
		{
			set
			{
				Helpers.ThrowIfNull(value, "value");
				this._backend.LangId = value.LCID;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x000269EA File Offset: 0x00024BEA
		GrammarType IGrammar.Mode
		{
			set
			{
				this._backend.GrammarMode = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x000269F8 File Offset: 0x00024BF8
		AlphabetType IGrammar.PhoneticAlphabet
		{
			set
			{
				this._backend.Alphabet = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00026A06 File Offset: 0x00024C06
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x00026A18 File Offset: 0x00024C18
		SrgsTagFormat IGrammar.TagFormat
		{
			get
			{
				return SrgsDocument.GrammarOptions2TagFormat(this._backend.GrammarOptions);
			}
			set
			{
				this._backend.GrammarOptions = SrgsDocument.TagFormat2GrammarOptions(value);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00026A2B File Offset: 0x00024C2B
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00026A38 File Offset: 0x00024C38
		Collection<string> IGrammar.GlobalTags
		{
			get
			{
				return this._backend.GlobalTags;
			}
			set
			{
				this._backend.GlobalTags = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00026A46 File Offset: 0x00024C46
		internal List<Rule> UndefRules
		{
			get
			{
				return this._undefRules;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00026A4E File Offset: 0x00024C4E
		internal Backend Backend
		{
			get
			{
				return this._backend;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00026A64 File Offset: 0x00024C64
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00026A56 File Offset: 0x00024C56
		string IGrammar.Language
		{
			get
			{
				return this._cg._language;
			}
			set
			{
				this._cg._language = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00026A7F File Offset: 0x00024C7F
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00026A71 File Offset: 0x00024C71
		string IGrammar.Namespace
		{
			get
			{
				return this._cg._namespace;
			}
			set
			{
				this._cg._namespace = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00026A9A File Offset: 0x00024C9A
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00026A8C File Offset: 0x00024C8C
		Collection<string> IGrammar.CodeBehind
		{
			get
			{
				return this._cg._codebehind;
			}
			set
			{
				this._cg._codebehind = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00026AA7 File Offset: 0x00024CA7
		bool IGrammar.Debug
		{
			set
			{
				this._cg._fDebugScript = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x00026AC3 File Offset: 0x00024CC3
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x00026AB5 File Offset: 0x00024CB5
		Collection<string> IGrammar.ImportNamespaces
		{
			get
			{
				return this._cg._importNamespaces;
			}
			set
			{
				this._cg._importNamespaces = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00026ADE File Offset: 0x00024CDE
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x00026AD0 File Offset: 0x00024CD0
		Collection<string> IGrammar.AssemblyReferences
		{
			get
			{
				return this._cg._assemblyReferences;
			}
			set
			{
				this._cg._assemblyReferences = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00026AEB File Offset: 0x00024CEB
		internal CustomGrammar CustomGrammar
		{
			get
			{
				return this._cg;
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00026AF4 File Offset: 0x00024CF4
		private Rule GetRule(string sRuleId, SPCFGRULEATTRIBUTES dwAttributes)
		{
			Rule rule = this._backend.FindRule(sRuleId);
			if (rule != null)
			{
				int num = this._undefRules.IndexOf(rule);
				if (num != -1)
				{
					this._backend.SetRuleAttributes(rule, dwAttributes);
					this._undefRules.RemoveAt(num);
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.RuleRedefinition, new object[] { sRuleId });
				}
			}
			else
			{
				rule = this._backend.CreateRule(sRuleId, dwAttributes);
			}
			return rule;
		}

		// Token: 0x0400060D RID: 1549
		private Backend _backend;

		// Token: 0x0400060E RID: 1550
		private List<Rule> _undefRules = new List<Rule>();

		// Token: 0x0400060F RID: 1551
		private List<Rule> _rules = new List<Rule>();

		// Token: 0x04000610 RID: 1552
		private CustomGrammar _cg;

		// Token: 0x04000611 RID: 1553
		private string _sRoot;

		// Token: 0x04000612 RID: 1554
		private bool _hasRoot;
	}
}
