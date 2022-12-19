using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000FE RID: 254
	internal class TextWriterEngine : ISsmlParser
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0001B25B File Offset: 0x0001A25B
		internal TextWriterEngine(XmlTextWriter writer, CultureInfo culture)
		{
			this._writer = writer;
			this._culture = culture;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001B274 File Offset: 0x0001A274
		public object ProcessSpeak(string sVersion, string baseUri, CultureInfo culture, List<SsmlXmlAttribute> extraNamespace)
		{
			if (!string.IsNullOrEmpty(baseUri))
			{
				throw new ArgumentException(SR.Get(SRID.InvalidSpeakAttribute, new object[] { "baseUri", "speak" }), "baseUri");
			}
			if ((culture != null && !culture.Equals(this._culture)) || !string.IsNullOrEmpty(this._pexmlPrefix) || extraNamespace.Count > 0)
			{
				this._writer.WriteStartElement("voice");
				this._writer.WriteAttributeString("xml", "lang", null, (culture != null) ? culture.Name : this._culture.Name);
				foreach (SsmlXmlAttribute ssmlXmlAttribute in extraNamespace)
				{
					this._writer.WriteAttributeString("xmlns", ssmlXmlAttribute._name, ssmlXmlAttribute._ns, ssmlXmlAttribute._value);
				}
				if (!string.IsNullOrEmpty(this._pexmlPrefix))
				{
					this._writer.WriteAttributeString("xmlns", this._pexmlPrefix, null, "http://schemas.microsoft.com/Speech/2003/03/PromptEngine");
				}
				this._closeSpeak = true;
			}
			return null;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001B3B4 File Offset: 0x0001A3B4
		public void ProcessText(string text, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this._writer.WriteString(text);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001B3C2 File Offset: 0x0001A3C2
		public void ProcessAudio(object voice, string uri, string baseUri, bool fIgnore)
		{
			this._writer.WriteStartElement("audio");
			this._writer.WriteAttributeString("src", uri);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001B3E8 File Offset: 0x0001A3E8
		public void ProcessBreak(object voice, ref FragmentState fragmentState, EmphasisBreak eBreak, int time, bool fIgnore)
		{
			this._writer.WriteStartElement("break");
			if (time > 0 && eBreak == EmphasisBreak.None)
			{
				this._writer.WriteAttributeString("time", time.ToString(CultureInfo.InvariantCulture) + "ms");
				return;
			}
			string text = null;
			switch (eBreak)
			{
			case EmphasisBreak.ExtraStrong:
				text = "x-strong";
				break;
			case EmphasisBreak.Strong:
				text = "strong";
				break;
			case EmphasisBreak.Medium:
				text = "medium";
				break;
			case EmphasisBreak.Weak:
				text = "weak";
				break;
			case EmphasisBreak.ExtraWeak:
				text = "x-weak";
				break;
			case EmphasisBreak.None:
				text = "none";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this._writer.WriteAttributeString("strength", text);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001B4A3 File Offset: 0x0001A4A3
		public void ProcessDesc(CultureInfo culture)
		{
			this._writer.WriteStartElement("desc");
			if (culture != null)
			{
				this._writer.WriteAttributeString("xml", "lang", null, culture.Name);
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001B4D4 File Offset: 0x0001A4D4
		public void ProcessEmphasis(bool noLevel, EmphasisWord word)
		{
			this._writer.WriteStartElement("emphasis");
			if (word != EmphasisWord.Default)
			{
				this._writer.WriteAttributeString("level", word.ToString().ToLowerInvariant());
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001B509 File Offset: 0x0001A509
		public void ProcessMark(object voice, ref FragmentState fragmentState, string name, bool fIgnore)
		{
			this._writer.WriteStartElement("mark");
			this._writer.WriteAttributeString("name", name);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001B52C File Offset: 0x0001A52C
		public object ProcessTextBlock(bool isParagraph, object voice, ref FragmentState fragmentState, CultureInfo culture, bool newCulture, VoiceGender gender, VoiceAge age)
		{
			this._writer.WriteStartElement(isParagraph ? "p" : "s");
			if (culture != null)
			{
				this._writer.WriteAttributeString("xml", "lang", null, culture.Name);
			}
			return null;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001B56A File Offset: 0x0001A56A
		public void EndProcessTextBlock(bool isParagraph)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001B56C File Offset: 0x0001A56C
		public void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds)
		{
			this._writer.WriteStartElement("phoneme");
			if (alphabet != AlphabetType.Ipa)
			{
				this._writer.WriteAttributeString("alphabet", (alphabet == AlphabetType.Sapi) ? "x-microsoft-sapi" : "x-microsoft-ups");
			}
			this._writer.WriteAttributeString("ph", ph);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001B5C0 File Offset: 0x0001A5C0
		public void ProcessProsody(string pitch, string range, string rate, string volume, string duration, string points)
		{
			this._writer.WriteStartElement("prosody");
			if (!string.IsNullOrEmpty(range))
			{
				this._writer.WriteAttributeString("range", range);
			}
			if (!string.IsNullOrEmpty(rate))
			{
				this._writer.WriteAttributeString("rate", rate);
			}
			if (!string.IsNullOrEmpty(volume))
			{
				this._writer.WriteAttributeString("volume", volume);
			}
			if (!string.IsNullOrEmpty(duration))
			{
				this._writer.WriteAttributeString("duration", duration);
			}
			if (!string.IsNullOrEmpty(points))
			{
				this._writer.WriteAttributeString("range", points);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001B660 File Offset: 0x0001A660
		public void ProcessSayAs(string interpretAs, string format, string detail)
		{
			this._writer.WriteStartElement("say-as");
			this._writer.WriteAttributeString("interpret-as", interpretAs);
			if (!string.IsNullOrEmpty(format))
			{
				this._writer.WriteAttributeString("format", format);
			}
			if (!string.IsNullOrEmpty(detail))
			{
				this._writer.WriteAttributeString("detail", detail);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001B6C0 File Offset: 0x0001A6C0
		public void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this._writer.WriteStartElement("sub");
			this._writer.WriteAttributeString("alias", alias);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001B6E4 File Offset: 0x0001A6E4
		public object ProcessVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool fNewCulture, List<SsmlXmlAttribute> extraNamespace)
		{
			this._writer.WriteStartElement("voice");
			if (!string.IsNullOrEmpty(name))
			{
				this._writer.WriteAttributeString("name", name);
			}
			if (fNewCulture && culture != null)
			{
				this._writer.WriteAttributeString("xml", "lang", null, culture.Name);
			}
			if (gender != VoiceGender.NotSet)
			{
				this._writer.WriteAttributeString("gender", gender.ToString().ToLowerInvariant());
			}
			if (age != VoiceAge.NotSet)
			{
				XmlWriter writer = this._writer;
				string text = "age";
				int num = (int)age;
				writer.WriteAttributeString(text, num.ToString(CultureInfo.InvariantCulture));
			}
			if (variant > 0)
			{
				this._writer.WriteAttributeString("variant", variant.ToString(CultureInfo.InvariantCulture));
			}
			if (extraNamespace != null)
			{
				foreach (SsmlXmlAttribute ssmlXmlAttribute in extraNamespace)
				{
					this._writer.WriteAttributeString("xmlns", ssmlXmlAttribute._name, ssmlXmlAttribute._ns, ssmlXmlAttribute._value);
				}
			}
			return null;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001B808 File Offset: 0x0001A808
		public void ProcessLexicon(Uri uri, string type)
		{
			this._writer.WriteStartElement("lexicon");
			this._writer.WriteAttributeString("uri", uri.ToString());
			if (!string.IsNullOrEmpty(type))
			{
				this._writer.WriteAttributeString("type", type);
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001B854 File Offset: 0x0001A854
		public void EndElement()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001B861 File Offset: 0x0001A861
		public void EndSpeakElement()
		{
			if (this._closeSpeak)
			{
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001B876 File Offset: 0x0001A876
		public void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader)
		{
			this._writer.WriteNode(reader, false);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001B888 File Offset: 0x0001A888
		public void StartProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string sElement, List<SsmlXmlAttribute> extraAttributes)
		{
			foreach (SsmlXmlAttribute ssmlXmlAttribute in extraAttributes)
			{
				this._writer.WriteAttributeString(ssmlXmlAttribute._prefix, ssmlXmlAttribute._name, ssmlXmlAttribute._ns, ssmlXmlAttribute._value);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001B8F4 File Offset: 0x0001A8F4
		public void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string sElement, List<SsmlXmlAttribute> extraAttributes)
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001B8F6 File Offset: 0x0001A8F6
		public void ContainsPexml(string pexmlPrefix)
		{
			this._pexmlPrefix = pexmlPrefix;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001B900 File Offset: 0x0001A900
		private bool ProcessPromptEngine(string element, params KeyValuePair<string, string>[] attributes)
		{
			this._writer.WriteStartElement(this._pexmlPrefix, element, "http://schemas.microsoft.com/Speech/2003/03/PromptEngine");
			if (attributes != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in attributes)
				{
					if (keyValuePair.Value != null)
					{
						this._writer.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			return true;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001B967 File Offset: 0x0001A967
		public bool BeginPromptEngineOutput(object voice)
		{
			return this.ProcessPromptEngine("prompt_output", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001B97C File Offset: 0x0001A97C
		public bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset)
		{
			return this.ProcessPromptEngine("database", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("fname", fname),
				new KeyValuePair<string, string>("delta", delta),
				new KeyValuePair<string, string>("idset", idset)
			});
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001B9E2 File Offset: 0x0001A9E2
		public bool ProcessPromptEngineDiv(object voice)
		{
			return this.ProcessPromptEngine("div", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001B9F8 File Offset: 0x0001A9F8
		public bool ProcessPromptEngineId(object voice, string id)
		{
			return this.ProcessPromptEngine("id", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("id", id)
			});
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001BA2F File Offset: 0x0001AA2F
		public bool BeginPromptEngineTts(object voice)
		{
			return this.ProcessPromptEngine("tts", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001BA42 File Offset: 0x0001AA42
		public void EndPromptEngineTts(object voice)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001BA44 File Offset: 0x0001AA44
		public bool BeginPromptEngineWithTag(object voice, string tag)
		{
			return this.ProcessPromptEngine("withtag", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("tag", tag)
			});
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001BA7B File Offset: 0x0001AA7B
		public void EndPromptEngineWithTag(object voice, string tag)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001BA80 File Offset: 0x0001AA80
		public bool BeginPromptEngineRule(object voice, string name)
		{
			return this.ProcessPromptEngine("rule", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("name", name)
			});
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001BAB7 File Offset: 0x0001AAB7
		public void EndPromptEngineRule(object voice, string name)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001BAB9 File Offset: 0x0001AAB9
		public void EndPromptEngineOutput(object voice)
		{
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001BABB File Offset: 0x0001AABB
		public string Ssml
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040004BD RID: 1213
		private const string xmlNamespacePrompt = "http://schemas.microsoft.com/Speech/2003/03/PromptEngine";

		// Token: 0x040004BE RID: 1214
		private XmlTextWriter _writer;

		// Token: 0x040004BF RID: 1215
		private CultureInfo _culture;

		// Token: 0x040004C0 RID: 1216
		private bool _closeSpeak;

		// Token: 0x040004C1 RID: 1217
		private string _pexmlPrefix;
	}
}
