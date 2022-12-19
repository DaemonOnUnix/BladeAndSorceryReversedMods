using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200007E RID: 126
	[DebuggerDisplay("Rule={_id.ToString()} Scope={_scope.ToString()}")]
	[DebuggerTypeProxy(typeof(SrgsRule.SrgsRuleDebugDisplay))]
	[Serializable]
	public class SrgsRule : IRule, IElement
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x0001106C File Offset: 0x0000F26C
		private SrgsRule()
		{
			this._elements = new SrgsElementList();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00011098 File Offset: 0x0000F298
		public SrgsRule(string id)
			: this()
		{
			XmlParser.ValidateRuleId(id);
			this.Id = id;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000110B0 File Offset: 0x0000F2B0
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

		// Token: 0x06000438 RID: 1080 RVA: 0x00011113 File Offset: 0x0000F313
		public void Add(SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			this.Elements.Add(element);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0001112C File Offset: 0x0000F32C
		public Collection<SrgsElement> Elements
		{
			get
			{
				return this._elements;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00011134 File Offset: 0x0000F334
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0001113C File Offset: 0x0000F33C
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

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001114B File Offset: 0x0000F34B
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00011153 File Offset: 0x0000F353
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0001116C File Offset: 0x0000F36C
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00011163 File Offset: 0x0000F363
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00011188 File Offset: 0x0000F388
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00011174 File Offset: 0x0000F374
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000111A0 File Offset: 0x0000F3A0
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00011190 File Offset: 0x0000F390
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000111B8 File Offset: 0x0000F3B8
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000111A8 File Offset: 0x0000F3A8
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x000111D0 File Offset: 0x0000F3D0
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x000111C0 File Offset: 0x0000F3C0
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x000111E8 File Offset: 0x0000F3E8
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x000111D8 File Offset: 0x0000F3D8
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

		// Token: 0x0600044A RID: 1098 RVA: 0x000111F0 File Offset: 0x0000F3F0
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
				SrgsRuleScope scope = this._scope;
				if (scope != SrgsRuleScope.Public)
				{
					if (scope == SrgsRuleScope.Private)
					{
						writer.WriteAttributeString("scope", "private");
					}
				}
				else
				{
					writer.WriteAttributeString("scope", "public");
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

		// Token: 0x0600044B RID: 1099 RVA: 0x000113F0 File Offset: 0x0000F5F0
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

		// Token: 0x0600044C RID: 1100 RVA: 0x000114FC File Offset: 0x0000F6FC
		void IElement.PostParse(IElement grammar)
		{
			((SrgsGrammar)grammar).Rules.Add(new SrgsRule[] { this });
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011518 File Offset: 0x0000F718
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0001155C File Offset: 0x0000F75C
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00011553 File Offset: 0x0000F753
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00011564 File Offset: 0x0000F764
		internal bool HasCode
		{
			get
			{
				return this._script.Length > 0;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00011574 File Offset: 0x0000F774
		private void WriteScriptElement(XmlWriter writer, string sCode)
		{
			writer.WriteStartElement("sapi", "script", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions");
			writer.WriteAttributeString("sapi", "rule", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._id);
			writer.WriteCData(sCode);
			writer.WriteEndElement();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000115B4 File Offset: 0x0000F7B4
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

		// Token: 0x040003EB RID: 1003
		private SrgsElementList _elements;

		// Token: 0x040003EC RID: 1004
		private string _id;

		// Token: 0x040003ED RID: 1005
		private SrgsRuleScope _scope = SrgsRuleScope.Private;

		// Token: 0x040003EE RID: 1006
		private RuleDynamic _dynamic = RuleDynamic.NotSet;

		// Token: 0x040003EF RID: 1007
		private bool _isScopeSet;

		// Token: 0x040003F0 RID: 1008
		private string _baseclass;

		// Token: 0x040003F1 RID: 1009
		private string _script = string.Empty;

		// Token: 0x040003F2 RID: 1010
		private string _onInit;

		// Token: 0x040003F3 RID: 1011
		private string _onParse;

		// Token: 0x040003F4 RID: 1012
		private string _onError;

		// Token: 0x040003F5 RID: 1013
		private string _onRecognition;

		// Token: 0x040003F6 RID: 1014
		private static readonly char[] invalidChars = new char[]
		{
			'?', '*', '+', '|', '(', ')', '^', '$', '/', ';',
			'.', '=', '<', '>', '[', ']', '{', '}', '\\', ' ',
			'\t', '\r', '\n'
		};

		// Token: 0x02000181 RID: 385
		internal class SrgsRuleDebugDisplay
		{
			// Token: 0x06000B5F RID: 2911 RVA: 0x0002D774 File Offset: 0x0002B974
			public SrgsRuleDebugDisplay(SrgsRule rule)
			{
				this._rule = rule;
			}

			// Token: 0x1700020C RID: 524
			// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0002D783 File Offset: 0x0002B983
			public object Id
			{
				get
				{
					return this._rule.Id;
				}
			}

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0002D790 File Offset: 0x0002B990
			public object Scope
			{
				get
				{
					return this._rule.Scope;
				}
			}

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0002D7A2 File Offset: 0x0002B9A2
			public object BaseClass
			{
				get
				{
					return this._rule.BaseClass;
				}
			}

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002D7AF File Offset: 0x0002B9AF
			public object Script
			{
				get
				{
					return this._rule.Script;
				}
			}

			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0002D7BC File Offset: 0x0002B9BC
			public object OnInit
			{
				get
				{
					return this._rule.OnInit;
				}
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0002D7C9 File Offset: 0x0002B9C9
			public object OnParse
			{
				get
				{
					return this._rule.OnParse;
				}
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0002D7D6 File Offset: 0x0002B9D6
			public object OnError
			{
				get
				{
					return this._rule.OnError;
				}
			}

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0002D7E3 File Offset: 0x0002B9E3
			public object OnRecognition
			{
				get
				{
					return this._rule.OnRecognition;
				}
			}

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0002D7F0 File Offset: 0x0002B9F0
			public object Count
			{
				get
				{
					return this._rule._elements.Count;
				}
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0002D808 File Offset: 0x0002BA08
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
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

			// Token: 0x040008B8 RID: 2232
			private SrgsRule _rule;
		}
	}
}
