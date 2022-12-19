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
	// Token: 0x020000F6 RID: 246
	internal static class SsmlParser
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x00017FD4 File Offset: 0x00016FD4
		internal static void Parse(string ssml, ISsmlParser engine, object voice)
		{
			string text = ssml.Replace('\n', ' ');
			text = text.Replace('\r', ' ');
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(text));
			SsmlParser.Parse(xmlTextReader, engine, voice);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001800C File Offset: 0x0001700C
		internal static void Parse(XmlReader reader, ISsmlParser engine, object voice)
		{
			try
			{
				bool flag = false;
				while (reader.Read())
				{
					if (reader.NodeType == 1 && reader.LocalName == "speak")
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

		// Token: 0x0600058A RID: 1418 RVA: 0x00018098 File Offset: 0x00017098
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "version")
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
					string localName2;
					if ((localName2 = reader.LocalName) != null)
					{
						if (!(localName2 == "lang"))
						{
							if (!(localName2 == "base"))
							{
								goto IL_184;
							}
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							try
							{
								cultureInfo = new CultureInfo(text2);
								goto IL_231;
							}
							catch (ArgumentException ex2)
							{
								if (string.Compare("es-us", text2, 5) == 0)
								{
									Helpers.CombineCulture("es-ES", "en-US", new CultureInfo("es"), 21514);
									cultureInfo = new CultureInfo(text2);
								}
								else
								{
									ex = ex2;
									int num = reader.Value.IndexOf("-", 4);
									if (num > 0)
									{
										try
										{
											cultureInfo = new CultureInfo(reader.Value.Substring(0, num));
											goto IL_172;
										}
										catch (ArgumentException)
										{
											flag = true;
											goto IL_172;
										}
									}
									flag = true;
								}
								IL_172:
								goto IL_231;
							}
						}
						SsmlParser.CheckForDuplicates(ref text3, reader);
						goto IL_231;
					}
					IL_184:
					flag = true;
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
				IL_231:
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

		// Token: 0x0600058B RID: 1419 RVA: 0x0001841C File Offset: 0x0001741C
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
				for (;;)
				{
					XmlNodeType nodeType = reader.NodeType;
					switch (nodeType)
					{
					case 1:
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
								break;
							}
							SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
						}
						reader.Read();
						break;
					}
					case 2:
						goto IL_135;
					case 3:
						if ((possibleElements & SsmlElement.Text) != (SsmlElement)0)
						{
							engine.ProcessText(reader.Value, ssmlAttributes._voice, ref ssmlAttributes._fragmentState, SsmlParser.GetColumnPosition(reader), fIgnore);
						}
						else
						{
							SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
						}
						reader.Read();
						break;
					default:
						if (nodeType != 15)
						{
							goto IL_135;
						}
						break;
					}
					IL_13C:
					if (reader.NodeType == 15 || reader.NodeType == null)
					{
						break;
					}
					continue;
					IL_135:
					reader.Read();
					goto IL_13C;
				}
			}
			if (extraAttributes != null && extraAttributes.Count > 0)
			{
				engine.EndProcessUnknownAttributes(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, sElement, extraAttributes);
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000185A0 File Offset: 0x000175A0
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "src")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							engine.ProcessAudio(ssmlAttributes._voice, text2, ssmlAttributes._baseUri, fIgnore);
							goto IL_84;
						}
						catch (IOException)
						{
							flag = true;
							goto IL_84;
						}
						catch (WebException)
						{
							flag = true;
							goto IL_84;
						}
					}
					flag2 = true;
				}
				IL_84:
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

		// Token: 0x0600058D RID: 1421 RVA: 0x000186B4 File Offset: 0x000176B4
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "time")
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							ssmlAttributes._fragmentState.Emphasis = -1;
							ssmlAttributes._fragmentState.Duration = SsmlParser.ParseCSS2Time(text2);
							flag = ssmlAttributes._fragmentState.Duration < 0;
							goto IL_11E;
						}
						if (localName == "strength")
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							if (text2 != null)
							{
								goto IL_11E;
							}
							ssmlAttributes._fragmentState.Duration = 0;
							int num = Array.BinarySearch<string>(SsmlParser._breakStrength, text3);
							if (num < 0)
							{
								flag = true;
								goto IL_11E;
							}
							if (ssmlAttributes._fragmentState.Emphasis != -1)
							{
								ssmlAttributes._fragmentState.Emphasis = (int)SsmlParser._breakEmphasis[num];
								goto IL_11E;
							}
							goto IL_11E;
						}
					}
					flag = true;
				}
				IL_11E:
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidSpeakAttribute, new object[] { reader.Name, "break" });
				}
			}
			engine.ProcessBreak(ssmlAttributes._voice, ref ssmlAttributes._fragmentState, (EmphasisBreak)ssmlAttributes._fragmentState.Emphasis, ssmlAttributes._fragmentState.Duration, fIgnore);
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00018864 File Offset: 0x00017864
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "lang")
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

		// Token: 0x0600058F RID: 1423 RVA: 0x0001895C File Offset: 0x0001795C
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "level")
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

		// Token: 0x06000590 RID: 1424 RVA: 0x00018A7C File Offset: 0x00017A7C
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "name")
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

		// Token: 0x06000591 RID: 1425 RVA: 0x00018B80 File Offset: 0x00017B80
		private static void ParseMetaData(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ValidateElement(element, SsmlElement.MetaData, reader.Name);
			if (!reader.IsEmptyElement)
			{
				int num = 1;
				do
				{
					reader.Read();
					if (reader.NodeType == 1)
					{
						num++;
					}
					if (reader.NodeType == 15 || reader.NodeType == null)
					{
						num--;
					}
				}
				while (num > 0);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00018BD4 File Offset: 0x00017BD4
		private static void ParseParagraph(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Paragraph, reader.Name);
			SsmlParser.ParseTextBlock(reader, engine, true, text, ssmAttributesParent, fIgnore);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00018C00 File Offset: 0x00017C00
		private static void ParseSentence(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.Sentence, reader.Name);
			SsmlParser.ParseTextBlock(reader, engine, false, text, ssmAttributesParent, fIgnore);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00018C28 File Offset: 0x00017C28
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "lang")
					{
						SsmlParser.CheckForDuplicates(ref text, reader);
						try
						{
							cultureInfo = new CultureInfo(text);
							goto IL_5C;
						}
						catch (ArgumentException)
						{
							flag = true;
							goto IL_5C;
						}
					}
					flag = true;
				}
				IL_5C:
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

		// Token: 0x06000595 RID: 1429 RVA: 0x00018D6C File Offset: 0x00017D6C
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "alphabet")
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							string text4;
							if ((text4 = text2) != null)
							{
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000585-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
									dictionary.Add("ipa", 0);
									dictionary.Add("sapi", 1);
									dictionary.Add("x-sapi", 2);
									dictionary.Add("x-microsoft-sapi", 3);
									dictionary.Add("ups", 4);
									dictionary.Add("x-ups", 5);
									dictionary.Add("x-microsoft-ups", 6);
									<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000585-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000585-1.TryGetValue(text4, ref num))
								{
									switch (num)
									{
									case 0:
										alphabetType = AlphabetType.Ipa;
										goto IL_15E;
									case 1:
									case 2:
									case 3:
										alphabetType = AlphabetType.Sapi;
										goto IL_15E;
									case 4:
									case 5:
									case 6:
										alphabetType = AlphabetType.Ups;
										goto IL_15E;
									}
								}
							}
							throw new FormatException(SR.Get(SRID.UnsupportedAlphabet, new object[0]));
						}
						if (localName == "ph")
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							goto IL_15E;
						}
					}
					flag = true;
				}
				IL_15E:
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
					goto IL_247;
				case AlphabetType.Ups:
					array = PhonemeConverter.UpsConverter.ConvertPronToId(text3).ToCharArray();
					alphabetType = AlphabetType.Ipa;
					goto IL_247;
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
				IL_247:;
			}
			catch (FormatException)
			{
				SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { "phoneme" });
			}
			engine.ProcessPhoneme(ref ssmlAttributes._fragmentState, alphabetType, text3, array);
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.Text, ssmlAttributes, fIgnore, list);
			engine.EndElement();
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00019028 File Offset: 0x00018028
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000586-1 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
							dictionary.Add("pitch", 0);
							dictionary.Add("range", 1);
							dictionary.Add("rate", 2);
							dictionary.Add("volume", 3);
							dictionary.Add("duration", 4);
							dictionary.Add("contour", 5);
							<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000586-1 = dictionary;
						}
						int num;
						if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x6000586-1.TryGetValue(localName, ref num))
						{
							switch (num)
							{
							case 0:
								flag = SsmlParser.ParseNumberHz(reader, ref text2, SsmlParser._pitchNames, SsmlParser._pitchWords, ref prosody._pitch);
								goto IL_1D1;
							case 1:
								flag = SsmlParser.ParseNumberHz(reader, ref text4, SsmlParser._rangeNames, SsmlParser._rangeWords, ref prosody._range);
								goto IL_1D1;
							case 2:
								flag = SsmlParser.ParseNumberRelative(reader, ref text5, SsmlParser._rateNames, SsmlParser._rateWords, ref prosody._rate);
								goto IL_1D1;
							case 3:
								flag = SsmlParser.ParseNumberRelative(reader, ref text7, SsmlParser._volumeNames, SsmlParser._volumeWords, ref prosody._volume);
								goto IL_1D1;
							case 4:
								SsmlParser.CheckForDuplicates(ref text6, reader);
								prosody.Duration = SsmlParser.ParseCSS2Time(text6);
								goto IL_1D1;
							case 5:
								SsmlParser.CheckForDuplicates(ref text3, reader);
								prosody.SetContourPoints(SsmlParser.ParseContour(text3));
								if (prosody.GetContourPoints() == null)
								{
									flag = true;
									goto IL_1D1;
								}
								goto IL_1D1;
							}
						}
					}
					flag = true;
				}
				IL_1D1:
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

		// Token: 0x06000597 RID: 1431 RVA: 0x000192D8 File Offset: 0x000182D8
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "type" || localName == "interpret-as")
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							sayAs.InterpretAs = text2;
							goto IL_CC;
						}
						if (localName == "format")
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							sayAs.Format = text3;
							goto IL_CC;
						}
						if (localName == "detail")
						{
							SsmlParser.CheckForDuplicates(ref text4, reader);
							sayAs.Detail = text4;
							goto IL_CC;
						}
					}
					flag = true;
				}
				IL_CC:
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

		// Token: 0x06000598 RID: 1432 RVA: 0x00019448 File Offset: 0x00018448
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
					string localName;
					if ((localName = reader.LocalName) != null && localName == "alias")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						XmlTextReader xmlTextReader = reader as XmlTextReader;
						if (xmlTextReader != null && engine.Ssml != null)
						{
							num = engine.Ssml.IndexOf(reader.Value, xmlTextReader.LinePosition + reader.LocalName.Length, 4);
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

		// Token: 0x06000599 RID: 1433 RVA: 0x00019588 File Offset: 0x00018588
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (!(localName == "gender"))
						{
							if (!(localName == "age"))
							{
								if (!(localName == "variant"))
								{
									if (localName == "name")
									{
										SsmlParser.CheckForDuplicates(ref text5, reader);
										goto IL_255;
									}
								}
								else
								{
									SsmlParser.CheckForDuplicates(ref text4, reader);
									if (!int.TryParse(text4, ref num) || num <= 0)
									{
										flag = true;
										goto IL_255;
									}
									goto IL_255;
								}
							}
							else
							{
								SsmlParser.CheckForDuplicates(ref text6, reader);
								VoiceAge voiceAge;
								if (!SsmlParserHelpers.TryConvertAge(text6, out voiceAge))
								{
									flag = true;
									goto IL_255;
								}
								ssmlAttributes._age = voiceAge;
								goto IL_255;
							}
						}
						else
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							VoiceGender voiceGender;
							if (!SsmlParserHelpers.TryConvertGender(text3, out voiceGender))
							{
								flag = true;
								goto IL_255;
							}
							ssmlAttributes._gender = voiceGender;
							goto IL_255;
						}
					}
					flag = true;
				}
				else if (reader.Prefix == "xmlns" && reader.Value == "http://schemas.microsoft.com/Speech/2003/03/PromptEngine")
				{
					SsmlParser.CheckForDuplicates(ref text7, reader);
				}
				else if (reader.NamespaceURI == "http://www.w3.org/XML/1998/namespace")
				{
					string localName2;
					if ((localName2 = reader.LocalName) != null && localName2 == "lang")
					{
						SsmlParser.CheckForDuplicates(ref text2, reader);
						try
						{
							cultureInfo = new CultureInfo(text2);
							goto IL_255;
						}
						catch (ArgumentException)
						{
							flag = true;
							goto IL_255;
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
				IL_255:
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

		// Token: 0x0600059A RID: 1434 RVA: 0x000199B0 File Offset: 0x000189B0
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
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "uri")
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							goto IL_7B;
						}
						if (localName == "type")
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							goto IL_7B;
						}
					}
					flag = true;
				}
				IL_7B:
				if (flag && !ssmlAttributes.AddUnknowAttribute(reader, ref list))
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				SsmlParser.ThrowFormatException(SRID.MissingRequiredAttribute, new object[] { "uri", "lexicon" });
			}
			Uri uri = new Uri(text2, 0);
			if (!uri.IsAbsoluteUri && ssmlAttributes._baseUri != null)
			{
				text2 = ssmlAttributes._baseUri + '/' + text2;
				uri = new Uri(text2, 0);
			}
			engine.ProcessLexicon(uri, text3);
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, true, list);
			engine.EndElement();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00019AF4 File Offset: 0x00018AF4
		private static void ParsePromptEngine0(XmlReader reader, ISsmlParser engine, SsmlElement elementAllowed, SsmlElement element, SsmlParser.ProcessPromptEngine0 process, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(elementAllowed, element, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes = ssmAttributesParent;
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
			if (!process(ssmlAttributes._voice))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmlAttributes), ssmlAttributes, fIgnore, null);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00019BB4 File Offset: 0x00018BB4
		private static string ParsePromptEngine1(XmlReader reader, ISsmlParser engine, SsmlElement elementAllowed, SsmlElement element, string attribute, SsmlParser.ProcessPromptEngine1 process, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(elementAllowed, element, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes = ssmAttributesParent;
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
			if (!process(ssmlAttributes._voice, text2))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, SsmlElement.AudioMarkTextWithStyle | SsmlParser.ElementPromptEngine(ssmlAttributes), ssmlAttributes, fIgnore, null);
			return text2;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00019C64 File Offset: 0x00018C64
		private static void ParsePromptOutput(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			ssmAttributesParent._cPromptOutput += 1;
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineOutput, new SsmlParser.ProcessPromptEngine0(engine.BeginPromptEngineOutput), ssmAttributesParent, fIgnore);
			engine.EndElement();
			ssmAttributesParent._cPromptOutput -= 1;
			engine.EndPromptEngineOutput(ssmAttributesParent._voice);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00019CC1 File Offset: 0x00018CC1
		private static void ParseDiv(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineDiv, new SsmlParser.ProcessPromptEngine0(engine.ProcessPromptEngineDiv), ssmAttributesParent, fIgnore);
			engine.EndElement();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00019CE8 File Offset: 0x00018CE8
		private static void ParseDatabase(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ValidateElement(element, SsmlElement.PromptEngineDatabase, reader.Name);
			SsmlParser.SsmlAttributes ssmlAttributes = default(SsmlParser.SsmlAttributes);
			ssmlAttributes = ssmAttributesParent;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			while (reader.MoveToNextAttribute())
			{
				bool flag = false;
				if (!flag)
				{
					string localName;
					if ((localName = reader.LocalName) != null)
					{
						if (localName == "fname")
						{
							SsmlParser.CheckForDuplicates(ref text2, reader);
							goto IL_87;
						}
						if (localName == "idset")
						{
							SsmlParser.CheckForDuplicates(ref text4, reader);
							goto IL_87;
						}
						if (localName == "delta")
						{
							SsmlParser.CheckForDuplicates(ref text3, reader);
							goto IL_87;
						}
					}
					flag = true;
				}
				IL_87:
				if (flag)
				{
					SsmlParser.ThrowFormatException(SRID.InvalidItemAttribute, new object[] { reader.Name });
				}
			}
			if (!engine.ProcessPromptEngineDatabase(ssmlAttributes._voice, text2, text3, text4))
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { reader.Name });
			}
			SsmlParser.ProcessElement(reader, engine, text, (SsmlElement)0, ssmlAttributes, fIgnore, null);
			engine.EndElement();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00019DED File Offset: 0x00018DED
		private static void ParseId(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineId, "id", new SsmlParser.ProcessPromptEngine1(engine.ProcessPromptEngineId), ssmAttributesParent, fIgnore);
			engine.EndElement();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00019E18 File Offset: 0x00018E18
		private static void ParseTts(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			SsmlParser.ParsePromptEngine0(reader, engine, element, SsmlElement.PromptEngineTTS, new SsmlParser.ProcessPromptEngine0(engine.BeginPromptEngineTts), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineTts(ssmAttributesParent._voice);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00019E4C File Offset: 0x00018E4C
		private static void ParseWithTag(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineWithTag, "tag", new SsmlParser.ProcessPromptEngine1(engine.BeginPromptEngineWithTag), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineWithTag(ssmAttributesParent._voice, text);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00019E90 File Offset: 0x00018E90
		private static void ParseRule(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmAttributesParent, bool fIgnore)
		{
			string text = SsmlParser.ParsePromptEngine1(reader, engine, element, SsmlElement.PromptEngineRule, "name", new SsmlParser.ProcessPromptEngine1(engine.BeginPromptEngineRule), ssmAttributesParent, fIgnore);
			engine.EndElement();
			engine.EndPromptEngineRule(ssmAttributesParent._voice, text);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00019ED4 File Offset: 0x00018ED4
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

		// Token: 0x060005A5 RID: 1445 RVA: 0x00019F44 File Offset: 0x00018F44
		private static int ParseCSS2Time(string time)
		{
			time = time.Trim(Helpers._achTrimChars);
			int num = time.IndexOf("ms", 4);
			int num2 = -1;
			if (num > 0 && time.Length == num + 2)
			{
				float num3;
				if (!float.TryParse(time.Substring(0, num), ref num3))
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
				if (!float.TryParse(time.Substring(0, num), ref num3))
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

		// Token: 0x060005A6 RID: 1446 RVA: 0x00019FD8 File Offset: 0x00018FD8
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

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001A1E4 File Offset: 0x000191E4
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

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001A2BC File Offset: 0x000192BC
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

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001A304 File Offset: 0x00019304
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

		// Token: 0x060005AA RID: 1450 RVA: 0x0001A348 File Offset: 0x00019348
		private static bool TryParseNumber(string sNumber, ref ProsodyNumber number)
		{
			bool flag = false;
			decimal num = 0m;
			number.Unit = ProsodyUnit.Default;
			sNumber = sNumber.Trim(Helpers._achTrimChars);
			if (!string.IsNullOrEmpty(sNumber))
			{
				if (!decimal.TryParse(sNumber, ref num))
				{
					if (sNumber.get_Chars(sNumber.Length - 1) == '%' && decimal.TryParse(sNumber.Substring(0, sNumber.Length - 1), ref num))
					{
						float num2 = (float)num / 100f;
						if (sNumber.get_Chars(0) != '+' && sNumber.get_Chars(0) != '-')
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
					if (sNumber.get_Chars(0) != '+' && sNumber.get_Chars(0) != '-')
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

		// Token: 0x060005AB RID: 1451 RVA: 0x0001A470 File Offset: 0x00019470
		private static bool TryParseHz(string sNumber, ref ProsodyNumber number, bool acceptHzRelative, out bool isHz)
		{
			isHz = false;
			number.SsmlAttributeId = int.MaxValue;
			ProsodyUnit prosodyUnit = ProsodyUnit.Default;
			sNumber = sNumber.Trim(Helpers._achTrimChars);
			if (sNumber.IndexOf("Hz", 4) == sNumber.Length - 2)
			{
				prosodyUnit = ProsodyUnit.Hz;
			}
			else if (sNumber.IndexOf("st", 4) == sNumber.Length - 2)
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

		// Token: 0x060005AC RID: 1452 RVA: 0x0001A520 File Offset: 0x00019520
		private static string ValidateElement(SsmlElement possibleElements, SsmlElement currentElement, string sElement)
		{
			if ((possibleElements & currentElement) == (SsmlElement)0)
			{
				SsmlParser.ThrowFormatException(SRID.InvalidElement, new object[] { sElement });
			}
			return sElement;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001A549 File Offset: 0x00019549
		private static void ThrowFormatException(SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args));
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001A557 File Offset: 0x00019557
		private static void ThrowFormatException(Exception innerException, SRID id, params object[] args)
		{
			throw new FormatException(SR.Get(id, args), innerException);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001A566 File Offset: 0x00019566
		private static void NoOp(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmlAttributes, bool fIgnore)
		{
			SsmlParser.ProcessElement(reader, engine, null, (SsmlElement)0, ssmlAttributes, true, null);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001A574 File Offset: 0x00019574
		private static SsmlElement ElementPromptEngine(SsmlParser.SsmlAttributes ssmlAttributes)
		{
			if (ssmlAttributes._cPromptOutput <= 0)
			{
				return (SsmlElement)0;
			}
			return SsmlElement.PromptEngineChildren;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001A588 File Offset: 0x00019588
		private static int GetColumnPosition(XmlReader reader)
		{
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader == null)
			{
				return 0;
			}
			return xmlTextReader.LinePosition - 1;
		}

		// Token: 0x0400047F RID: 1151
		private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x04000480 RID: 1152
		private const string xmlNamespaceSsml = "http://www.w3.org/2001/10/synthesis";

		// Token: 0x04000481 RID: 1153
		private const string xmlNamespaceXmlns = "http://www.w3.org/2000/xmlns/";

		// Token: 0x04000482 RID: 1154
		private const string xmlNamespacePrompt = "http://schemas.microsoft.com/Speech/2003/03/PromptEngine";

		// Token: 0x04000483 RID: 1155
		private static readonly string[] _elementsName = new string[]
		{
			"audio", "break", "database", "desc", "div", "emphasis", "id", "lexicon", "mark", "meta",
			"metadata", "p", "paragraph", "phoneme", "prompt_output", "prosody", "rule", "s", "say-as", "sentence",
			"speak", "sub", "tts", "voice", "withtag"
		};

		// Token: 0x04000484 RID: 1156
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

		// Token: 0x04000485 RID: 1157
		private static readonly string[] _breakStrength = new string[] { "medium", "none", "strong", "weak", "x-strong", "x-weak" };

		// Token: 0x04000486 RID: 1158
		private static readonly EmphasisBreak[] _breakEmphasis = new EmphasisBreak[]
		{
			EmphasisBreak.Medium,
			EmphasisBreak.None,
			EmphasisBreak.Strong,
			EmphasisBreak.Weak,
			EmphasisBreak.ExtraStrong,
			EmphasisBreak.ExtraWeak
		};

		// Token: 0x04000487 RID: 1159
		private static readonly string[] _emphasisNames = new string[] { "moderate", "none", "reduced", "strong" };

		// Token: 0x04000488 RID: 1160
		private static readonly EmphasisWord[] _emphasisWord = new EmphasisWord[]
		{
			EmphasisWord.Moderate,
			EmphasisWord.None,
			EmphasisWord.Reduced,
			EmphasisWord.Strong
		};

		// Token: 0x04000489 RID: 1161
		private static readonly int[] _pitchWords = new int[] { 0, 4, 2, 3, 5, 1 };

		// Token: 0x0400048A RID: 1162
		private static readonly string[] _pitchNames = new string[] { "default", "high", "low", "medium", "x-high", "x-low" };

		// Token: 0x0400048B RID: 1163
		private static readonly int[] _rangeWords = new int[] { 0, 4, 2, 3, 5, 1 };

		// Token: 0x0400048C RID: 1164
		private static readonly string[] _rangeNames = new string[] { "default", "high", "low", "medium", "x-high", "x-low" };

		// Token: 0x0400048D RID: 1165
		private static readonly int[] _rateWords = new int[] { 0, 4, 3, 2, 5, 1 };

		// Token: 0x0400048E RID: 1166
		private static readonly string[] _rateNames = new string[] { "default", "fast", "medium", "slow", "x-fast", "x-slow" };

		// Token: 0x0400048F RID: 1167
		private static readonly int[] _volumeWords = new int[] { -1, -6, -5, -2, -4, -7, -3 };

		// Token: 0x04000490 RID: 1168
		private static readonly string[] _volumeNames = new string[] { "default", "loud", "medium", "silent", "soft", "x-loud", "x-soft" };

		// Token: 0x020000F7 RID: 247
		// (Invoke) Token: 0x060005B4 RID: 1460
		private delegate bool ProcessPromptEngine0(object voice);

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x060005B8 RID: 1464
		private delegate bool ProcessPromptEngine1(object voice, string value);

		// Token: 0x020000F9 RID: 249
		private struct SsmlAttributes
		{
			// Token: 0x060005BB RID: 1467 RVA: 0x0001AAD0 File Offset: 0x00019AD0
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

			// Token: 0x060005BC RID: 1468 RVA: 0x0001AB4C File Offset: 0x00019B4C
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

			// Token: 0x060005BD RID: 1469 RVA: 0x0001ABE4 File Offset: 0x00019BE4
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

			// Token: 0x04000491 RID: 1169
			internal object _voice;

			// Token: 0x04000492 RID: 1170
			internal FragmentState _fragmentState;

			// Token: 0x04000493 RID: 1171
			internal bool _fRenderDesc;

			// Token: 0x04000494 RID: 1172
			internal VoiceAge _age;

			// Token: 0x04000495 RID: 1173
			internal VoiceGender _gender;

			// Token: 0x04000496 RID: 1174
			internal string _baseUri;

			// Token: 0x04000497 RID: 1175
			internal short _cPromptOutput;

			// Token: 0x04000498 RID: 1176
			internal List<SsmlXmlAttribute> _unknownNamespaces;
		}

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x060005BF RID: 1471
		private delegate void ParseElementDelegates(XmlReader reader, ISsmlParser engine, SsmlElement element, SsmlParser.SsmlAttributes ssmlAttributes, bool fIgnore);
	}
}
