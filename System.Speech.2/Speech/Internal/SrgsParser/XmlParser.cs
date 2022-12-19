using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;
using System.Xml;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000E4 RID: 228
	internal class XmlParser : ISrgsParser
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
		internal XmlParser(XmlReader reader, Uri uri)
		{
			this._reader = reader;
			this._xmlTextReader = reader as XmlTextReader;
			if (uri == null && this._xmlTextReader != null && this._xmlTextReader.BaseURI.Length > 0)
			{
				try
				{
					uri = new Uri(this._xmlTextReader.BaseURI);
				}
				catch (UriFormatException)
				{
				}
			}
			if (uri != null)
			{
				this._filename = ((!uri.IsAbsoluteUri || !uri.IsFile) ? uri.OriginalString : uri.LocalPath);
				int num = this._filename.LastIndexOfAny(XmlParser._SlashBackSlash);
				this._shortFilename = ((num >= 0) ? this._filename.Substring(num + 1) : this._filename);
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001FE98 File Offset: 0x0001E098
		public void Parse()
		{
			try
			{
				bool flag = false;
				while (this._reader.Read())
				{
					if (this._reader.NodeType == XmlNodeType.Element && this._reader.LocalName == "grammar")
					{
						if (this._reader.NamespaceURI != "http://www.w3.org/2001/06/grammar")
						{
							XmlParser.ThrowSrgsException(SRID.InvalidSrgsNamespace, new object[0]);
						}
						if (flag)
						{
							XmlParser.ThrowSrgsException(SRID.GrammarDefTwice, new object[0]);
						}
						else
						{
							this.ParseGrammar(this._reader, this._parser.Grammar);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidSrgs, new object[0]);
				}
			}
			catch (XmlException ex)
			{
				this._parser.RemoveAllRules();
				XmlParser.ThrowSrgsExceptionWithPosition(this._filename, this._reader, SR.Get(SRID.InvalidXmlFormat, new object[0]), ex);
			}
			catch (FormatException ex2)
			{
				this._parser.RemoveAllRules();
				XmlParser.ThrowSrgsExceptionWithPosition(this._filename, this._reader, ex2.Message, ex2.InnerException);
			}
			catch
			{
				this._parser.RemoveAllRules();
				throw;
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001FFD4 File Offset: 0x0001E1D4
		internal static void ParseText(IElement parent, string sChars, string pronunciation, string display, float reqConfidence, CreateTokenCallback createTokens)
		{
			sChars = sChars.Trim(Helpers._achTrimChars);
			char[] array = sChars.ToCharArray();
			int length = sChars.Length;
			int num;
			for (int i = 0; i < array.Length; i = num + 1)
			{
				if (array[i] == ' ')
				{
					num = i;
				}
				else
				{
					if (array[i] == '"')
					{
						i = (num = i + 1);
						while (num < length && array[num] != '"')
						{
							num++;
						}
						if (num >= length || array[num] != '"')
						{
							XmlParser.ThrowSrgsException(SRID.InvalidQuotedString, new object[0]);
						}
						if (num + 1 != length && array[num + 1] != ' ')
						{
							XmlParser.ThrowSrgsException(SRID.InvalidQuotedString, new object[0]);
						}
					}
					else
					{
						num = i + 1;
						while (num < length && array[num] != ' ')
						{
							num++;
						}
					}
					string text = sChars.Substring(i, num - i);
					if (text.IndexOf('"') != -1)
					{
						XmlParser.ThrowSrgsException(SRID.InvalidTokenString, new object[0]);
					}
					if (createTokens != null)
					{
						createTokens(parent, text, pronunciation, display, reqConfidence);
					}
				}
			}
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001B6BB File Offset: 0x000198BB
		internal static void ThrowSrgsException(SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000200C4 File Offset: 0x0001E2C4
		internal static void ThrowSrgsExceptionWithPosition(string filename, XmlReader xmlReader, string sError, Exception innerException)
		{
			XmlTextReader xmlTextReader = xmlReader as XmlTextReader;
			if (xmlTextReader != null)
			{
				string text = SR.Get(SRID.Line, new object[0]);
				string text2 = SR.Get(SRID.Position, new object[0]);
				int lineNumber = xmlTextReader.LineNumber;
				int linePosition = xmlTextReader.LinePosition;
				if (filename == null)
				{
					sError += string.Format(CultureInfo.InvariantCulture, " [{0}={1}, {2}={3}]", new object[] { text, lineNumber, text2, linePosition });
				}
				else
				{
					sError = string.Format(CultureInfo.InvariantCulture, "{0}({1},{2}): error : {3}", new object[] { filename, lineNumber, linePosition, sError });
				}
			}
			throw new FormatException(sError, innerException);
		}

		// Token: 0x1700018F RID: 399
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00020185 File Offset: 0x0001E385
		public IElementFactory ElementFactory
		{
			set
			{
				this._parser = value;
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00020190 File Offset: 0x0001E390
		private void ParseGrammar(XmlReader reader, IGrammar grammar)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			GrammarType grammarType = GrammarType.VoiceGrammar;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string text5 = reader.NamespaceURI;
				if (text5 == null || text5.Length != 0)
				{
					if (!(text5 == "http://www.w3.org/XML/1998/namespace"))
					{
						if (text5 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							text5 = reader.LocalName;
							if (!(text5 == "alphabet"))
							{
								if (!(text5 == "language"))
								{
									if (!(text5 == "namespace"))
									{
										if (!(text5 == "codebehind"))
										{
											bool flag2;
											if (!(text5 == "debug"))
											{
												flag = true;
											}
											else if (bool.TryParse(reader.Value, out flag2))
											{
												grammar.Debug = flag2;
											}
										}
										else
										{
											if (reader.Value.Length == 0)
											{
												XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "codebehind" });
											}
											grammar.CodeBehind.Add(reader.Value);
										}
									}
									else
									{
										XmlParser.CheckForDuplicates(ref text3, reader);
										if (string.IsNullOrEmpty(text3))
										{
											XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "namespace" });
										}
										grammar.Namespace = text3;
									}
								}
								else
								{
									XmlParser.CheckForDuplicates(ref text2, reader);
									if (text2 == "C#" || text2 == "VB.Net")
									{
										grammar.Language = text2;
									}
									else
									{
										XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { reader.Value });
									}
								}
							}
							else
							{
								XmlParser.CheckForDuplicates(ref text, reader);
								uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
								if (num <= 898290601U)
								{
									if (num != 428147290U)
									{
										if (num != 599994509U)
										{
											if (num != 898290601U)
											{
												goto IL_3E9;
											}
											if (!(text == "ups"))
											{
												goto IL_3E9;
											}
											goto IL_3DD;
										}
										else if (!(text == "x-sapi"))
										{
											goto IL_3E9;
										}
									}
									else if (!(text == "x-microsoft-sapi"))
									{
										goto IL_3E9;
									}
								}
								else if (num <= 2767308742U)
								{
									if (num != 1781664288U)
									{
										if (num != 2767308742U)
										{
											goto IL_3E9;
										}
										if (!(text == "sapi"))
										{
											goto IL_3E9;
										}
									}
									else
									{
										if (!(text == "x-ups"))
										{
											goto IL_3E9;
										}
										goto IL_3DD;
									}
								}
								else if (num != 2801901063U)
								{
									if (num != 3044962637U)
									{
										goto IL_3E9;
									}
									if (!(text == "x-microsoft-ups"))
									{
										goto IL_3E9;
									}
									goto IL_3DD;
								}
								else
								{
									if (!(text == "ipa"))
									{
										goto IL_3E9;
									}
									grammar.PhoneticAlphabet = AlphabetType.Ipa;
									goto IL_4C9;
								}
								grammar.PhoneticAlphabet = AlphabetType.Sapi;
								goto IL_4C9;
								IL_3DD:
								grammar.PhoneticAlphabet = AlphabetType.Ups;
								goto IL_4C9;
								IL_3E9:
								XmlParser.ThrowSrgsException(SRID.UnsupportedPhoneticAlphabet, new object[] { reader.Value });
							}
						}
					}
					else
					{
						text5 = reader.LocalName;
						if (!(text5 == "lang"))
						{
							if (!(text5 == "base"))
							{
								goto IL_4C9;
							}
						}
						else
						{
							string value = reader.Value;
							try
							{
								grammar.Culture = (this._langId = new CultureInfo(value));
								goto IL_4C9;
							}
							catch (ArgumentException)
							{
								int num2 = reader.Value.IndexOf("-", StringComparison.Ordinal);
								if (num2 > 0)
								{
									grammar.Culture = (this._langId = new CultureInfo(reader.Value.Substring(0, num2)));
									goto IL_4C9;
								}
								throw;
							}
						}
						grammar.XmlBase = new Uri(reader.Value);
					}
				}
				else
				{
					text5 = reader.LocalName;
					if (!(text5 == "root"))
					{
						if (!(text5 == "version"))
						{
							if (!(text5 == "tag-format"))
							{
								if (!(text5 == "mode"))
								{
									flag = true;
								}
								else
								{
									text5 = reader.Value;
									if (!(text5 == "voice"))
									{
										if (!(text5 == "dtmf"))
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarMode, new object[0]);
										}
										else
										{
											grammarType = (grammar.Mode = GrammarType.DtmfGrammar);
										}
									}
									else
									{
										grammar.Mode = GrammarType.VoiceGrammar;
									}
								}
							}
							else
							{
								text5 = reader.Value;
								if (!(text5 == "semantics/1.0"))
								{
									if (!(text5 == "semantics-ms/1.0"))
									{
										if (!(text5 == "properties-ms/1.0"))
										{
											if (text5 != null)
											{
												if (text5.Length == 0)
												{
													goto IL_4C9;
												}
											}
											XmlParser.ThrowSrgsException(SRID.InvalidTagFormat, new object[0]);
										}
										else
										{
											grammar.TagFormat = SrgsTagFormat.KeyValuePairs;
											this._hasTagFormat = true;
										}
									}
									else
									{
										grammar.TagFormat = SrgsTagFormat.MssV1;
										this._hasTagFormat = true;
									}
								}
								else
								{
									grammar.TagFormat = SrgsTagFormat.W3cV1;
									this._hasTagFormat = true;
								}
							}
						}
						else
						{
							XmlParser.CheckForDuplicates(ref text4, reader);
							if (text4 != "1.0")
							{
								XmlParser.ThrowSrgsException(SRID.InvalidVersion, new object[0]);
							}
						}
					}
					else if (grammar.Root == null)
					{
						grammar.Root = reader.Value;
					}
					else
					{
						XmlParser.ThrowSrgsException(SRID.RootRuleAlreadyDefined, new object[0]);
					}
				}
				IL_4C9:
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidGrammarAttribute, new object[] { reader.Name });
				}
			}
			if (text4 == null)
			{
				XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "version", "grammar" });
			}
			if (this._langId == null)
			{
				if (grammarType == GrammarType.VoiceGrammar)
				{
					XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "xml:lang", "grammar" });
				}
				else
				{
					this._langId = CultureInfo.CurrentUICulture;
				}
			}
			this.ProcessRulesAndScriptsNodes(reader, grammar);
			this.ValidateScripts();
			foreach (XmlParser.ForwardReference forwardReference in this._scripts)
			{
				this._parser.AddScript(grammar, forwardReference._name, forwardReference._value);
			}
			grammar.PostParse(null);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00020760 File Offset: 0x0001E960
		private IRule ParseRule(IGrammar grammar, XmlReader reader)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			RulePublic rulePublic = RulePublic.NotSet;
			RuleDynamic ruleDynamic = RuleDynamic.NotSet;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			string text7 = null;
			string text8 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string text9 = reader.NamespaceURI;
				if (text9 == null || text9.Length != 0)
				{
					if (text9 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						text9 = reader.LocalName;
						if (!(text9 == "dynamic"))
						{
							if (!(text9 == "baseclass"))
							{
								if (!(text9 == "onInit"))
								{
									if (!(text9 == "onParse"))
									{
										if (!(text9 == "onError"))
										{
											if (!(text9 == "onRecognition"))
											{
												flag = true;
											}
											else
											{
												XmlParser.CheckForDuplicates(ref text8, reader);
											}
										}
										else
										{
											XmlParser.CheckForDuplicates(ref text7, reader);
											text7 = reader.Value;
										}
									}
									else
									{
										XmlParser.CheckForDuplicates(ref text6, reader);
										text6 = reader.Value;
									}
								}
								else
								{
									XmlParser.CheckForDuplicates(ref text5, reader);
									text5 = reader.Value;
								}
							}
							else
							{
								XmlParser.CheckForDuplicates(ref text4, reader);
								if (string.IsNullOrEmpty(text4))
								{
									XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "baseclass" });
								}
							}
						}
						else
						{
							XmlParser.CheckForDuplicates(ref text3, reader);
							if (!(text3 == "true"))
							{
								if (!(text3 == "false"))
								{
									XmlParser.ThrowSrgsException(SRID.InvalidDynamicSetting, new object[0]);
								}
								else
								{
									ruleDynamic = RuleDynamic.False;
								}
							}
							else
							{
								ruleDynamic = RuleDynamic.True;
							}
						}
					}
				}
				else
				{
					text9 = reader.LocalName;
					if (!(text9 == "id"))
					{
						if (!(text9 == "scope"))
						{
							flag = true;
						}
						else
						{
							XmlParser.CheckForDuplicates(ref text2, reader);
							if (!(text2 == "private"))
							{
								if (!(text2 == "public"))
								{
									XmlParser.ThrowSrgsException(SRID.InvalidRuleScope, new object[0]);
								}
								else
								{
									rulePublic = RulePublic.True;
								}
							}
							else
							{
								rulePublic = RulePublic.False;
							}
						}
					}
					else
					{
						XmlParser.CheckForDuplicates(ref text, reader);
					}
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidRuleAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				XmlParser.ThrowSrgsException(SRID.NoRuleId, new object[0]);
			}
			if (text5 != null && rulePublic != RulePublic.True)
			{
				XmlParser.ThrowSrgsException(SRID.OnInitOnPublicRule, new object[] { "OnInit", text });
			}
			if (text8 != null && rulePublic != RulePublic.True)
			{
				XmlParser.ThrowSrgsException(SRID.OnInitOnPublicRule, new object[] { "OnRecognition", text });
			}
			XmlParser.ValidateRuleId(text);
			bool flag2 = text5 != null || text6 != null || text7 != null || text8 != null;
			IRule rule = grammar.CreateRule(text, rulePublic, ruleDynamic, flag2);
			if (!string.IsNullOrEmpty(text5))
			{
				rule.CreateScript(grammar, text, text5, RuleMethodScript.onInit);
			}
			if (!string.IsNullOrEmpty(text6))
			{
				rule.CreateScript(grammar, text, text6, RuleMethodScript.onParse);
			}
			if (!string.IsNullOrEmpty(text7))
			{
				rule.CreateScript(grammar, text, text7, RuleMethodScript.onError);
			}
			if (!string.IsNullOrEmpty(text8))
			{
				rule.CreateScript(grammar, text, text8, RuleMethodScript.onRecognition);
			}
			rule.BaseClass = text4;
			this._rules.Add(text);
			if (!this.ProcessChildNodes(reader, rule, rule, "rule") && ruleDynamic != RuleDynamic.True)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyRule, new object[] { "rule", text });
			}
			return rule;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00020AA0 File Offset: 0x0001ECA0
		private IRuleRef ParseRuleRef(IElement parent, XmlReader reader)
		{
			IRuleRef ruleRef = null;
			string text = null;
			string text2 = null;
			string text3 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string text4 = reader.NamespaceURI;
				if (text4 == null || text4.Length != 0)
				{
					if (!(text4 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
					{
						flag = true;
					}
					else
					{
						text4 = reader.LocalName;
						if (!(text4 == "semantic-key"))
						{
							if (!(text4 == "params"))
							{
								flag = true;
							}
							else
							{
								XmlParser.CheckForDuplicates(ref text2, reader);
							}
						}
						else
						{
							XmlParser.CheckForDuplicates(ref text, reader);
						}
					}
				}
				else
				{
					text4 = reader.LocalName;
					if (!(text4 == "uri"))
					{
						if (!(text4 == "special"))
						{
							if (!(text4 == "type"))
							{
								flag = true;
							}
						}
						else
						{
							if (ruleRef != null)
							{
								XmlParser.ThrowSrgsException(SRID.InvalidAttributeDefinedTwice, new object[] { reader.Value, "special" });
							}
							text4 = reader.Value;
							if (!(text4 == "NULL"))
							{
								if (!(text4 == "VOID"))
								{
									if (!(text4 == "GARBAGE"))
									{
										XmlParser.ThrowSrgsException(SRID.InvalidSpecialRuleRef, new object[0]);
									}
									else
									{
										ruleRef = this._parser.Garbage;
									}
								}
								else
								{
									ruleRef = this._parser.Void;
								}
							}
							else
							{
								ruleRef = this._parser.Null;
							}
							this._parser.InitSpecialRuleRef(parent, ruleRef);
						}
					}
					else
					{
						XmlParser.CheckForDuplicates(ref text3, reader);
						this.ValidateRulerefNotPointingToSelf(text3);
					}
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidRulerefAttribute, new object[] { reader.Name });
				}
			}
			this.ProcessChildNodes(reader, null, null, "ruleref");
			if (ruleRef == null)
			{
				if (text3 == null)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidRuleRef, new object[] { "uri" });
				}
				ruleRef = this._parser.CreateRuleRef(parent, new Uri(text3, UriKind.RelativeOrAbsolute), text, text2);
			}
			else
			{
				if (text3 != null)
				{
					XmlParser.ThrowSrgsException(SRID.NoUriForSpecialRuleRef, new object[0]);
				}
				if (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(text2))
				{
					XmlParser.ThrowSrgsException(SRID.NoAliasForSpecialRuleRef, new object[0]);
				}
			}
			ruleRef.PostParse(parent);
			return ruleRef;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00020CC0 File Offset: 0x0001EEC0
		private IOneOf ParseOneOf(IElement parent, IRule rule, XmlReader reader)
		{
			IOneOf oneOf = this._parser.CreateOneOf(parent, rule);
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI = reader.NamespaceURI;
				if ((namespaceURI != null && namespaceURI.Length == 0) || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
				{
					flag = true;
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidOneOfAttribute, new object[] { reader.Name });
				}
			}
			this.ProcessChildNodes(reader, oneOf, rule, "one-of");
			oneOf.PostParse(parent);
			return oneOf;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00020D3C File Offset: 0x0001EF3C
		private IItem ParseItem(IElement parent, IRule rule, XmlReader reader)
		{
			float num = 0.5f;
			int num2 = 1;
			int num3 = 1;
			float num4 = 1f;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string text = reader.NamespaceURI;
				if (text == null || text.Length != 0)
				{
					if (text == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						flag = true;
					}
				}
				else
				{
					text = reader.LocalName;
					if (!(text == "repeat"))
					{
						if (!(text == "repeat-prob"))
						{
							if (!(text == "weight"))
							{
								flag = true;
							}
							else
							{
								num4 = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
							}
						}
						else
						{
							num = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
						}
					}
					else
					{
						XmlParser.SetRepeatValues(reader.Value, out num2, out num3);
					}
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			IItem item = this._parser.CreateItem(parent, rule, num2, num3, num, num4);
			this.ProcessChildNodes(reader, item, rule, "item");
			item.PostParse(parent);
			return item;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00020E50 File Offset: 0x0001F050
		private ISubset ParseSubset(IElement parent, XmlReader reader)
		{
			string text = null;
			MatchMode matchMode = MatchMode.Subsequence;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length == 0;
				string text2 = reader.NamespaceURI;
				if (text2 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
				{
					text2 = reader.LocalName;
					if (text2 == "match")
					{
						XmlParser.CheckForDuplicates(ref text, reader);
						text2 = reader.Value;
						if (!(text2 == "subsequence"))
						{
							if (!(text2 == "ordered-subset"))
							{
								if (!(text2 == "subsequence-content-required"))
								{
									if (!(text2 == "ordered-subset-content-required"))
									{
										flag = true;
									}
									else
									{
										matchMode = MatchMode.OrderedSubsetContentRequired;
									}
								}
								else
								{
									matchMode = MatchMode.SubsequenceContentRequired;
								}
							}
							else
							{
								matchMode = MatchMode.OrderedSubset;
							}
						}
						else
						{
							matchMode = MatchMode.Subsequence;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidSubsetAttribute, new object[] { reader.Name });
				}
			}
			string text3 = XmlParser.GetStringContent(reader).Trim();
			if (text3.Length == 0)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyElement, new object[] { "subset" });
			}
			return this._parser.CreateSubset(parent, text3, matchMode);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00020F60 File Offset: 0x0001F160
		private IToken ParseToken(IElement parent, XmlReader reader)
		{
			string text = null;
			string text2 = null;
			float num = -1f;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string text3 = reader.NamespaceURI;
				if (text3 == null || text3.Length != 0)
				{
					if (text3 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						text3 = reader.LocalName;
						if (!(text3 == "pron"))
						{
							if (!(text3 == "display"))
							{
								if (!(text3 == "reqconf"))
								{
									flag = true;
								}
								else
								{
									text3 = reader.Value;
									if (!(text3 == "high"))
									{
										if (!(text3 == "normal"))
										{
											if (!(text3 == "low"))
											{
												XmlParser.ThrowSrgsException(SRID.InvalidReqConfAttribute, new object[] { reader.Name });
											}
											else
											{
												num = 0.2f;
											}
										}
										else
										{
											num = 0.5f;
										}
									}
									else
									{
										num = 0.8f;
									}
								}
							}
							else if (string.IsNullOrEmpty(text2))
							{
								text2 = reader.Value.Trim(Helpers._achTrimChars);
								if (string.IsNullOrEmpty(text2))
								{
									XmlParser.ThrowSrgsException(SRID.EmptyDisplayString, new object[0]);
								}
							}
							else
							{
								XmlParser.ThrowSrgsException(SRID.MultipleDisplayString, new object[0]);
							}
						}
						else if (string.IsNullOrEmpty(text))
						{
							text = reader.Value.Trim(Helpers._achTrimChars);
							if (string.IsNullOrEmpty(text))
							{
								XmlParser.ThrowSrgsException(SRID.EmptyPronunciationString, new object[0]);
							}
						}
						else
						{
							XmlParser.ThrowSrgsException(SRID.MuliplePronunciationString, new object[0]);
						}
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidTokenAttribute, new object[] { reader.Name });
				}
			}
			string text4 = XmlParser.GetStringContent(reader).Trim(Helpers._achTrimChars);
			if (string.IsNullOrEmpty(text4))
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyElement, new object[] { "token" });
			}
			if (text4.IndexOf('"') >= 0)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidTokenString, new object[0]);
			}
			return this._parser.CreateToken(parent, text4, text, text2, num);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002115B File Offset: 0x0001F35B
		private void ParseText(IElement parent, string sChars, string pronunciation, string display, float reqConfidence)
		{
			XmlParser.ParseText(parent, sChars, pronunciation, display, reqConfidence, new CreateTokenCallback(this._parser.CreateToken));
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002117C File Offset: 0x0001F37C
		private IElement ParseTag(IElement parent, XmlReader reader)
		{
			string tagContent = this.GetTagContent(parent, reader);
			if (string.IsNullOrEmpty(tagContent))
			{
				return this._parser.CreateSemanticTag(parent);
			}
			if (this._parser.Grammar.TagFormat != SrgsTagFormat.KeyValuePairs)
			{
				ISemanticTag semanticTag = this._parser.CreateSemanticTag(parent);
				semanticTag.Content(parent, tagContent, 0);
				return semanticTag;
			}
			IPropertyTag propertyTag = this._parser.CreatePropertyTag(parent);
			string text;
			object obj;
			XmlParser.ParsePropertyTag(tagContent, out text, out obj);
			propertyTag.NameValue(parent, text, obj);
			return propertyTag;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000211F8 File Offset: 0x0001F3F8
		private string GetTagContent(IElement parent, XmlReader reader)
		{
			if (!this._hasTagFormat)
			{
				XmlParser.ThrowSrgsException(SRID.MissingTagFormat, new object[0]);
			}
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI = reader.NamespaceURI;
				if ((namespaceURI != null && namespaceURI.Length == 0) || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
				{
					flag = true;
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidTagAttribute, new object[] { reader.Name });
				}
			}
			return XmlParser.GetStringContent(reader).Trim(Helpers._achTrimChars);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00021278 File Offset: 0x0001F478
		private static void ParseLexicon(XmlReader reader)
		{
			bool flag = false;
			bool flag2 = false;
			while (reader.MoveToNextAttribute())
			{
				string localName = reader.LocalName;
				if (!(localName == "uri"))
				{
					if (!(localName == "type"))
					{
						flag = true;
					}
				}
				else
				{
					flag2 = true;
				}
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidLexiconAttribute, new object[] { reader.Name });
				}
			}
			if (!flag2)
			{
				XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "uri", "lexicon" });
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000212F8 File Offset: 0x0001F4F8
		private static void ParseMeta(XmlReader reader)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			while (reader.MoveToNextAttribute())
			{
				string localName = reader.LocalName;
				if (!(localName == "name") && !(localName == "http-equiv"))
				{
					if (!(localName == "content"))
					{
						flag3 = true;
					}
					else
					{
						flag3 = flag;
						flag = true;
					}
				}
				else
				{
					if (flag2)
					{
						XmlParser.ThrowSrgsException(SRID.MetaNameHTTPEquiv, new object[0]);
					}
					flag2 = true;
				}
				if (flag3)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidMetaAttribute, new object[] { reader.Name });
				}
			}
			if (!flag)
			{
				XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "content", "meta" });
			}
			if (!flag2)
			{
				XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "name or http-equiv", "meta" });
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000213C0 File Offset: 0x0001F5C0
		private void ParseScript(XmlReader reader, IGrammar grammar)
		{
			int num = ((this._filename != null) ? this._xmlTextReader.LineNumber : (-1));
			string text = null;
			while (reader.MoveToNextAttribute())
			{
				string text2 = reader.NamespaceURI;
				if (text2 == null || text2.Length != 0)
				{
					if (text2 == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						text2 = reader.LocalName;
						if (text2 == "rule")
						{
							if (string.IsNullOrEmpty(text))
							{
								text = reader.Value;
							}
							else
							{
								XmlParser.ThrowSrgsException(SRID.RuleAttributeDefinedMultipeTimes, new object[0]);
							}
						}
						else
						{
							XmlParser.ThrowSrgsException(SRID.InvalidScriptAttribute, new object[0]);
						}
					}
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.InvalidScriptAttribute, new object[0]);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				this._parser.AddScript(grammar, XmlParser.GetStringContent(reader), this._filename, num);
				return;
			}
			this._scripts.Add(new XmlParser.ForwardReference(text, this._parser.AddScript(grammar, text, XmlParser.GetStringContent(reader), this._filename, num)));
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000214B4 File Offset: 0x0001F6B4
		private static void ParseAssemblyReference(XmlReader reader, IGrammar grammar)
		{
			while (reader.MoveToNextAttribute())
			{
				string text = reader.NamespaceURI;
				if (text == null || text.Length != 0)
				{
					if (text == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						text = reader.LocalName;
						if (text == "assembly")
						{
							grammar.AssemblyReferences.Add(reader.Value);
						}
						else
						{
							XmlParser.ThrowSrgsException(SRID.InvalidAssemblyReferenceAttribute, new object[0]);
						}
					}
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.InvalidScriptAttribute, new object[0]);
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00021534 File Offset: 0x0001F734
		private static void ParseImportNamespace(XmlReader reader, IGrammar grammar)
		{
			while (reader.MoveToNextAttribute())
			{
				string text = reader.NamespaceURI;
				if (text == null || text.Length != 0)
				{
					if (text == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
					{
						text = reader.LocalName;
						if (text == "namespace")
						{
							grammar.ImportNamespaces.Add(reader.Value);
						}
						else
						{
							XmlParser.ThrowSrgsException(SRID.InvalidImportNamespaceAttribute, new object[0]);
						}
					}
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.InvalidScriptAttribute, new object[0]);
				}
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000215B4 File Offset: 0x0001F7B4
		private bool ProcessChildNodes(XmlReader reader, IElement parent, IRule rule, string parentName)
		{
			bool flag = true;
			List<IPropertyTag> list = null;
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					bool flag2 = false;
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (parent == null)
						{
							XmlParser.ThrowSrgsException(SRID.InvalidNotEmptyElement, new object[] { parentName });
						}
						IElement element = null;
						string text = reader.NamespaceURI;
						if (!(text == "http://www.w3.org/2001/06/grammar"))
						{
							if (!(text == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
							{
								reader.Skip();
							}
							else
							{
								text = reader.LocalName;
								if (text == "subset")
								{
									if (parent is IRule || parent is IItem)
									{
										element = this.ParseSubset(parent, reader);
									}
									else
									{
										flag2 = true;
									}
								}
								else
								{
									flag2 = true;
								}
							}
						}
						else
						{
							text = reader.LocalName;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
							if (num <= 2491017778U)
							{
								if (num != 1406988087U)
								{
									if (num != 2347908769U)
									{
										if (num == 2491017778U)
										{
											if (text == "token")
											{
												element = this.ParseToken(parent, reader);
												goto IL_24B;
											}
										}
									}
									else if (text == "example")
									{
										if (!(parent is IRule) || !flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidExampleOrdering, new object[0]);
											goto IL_24B;
										}
										reader.Skip();
										continue;
									}
								}
								else if (text == "one-of")
								{
									element = this.ParseOneOf(parent, rule, reader);
									goto IL_24B;
								}
							}
							else if (num <= 2591899532U)
							{
								if (num != 2516003219U)
								{
									if (num == 2591899532U)
									{
										if (text == "ruleref")
										{
											element = this.ParseRuleRef(parent, reader);
											goto IL_24B;
										}
									}
								}
								else if (text == "tag")
								{
									element = this.ParseTag(parent, reader);
									IPropertyTag propertyTag = element as IPropertyTag;
									if (propertyTag != null)
									{
										if (list == null)
										{
											list = new List<IPropertyTag>();
										}
										list.Add(propertyTag);
										goto IL_24B;
									}
									goto IL_24B;
								}
							}
							else if (num != 2671260646U)
							{
								if (num == 4230889683U)
								{
									if (!(text == "rule"))
									{
									}
								}
							}
							else if (text == "item")
							{
								element = this.ParseItem(parent, rule, reader);
								goto IL_24B;
							}
							flag2 = true;
						}
						IL_24B:
						flag2 = this.ParseChildNodeElement(parent, flag2, element);
						flag = false;
					}
					else if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA)
					{
						if (parent == null)
						{
							XmlParser.ThrowSrgsException(SRID.InvalidNotEmptyElement, new object[] { parentName });
						}
						flag2 = this.ParseChildNodeText(reader, parent);
						flag = false;
					}
					else
					{
						reader.Skip();
					}
					if (flag2)
					{
						XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { reader.Name });
					}
				}
			}
			reader.Read();
			if (list != null)
			{
				foreach (IPropertyTag propertyTag2 in list)
				{
					propertyTag2.PostParse(parent);
				}
			}
			return !flag;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000218D0 File Offset: 0x0001FAD0
		private bool ParseChildNodeText(XmlReader reader, IElement parent)
		{
			bool flag = false;
			string value = reader.Value;
			IElementText elementText = this._parser.CreateText(parent, value);
			this.ParseText(parent, value, null, null, -1f);
			if (parent is IOneOf)
			{
				flag = true;
			}
			else
			{
				IRule rule = parent as IRule;
				if (rule != null)
				{
					this._parser.AddElement(rule, elementText);
				}
				else
				{
					IItem item = parent as IItem;
					if (item != null)
					{
						this._parser.AddElement(item, elementText);
					}
					else
					{
						flag = true;
					}
				}
			}
			reader.Read();
			return flag;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00021950 File Offset: 0x0001FB50
		private bool ParseChildNodeElement(IElement parent, bool isInvalidNode, IElement child)
		{
			if (child != null)
			{
				IOneOf oneOf = parent as IOneOf;
				if (oneOf != null)
				{
					IItem item = child as IItem;
					if (item != null)
					{
						this._parser.AddItem(oneOf, item);
					}
					else
					{
						isInvalidNode = true;
					}
				}
				else
				{
					IRule rule = parent as IRule;
					if (rule != null)
					{
						this._parser.AddElement(rule, child);
					}
					else
					{
						IItem item2 = parent as IItem;
						if (item2 != null)
						{
							this._parser.AddElement(item2, child);
						}
						else
						{
							isInvalidNode = true;
						}
					}
				}
			}
			return isInvalidNode;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000219C0 File Offset: 0x0001FBC0
		private void ProcessRulesAndScriptsNodes(XmlReader reader, IGrammar grammar)
		{
			bool flag = false;
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					bool flag2 = false;
					if (reader.NodeType == XmlNodeType.Element)
					{
						string text = reader.NamespaceURI;
						if (!(text == "http://www.w3.org/2001/06/grammar"))
						{
							if (!(text == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
							{
								reader.Skip();
							}
							else
							{
								text = reader.LocalName;
								if (!(text == "script"))
								{
									if (!(text == "assemblyReference"))
									{
										if (!(text == "importNamespace"))
										{
											flag2 = true;
										}
										else
										{
											XmlParser.ParseImportNamespace(reader, grammar);
											flag = true;
										}
									}
									else
									{
										XmlParser.ParseAssemblyReference(reader, grammar);
										flag = true;
									}
								}
								else
								{
									this.ParseScript(reader, grammar);
									flag = true;
								}
							}
						}
						else
						{
							text = reader.LocalName;
							if (!(text == "lexicon"))
							{
								if (!(text == "meta"))
								{
									if (!(text == "metadata"))
									{
										if (!(text == "rule"))
										{
											if (!(text == "tag"))
											{
												flag2 = true;
											}
											else
											{
												if (flag || (this._hasTagFormat && grammar.TagFormat != SrgsTagFormat.W3cV1))
												{
													XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
												}
												grammar.GlobalTags.Add(this.GetTagContent(grammar, reader));
											}
										}
										else
										{
											IRule rule = this.ParseRule(grammar, reader);
											rule.PostParse(grammar);
											flag = true;
										}
									}
									else
									{
										if (flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
										}
										reader.Skip();
									}
								}
								else
								{
									if (flag)
									{
										XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
									}
									XmlParser.ParseMeta(reader);
								}
							}
							else
							{
								if (flag)
								{
									XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
								}
								XmlParser.ParseLexicon(reader);
							}
						}
					}
					else
					{
						if (reader.NodeType == XmlNodeType.Text)
						{
							XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { "text" });
						}
						reader.Skip();
					}
					if (flag2)
					{
						XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { reader.Name });
					}
				}
			}
			reader.Read();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00021BD0 File Offset: 0x0001FDD0
		private static string GetStringContent(XmlReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					stringBuilder.Append(reader.ReadString());
					bool flag = false;
					if (reader.NodeType == XmlNodeType.Element)
					{
						string namespaceURI = reader.NamespaceURI;
						if (namespaceURI == "http://www.w3.org/2001/06/grammar" || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							flag = true;
						}
						else
						{
							reader.Skip();
						}
					}
					else if (reader.NodeType != XmlNodeType.EndElement)
					{
						reader.Skip();
					}
					if (flag)
					{
						XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { reader.Name });
					}
				}
			}
			reader.Read();
			return stringBuilder.ToString();
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00021C88 File Offset: 0x0001FE88
		private static void ParsePropertyTag(string sTag, out string name, out object value)
		{
			name = null;
			value = string.Empty;
			int num = sTag.IndexOf('=');
			if (num >= 0)
			{
				name = sTag.Substring(0, num).Trim(Helpers._achTrimChars);
				num++;
			}
			else
			{
				num = 0;
			}
			int length = sTag.Length;
			if (num < length)
			{
				if (sTag[num] == '"')
				{
					num++;
					int num2 = sTag.IndexOf('"', num + 1);
					if (num2 + 1 != length)
					{
						XmlParser.ThrowSrgsException(SRID.IncorrectAttributeValue, new object[]
						{
							name,
							sTag.Substring(num)
						});
					}
					value = sTag.Substring(num, num2 - num);
					return;
				}
				string text = sTag.Substring(num);
				int num3;
				if (int.TryParse(text, out num3))
				{
					value = num3;
					return;
				}
				double num4;
				if (double.TryParse(text, out num4))
				{
					value = num4;
					return;
				}
				bool flag;
				if (bool.TryParse(text, out flag))
				{
					value = flag;
					return;
				}
				XmlParser.ThrowSrgsException(SRID.InvalidNameValueProperty, new object[] { name, text });
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00021D80 File Offset: 0x0001FF80
		private static void SetRepeatValues(string repeat, out int minRepeat, out int maxRepeat)
		{
			minRepeat = (maxRepeat = 1);
			if (!string.IsNullOrEmpty(repeat))
			{
				int num = repeat.IndexOf("-", StringComparison.Ordinal);
				if (num < 0)
				{
					int num2 = Convert.ToInt32(repeat, CultureInfo.InvariantCulture);
					if (num2 < 0 || num2 > 255)
					{
						XmlParser.ThrowSrgsException(SRID.MinMaxOutOfRange, new object[] { num2, num2 });
					}
					minRepeat = (maxRepeat = num2);
					return;
				}
				if (0 >= num)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidItemRepeatAttribute, new object[] { repeat });
					return;
				}
				minRepeat = Convert.ToInt32(repeat.Substring(0, num), CultureInfo.InvariantCulture);
				if (num < repeat.Length - 1)
				{
					maxRepeat = Convert.ToInt32(repeat.Substring(num + 1), CultureInfo.InvariantCulture);
				}
				else
				{
					maxRepeat = int.MaxValue;
				}
				if (minRepeat < 0 || minRepeat > 255 || (maxRepeat != 2147483647 && (maxRepeat < 0 || maxRepeat > 255)))
				{
					XmlParser.ThrowSrgsException(SRID.MinMaxOutOfRange, new object[] { minRepeat, maxRepeat });
				}
				if (minRepeat > maxRepeat)
				{
					throw new ArgumentException(SR.Get(SRID.MinGreaterThanMax, new object[0]));
				}
			}
			else
			{
				XmlParser.ThrowSrgsException(SRID.InvalidItemAttribute2, new object[0]);
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00021EBC File Offset: 0x000200BC
		private static void CheckForDuplicates(ref string dest, XmlReader reader)
		{
			if (!string.IsNullOrEmpty(dest))
			{
				StringBuilder stringBuilder = new StringBuilder(reader.LocalName);
				if (reader.NamespaceURI.Length > 0)
				{
					stringBuilder.Append(reader.NamespaceURI);
					stringBuilder.Append(":");
				}
				XmlParser.ThrowSrgsException(SRID.InvalidAttributeDefinedTwice, new object[] { reader.Value, stringBuilder });
			}
			dest = reader.Value;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00021F28 File Offset: 0x00020128
		internal static void ValidateRuleId(string id)
		{
			Helpers.ThrowIfEmptyOrNull(id, "id");
			if (!XmlReader.IsName(id) || id == "NULL" || id == "VOID" || id == "GARBAGE" || id.IndexOfAny(XmlParser._invalidRuleIdChars) != -1)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidRuleId, new object[] { id });
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00021F90 File Offset: 0x00020190
		private void ValidateRulerefNotPointingToSelf(string uri)
		{
			if (this._filename != null && uri.IndexOf(this._shortFilename, StringComparison.Ordinal) == 0 && ((uri.Length > this._shortFilename.Length && uri[this._shortFilename.Length] == '#') || uri.Length == this._shortFilename.Length))
			{
				XmlParser.ThrowSrgsException(SRID.InvalidRuleRefSelf, new object[0]);
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00021FFC File Offset: 0x000201FC
		private void ValidateScripts()
		{
			foreach (XmlParser.ForwardReference forwardReference in this._scripts)
			{
				if (!this._rules.Contains(forwardReference._name))
				{
					XmlParser.ThrowSrgsException(SRID.InvalidScriptDefinition, new object[] { forwardReference._name });
				}
			}
			List<string> list = new List<string>();
			foreach (string text in this._rules)
			{
				if (list.Contains(text))
				{
					XmlParser.ThrowSrgsException(SRID.RuleAttributeDefinedMultipeTimes, new object[] { text });
				}
				list.Add(text);
			}
		}

		// Token: 0x040005A1 RID: 1441
		internal const string emptyNamespace = "";

		// Token: 0x040005A2 RID: 1442
		internal const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x040005A3 RID: 1443
		internal const string srgsNamespace = "http://www.w3.org/2001/06/grammar";

		// Token: 0x040005A4 RID: 1444
		internal const string sapiNamespace = "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions";

		// Token: 0x040005A5 RID: 1445
		private IElementFactory _parser;

		// Token: 0x040005A6 RID: 1446
		private XmlReader _reader;

		// Token: 0x040005A7 RID: 1447
		private XmlTextReader _xmlTextReader;

		// Token: 0x040005A8 RID: 1448
		private string _filename;

		// Token: 0x040005A9 RID: 1449
		private string _shortFilename;

		// Token: 0x040005AA RID: 1450
		private CultureInfo _langId;

		// Token: 0x040005AB RID: 1451
		private bool _hasTagFormat;

		// Token: 0x040005AC RID: 1452
		private List<string> _rules = new List<string>();

		// Token: 0x040005AD RID: 1453
		private List<XmlParser.ForwardReference> _scripts = new List<XmlParser.ForwardReference>();

		// Token: 0x040005AE RID: 1454
		private static readonly char[] _invalidRuleIdChars = new char[] { '.', ':', '-', '#' };

		// Token: 0x040005AF RID: 1455
		private static readonly char[] _SlashBackSlash = new char[] { '\\', '/' };

		// Token: 0x020001A1 RID: 417
		[Serializable]
		internal class ForwardReference
		{
			// Token: 0x06000BAA RID: 2986 RVA: 0x0002DE3E File Offset: 0x0002C03E
			internal ForwardReference(string name, string value)
			{
				this._name = name;
				this._value = value;
			}

			// Token: 0x0400095E RID: 2398
			internal string _name;

			// Token: 0x0400095F RID: 2399
			internal string _value;
		}
	}
}
