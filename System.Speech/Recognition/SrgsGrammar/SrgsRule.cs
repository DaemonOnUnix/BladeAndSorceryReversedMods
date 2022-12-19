using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200011F RID: 287
	[DebuggerDisplay("Rule={_id.ToString()} Scope={_scope.ToString()}")]
	[DebuggerTypeProxy(typeof(SrgsRule.SrgsRuleDebugDisplay))]
	[Serializable]
	public class SrgsRule : IRule, IElement
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x000219B8 File Offset: 0x000209B8
		private SrgsRule()
		{
			this._elements = new SrgsElementList();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000219E4 File Offset: 0x000209E4
		public SrgsRule(string id)
			: this()
		{
			XmlParser.ValidateRuleId(id);
			this.Id = id;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000219FC File Offset: 0x000209FC
		public SrgsRule(string id, params SrgsElement[] elements)
			: this()
		{
			Helpers.ThrowIfNull(elements, "elements");
			XmlParser.ValidateRuleId(id);
			this.Id = id;
			for (int i = 0; i < elements.Length; i++)
			{
				if (elements[i] == null)
				{
					throw new ArgumentNullException("elements", SR.Get(SRID.ParamsEntryNullIllegal, new object[0]));
				}
				this._elements.Add(elements[i]);
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00021A5F File Offset: 0x00020A5F
		public void Add(SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			this.Elements.Add(element);
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00021A78 File Offset: 0x00020A78
		public Collection<SrgsElement> Elements
		{
			get
			{
				return this._elements;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00021A80 File Offset: 0x00020A80
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x00021A88 File Offset: 0x00020A88
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				XmlParser.ValidateRuleId(value);
				this._id = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00021A97 File Offset: 0x00020A97
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x00021A9F File Offset: 0x00020A9F
		public SrgsRuleScope Scope
		{
			get
			{
				return this._scope;
			}
			set
			{
				this._scope = value;
				this._isScopeSet = true;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x00021AB8 File Offset: 0x00020AB8
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x00021AAF File Offset: 0x00020AAF
		public string BaseClass
		{
			get
			{
				return this._baseclass;
			}
			set
			{
				this._baseclass = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00021AD4 File Offset: 0x00020AD4
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x00021AC0 File Offset: 0x00020AC0
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00021AEC File Offset: 0x00020AEC
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x00021ADC File Offset: 0x00020ADC
		public string OnInit
		{
			get
			{
				return this._onInit;
			}
			set
			{
				this.ValidateIdentifier(value);
				this._onInit = value;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00021B04 File Offset: 0x00020B04
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00021AF4 File Offset: 0x00020AF4
		public string OnParse
		{
			get
			{
				return this._onParse;
			}
			set
			{
				this.ValidateIdentifier(value);
				this._onParse = value;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00021B1C File Offset: 0x00020B1C
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00021B0C File Offset: 0x00020B0C
		public string OnError
		{
			get
			{
				return this._onError;
			}
			set
			{
				this.ValidateIdentifier(value);
				this._onError = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00021B34 File Offset: 0x00020B34
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00021B24 File Offset: 0x00020B24
		public string OnRecognition
		{
			get
			{
				return this._onRecognition;
			}
			set
			{
				this.ValidateIdentifier(value);
				this._onRecognition = value;
			}
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00021B3C File Offset: 0x00020B3C
		internal void WriteSrgs(XmlWriter writer)
		{
			if (this.Elements.Count == 0)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyRule, new object[] { "rule", this._id });
			}
			writer.WriteStartElement("rule");
			writer.WriteAttributeString("id", this._id);
			if (this._isScopeSet)
			{
				switch (this._scope)
				{
				case SrgsRuleScope.Public:
					writer.WriteAttributeString("scope", "public");
					break;
				case SrgsRuleScope.Private:
					writer.WriteAttributeString("scope", "private");
					break;
				}
			}
			if (this._baseclass != null)
			{
				writer.WriteAttributeString("sapi", "baseclass", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._baseclass);
			}
			if (this._dynamic != RuleDynamic.NotSet)
			{
				writer.WriteAttributeString("sapi", "dynamic", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", (this._dynamic == RuleDynamic.True) ? "true" : "false");
			}
			if (this.OnInit != null)
			{
				writer.WriteAttributeString("sapi", "onInit", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this.OnInit);
			}
			if (this.OnParse != null)
			{
				writer.WriteAttributeString("sapi", "onParse", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this.OnParse);
			}
			if (this.OnError != null)
			{
				writer.WriteAttributeString("sapi", "onError", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this.OnError);
			}
			if (this.OnRecognition != null)
			{
				writer.WriteAttributeString("sapi", "onRecognition", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this.OnRecognition);
			}
			Type type = null;
			foreach (SrgsElement srgsElement in this._elements)
			{
				Type type2 = srgsElement.GetType();
				if (type2 == typeof(SrgsText) && type2 == type)
				{
					writer.WriteString(" ");
				}
				type = type2;
				srgsElement.WriteSrgs(writer);
			}
			writer.WriteEndElement();
			if (this.HasCode)
			{
				this.WriteScriptElement(writer, this._script);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00021D40 File Offset: 0x00020D40
		internal void Validate(SrgsGrammar grammar)
		{
			bool flag = this.HasCode || this._onInit != null || this._onParse != null || this._onError != null || this._onRecognition != null || this._baseclass != null;
			grammar._fContainsCode = grammar._fContainsCode || flag;
			grammar.HasSapiExtension = grammar.HasSapiExtension || flag;
			if (this._dynamic != RuleDynamic.NotSet)
			{
				grammar.HasSapiExtension = true;
			}
			if (this.OnInit != null && this.Scope != SrgsRuleScope.Public)
			{
				XmlParser.ThrowSrgsException(SRID.OnInitOnPublicRule, new object[] { "OnInit", this.Id });
			}
			if (this.OnRecognition != null && this.Scope != SrgsRuleScope.Public)
			{
				XmlParser.ThrowSrgsException(SRID.OnInitOnPublicRule, new object[] { "OnRecognition", this.Id });
			}
			foreach (SrgsElement srgsElement in this._elements)
			{
				srgsElement.Validate(grammar);
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00021E58 File Offset: 0x00020E58
		void IElement.PostParse(IElement grammar)
		{
			((SrgsGrammar)grammar).Rules.Add(new SrgsRule[] { this });
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00021E84 File Offset: 0x00020E84
		void IRule.CreateScript(IGrammar grammar, string rule, string method, RuleMethodScript type)
		{
			switch (type)
			{
			case RuleMethodScript.onInit:
				this._onInit = method;
				return;
			case RuleMethodScript.onParse:
				this._onParse = method;
				return;
			case RuleMethodScript.onRecognition:
				this._onRecognition = method;
				return;
			case RuleMethodScript.onError:
				this._onError = method;
				return;
			default:
				return;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00021ED5 File Offset: 0x00020ED5
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x00021ECC File Offset: 0x00020ECC
		internal RuleDynamic Dynamic
		{
			get
			{
				return this._dynamic;
			}
			set
			{
				this._dynamic = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00021EDD File Offset: 0x00020EDD
		internal bool HasCode
		{
			get
			{
				return this._script.Length > 0;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00021EED File Offset: 0x00020EED
		private void WriteScriptElement(XmlWriter writer, string sCode)
		{
			writer.WriteStartElement("sapi", "script", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
			writer.WriteAttributeString("sapi", "rule", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._id);
			writer.WriteCData(sCode);
			writer.WriteEndElement();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00021F2C File Offset: 0x00020F2C
		private void ValidateIdentifier(string s)
		{
			if (s == this._id)
			{
				XmlParser.ThrowSrgsException(SRID.ConstructorNotAllowed, new object[] { this._id });
			}
			if (s != null && (s.IndexOfAny(SrgsRule.invalidChars) >= 0 || s.Length == 0))
			{
				XmlParser.ThrowSrgsException(SRID.InvalidMethodName, new object[0]);
			}
		}

		// Token: 0x04000575 RID: 1397
		private SrgsElementList _elements;

		// Token: 0x04000576 RID: 1398
		private string _id;

		// Token: 0x04000577 RID: 1399
		private SrgsRuleScope _scope = SrgsRuleScope.Private;

		// Token: 0x04000578 RID: 1400
		private RuleDynamic _dynamic = RuleDynamic.NotSet;

		// Token: 0x04000579 RID: 1401
		private bool _isScopeSet;

		// Token: 0x0400057A RID: 1402
		private string _baseclass;

		// Token: 0x0400057B RID: 1403
		private string _script = string.Empty;

		// Token: 0x0400057C RID: 1404
		private string _onInit;

		// Token: 0x0400057D RID: 1405
		private string _onParse;

		// Token: 0x0400057E RID: 1406
		private string _onError;

		// Token: 0x0400057F RID: 1407
		private string _onRecognition;

		// Token: 0x04000580 RID: 1408
		private static readonly char[] invalidChars = new char[]
		{
			'?', '*', '+', '|', '(', ')', '^', '$', '/', ';',
			'.', '=', '<', '>', '[', ']', '{', '}', '\\', ' ',
			'\t', '\r', '\n'
		};

		// Token: 0x02000120 RID: 288
		internal class SrgsRuleDebugDisplay
		{
			// Token: 0x060007A5 RID: 1957 RVA: 0x00021FCF File Offset: 0x00020FCF
			public SrgsRuleDebugDisplay(SrgsRule rule)
			{
				this._rule = rule;
			}

			// Token: 0x17000137 RID: 311
			// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00021FDE File Offset: 0x00020FDE
			public object Id
			{
				get
				{
					return this._rule.Id;
				}
			}

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00021FEB File Offset: 0x00020FEB
			public object Scope
			{
				get
				{
					return this._rule.Scope;
				}
			}

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00021FFD File Offset: 0x00020FFD
			public object BaseClass
			{
				get
				{
					return this._rule.BaseClass;
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0002200A File Offset: 0x0002100A
			public object Script
			{
				get
				{
					return this._rule.Script;
				}
			}

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x060007AA RID: 1962 RVA: 0x00022017 File Offset: 0x00021017
			public object OnInit
			{
				get
				{
					return this._rule.OnInit;
				}
			}

			// Token: 0x1700013C RID: 316
			// (get) Token: 0x060007AB RID: 1963 RVA: 0x00022024 File Offset: 0x00021024
			public object OnParse
			{
				get
				{
					return this._rule.OnParse;
				}
			}

			// Token: 0x1700013D RID: 317
			// (get) Token: 0x060007AC RID: 1964 RVA: 0x00022031 File Offset: 0x00021031
			public object OnError
			{
				get
				{
					return this._rule.OnError;
				}
			}

			// Token: 0x1700013E RID: 318
			// (get) Token: 0x060007AD RID: 1965 RVA: 0x0002203E File Offset: 0x0002103E
			public object OnRecognition
			{
				get
				{
					return this._rule.OnRecognition;
				}
			}

			// Token: 0x1700013F RID: 319
			// (get) Token: 0x060007AE RID: 1966 RVA: 0x0002204B File Offset: 0x0002104B
			public object Count
			{
				get
				{
					return this._rule._elements.Count;
				}
			}

			// Token: 0x17000140 RID: 320
			// (get) Token: 0x060007AF RID: 1967 RVA: 0x00022064 File Offset: 0x00021064
			[DebuggerBrowsable(3)]
			public SrgsElement[] AKeys
			{
				get
				{
					SrgsElement[] array = new SrgsElement[this._rule._elements.Count];
					for (int i = 0; i < this._rule._elements.Count; i++)
					{
						array[i] = this._rule._elements[i];
					}
					return array;
				}
			}

			// Token: 0x04000581 RID: 1409
			private SrgsRule _rule;
		}
	}
}
