using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000080 RID: 128
	[ImmutableObject(true)]
	[DebuggerDisplay("{DebuggerDisplayString()}")]
	[Serializable]
	public class SrgsRuleRef : SrgsElement, IRuleRef, IElement
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x00011624 File Offset: 0x0000F824
		public SrgsRuleRef(Uri uri)
		{
			this.UriInit(uri, null, null, null);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00011636 File Offset: 0x0000F836
		public SrgsRuleRef(Uri uri, string rule)
		{
			Helpers.ThrowIfEmptyOrNull(rule, "rule");
			this.UriInit(uri, rule, null, null);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00011653 File Offset: 0x0000F853
		public SrgsRuleRef(Uri uri, string rule, string semanticKey)
		{
			Helpers.ThrowIfEmptyOrNull(semanticKey, "semanticKey");
			this.UriInit(uri, rule, semanticKey, null);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011670 File Offset: 0x0000F870
		public SrgsRuleRef(Uri uri, string rule, string semanticKey, string parameters)
		{
			Helpers.ThrowIfEmptyOrNull(parameters, "parameters");
			this.UriInit(uri, rule, semanticKey, parameters);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001168F File Offset: 0x0000F88F
		public SrgsRuleRef(SrgsRule rule)
		{
			Helpers.ThrowIfNull(rule, "rule");
			this._uri = new Uri("#" + rule.Id, UriKind.Relative);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000116BE File Offset: 0x0000F8BE
		public SrgsRuleRef(SrgsRule rule, string semanticKey)
			: this(rule)
		{
			Helpers.ThrowIfEmptyOrNull(semanticKey, "semanticKey");
			this._semanticKey = semanticKey;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000116D9 File Offset: 0x0000F8D9
		public SrgsRuleRef(SrgsRule rule, string semanticKey, string parameters)
			: this(rule)
		{
			Helpers.ThrowIfEmptyOrNull(parameters, "parameters");
			this._semanticKey = semanticKey;
			this._params = parameters;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000116FB File Offset: 0x0000F8FB
		private SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType type)
		{
			this._type = type;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001170A File Offset: 0x0000F90A
		internal SrgsRuleRef(string semanticKey, string parameters, Uri uri)
		{
			this._uri = uri;
			this._semanticKey = semanticKey;
			this._params = parameters;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00011727 File Offset: 0x0000F927
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001172F File Offset: 0x0000F92F
		public string SemanticKey
		{
			get
			{
				return this._semanticKey;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00011737 File Offset: 0x0000F937
		public string Params
		{
			get
			{
				return this._params;
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011740 File Offset: 0x0000F940
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

		// Token: 0x06000461 RID: 1121 RVA: 0x00011818 File Offset: 0x0000FA18
		internal override void Validate(SrgsGrammar grammar)
		{
			bool flag = this._params != null || this._semanticKey != null;
			grammar._fContainsCode = grammar._fContainsCode || flag;
			grammar.HasSapiExtension = grammar.HasSapiExtension || flag;
			if (this._uri != null)
			{
				string text = this._uri.ToString();
				if (text[0] == '#')
				{
					bool flag2 = false;
					if (text.IndexOf("#grammar:dictation", StringComparison.Ordinal) == 0 || text.IndexOf("#grammar:dictation#spelling", StringComparison.Ordinal) == 0)
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

		// Token: 0x06000462 RID: 1122 RVA: 0x00011910 File Offset: 0x0000FB10
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

		// Token: 0x06000463 RID: 1123 RVA: 0x00011998 File Offset: 0x0000FB98
		private void UriInit(Uri uri, string rule, string semanticKey, string initParameters)
		{
			Helpers.ThrowIfNull(uri, "uri");
			if (string.IsNullOrEmpty(rule))
			{
				this._uri = uri;
			}
			else
			{
				this._uri = new Uri(uri.ToString() + "#" + rule, UriKind.RelativeOrAbsolute);
			}
			this._semanticKey = semanticKey;
			this._params = initParameters;
		}

		// Token: 0x040003FA RID: 1018
		public static readonly SrgsRuleRef Null = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Null);

		// Token: 0x040003FB RID: 1019
		public static readonly SrgsRuleRef Void = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Void);

		// Token: 0x040003FC RID: 1020
		public static readonly SrgsRuleRef Garbage = new SrgsRuleRef(SrgsRuleRef.SpecialRuleRefType.Garbage);

		// Token: 0x040003FD RID: 1021
		public static readonly SrgsRuleRef Dictation = new SrgsRuleRef(new Uri("grammar:dictation"));

		// Token: 0x040003FE RID: 1022
		public static readonly SrgsRuleRef MnemonicSpelling = new SrgsRuleRef(new Uri("grammar:dictation#spelling"));

		// Token: 0x040003FF RID: 1023
		private Uri _uri;

		// Token: 0x04000400 RID: 1024
		private SrgsRuleRef.SpecialRuleRefType _type;

		// Token: 0x04000401 RID: 1025
		private string _semanticKey;

		// Token: 0x04000402 RID: 1026
		private string _params;

		// Token: 0x02000182 RID: 386
		private enum SpecialRuleRefType
		{
			// Token: 0x040008BA RID: 2234
			Null,
			// Token: 0x040008BB RID: 2235
			Void,
			// Token: 0x040008BC RID: 2236
			Garbage
		}
	}
}
