using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000AC RID: 172
	internal class GrammarElement : ParseElement, IGrammar, IElement
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x0000EFB8 File Offset: 0x0000DFB8
		internal GrammarElement(Backend backend, CustomGrammar cg)
			: base(null)
		{
			this._cg = cg;
			this._backend = backend;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000EFEE File Offset: 0x0000DFEE
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000EFE5 File Offset: 0x0000DFE5
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

		// Token: 0x060003C7 RID: 967 RVA: 0x0000EFF8 File Offset: 0x0000DFF8
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

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F060 File Offset: 0x0000E060
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

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F124 File Offset: 0x0000E124
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

		// Token: 0x17000078 RID: 120
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0000F194 File Offset: 0x0000E194
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

		// Token: 0x17000079 RID: 121
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000F1B0 File Offset: 0x0000E1B0
		CultureInfo IGrammar.Culture
		{
			set
			{
				Helpers.ThrowIfNull(value, "value");
				this._backend.LangId = value.LCID;
			}
		}

		// Token: 0x1700007A RID: 122
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0000F1CE File Offset: 0x0000E1CE
		GrammarType IGrammar.Mode
		{
			set
			{
				this._backend.GrammarMode = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000F1DC File Offset: 0x0000E1DC
		AlphabetType IGrammar.PhoneticAlphabet
		{
			set
			{
				this._backend.Alphabet = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000F1EA File Offset: 0x0000E1EA
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000F1FC File Offset: 0x0000E1FC
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000F20F File Offset: 0x0000E20F
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000F21C File Offset: 0x0000E21C
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000F22A File Offset: 0x0000E22A
		internal List<Rule> UndefRules
		{
			get
			{
				return this._undefRules;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000F232 File Offset: 0x0000E232
		internal Backend Backend
		{
			get
			{
				return this._backend;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000F248 File Offset: 0x0000E248
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0000F23A File Offset: 0x0000E23A
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000F263 File Offset: 0x0000E263
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000F255 File Offset: 0x0000E255
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000F27E File Offset: 0x0000E27E
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000F270 File Offset: 0x0000E270
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

		// Token: 0x17000083 RID: 131
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000F28B File Offset: 0x0000E28B
		bool IGrammar.Debug
		{
			set
			{
				this._cg._fDebugScript = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000F2A7 File Offset: 0x0000E2A7
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000F299 File Offset: 0x0000E299
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000F2C2 File Offset: 0x0000E2C2
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000F2B4 File Offset: 0x0000E2B4
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000F2CF File Offset: 0x0000E2CF
		internal CustomGrammar CustomGrammar
		{
			get
			{
				return this._cg;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F2D8 File Offset: 0x0000E2D8
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

		// Token: 0x04000364 RID: 868
		private Backend _backend;

		// Token: 0x04000365 RID: 869
		private List<Rule> _undefRules = new List<Rule>();

		// Token: 0x04000366 RID: 870
		private List<Rule> _rules = new List<Rule>();

		// Token: 0x04000367 RID: 871
		private CustomGrammar _cg;

		// Token: 0x04000368 RID: 872
		private string _sRoot;

		// Token: 0x04000369 RID: 873
		private bool _hasRoot;
	}
}
