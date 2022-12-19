using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;
using System.Xml;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000CC RID: 204
	internal class XmlParser : ISrgsParser
	{
		// Token: 0x0600046D RID: 1133 RVA: 0x000116A8 File Offset: 0x000106A8
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

		// Token: 0x0600046E RID: 1134 RVA: 0x00011790 File Offset: 0x00010790
		public void Parse()
		{
			try
			{
				bool flag = false;
				while (this._reader.Read())
				{
					if (this._reader.NodeType == 1 && this._reader.LocalName == "grammar")
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

		// Token: 0x0600046F RID: 1135 RVA: 0x000118CC File Offset: 0x000108CC
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

		// Token: 0x06000470 RID: 1136 RVA: 0x000119B9 File Offset: 0x000109B9
		internal static void ThrowSrgsException(SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args));
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000119C8 File Offset: 0x000109C8
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

		// Token: 0x170000A6 RID: 166
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00011A99 File Offset: 0x00010A99
		public IElementFactory ElementFactory
		{
			set
			{
				this._parser = value;
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00011AA4 File Offset: 0x00010AA4
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
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						string localName2;
						if (!(namespaceURI == "http://www.w3.org/XML/1998/namespace"))
						{
							if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
							{
								string localName;
								if ((localName = reader.LocalName) != null)
								{
									if (localName == "alphabet")
									{
										XmlParser.CheckForDuplicates(ref text, reader);
										string text5;
										if ((text5 = text) != null)
										{
											if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000467-1 == null)
											{
												Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
												dictionary.Add("ipa", 0);
												dictionary.Add("sapi", 1);
												dictionary.Add("x-sapi", 2);
												dictionary.Add("x-microsoft-sapi", 3);
												dictionary.Add("ups", 4);
												dictionary.Add("x-ups", 5);
												dictionary.Add("x-microsoft-ups", 6);
												<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000467-1 = dictionary;
											}
											int num;
											if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000467-1.TryGetValue(text5, ref num))
											{
												switch (num)
												{
												case 0:
													grammar.PhoneticAlphabet = AlphabetType.Ipa;
													goto IL_523;
												case 1:
												case 2:
												case 3:
													grammar.PhoneticAlphabet = AlphabetType.Sapi;
													goto IL_523;
												case 4:
												case 5:
												case 6:
													grammar.PhoneticAlphabet = AlphabetType.Ups;
													goto IL_523;
												}
											}
										}
										XmlParser.ThrowSrgsException(SRID.UnsupportedPhoneticAlphabet, new object[] { reader.Value });
										goto IL_523;
									}
									if (!(localName == "language"))
									{
										if (localName == "namespace")
										{
											XmlParser.CheckForDuplicates(ref text3, reader);
											if (string.IsNullOrEmpty(text3))
											{
												XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "namespace" });
											}
											grammar.Namespace = text3;
											goto IL_523;
										}
										if (localName == "codebehind")
										{
											if (reader.Value.Length == 0)
											{
												XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "codebehind" });
											}
											grammar.CodeBehind.Add(reader.Value);
											goto IL_523;
										}
										if (localName == "debug")
										{
											bool flag2;
											if (bool.TryParse(reader.Value, ref flag2))
											{
												grammar.Debug = flag2;
												goto IL_523;
											}
											goto IL_523;
										}
									}
									else
									{
										XmlParser.CheckForDuplicates(ref text2, reader);
										if (text2 == "C#" || text2 == "VB.Net")
										{
											grammar.Language = text2;
											goto IL_523;
										}
										XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { reader.Value });
										goto IL_523;
									}
								}
								flag = true;
							}
						}
						else if ((localName2 = reader.LocalName) != null)
						{
							if (!(localName2 == "lang"))
							{
								if (!(localName2 == "base"))
								{
									goto IL_523;
								}
							}
							else
							{
								string value = reader.Value;
								try
								{
									grammar.Culture = (this._langId = new CultureInfo(value));
									goto IL_523;
								}
								catch (ArgumentException)
								{
									if (string.Compare("es-us", value, 5) == 0)
									{
										Helpers.CombineCulture("es-ES", "en-US", new CultureInfo("es"), 21514);
										grammar.Culture = (this._langId = new CultureInfo(value));
									}
									else
									{
										int num2 = reader.Value.IndexOf("-", 4);
										if (num2 <= 0)
										{
											throw;
										}
										grammar.Culture = (this._langId = new CultureInfo(reader.Value.Substring(0, num2)));
									}
									goto IL_523;
								}
							}
							grammar.XmlBase = new Uri(reader.Value);
						}
					}
					else
					{
						string localName3;
						if ((localName3 = reader.LocalName) != null)
						{
							if (!(localName3 == "root"))
							{
								if (!(localName3 == "version"))
								{
									if (localName3 == "tag-format")
									{
										string value2;
										if ((value2 = reader.Value) != null)
										{
											if (value2 == "semantics/1.0")
											{
												grammar.TagFormat = SrgsTagFormat.W3cV1;
												this._hasTagFormat = true;
												goto IL_523;
											}
											if (value2 == "semantics-ms/1.0")
											{
												grammar.TagFormat = SrgsTagFormat.MssV1;
												this._hasTagFormat = true;
												goto IL_523;
											}
											if (value2 == "properties-ms/1.0")
											{
												grammar.TagFormat = SrgsTagFormat.KeyValuePairs;
												this._hasTagFormat = true;
												goto IL_523;
											}
											if (value2 == "")
											{
												goto IL_523;
											}
										}
										XmlParser.ThrowSrgsException(SRID.InvalidTagFormat, new object[0]);
										goto IL_523;
									}
									if (localName3 == "mode")
									{
										string value3;
										if ((value3 = reader.Value) != null)
										{
											if (value3 == "voice")
											{
												grammar.Mode = GrammarType.VoiceGrammar;
												goto IL_523;
											}
											if (value3 == "dtmf")
											{
												grammarType = (grammar.Mode = GrammarType.DtmfGrammar);
												goto IL_523;
											}
										}
										XmlParser.ThrowSrgsException(SRID.InvalidGrammarMode, new object[0]);
										goto IL_523;
									}
								}
								else
								{
									XmlParser.CheckForDuplicates(ref text4, reader);
									if (text4 != "1.0")
									{
										XmlParser.ThrowSrgsException(SRID.InvalidVersion, new object[0]);
										goto IL_523;
									}
									goto IL_523;
								}
							}
							else
							{
								if (grammar.Root == null)
								{
									grammar.Root = reader.Value;
									goto IL_523;
								}
								XmlParser.ThrowSrgsException(SRID.RootRuleAlreadyDefined, new object[0]);
								goto IL_523;
							}
						}
						flag = true;
					}
				}
				IL_523:
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

		// Token: 0x06000474 RID: 1140 RVA: 0x000120E0 File Offset: 0x000110E0
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
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							string localName;
							if ((localName = reader.LocalName) != null)
							{
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000468-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
									dictionary.Add("dynamic", 0);
									dictionary.Add("baseclass", 1);
									dictionary.Add("onInit", 2);
									dictionary.Add("onParse", 3);
									dictionary.Add("onError", 4);
									dictionary.Add("onRecognition", 5);
									<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000468-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000468-1.TryGetValue(localName, ref num))
								{
									switch (num)
									{
									case 0:
									{
										XmlParser.CheckForDuplicates(ref text3, reader);
										string text9;
										if ((text9 = text3) != null)
										{
											if (text9 == "true")
											{
												ruleDynamic = RuleDynamic.True;
												goto IL_244;
											}
											if (text9 == "false")
											{
												ruleDynamic = RuleDynamic.False;
												goto IL_244;
											}
										}
										XmlParser.ThrowSrgsException(SRID.InvalidDynamicSetting, new object[0]);
										goto IL_244;
									}
									case 1:
										XmlParser.CheckForDuplicates(ref text4, reader);
										if (string.IsNullOrEmpty(text4))
										{
											XmlParser.ThrowSrgsException(SRID.NoName1, new object[] { "baseclass" });
											goto IL_244;
										}
										goto IL_244;
									case 2:
										XmlParser.CheckForDuplicates(ref text5, reader);
										text5 = reader.Value;
										goto IL_244;
									case 3:
										XmlParser.CheckForDuplicates(ref text6, reader);
										text6 = reader.Value;
										goto IL_244;
									case 4:
										XmlParser.CheckForDuplicates(ref text7, reader);
										text7 = reader.Value;
										goto IL_244;
									case 5:
										XmlParser.CheckForDuplicates(ref text8, reader);
										goto IL_244;
									}
								}
							}
							flag = true;
						}
					}
					else
					{
						string localName2;
						if ((localName2 = reader.LocalName) != null)
						{
							if (localName2 == "id")
							{
								XmlParser.CheckForDuplicates(ref text, reader);
								goto IL_244;
							}
							if (localName2 == "scope")
							{
								XmlParser.CheckForDuplicates(ref text2, reader);
								string text10;
								if ((text10 = text2) != null)
								{
									if (text10 == "private")
									{
										rulePublic = RulePublic.False;
										goto IL_244;
									}
									if (text10 == "public")
									{
										rulePublic = RulePublic.True;
										goto IL_244;
									}
								}
								XmlParser.ThrowSrgsException(SRID.InvalidRuleScope, new object[0]);
								goto IL_244;
							}
						}
						flag = true;
					}
				}
				IL_244:
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

		// Token: 0x06000475 RID: 1141 RVA: 0x00012490 File Offset: 0x00011490
		private IRuleRef ParseRuleRef(IElement parent, XmlReader reader)
		{
			IRuleRef ruleRef = null;
			string text = null;
			string text2 = null;
			string text3 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) == null)
				{
					goto IL_181;
				}
				if (!(namespaceURI == ""))
				{
					if (!(namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
					{
						goto IL_181;
					}
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "semantic-key")
						{
							XmlParser.CheckForDuplicates(ref text, reader);
							goto IL_184;
						}
						if (localName == "params")
						{
							XmlParser.CheckForDuplicates(ref text2, reader);
							goto IL_184;
						}
					}
					flag = true;
				}
				else
				{
					string localName2;
					if ((localName2 = reader.LocalName) != null)
					{
						if (localName2 == "uri")
						{
							XmlParser.CheckForDuplicates(ref text3, reader);
							this.ValidateRulerefNotPointingToSelf(text3);
							goto IL_184;
						}
						if (localName2 == "special")
						{
							if (ruleRef != null)
							{
								XmlParser.ThrowSrgsException(SRID.InvalidAttributeDefinedTwice, new object[] { reader.Value, "special" });
							}
							string value;
							if ((value = reader.Value) == null)
							{
								goto IL_11E;
							}
							if (!(value == "NULL"))
							{
								if (!(value == "VOID"))
								{
									if (!(value == "GARBAGE"))
									{
										goto IL_11E;
									}
									ruleRef = this._parser.Garbage;
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
							IL_12B:
							this._parser.InitSpecialRuleRef(parent, ruleRef);
							goto IL_184;
							IL_11E:
							XmlParser.ThrowSrgsException(SRID.InvalidSpecialRuleRef, new object[0]);
							goto IL_12B;
						}
						if (localName2 == "type")
						{
							goto IL_184;
						}
					}
					flag = true;
				}
				IL_184:
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidRulerefAttribute, new object[] { reader.Name });
					continue;
				}
				continue;
				IL_181:
				flag = true;
				goto IL_184;
			}
			this.ProcessChildNodes(reader, null, null, "ruleref");
			if (ruleRef == null)
			{
				if (text3 == null)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidRuleRef, new object[] { "uri" });
				}
				ruleRef = this._parser.CreateRuleRef(parent, new Uri(text3, 0), text, text2);
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

		// Token: 0x06000476 RID: 1142 RVA: 0x000126D0 File Offset: 0x000116D0
		private IOneOf ParseOneOf(IElement parent, IRule rule, XmlReader reader)
		{
			IOneOf oneOf = this._parser.CreateOneOf(parent, rule);
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null && (namespaceURI == "" || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
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

		// Token: 0x06000477 RID: 1143 RVA: 0x00012754 File Offset: 0x00011754
		private IItem ParseItem(IElement parent, IRule rule, XmlReader reader)
		{
			float num = 0.5f;
			int num2 = 1;
			int num3 = 1;
			float num4 = 1f;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							flag = true;
						}
					}
					else
					{
						string localName;
						if ((localName = reader.LocalName) != null)
						{
							if (localName == "repeat")
							{
								XmlParser.SetRepeatValues(reader.Value, out num2, out num3);
								goto IL_BA;
							}
							if (localName == "repeat-prob")
							{
								num = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
								goto IL_BA;
							}
							if (localName == "weight")
							{
								num4 = Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
								goto IL_BA;
							}
						}
						flag = true;
					}
				}
				IL_BA:
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

		// Token: 0x06000478 RID: 1144 RVA: 0x00012878 File Offset: 0x00011878
		private ISubset ParseSubset(IElement parent, XmlReader reader)
		{
			string text = null;
			MatchMode matchMode = MatchMode.Subsequence;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length == 0;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null && namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
				{
					string localName;
					if ((localName = reader.LocalName) != null && localName == "match")
					{
						XmlParser.CheckForDuplicates(ref text, reader);
						string value;
						if ((value = reader.Value) != null)
						{
							if (value == "subsequence")
							{
								matchMode = MatchMode.Subsequence;
								goto IL_B0;
							}
							if (value == "ordered-subset")
							{
								matchMode = MatchMode.OrderedSubset;
								goto IL_B0;
							}
							if (value == "subsequence-content-required")
							{
								matchMode = MatchMode.SubsequenceContentRequired;
								goto IL_B0;
							}
							if (value == "ordered-subset-content-required")
							{
								matchMode = MatchMode.OrderedSubsetContentRequired;
								goto IL_B0;
							}
						}
						flag = true;
					}
					else
					{
						flag = true;
					}
				}
				IL_B0:
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidSubsetAttribute, new object[] { reader.Name });
				}
			}
			string text2 = XmlParser.GetStringContent(reader).Trim();
			if (text2.Length == 0)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyElement, new object[] { "subset" });
			}
			return this._parser.CreateSubset(parent, text2, matchMode);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000129A0 File Offset: 0x000119A0
		private IToken ParseToken(IElement parent, XmlReader reader)
		{
			string text = null;
			string text2 = null;
			float num = -1f;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							string localName;
							if ((localName = reader.LocalName) != null)
							{
								if (!(localName == "pron"))
								{
									if (!(localName == "display"))
									{
										if (localName == "reqconf")
										{
											string value;
											if ((value = reader.Value) != null)
											{
												if (value == "high")
												{
													num = 0.8f;
													goto IL_185;
												}
												if (value == "normal")
												{
													num = 0.5f;
													goto IL_185;
												}
												if (value == "low")
												{
													num = 0.2f;
													goto IL_185;
												}
											}
											XmlParser.ThrowSrgsException(SRID.InvalidReqConfAttribute, new object[] { reader.Name });
											goto IL_185;
										}
									}
									else
									{
										if (!string.IsNullOrEmpty(text2))
										{
											XmlParser.ThrowSrgsException(SRID.MultipleDisplayString, new object[0]);
											goto IL_185;
										}
										text2 = reader.Value.Trim(Helpers._achTrimChars);
										if (string.IsNullOrEmpty(text2))
										{
											XmlParser.ThrowSrgsException(SRID.EmptyDisplayString, new object[0]);
											goto IL_185;
										}
										goto IL_185;
									}
								}
								else
								{
									if (!string.IsNullOrEmpty(text))
									{
										XmlParser.ThrowSrgsException(SRID.MuliplePronunciationString, new object[0]);
										goto IL_185;
									}
									text = reader.Value.Trim(Helpers._achTrimChars);
									if (string.IsNullOrEmpty(text))
									{
										XmlParser.ThrowSrgsException(SRID.EmptyPronunciationString, new object[0]);
										goto IL_185;
									}
									goto IL_185;
								}
							}
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				IL_185:
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidTokenAttribute, new object[] { reader.Name });
				}
			}
			string text3 = XmlParser.GetStringContent(reader).Trim(Helpers._achTrimChars);
			if (string.IsNullOrEmpty(text3))
			{
				XmlParser.ThrowSrgsException(SRID.InvalidEmptyElement, new object[] { "token" });
			}
			if (text3.IndexOf('"') >= 0)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidTokenString, new object[0]);
			}
			return this._parser.CreateToken(parent, text3, text, text2, num);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012BBD File Offset: 0x00011BBD
		private void ParseText(IElement parent, string sChars, string pronunciation, string display, float reqConfidence)
		{
			XmlParser.ParseText(parent, sChars, pronunciation, display, reqConfidence, new CreateTokenCallback(this._parser.CreateToken));
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00012BE0 File Offset: 0x00011BE0
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

		// Token: 0x0600047C RID: 1148 RVA: 0x00012C58 File Offset: 0x00011C58
		private string GetTagContent(IElement parent, XmlReader reader)
		{
			if (!this._hasTagFormat)
			{
				XmlParser.ThrowSrgsException(SRID.MissingTagFormat, new object[0]);
			}
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null && (namespaceURI == "" || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
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

		// Token: 0x0600047D RID: 1149 RVA: 0x00012CE0 File Offset: 0x00011CE0
		private static void ParseLexicon(XmlReader reader)
		{
			bool flag = false;
			bool flag2 = false;
			while (reader.MoveToNextAttribute())
			{
				string localName;
				if ((localName = reader.LocalName) == null)
				{
					goto IL_30;
				}
				if (!(localName == "uri"))
				{
					if (!(localName == "type"))
					{
						goto IL_30;
					}
				}
				else
				{
					flag2 = true;
				}
				IL_32:
				if (flag)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidLexiconAttribute, new object[] { reader.Name });
					continue;
				}
				continue;
				IL_30:
				flag = true;
				goto IL_32;
			}
			if (!flag2)
			{
				XmlParser.ThrowSrgsException(SRID.MissingRequiredAttribute, new object[] { "uri", "lexicon" });
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00012D6C File Offset: 0x00011D6C
		private static void ParseMeta(XmlReader reader)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			while (reader.MoveToNextAttribute())
			{
				string localName;
				if ((localName = reader.LocalName) == null)
				{
					goto IL_55;
				}
				if (!(localName == "name") && !(localName == "http-equiv"))
				{
					if (!(localName == "content"))
					{
						goto IL_55;
					}
					flag3 = flag;
					flag = true;
				}
				else
				{
					if (flag2)
					{
						XmlParser.ThrowSrgsException(SRID.MetaNameHTTPEquiv, new object[0]);
					}
					flag2 = true;
				}
				IL_57:
				if (flag3)
				{
					XmlParser.ThrowSrgsException(SRID.InvalidMetaAttribute, new object[] { reader.Name });
					continue;
				}
				continue;
				IL_55:
				flag3 = true;
				goto IL_57;
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

		// Token: 0x0600047F RID: 1151 RVA: 0x00012E48 File Offset: 0x00011E48
		private void ParseScript(XmlReader reader, IGrammar grammar)
		{
			int num = ((this._filename != null) ? this._xmlTextReader.LineNumber : (-1));
			string text = null;
			while (reader.MoveToNextAttribute())
			{
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							string localName;
							if ((localName = reader.LocalName) != null && localName == "rule")
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
			}
			if (string.IsNullOrEmpty(text))
			{
				this._parser.AddScript(grammar, XmlParser.GetStringContent(reader), this._filename, num);
				return;
			}
			this._scripts.Add(new XmlParser.ForwardReference(text, this._parser.AddScript(grammar, text, XmlParser.GetStringContent(reader), this._filename, num)));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00012F44 File Offset: 0x00011F44
		private static void ParseAssemblyReference(XmlReader reader, IGrammar grammar)
		{
			while (reader.MoveToNextAttribute())
			{
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							string localName;
							if ((localName = reader.LocalName) != null && localName == "assembly")
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
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00012FCC File Offset: 0x00011FCC
		private static void ParseImportNamespace(XmlReader reader, IGrammar grammar)
		{
			while (reader.MoveToNextAttribute())
			{
				string namespaceURI;
				if ((namespaceURI = reader.NamespaceURI) != null)
				{
					if (!(namespaceURI == ""))
					{
						if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
						{
							string localName;
							if ((localName = reader.LocalName) != null && localName == "namespace")
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
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00013054 File Offset: 0x00012054
		private bool ProcessChildNodes(XmlReader reader, IElement parent, IRule rule, string parentName)
		{
			bool flag = true;
			List<IPropertyTag> list = null;
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != 15)
				{
					bool flag2 = false;
					if (reader.NodeType == 1)
					{
						if (parent == null)
						{
							XmlParser.ThrowSrgsException(SRID.InvalidNotEmptyElement, new object[] { parentName });
						}
						IElement element = null;
						string namespaceURI;
						if ((namespaceURI = reader.NamespaceURI) == null)
						{
							goto IL_1F7;
						}
						if (!(namespaceURI == "http://www.w3.org/2001/06/grammar"))
						{
							if (!(namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
							{
								goto IL_1F7;
							}
							string localName;
							if ((localName = reader.LocalName) != null && localName == "subset")
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
						else
						{
							string localName2;
							if ((localName2 = reader.LocalName) != null)
							{
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000476-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
									dictionary.Add("example", 0);
									dictionary.Add("ruleref", 1);
									dictionary.Add("one-of", 2);
									dictionary.Add("item", 3);
									dictionary.Add("token", 4);
									dictionary.Add("tag", 5);
									dictionary.Add("rule", 6);
									<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000476-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000476-1.TryGetValue(localName2, ref num))
								{
									switch (num)
									{
									case 0:
										if (!(parent is IRule) || !flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidExampleOrdering, new object[0]);
											goto IL_1FD;
										}
										reader.Skip();
										continue;
									case 1:
										element = this.ParseRuleRef(parent, reader);
										goto IL_1FD;
									case 2:
										element = this.ParseOneOf(parent, rule, reader);
										goto IL_1FD;
									case 3:
										element = this.ParseItem(parent, rule, reader);
										goto IL_1FD;
									case 4:
										element = this.ParseToken(parent, reader);
										goto IL_1FD;
									case 5:
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
											goto IL_1FD;
										}
										goto IL_1FD;
									}
									}
								}
							}
							flag2 = true;
						}
						IL_1FD:
						flag2 = this.ParseChildNodeElement(parent, flag2, element);
						flag = false;
						goto IL_24A;
						IL_1F7:
						reader.Skip();
						goto IL_1FD;
					}
					if (reader.NodeType == 3 || reader.NodeType == 4)
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
					IL_24A:
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

		// Token: 0x06000483 RID: 1155 RVA: 0x0001332C File Offset: 0x0001232C
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

		// Token: 0x06000484 RID: 1156 RVA: 0x000133AC File Offset: 0x000123AC
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

		// Token: 0x06000485 RID: 1157 RVA: 0x0001341C File Offset: 0x0001241C
		private void ProcessRulesAndScriptsNodes(XmlReader reader, IGrammar grammar)
		{
			bool flag = false;
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != 15)
				{
					bool flag2 = false;
					if (reader.NodeType == 1)
					{
						string namespaceURI;
						if ((namespaceURI = reader.NamespaceURI) != null)
						{
							if (namespaceURI == "http://www.w3.org/2001/06/grammar")
							{
								string localName;
								if ((localName = reader.LocalName) != null)
								{
									if (localName == "lexicon")
									{
										if (flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
										}
										XmlParser.ParseLexicon(reader);
										goto IL_1EF;
									}
									if (localName == "meta")
									{
										if (flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
										}
										XmlParser.ParseMeta(reader);
										goto IL_1EF;
									}
									if (localName == "metadata")
									{
										if (flag)
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
										}
										reader.Skip();
										goto IL_1EF;
									}
									if (localName == "rule")
									{
										IRule rule = this.ParseRule(grammar, reader);
										rule.PostParse(grammar);
										flag = true;
										goto IL_1EF;
									}
									if (localName == "tag")
									{
										if (flag || (this._hasTagFormat && grammar.TagFormat != SrgsTagFormat.W3cV1))
										{
											XmlParser.ThrowSrgsException(SRID.InvalidGrammarOrdering, new object[0]);
										}
										grammar.GlobalTags.Add(this.GetTagContent(grammar, reader));
										goto IL_1EF;
									}
								}
								flag2 = true;
								goto IL_1EF;
							}
							if (namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions")
							{
								string localName2;
								if ((localName2 = reader.LocalName) != null)
								{
									if (localName2 == "script")
									{
										this.ParseScript(reader, grammar);
										flag = true;
										goto IL_1EF;
									}
									if (localName2 == "assemblyReference")
									{
										XmlParser.ParseAssemblyReference(reader, grammar);
										flag = true;
										goto IL_1EF;
									}
									if (localName2 == "importNamespace")
									{
										XmlParser.ParseImportNamespace(reader, grammar);
										flag = true;
										goto IL_1EF;
									}
								}
								flag2 = true;
								goto IL_1EF;
							}
						}
						reader.Skip();
					}
					else
					{
						if (reader.NodeType == 3)
						{
							XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { "text" });
						}
						reader.Skip();
					}
					IL_1EF:
					if (flag2)
					{
						XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[] { reader.Name });
					}
				}
			}
			reader.Read();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013650 File Offset: 0x00012650
		private static string GetStringContent(XmlReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != 15)
				{
					stringBuilder.Append(reader.ReadString());
					bool flag = false;
					if (reader.NodeType == 1)
					{
						string namespaceURI;
						if ((namespaceURI = reader.NamespaceURI) != null && (namespaceURI == "http://www.w3.org/2001/06/grammar" || namespaceURI == "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions"))
						{
							flag = true;
						}
						else
						{
							reader.Skip();
						}
					}
					else if (reader.NodeType != 15)
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

		// Token: 0x06000487 RID: 1159 RVA: 0x0001370C File Offset: 0x0001270C
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
				if (sTag.get_Chars(num) == '"')
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
				if (int.TryParse(text, ref num3))
				{
					value = num3;
					return;
				}
				double num4;
				if (double.TryParse(text, ref num4))
				{
					value = num4;
					return;
				}
				bool flag;
				if (bool.TryParse(text, ref flag))
				{
					value = flag;
					return;
				}
				XmlParser.ThrowSrgsException(SRID.InvalidNameValueProperty, new object[] { name, text });
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00013810 File Offset: 0x00012810
		private static void SetRepeatValues(string repeat, out int minRepeat, out int maxRepeat)
		{
			minRepeat = (maxRepeat = 1);
			if (!string.IsNullOrEmpty(repeat))
			{
				int num = repeat.IndexOf("-", 4);
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

		// Token: 0x06000489 RID: 1161 RVA: 0x00013958 File Offset: 0x00012958
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

		// Token: 0x0600048A RID: 1162 RVA: 0x000139C8 File Offset: 0x000129C8
		internal static void ValidateRuleId(string id)
		{
			Helpers.ThrowIfEmptyOrNull(id, "id");
			if (!XmlReader.IsName(id) || id == "NULL" || id == "VOID" || id == "GARBAGE" || id.IndexOfAny(XmlParser._invalidRuleIdChars) != -1)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidRuleId, new object[] { id });
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00013A30 File Offset: 0x00012A30
		private void ValidateRulerefNotPointingToSelf(string uri)
		{
			if (this._filename != null && uri.IndexOf(this._shortFilename, 4) == 0 && ((uri.Length > this._shortFilename.Length && uri.get_Chars(this._shortFilename.Length) == '#') || uri.Length == this._shortFilename.Length))
			{
				XmlParser.ThrowSrgsException(SRID.InvalidRuleRefSelf, new object[0]);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00013A9C File Offset: 0x00012A9C
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

		// Token: 0x040003AE RID: 942
		internal const string emptyNamespace = "";

		// Token: 0x040003AF RID: 943
		internal const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x040003B0 RID: 944
		internal const string srgsNamespace = "http://www.w3.org/2001/06/grammar";

		// Token: 0x040003B1 RID: 945
		internal const string sapiNamespace = "http://schemas.microsoft.com/Speech/2002/06/SRGSExtensions";

		// Token: 0x040003B2 RID: 946
		private IElementFactory _parser;

		// Token: 0x040003B3 RID: 947
		private XmlReader _reader;

		// Token: 0x040003B4 RID: 948
		private XmlTextReader _xmlTextReader;

		// Token: 0x040003B5 RID: 949
		private string _filename;

		// Token: 0x040003B6 RID: 950
		private string _shortFilename;

		// Token: 0x040003B7 RID: 951
		private CultureInfo _langId;

		// Token: 0x040003B8 RID: 952
		private bool _hasTagFormat;

		// Token: 0x040003B9 RID: 953
		private List<string> _rules = new List<string>();

		// Token: 0x040003BA RID: 954
		private List<XmlParser.ForwardReference> _scripts = new List<XmlParser.ForwardReference>();

		// Token: 0x040003BB RID: 955
		private static readonly char[] _invalidRuleIdChars = new char[] { '.', ':', '-', '#' };

		// Token: 0x040003BC RID: 956
		private static readonly char[] _SlashBackSlash = new char[] { '\\', '/' };

		// Token: 0x020000CD RID: 205
		[Serializable]
		internal class ForwardReference
		{
			// Token: 0x0600048E RID: 1166 RVA: 0x00013BC2 File Offset: 0x00012BC2
			internal ForwardReference(string name, string value)
			{
				this._name = name;
				this._value = value;
			}

			// Token: 0x040003BD RID: 957
			internal string _name;

			// Token: 0x040003BE RID: 958
			internal string _value;
		}
	}
}
