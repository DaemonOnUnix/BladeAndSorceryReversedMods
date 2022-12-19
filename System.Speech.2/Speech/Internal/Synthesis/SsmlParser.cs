using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Text;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C2 RID: 194
	internal static class SsmlParser
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x000192D0 File Offset: 0x000174D0
		internal static void Parse(string ssml, ISsmlParser engine, object voice)
		{
			string text = ssml.Replace('\n', ' ');
			text = text.Replace('\r', ' ');
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(text));
			SsmlParser.Parse(xmlTextReader, engine, voice);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00019308 File Offset: 0x00017508
		internal static void Parse(XmlReader reader, ISsmlParser engine, object voice)
		{
			try
			{
				bool flag = false;
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "speak")
					{
						if (flag)
						{
							SsmlParser.ThrowFormatException(SRID.GrammarDefTwice, new object[0]);
						}
						else
						{
							SsmlParser.ProcessSpeakElement(reader, engine, voice);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					SsmlParser.ThrowFormatException(SRID.SynthesizerNoSpeak, new object[0]);
				}
			}
			catch (XmlException ex)
			{
				throw new FormatException(SR.Get(SRID.InvalidXml, new object[0]), ex);
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00019394 File Offset: 0x00017594
		private static void ProcessSpeakElement(XmlReader reader, ISsmlParser engine, object voice)
		{
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes._voice = voice;
			ssmlAttributes._age = VoiceAge.NotSet;
			ssmlAttributes._gender = VoiceGender.NotSet;
			ssmlAttributes._unknownNamespaces = new List<SsmlXmlAttribute>();
			string text = null;
			string text2 = null;
			string text3 = null;
			CultureInfo cultureInfo = null;
			List<SsmlXmlAttribute> list = new List<SsmlXmlAttribute>();
			Exception ex = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				if (reader.NamespaceURI.Length == 0)
				{
					string text4 = reader.LocalName;
					if (text4 == "version")
					{
						SsmlParser.CheckForDuplicates(ref text, reader);
						if (text != "1.0")
						{
							SsmlParser.ThrowFormatException(SRID.InvalidVersion, new object[0]);
						}
					}
					else
					{
						flag = true;
					}
				}
				else if (reader.NamespaceURI == "http://www.w3.org/XML/1998/namespace")
				{
					string text4 = reader.LocalName;
					if (!(text4 == "lang"))
					{
						if (!(text4 == "base"))
						{
							flag = true;
							goto IL_1EB;
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							cultureInfo = new CultureInfo(text2);
							goto IL_1EB;
						}
						catch (ArgumentException ex2)
						{
							ex = ex2;
							int num = reader.Value.IndexOf("-", StringComparison.Ordinal);
							if (num > 0)
							{
								try
								{
									cultureInfo = new CultureInfo(reader.Value.Substring(0, num));
									goto IL_12D;
								}
								catch (ArgumentException)
								{
									flag = true;
									goto IL_12D;
								}
							}
							flag = true;
							IL_12D:
							goto IL_1EB;
						}
					}
					SsmlParser.CheckForDuplicates(ref text3, reader);
				}
				else if (reader.NamespaceURI == "http://www.w3.org/2000/xmlns/")
				{
					if (reader.Value != "http://www.w3.org/2001/10/synthesis" && reader.Value != "http://schemas.microsoft.com/Speech/2003/03/PromptEngine")
					{
						ssmlAttributes._unknownNamespaces.Add(new SsmlXmlAttribute(reader.Prefix, reader.LocalName, reader.Value, reader.NamespaceURI));
					}
					else if (reader.Value == "http://schemas.microsoft.com/Speech/2003/03/PromptEngine")
					{
						engine.ContainsPexml(reader.LocalName);
					}
				}
				else
				{
					list.Add(new SsmlXmlAttribute(reader.Prefix, reader.LocalName, reader.Value, reader.NamespaceURI));
				}
				IL_1EB:
				if (flag)
				{
					SsmlParser.ThrowFormatException(ex, SRID.InvalidElement, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "version", "speak" });
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "lang", "speak" });
			}
			List<SsmlXmlAttribute> list2 = null;
			foreach (SsmlXmlAttribute ssmlXmlAttribute in list)
			{
				ssmlAttributes.AddUnknowAttribute(ssmlXmlAttribute, ref list2);
			}
			voice = engine.ProcessSpeak(text, text3, cultureInfo, ssmlAttributes._unknownNamespaces);
			ssmlAttributes._fragmentState.LangId = cultureInfo.LCID;
			ssmlAttributes._voice = voice;
			ssmlAttributes._baseUri = text3;
			SsmlElement ssmlElement = SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Lexicon | SsmlElement.Meta | SsmlElement.MetaData | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput | SsmlParser.ElementPromptEngine(ssmlAttributes);
			SsmlParser.ProcessElement(reader, engine, "speak", ssmlElement, ssmlAttributes, false, list2);
			engine.EndSpeakElement();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000196C0 File Offset: 0x000178C0
		private static void ProcessElement(XmlReader reader, ISsmlParser engine, string sElement, SsmlElement possibleElements, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore, List<SsmlXmlAttribute> extraAttributes)
		{
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes = ssmAttributesParent;
			if (extraAttributes != null && extraAttributes.Count > 0)
			{
				engine.StartProcessUnknownAttributes(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, sElement, extraAttributes);
			}
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				do
				{
					XmlNodeType nodeType = reader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Text)
						{
							if (nodeType != XmlNodeType.EndElement)
							{
								reader.Read();
							}
						}
						else
						{
							if ((possibleElements & SsmlElement.Text) != (SsmlElement)0)
							{
								engine.ProcessText(reader.Value, ssmlAttributes._voice, ref ssmlAttributes._fragmentState, SsmlParser.GetColumnPosition(reader), fIgnore);
							}
							else
							{
								SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
							}
							reader.Read();
						}
					}
					else
					{
						int num = Array.BinarySearch<string>(SsmlParser._elementsName, reader.LocalName);
						if (num >= 0)
						{
							SsmlParser._parseElements[num](reader, engine, possibleElements, ssmlAttributes, fIgnore);
						}
						else
						{
							if (ssmlAttributes.IsOtherNamespaceElement(reader))
							{
								engine.ProcessUnknownElement(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, reader);
								goto IL_126;
							}
							SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
						}
						reader.Read();
					}
					IL_126:;
				}
				while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None);
			}
			if (extraAttributes != null && extraAttributes.Count > 0)
			{
				engine.EndProcessUnknownAttributes(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, sElement, extraAttributes);
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001982C File Offset: 0x00017A2C
		private static void ParseAudio(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Audio, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			bool flag = false;
			while (reader.MoveToNextAttribute())
			{
				bool flag2 = reader.NamespaceURI.Length != 0;
				if (!flag2)
				{
					string localName = reader.LocalName;
					if (localName == "src")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							engine.ProcessAudio(ssmlAttributes._voice, text2, ssmlAttributes._baseUri, fIgnore);
							goto IL_7C;
						}
						catch (IOException)
						{
							flag = true;
							goto IL_7C;
						}
						catch (WebException)
						{
							flag = true;
							goto IL_7C;
						}
					}
					flag2 = true;
				}
				IL_7C:
				if (flag2 && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			ssmlAttributes._fRenderDesc = flag;
			SsmlElement ssmlElement = SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Desc | SsmlElement.Text | SsmlElement.PromptEngineOutput | SsmlParser.ElementPromptEngine(ssmlAttributes);
			SsmlParser.ProcessElement(reader, engine, text, ssmlElement, ssmlAttributes, !flag, list);
			engine.EndElement();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00019934 File Offset: 0x00017B34
		private static void ParseBreak(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Break, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			ssmlAttributes._fragmentState.Action = TtsEngineAction.Silence;
			ssmlAttributes._fragmentState.Emphasis = -7;
			string text2 = null;
			string text3 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (!(localName == "time"))
					{
						if (!(localName == "strength"))
						{
							flag = true;
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							if (text2 == null)
							{
								ssmlAttributes._fragmentState.Duration = 0;
								int num = Array.BinarySearch<string>(SsmlParser._breakStrength, text3);
								if (num < 0)
								{
									flag = true;
								}
								else if (ssmlAttributes._fragmentState.Emphasis != -1)
								{
									ssmlAttributes._fragmentState.Emphasis = (int)SsmlParser._breakEmphasis[num];
								}
							}
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						ssmlAttributes._fragmentState.Emphasis = -1;
						ssmlAttributes._fragmentState.Duration = SsmlParser.ParseCSS2Time(text2);
						flag = ssmlAttributes._fragmentState.Duration < 0;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidSpeakAttribute, new object[] { reader.Name, "break" });
				}
			}
			engine.ProcessBreak(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, (EmphasisBreak)ssmlAttributes._fragmentState.Emphasis, ssmlAttributes._fragmentState.Duration, fIgnore);
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00019AD4 File Offset: 0x00017CD4
		private static void ParseDesc(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Desc, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			CultureInfo cultureInfo = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI != "http://www.w3.org/XML/1998/namespace";
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "lang")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							cultureInfo = new CultureInfo(text2);
						}
						catch (ArgumentException)
						{
							flag = true;
						}
						flag &= cultureInfo != null;
					}
					else
					{
						flag = true;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			engine.ProcessDesc(cultureInfo);
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.Text, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00019BBC File Offset: 0x00017DBC
		private static void ParseEmphasis(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Emphasis, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			ssmlAttributes._fragmentState.Emphasis = 2;
			string text2 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "level")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						int num = Array.BinarySearch<string>(SsmlParser._emphasisNames, text2);
						if (num < 0)
						{
							flag = true;
						}
						else
						{
							ssmlAttributes._fragmentState.Emphasis = (int)SsmlParser._emphasisWord[num];
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			engine.ProcessEmphasis(!string.IsNullOrEmpty(text2), (EmphasisWord)ssmlAttributes._fragmentState.Emphasis);
			SsmlElement ssmlElement = SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmlAttributes);
			SsmlParser.ProcessElement(reader, engine, text, ssmlElement, ssmlAttributes, fIgnore, list);
			engine.EndElement();
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00019CD0 File Offset: 0x00017ED0
		private static void ParseMark(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Mark, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "name")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
					}
					else
					{
						flag = true;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "name", "mark" });
			}
			ssmlAttributes._fragmentState.Action = TtsEngineAction.Bookmark;
			engine.ProcessMark(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, text2, fIgnore);
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00019DC4 File Offset: 0x00017FC4
		private static void ParseMetaData(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ValidateElement(element, SsmlElement.MetaData, reader.Name);
			if (!reader.IsEmptyElement)
			{
				int num = 1;
				do
				{
					reader.Read();
					if (reader.NodeType == XmlNodeType.Element)
					{
						num++;
					}
					if (reader.NodeType == XmlNodeType.EndElement || reader.NodeType == XmlNodeType.None)
					{
						num--;
					}
				}
				while (num > 0);
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00019E18 File Offset: 0x00018018
		private static void ParseParagraph(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Paragraph, reader.Name);
			SsmlParser.ParseTextBlock(reader, engine, true, text, ssmAttributesParent, fIgnore);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00019E44 File Offset: 0x00018044
		private static void ParseSentence(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Sentence, reader.Name);
			SsmlParser.ParseTextBlock(reader, engine, false, text, ssmAttributesParent, fIgnore);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00019E6C File Offset: 0x0001806C
		private static void ParseTextBlock(XmlReader reader, ISsmlParser engine, bool isParagraph, string sElement, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text = null;
			CultureInfo cultureInfo = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI != "http://www.w3.org/XML/1998/namespace";
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "lang")
					{
						SsmlParser.CheckForDuplicates(ref text, reader);
						try
						{
							cultureInfo = new CultureInfo(text);
							goto IL_59;
						}
						catch (ArgumentException)
						{
							flag = true;
							goto IL_59;
						}
					}
					flag = true;
				}
				IL_59:
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			bool flag2 = cultureInfo != null && cultureInfo.LCID != ssmlAttributes._fragmentState.LangId;
			ssmlAttributes._voice = engine.ProcessTextBlock(isParagraph, ssmlAttributes._voice, ref ssmlAttributes._fragmentState, cultureInfo, flag2, ssmlAttributes._gender, ssmlAttributes._age);
			if (flag2)
			{
				ssmlAttributes._fragmentState.LangId = cultureInfo.LCID;
			}
			SsmlElement ssmlElement = SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmlAttributes);
			if (isParagraph)
			{
				ssmlElement |= SsmlElement.Sentence;
			}
			SsmlParser.ProcessElement(reader, engine, sElement, ssmlElement, ssmlAttributes, fIgnore, list);
			engine.EndProcessTextBlock(isParagraph);
			engine.EndElement();
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00019FA8 File Offset: 0x000181A8
		private static void ParsePhoneme(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Phoneme, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			AlphabetType alphabetType = AlphabetType.Ipa;
			string text3 = null;
			char[] array = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "alphabet")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 898290601U)
						{
							if (num != 428147290U)
							{
								if (num != 599994509U)
								{
									if (num != 898290601U)
									{
										goto IL_156;
									}
									if (!(text2 == "ups"))
									{
										goto IL_156;
									}
									goto IL_151;
								}
								else if (!(text2 == "x-sapi"))
								{
									goto IL_156;
								}
							}
							else if (!(text2 == "x-microsoft-sapi"))
							{
								goto IL_156;
							}
						}
						else if (num <= 2767308742U)
						{
							if (num != 1781664288U)
							{
								if (num != 2767308742U)
								{
									goto IL_156;
								}
								if (!(text2 == "sapi"))
								{
									goto IL_156;
								}
							}
							else
							{
								if (!(text2 == "x-ups"))
								{
									goto IL_156;
								}
								goto IL_151;
							}
						}
						else if (num != 2801901063U)
						{
							if (num != 3044962637U)
							{
								goto IL_156;
							}
							if (!(text2 == "x-microsoft-ups"))
							{
								goto IL_156;
							}
							goto IL_151;
						}
						else
						{
							if (!(text2 == "ipa"))
							{
								goto IL_156;
							}
							alphabetType = AlphabetType.Ipa;
							goto IL_179;
						}
						alphabetType = AlphabetType.Sapi;
						goto IL_179;
						IL_151:
						alphabetType = AlphabetType.Ups;
						goto IL_179;
						IL_156:
						throw new FormatException(SR.Get(SRID.UnsupportedAlphabet, new object[0]));
					}
					if (!(localName == "ph"))
					{
						flag = true;
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text3, reader);
					}
				}
				IL_179:
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text3))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "ph", "phoneme" });
			}
			try
			{
				switch (alphabetType)
				{
				case AlphabetType.Sapi:
					array = PhonemeConverter.ConvertPronToId(text3, ssmlAttributes._fragmentState.LangId).ToCharArray();
					goto IL_254;
				case AlphabetType.Ups:
					array = PhonemeConverter.UpsConverter.ConvertPronToId(text3).ToCharArray();
					alphabetType = AlphabetType.Ipa;
					goto IL_254;
				}
				array = text3.ToCharArray();
				try
				{
					PhonemeConverter.ValidateUpsIds(array);
				}
				catch (FormatException)
				{
					if (text2 != null)
					{
						throw;
					}
					array = PhonemeConverter.ConvertPronToId(text3, ssmlAttributes._fragmentState.LangId).ToCharArray();
					alphabetType = AlphabetType.Sapi;
				}
				IL_254:;
			}
			catch (FormatException)
			{
				SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { "phoneme" });
			}
			engine.ProcessPhoneme(ref ssmlAttributes._fragmentState, alphabetType, text3, array);
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.Text, ssmlAttributes, fIgnore, list);
			engine.EndElement();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001A26C File Offset: 0x0001846C
		private static void ParseProsody(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Prosody, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			string text7 = null;
			Prosody prosody = ((ssmlAttributes._fragmentState.Prosody != null) ? ssmlAttributes._fragmentState.Prosody.Clone() : new Prosody());
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (!(localName == "pitch"))
					{
						if (!(localName == "range"))
						{
							if (!(localName == "rate"))
							{
								if (!(localName == "volume"))
								{
									if (!(localName == "duration"))
									{
										if (!(localName == "contour"))
										{
											flag = true;
										}
										else
										{
											SsmlParser.CheckForDuplicates(ref text3, reader);
											prosody.SetContourPoints(SsmlParser.ParseContour(text3));
											if (prosody.GetContourPoints() == null)
											{
												flag = true;
											}
										}
									}
									else
									{
										SsmlParser.CheckForDuplicates(ref text6, reader);
										prosody.Duration = SsmlParser.ParseCSS2Time(text6);
									}
								}
								else
								{
									flag = SsmlParser.ParseNumberRelative(reader, ref text7, SsmlParser._volumeNames, SsmlParser._volumeWords, ref prosody._volume);
								}
							}
							else
							{
								flag = SsmlParser.ParseNumberRelative(reader, ref text5, SsmlParser._rateNames, SsmlParser._rateWords, ref prosody._rate);
							}
						}
						else
						{
							flag = SsmlParser.ParseNumberHz(reader, ref text4, SsmlParser._rangeNames, SsmlParser._rangeWords, ref prosody._range);
						}
					}
					else
					{
						flag = SsmlParser.ParseNumberHz(reader, ref text2, SsmlParser._pitchNames, SsmlParser._pitchWords, ref prosody._pitch);
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4) && string.IsNullOrEmpty(text5) && string.IsNullOrEmpty(text6) && string.IsNullOrEmpty(text7))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "pitch, contour, range, rate, duration, volume", "prosody" });
			}
			ssmlAttributes._fragmentState.Prosody = prosody;
			engine.ProcessProsody(text2, text4, text5, text7, text6, text3);
			SsmlElement ssmlElement = SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput | SsmlParser.ElementPromptEngine(ssmlAttributes);
			SsmlParser.ProcessElement(reader, engine, text, ssmlElement, ssmlAttributes, fIgnore, list);
			engine.EndElement();
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001A4D4 File Offset: 0x000186D4
		private static void ParseSayAs(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.SayAs, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			System.Speech.Synthesis.TtsEngine.SayAs sayAs = new System.Speech.Synthesis.TtsEngine.SayAs();
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (!(localName == "type") && !(localName == "interpret-as"))
					{
						if (!(localName == "format"))
						{
							if (!(localName == "detail"))
							{
								flag = true;
							}
							else
							{
								SsmlParser.CheckForDuplicates(ref text4, reader);
								sayAs.Detail = text4;
							}
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							sayAs.Format = text3;
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						sayAs.InterpretAs = text2;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "interpret-as", "say-as" });
			}
			ssmlAttributes._fragmentState.SayAs = sayAs;
			engine.ProcessSayAs(text2, text3, text4);
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.Text, ssmlAttributes, fIgnore, list);
			engine.EndElement();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001A630 File Offset: 0x00018830
		private static void ParseSub(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Sub, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			int num = 0;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (localName == "alias")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						XmlTextReader xmlTextReader = reader as XmlTextReader;
						if (xmlTextReader != null && engine.Ssml != null)
						{
							num = engine.Ssml.IndexOf(reader.Value, xmlTextReader.LinePosition + reader.LocalName.Length, StringComparison.Ordinal);
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "alias", "sub" });
			}
			engine.ProcessSub(text2, ssmlAttributes._voice, ref ssmlAttributes._fragmentState, num, fIgnore);
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.Text, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001A760 File Offset: 0x00018960
		private static void ParseVoice(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Voice, reader.Name);
			if (ssmAttributesParent._cPromptOutput > 0)
			{
				SsmlParser.ThrowFormatException(SRID.InvalidVoiceElementInPromptOutput, new object[0]);
			}
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			string text7 = null;
			CultureInfo cultureInfo = null;
			int num = -1;
			List<SsmlXmlAttribute> list = null;
			List<SsmlXmlAttribute> list2 = null;
			List<SsmlXmlAttribute> list3 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				if (reader.NamespaceURI.Length == 0)
				{
					string text8 = reader.LocalName;
					if (!(text8 == "gender"))
					{
						if (!(text8 == "age"))
						{
							if (!(text8 == "variant"))
							{
								if (!(text8 == "name"))
								{
									flag = true;
								}
								else
								{
									SsmlParser.CheckForDuplicates(ref text5, reader);
								}
							}
							else
							{
								SsmlParser.CheckForDuplicates(ref text4, reader);
								if (!int.TryParse(text4, out num) || num <= 0)
								{
									flag = true;
								}
							}
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text6, reader);
							VoiceAge voiceAge;
							if (!SsmlParserHelpers.TryConvertAge(text6, out voiceAge))
							{
								flag = true;
							}
							else
							{
								ssmlAttributes._age = voiceAge;
							}
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text3, reader);
						VoiceGender voiceGender;
						if (!SsmlParserHelpers.TryConvertGender(text3, out voiceGender))
						{
							flag = true;
						}
						else
						{
							ssmlAttributes._gender = voiceGender;
						}
					}
				}
				else if (reader.Prefix == "xmlns" && reader.Value == "http://schemas.microsoft.com/Speech/2003/03/PromptEngine")
				{
					SsmlParser.CheckForDuplicates(ref text7, reader);
				}
				else if (reader.NamespaceURI == "http://www.w3.org/XML/1998/namespace")
				{
					string text8 = reader.LocalName;
					if (text8 == "lang")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							cultureInfo = new CultureInfo(text2);
							goto IL_24A;
						}
						catch (ArgumentException)
						{
							flag = true;
							goto IL_24A;
						}
					}
					flag = true;
				}
				else if (reader.NamespaceURI == "http://www.w3.org/2000/xmlns/")
				{
					if (reader.Value != "http://www.w3.org/2001/10/synthesis")
					{
						if (list3 == null)
						{
							list3 = new List<SsmlXmlAttribute>();
						}
						SsmlXmlAttribute ssmlXmlAttribute = new SsmlXmlAttribute(reader.Prefix, reader.LocalName, reader.Value, reader.NamespaceURI);
						list3.Add(ssmlXmlAttribute);
						ssmlAttributes._unknownNamespaces.Add(ssmlXmlAttribute);
					}
				}
				else
				{
					if (list2 == null)
					{
						list2 = new List<SsmlXmlAttribute>();
					}
					list2.Add(new SsmlXmlAttribute(reader.Prefix, reader.LocalName, reader.Value, reader.NamespaceURI));
				}
				IL_24A:
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (list2 != null)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute2 in list2)
				{
					ssmlAttributes.AddUnknowAttribute(ssmlXmlAttribute2, ref list);
				}
			}
			if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text6) && string.IsNullOrEmpty(text4) && string.IsNullOrEmpty(text5) && string.IsNullOrEmpty(text7))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "'xml:lang' or 'gender' or 'age' or 'variant' or 'name'", "voice" });
			}
			cultureInfo = ((cultureInfo == null) ? new CultureInfo(ssmlAttributes._fragmentState.LangId) : cultureInfo);
			bool flag2 = cultureInfo.LCID != ssmlAttributes._fragmentState.LangId;
			ssmlAttributes._voice = engine.ProcessVoice(text5, cultureInfo, ssmlAttributes._gender, ssmlAttributes._age, num, flag2, list3);
			ssmlAttributes._fragmentState.LangId = cultureInfo.LCID;
			SsmlElement ssmlElement = SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput | SsmlParser.ElementPromptEngine(ssmlAttributes);
			SsmlParser.ProcessElement(reader, engine, text, ssmlElement, ssmlAttributes, fIgnore, list);
			if (list3 != null)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute3 in list3)
				{
					ssmlAttributes._unknownNamespaces.Remove(ssmlXmlAttribute3);
				}
			}
			engine.EndElement();
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001AB70 File Offset: 0x00018D70
		private static void ParseLexicon(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Lexicon, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			List<SsmlXmlAttribute> list = null;
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			string text3 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = reader.NamespaceURI.Length != 0;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (!(localName == "uri"))
					{
						if (!(localName == "type"))
						{
							flag = true;
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
					}
				}
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "uri", "lexicon" });
			}
			Uri uri = new Uri(text2, UriKind.RelativeOrAbsolute);
			if (!uri.IsAbsoluteUri && ssmlAttributes._baseUri != null)
			{
				text2 = ssmlAttributes._baseUri + "/" + text2;
				uri = new Uri(text2, UriKind.RelativeOrAbsolute);
			}
			engine.ProcessLexicon(uri, text3);
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001AC9C File Offset: 0x00018E9C
		private static void ParsePromptEngine0(XmlReader reader, ISsmlParser engine, SsmlElement elementAllowed, SsmlElement element, SsmlParser.ProcessPromptEngine0 process, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(elementAllowed, element, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			while (reader.MoveToNextAttribute())
			{
				if (reader.NamespaceURI == "http://www.w3.org/2000/xmlns/" && reader.Value == "http://schemas.microsoft.com/Speech/2003/03/PromptEngine")
				{
					engine.ContainsPexml(reader.LocalName);
				}
				else
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (!process(ssmAttributesParent._voice))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmAttributesParent), ssmAttributesParent, fIgnore, null);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001AD58 File Offset: 0x00018F58
		private static string ParsePromptEngine1(XmlReader reader, ISsmlParser engine, SsmlElement elementAllowed, SsmlElement element, string attribute, SsmlParser.ProcessPromptEngine1 process, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(elementAllowed, element, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			string text2 = null;
			while (reader.MoveToNextAttribute())
			{
				if (reader.LocalName == attribute)
				{
					SsmlParser.CheckForDuplicates(ref text2, reader);
				}
				else
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (!process(ssmAttributesParent._voice, text2))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmAttributesParent), ssmAttributesParent, fIgnore, null);
			return text2;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001AE00 File Offset: 0x00019000
		private static void ParsePromptOutput(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			ssmAttributesParent._cPromptOutput += 1;
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineOutput, new SsmlParser.ProcessPromptEngine0(engine.BeginPromptEngineOutput), ssmAttributesParent, fIgnore);
			engine.EndElement();
			ssmAttributesParent._cPromptOutput -= 1;
			engine.EndPromptEngineOutput(ssmAttributesParent._voice);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001AE56 File Offset: 0x00019056
		private static void ParseDiv(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineDiv, new SsmlParser.ProcessPromptEngine0(engine.ProcessPromptEngineDiv), ssmAttributesParent, fIgnore);
			engine.EndElement();
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001AE7C File Offset: 0x0001907C
		private static void ParseDatabase(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.PromptEngineDatabase, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			string text2 = null;
			string text3 = null;
			string text4 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				if (!flag)
				{
					string localName = reader.LocalName;
					if (!(localName == "fname"))
					{
						if (!(localName == "idset"))
						{
							if (!(localName == "delta"))
							{
								flag = true;
							}
							else
							{
								SsmlParser.CheckForDuplicates(ref text3, reader);
							}
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text4, reader);
						}
					}
					else
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
					}
				}
				if (flag)
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (!engine.ProcessPromptEngineDatabase(ssmAttributesParent._voice, text2, text3, text4))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmAttributesParent, fIgnore, null);
			engine.EndElement();
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001AF70 File Offset: 0x00019170
		private static void ParseId(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineId, "id", new SsmlParser.ProcessPromptEngine1(engine.ProcessPromptEngineId), ssmAttributesParent, fIgnore);
			engine.EndElement();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001AF9B File Offset: 0x0001919B
		private static void ParseTts(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineTTS, new SsmlParser.ProcessPromptEngine0(engine.BeginPromptEngineTts), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineTts(ssmAttributesParent._voice);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001AFCC File Offset: 0x000191CC
		private static void ParseWithTag(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineWithTag, "tag", new SsmlParser.ProcessPromptEngine1(engine.BeginPromptEngineWithTag), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineWithTag(ssmAttributesParent._voice, text);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001B010 File Offset: 0x00019210
		private static void ParseRule(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineRule, "name", new SsmlParser.ProcessPromptEngine1(engine.BeginPromptEngineRule), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineRule(ssmAttributesParent._voice, text);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001B054 File Offset: 0x00019254
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
				SsmlParser.ThrowFormatException(SRID.InvalidAttributeDefinedTwice, new object[] { reader.Value, stringBuilder });
			}
			dest = reader.Value;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001B0C0 File Offset: 0x000192C0
		private static int ParseCSS2Time(string time)
		{
			time = time.Trim(Helpers._achTrimChars);
			int num = time.IndexOf("ms", StringComparison.Ordinal);
			int num2 = -1;
			if (num > 0 && time.Length == num + 2)
			{
				float num3;
				if (!float.TryParse(time.Substring(0, num), out num3))
				{
					num2 = -1;
				}
				else
				{
					num2 = (int)((double)num3 + 0.5);
				}
			}
			else if ((num = time.IndexOf('s')) > 0 && time.Length == num + 1)
			{
				float num3;
				if (!float.TryParse(time.Substring(0, num), out num3))
				{
					num2 = -1;
				}
				else
				{
					num2 = (int)(num3 * 1000f);
				}
			}
			return num2;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001B154 File Offset: 0x00019354
		private static ContourPoint[] ParseContour(string contour)
		{
			char[] array = contour.ToCharArray();
			List<ContourPoint> list = new List<ContourPoint>();
			int num = 0;
			try
			{
				bool flag;
				while (num < array.Length && (num = SsmlParser.NextChar(array, num, '(', false, out flag)) >= 0)
				{
					bool flag2;
					int num2 = SsmlParser.NextChar(array, num, ',', true, out flag2);
					int num3 = SsmlParser.NextChar(array, num2, ')', true, out flag);
					ProsodyNumber prosodyNumber = default(ProsodyNumber);
					ProsodyNumber prosodyNumber2 = default(ProsodyNumber);
					if (!flag2 || !SsmlParser.TryParseNumber(contour.Substring(num, num2 - (num + 1)), ref prosodyNumber) || prosodyNumber.SsmlAttributeId == 2147483647)
					{
						return null;
					}
					bool flag3;
					if (!SsmlParser.TryParseHz(contour.Substring(num2, num3 - (num2 + 1)), ref prosodyNumber2, true, out flag3))
					{
						return null;
					}
					if (list.Count == 0)
					{
						if (prosodyNumber.Number > 0f && prosodyNumber.Number < 100f)
						{
							list.Add(new ContourPoint(0f, prosodyNumber2.Number, ContourPointChangeType.Hz));
						}
					}
					else if (list[list.Count - 1].Start > prosodyNumber.Number)
					{
						return null;
					}
					if (prosodyNumber.Number >= 0f && prosodyNumber.Number <= 1f)
					{
						list.Add(new ContourPoint(prosodyNumber.Number, prosodyNumber2.Number, flag3 ? ContourPointChangeType.Hz : ContourPointChangeType.Percentage));
					}
					num = num3;
				}
			}
			catch (InvalidOperationException)
			{
				return null;
			}
			if (list.Count < 1)
			{
				return null;
			}
			if (!list[list.Count - 1].Start.Equals(1.0))
			{
				list.Add(new ContourPoint(1f, list[list.Count - 1].Change, list[list.Count - 1].ChangeType));
			}
			return list.ToArray();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001B360 File Offset: 0x00019560
		private static int NextChar(char[] ach, int start, char expected, bool skipDigit, out bool percent)
		{
			percent = false;
			while (start < ach.Length && (ach[start] == ' ' || ach[start] == '\t' || ach[start] == '\n' || ach[start] == '\r'))
			{
				start++;
			}
			if (skipDigit)
			{
				while (start < ach.Length && ach[start] != expected)
				{
					if (!(percent = ach[start] == '%') && !char.IsDigit(ach[start]) && ach[start] != 'H' && ach[start] != 'z' && ach[start] != '.' && ach[start] != '+' && ach[start] != '-')
					{
						break;
					}
					start++;
				}
				while (start < ach.Length && (ach[start] == ' ' || ach[start] == '\t' || ach[start] == '\n' || ach[start] == '\r'))
				{
					start++;
				}
			}
			if (start < ach.Length && ach[start] == expected)
			{
				return start + 1;
			}
			if (!skipDigit && start == ach.Length)
			{
				return -1;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001B438 File Offset: 0x00019638
		private static bool ParseNumberHz(XmlReader reader, ref string attribute, string[] attributeValues, int[] attributeConst, ref ProsodyNumber number)
		{
			bool flag = false;
			SsmlParser.CheckForDuplicates(ref attribute, reader);
			int num = Array.BinarySearch<string>(attributeValues, attribute);
			if (num < 0)
			{
				bool flag2;
				if (!SsmlParser.TryParseHz(attribute, ref number, false, out flag2))
				{
					flag = true;
				}
			}
			else
			{
				number = new ProsodyNumber(attributeConst[num]);
			}
			return flag;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001B480 File Offset: 0x00019680
		private static bool ParseNumberRelative(XmlReader reader, ref string attribute, string[] attributeValues, int[] attributeConst, ref ProsodyNumber number)
		{
			bool flag = false;
			SsmlParser.CheckForDuplicates(ref attribute, reader);
			int num = Array.BinarySearch<string>(attributeValues, attribute);
			if (num < 0)
			{
				if (!SsmlParser.TryParseNumber(attribute, ref number))
				{
					flag = true;
				}
			}
			else
			{
				number = new ProsodyNumber(attributeConst[num]);
			}
			return flag;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001B4C4 File Offset: 0x000196C4
		private static bool TryParseNumber(string sNumber, ref ProsodyNumber number)
		{
			bool flag = false;
			decimal num = 0m;
			number.Unit = ProsodyUnit.Default;
			sNumber = sNumber.Trim(Helpers._achTrimChars);
			if (!string.IsNullOrEmpty(sNumber))
			{
				if (!decimal.TryParse(sNumber, out num))
				{
					if (sNumber[sNumber.Length - 1] == '%' && decimal.TryParse(sNumber.Substring(0, sNumber.Length - 1), out num))
					{
						float num2 = (float)num / 100f;
						if (sNumber[0] != '+' && sNumber[0] != '-')
						{
							number.Number *= num2;
						}
						else
						{
							number.Number += number.Number * num2;
						}
						flag = true;
					}
				}
				else
				{
					if (sNumber[0] != '+' && sNumber[0] != '-')
					{
						number.Number = (float)num;
						number.SsmlAttributeId = int.MaxValue;
					}
					else if (number.IsNumberPercent)
					{
						number.Number *= (float)num;
					}
					else
					{
						number.Number += (float)num;
					}
					number.IsNumberPercent = false;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001B5F0 File Offset: 0x000197F0
		private static bool TryParseHz(string sNumber, ref ProsodyNumber number, bool acceptHzRelative, out bool isHz)
		{
			isHz = false;
			number.SsmlAttributeId = int.MaxValue;
			ProsodyUnit prosodyUnit = ProsodyUnit.Default;
			sNumber = sNumber.Trim(Helpers._achTrimChars);
			if (sNumber.IndexOf("Hz", StringComparison.Ordinal) == sNumber.Length - 2)
			{
				prosodyUnit = ProsodyUnit.Hz;
			}
			else if (sNumber.IndexOf("st", StringComparison.Ordinal) == sNumber.Length - 2)
			{
				prosodyUnit = ProsodyUnit.Semitone;
			}
			bool flag;
			if (prosodyUnit != ProsodyUnit.Default)
			{
				flag = SsmlParser.TryParseNumber(sNumber.Substring(0, sNumber.Length - 2), ref number) && (acceptHzRelative || number.SsmlAttributeId == int.MaxValue);
				isHz = true;
			}
			else
			{
				flag = SsmlParser.TryParseNumber(sNumber, ref number) && number.SsmlAttributeId == int.MaxValue;
			}
			return flag;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001B69F File Offset: 0x0001989F
		private static string ValidateElement(SsmlElement possibleElements, SsmlElement currentElement, string sElement)
		{
			if ((possibleElements & currentElement) == (SsmlElement)0)
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { sElement });
			}
			return sElement;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001B6BB File Offset: 0x000198BB
		private static void ThrowFormatException(SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args));
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001B6C9 File Offset: 0x000198C9
		private static void ThrowFormatException(Exception innerException, SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args), innerException);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001B6D8 File Offset: 0x000198D8
		private static void NoOp(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmlAttributes, bool fIgnore)
		{
			SsmlParser.ProcessElement(reader, engine, null, (SsmlElement)0, ssmlAttributes, true, null);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001B6E6 File Offset: 0x000198E6
		private static SsmlElement ElementPromptEngine(SsmlParser.SsmlAttributes ssmlAttributes)
		{
			if (ssmlAttributes._cPromptOutput <= 0)
			{
				return (SsmlElement)0;
			}
			return SsmlElement.PromptEngineChildren;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001B6F8 File Offset: 0x000198F8
		private static int GetColumnPosition(XmlReader reader)
		{
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader == null)
			{
				return 0;
			}
			return xmlTextReader.LinePosition - 1;
		}

		// Token: 0x040004FE RID: 1278
		private static readonly string[] _elementsName = new string[]
		{
			"audio", "break", "database", "desc", "div", "emphasis", "id", "lexicon", "mark", "meta",
			"metadata", "p", "paragraph", "phoneme", "prompt_output", "prosody", "rule", "s", "say-as", "sentence",
			"speak", "sub", "tts", "voice", "withtag"
		};

		// Token: 0x040004FF RID: 1279
		private static readonly SsmlParser.ParseElementDelegates[] _parseElements = new SsmlParser.ParseElementDelegates[]
		{
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseAudio),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseBreak),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseDatabase),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseDesc),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseDiv),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseEmphasis),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseId),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseLexicon),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseMark),
			new SsmlParser.ParseElementDelegates(SsmlParser.NoOp),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseMetaData),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseParagraph),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseParagraph),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParsePhoneme),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParsePromptOutput),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseProsody),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseRule),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseSentence),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseSayAs),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseSentence),
			new SsmlParser.ParseElementDelegates(SsmlParser.NoOp),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseSub),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseTts),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseVoice),
			new SsmlParser.ParseElementDelegates(SsmlParser.ParseWithTag)
		};

		// Token: 0x04000500 RID: 1280
		private static readonly string[] _breakStrength = new string[] { "medium", "none", "strong", "weak", "x-strong", "x-weak" };

		// Token: 0x04000501 RID: 1281
		private static readonly EmphasisBreak[] _breakEmphasis = new EmphasisBreak[]
		{
			EmphasisBreak.Medium,
			EmphasisBreak.None,
			EmphasisBreak.Strong,
			EmphasisBreak.Weak,
			EmphasisBreak.ExtraStrong,
			EmphasisBreak.ExtraWeak
		};

		// Token: 0x04000502 RID: 1282
		private static readonly string[] _emphasisNames = new string[] { "moderate", "none", "reduced", "strong" };

		// Token: 0x04000503 RID: 1283
		private static readonly EmphasisWord[] _emphasisWord = new EmphasisWord[]
		{
			EmphasisWord.Moderate,
			EmphasisWord.None,
			EmphasisWord.Reduced,
			EmphasisWord.Strong
		};

		// Token: 0x04000504 RID: 1284
		private static readonly int[] _pitchWords = new int[] { 0, 4, 2, 3, 5, 1 };

		// Token: 0x04000505 RID: 1285
		private static readonly string[] _pitchNames = new string[] { "default", "high", "low", "medium", "x-high", "x-low" };

		// Token: 0x04000506 RID: 1286
		private static readonly int[] _rangeWords = new int[] { 0, 4, 2, 3, 5, 1 };

		// Token: 0x04000507 RID: 1287
		private static readonly string[] _rangeNames = new string[] { "default", "high", "low", "medium", "x-high", "x-low" };

		// Token: 0x04000508 RID: 1288
		private static readonly int[] _rateWords = new int[] { 0, 4, 3, 2, 5, 1 };

		// Token: 0x04000509 RID: 1289
		private static readonly string[] _rateNames = new string[] { "default", "fast", "medium", "slow", "x-fast", "x-slow" };

		// Token: 0x0400050A RID: 1290
		private static readonly int[] _volumeWords = new int[] { -1, -6, -5, -2, -4, -7, -3 };

		// Token: 0x0400050B RID: 1291
		private static readonly string[] _volumeNames = new string[] { "default", "loud", "medium", "silent", "soft", "x-loud", "x-soft" };

		// Token: 0x0400050C RID: 1292
		private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x0400050D RID: 1293
		private const string xmlNamespaceSsml = "http://www.w3.org/2001/10/synthesis";

		// Token: 0x0400050E RID: 1294
		private const string xmlNamespaceXmlns = "http://www.w3.org/2000/xmlns/";

		// Token: 0x0400050F RID: 1295
		private const string xmlNamespacePrompt = "http://schemas.microsoft.com/Speech/2003/03/PromptEngine";

		// Token: 0x02000199 RID: 409
		// (Invoke) Token: 0x06000B98 RID: 2968
		private delegate bool ProcessPromptEngine0(object voice);

		// Token: 0x0200019A RID: 410
		// (Invoke) Token: 0x06000B9C RID: 2972
		private delegate bool ProcessPromptEngine1(object voice, string value);

		// Token: 0x0200019B RID: 411
		private struct SsmlAttributes
		{
			// Token: 0x06000B9F RID: 2975 RVA: 0x0002DC58 File Offset: 0x0002BE58
			internal bool AddUnknowAttribute(SsmlXmlAttribute attribute, ref List<SsmlXmlAttribute> extraAttributes)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute in this._unknownNamespaces)
				{
					if (ssmlXmlAttribute._name == attribute._prefix)
					{
						if (extraAttributes == null)
						{
							extraAttributes = new List<SsmlXmlAttribute>();
						}
						extraAttributes.Add(attribute);
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000BA0 RID: 2976 RVA: 0x0002DCD4 File Offset: 0x0002BED4
			internal bool AddUnknowAttribute(XmlReader reader, ref List<SsmlXmlAttribute> extraAttributes)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute in this._unknownNamespaces)
				{
					if (ssmlXmlAttribute._name == reader.Prefix)
					{
						if (extraAttributes == null)
						{
							extraAttributes = new List<SsmlXmlAttribute>();
						}
						extraAttributes.Add(new SsmlXmlAttribute(reader.Prefix, reader.LocalName, reader.Value, reader.NamespaceURI));
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000BA1 RID: 2977 RVA: 0x0002DD6C File Offset: 0x0002BF6C
			internal bool IsOtherNamespaceElement(XmlReader reader)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute in this._unknownNamespaces)
				{
					if (ssmlXmlAttribute._name == reader.Prefix)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0400094A RID: 2378
			internal object _voice;

			// Token: 0x0400094B RID: 2379
			internal FragmentState _fragmentState;

			// Token: 0x0400094C RID: 2380
			internal bool _fRenderDesc;

			// Token: 0x0400094D RID: 2381
			internal VoiceAge _age;

			// Token: 0x0400094E RID: 2382
			internal VoiceGender _gender;

			// Token: 0x0400094F RID: 2383
			internal string _baseUri;

			// Token: 0x04000950 RID: 2384
			internal short _cPromptOutput;

			// Token: 0x04000951 RID: 2385
			internal List<SsmlXmlAttribute> _unknownNamespaces;
		}

		// Token: 0x0200019C RID: 412
		// (Invoke) Token: 0x06000BA3 RID: 2979
		private delegate void ParseElementDelegates(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmlAttributes, bool fIgnore);
	}
}
