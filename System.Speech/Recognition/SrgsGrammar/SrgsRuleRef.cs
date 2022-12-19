using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000122 RID: 290
	[ImmutableObject(true)]
	[DebuggerDisplay("{DebuggerDisplayString()}")]
	[Serializable]
	public class SrgsRuleRef : SrgsElement, IRuleRef, IElement
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x000220B7 File Offset: 0x000210B7
		public SrgsRuleRef(Uri uri)
		{
			this.UriInit(uri, null, null, null);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000220C9 File Offset: 0x000210C9
		public SrgsRuleRef(Uri uri, string rule)
		{
			Helpers.ThrowIfEmptyOrNull(rule, "rule");
			this.UriInit(uri, rule, null, null);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000220E6 File Offset: 0x000210E6
		public SrgsRuleRef(Uri uri, string rule, string semanticKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticKey, "semanticKey");
			this.UriInit(uri, rule, semanticKey, null);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00022103 File Offset: 0x00021103
		public SrgsRuleRef(Uri uri, string rule, string semanticKey, string parameters)
		{
			Helpers.ThrowIfEmptyOrNull(parameters, "parameters");
			this.UriInit(uri, rule, semanticKey, parameters);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00022122 File Offset: 0x00021122
		public SrgsRuleRef(SrgsRule rule)
		{
			Helpers.ThrowIfNull(rule, "rule");
			this._uri = new Uri("#" + rule.Id, 2);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00022151 File Offset: 0x00021151
		public SrgsRuleRef(SrgsRule rule, string semanticKey)
			: this(rule)
		{
			Helpers.ThrowIfEmptyOrNull(semanticKey, "semanticKey");
			this._semanticKey = semanticKey;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002216C File Offset: 0x0002116C
		public SrgsRuleRef(SrgsRule rule, string semanticKey, string parameters)
			: this(rule)
		{
			Helpers.ThrowIfEmptyOrNull(parameters, "parameters");
			this._semanticKey = semanticKey;
			this._params = parameters;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002218E File Offset: 0x0002118E
		private SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType type)
		{
			this._type = type;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002219D File Offset: 0x0002119D
		internal SrgsRuleRef(string semanticKey, string parameters, Uri uri)
		{
			this._uri = uri;
			this._semanticKey = semanticKey;
			this._params = parameters;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x000221BA File Offset: 0x000211BA
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x000221C2 File Offset: 0x000211C2
		public string SemanticKey
		{
			get
			{
				return this._semanticKey;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x000221CA File Offset: 0x000211CA
		public string Params
		{
			get
			{
				return this._params;
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000221D4 File Offset: 0x000211D4
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("ruleref");
			if (this._uri != null)
			{
				writer.WriteAttributeString("uri", this._uri.ToString());
			}
			else
			{
				string text;
				switch (this._type)
				{
				case SrgsRuleRef.SpecialRuleRefType.Null:
					text = "NULL";
					break;
				case SrgsRuleRef.SpecialRuleRefType.Void:
					text = "VOID";
					break;
				case SrgsRuleRef.SpecialRuleRefType.Garbage:
					text = "GARBAGE";
					break;
				default:
					XmlParser.ThrowSrgsException(SRID.InvalidSpecialRuleRef, new object[0]);
					text = null;
					break;
				}
				writer.WriteAttributeString("special", text);
			}
			if (this._semanticKey != null)
			{
				writer.WriteAttributeString("sapi", "semantic-key", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._semanticKey);
			}
			if (this._params != null)
			{
				writer.WriteAttributeString("sapi", "params", "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions", this._params);
			}
			writer.WriteEndElement();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000222AC File Offset: 0x000212AC
		internal override void Validate(SrgsGrammar grammar)
		{
			bool flag = this._params != null || this._semanticKey != null;
			grammar._fContainsCode = grammar._fContainsCode || flag;
			grammar.HasSapiExtension = grammar.HasSapiExtension || flag;
			if (this._uri != null)
			{
				string text = this._uri.ToString();
				if (text.get_Chars(0) == '#')
				{
					bool flag2 = false;
					if (text.IndexOf("#grammar:dictation", 4) == 0 || text.IndexOf("#grammar:dictation#spelling", 4) == 0)
					{
						flag2 = true;
					}
					else
					{
						text = text.Substring(1);
						foreach (SrgsRule srgsRule in grammar.Rules)
						{
							if (srgsRule.Id == text)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						XmlParser.ThrowSrgsException(SRID.UndefRuleRef, new object[] { text });
					}
				}
			}
			base.Validate(grammar);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000223B4 File Offset: 0x000213B4
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsRuleRef");
			if (this._uri != null)
			{
				stringBuilder.Append(" uri='");
				stringBuilder.Append(this._uri.ToString());
				stringBuilder.Append("'");
			}
			else
			{
				stringBuilder.Append(" special='");
				stringBuilder.Append(this._type.ToString());
				stringBuilder.Append("'");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002243C File Offset: 0x0002143C
		private void UriInit(Uri uri, string rule, string semanticKey, string initParameters)
		{
			Helpers.ThrowIfNull(uri, "uri");
			if (string.IsNullOrEmpty(rule))
			{
				this._uri = uri;
			}
			else
			{
				this._uri = new Uri(uri.ToString() + "#" + rule, 0);
			}
			this._semanticKey = semanticKey;
			this._params = initParameters;
		}

		// Token: 0x04000585 RID: 1413
		public static readonly SrgsRuleRef Null = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Null);

		// Token: 0x04000586 RID: 1414
		public static readonly SrgsRuleRef Void = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Void);

		// Token: 0x04000587 RID: 1415
		public static readonly SrgsRuleRef Garbage = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Garbage);

		// Token: 0x04000588 RID: 1416
		public static readonly SrgsRuleRef Dictation = new SrgsRuleRef(new Uri("grammar:dictation"));

		// Token: 0x04000589 RID: 1417
		public static readonly SrgsRuleRef MnemonicSpelling = new SrgsRuleRef(new Uri("grammar:dictation#spelling"));

		// Token: 0x0400058A RID: 1418
		private Uri _uri;

		// Token: 0x0400058B RID: 1419
		private SrgsRuleRef.SpecialRuleRefType _type;

		// Token: 0x0400058C RID: 1420
		private string _semanticKey;

		// Token: 0x0400058D RID: 1421
		private string _params;

		// Token: 0x02000123 RID: 291
		private enum SpecialRuleRefType
		{
			// Token: 0x0400058F RID: 1423
			Null,
			// Token: 0x04000590 RID: 1424
			Void,
			// Token: 0x04000591 RID: 1425
			Garbage
		}
	}
}
