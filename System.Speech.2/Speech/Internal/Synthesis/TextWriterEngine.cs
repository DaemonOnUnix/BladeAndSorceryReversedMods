using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C6 RID: 198
	internal class TextWriterEngine : ISsmlParser
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0001C13F File Offset: 0x0001A33F
		internal TextWriterEngine(XmlTextWriter writer, CultureInfo culture)
		{
			this._writer = writer;
			this._culture = culture;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001C158 File Offset: 0x0001A358
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

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001C298 File Offset: 0x0001A498
		public void ProcessText(string text, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this._writer.WriteString(text);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001C2A6 File Offset: 0x0001A4A6
		public void ProcessAudio(object voice, string uri, string baseUri, bool fIgnore)
		{
			this._writer.WriteStartElement("audio");
			this._writer.WriteAttributeString("src", uri);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001C2CC File Offset: 0x0001A4CC
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

		// Token: 0x060006BB RID: 1723 RVA: 0x0001C385 File Offset: 0x0001A585
		public void ProcessDesc(CultureInfo culture)
		{
			this._writer.WriteStartElement("desc");
			if (culture != null)
			{
				this._writer.WriteAttributeString("xml", "lang", null, culture.Name);
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001C3B6 File Offset: 0x0001A5B6
		public void ProcessEmphasis(bool noLevel, EmphasisWord word)
		{
			this._writer.WriteStartElement("emphasis");
			if (word != EmphasisWord.Default)
			{
				this._writer.WriteAttributeString("level", word.ToString().ToLowerInvariant());
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001C3ED File Offset: 0x0001A5ED
		public void ProcessMark(object voice, ref FragmentState fragmentState, string name, bool fIgnore)
		{
			this._writer.WriteStartElement("mark");
			this._writer.WriteAttributeString("name", name);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001C410 File Offset: 0x0001A610
		public object ProcessTextBlock(bool isParagraph, object voice, ref FragmentState fragmentState, CultureInfo culture, bool newCulture, VoiceGender gender, VoiceAge age)
		{
			this._writer.WriteStartElement(isParagraph ? "p" : "s");
			if (culture != null)
			{
				this._writer.WriteAttributeString("xml", "lang", null, culture.Name);
			}
			return null;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndProcessTextBlock(bool isParagraph)
		{
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001C450 File Offset: 0x0001A650
		public void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds)
		{
			this._writer.WriteStartElement("phoneme");
			if (alphabet != AlphabetType.Ipa)
			{
				this._writer.WriteAttributeString("alphabet", (alphabet == AlphabetType.Sapi) ? "x-microsoft-sapi" : "x-microsoft-ups");
			}
			this._writer.WriteAttributeString("ph", ph);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
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

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001C544 File Offset: 0x0001A744
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

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001C5A4 File Offset: 0x0001A7A4
		public void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this._writer.WriteStartElement("sub");
			this._writer.WriteAttributeString("alias", alias);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001C5C8 File Offset: 0x0001A7C8
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

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001C6EC File Offset: 0x0001A8EC
		public void ProcessLexicon(Uri uri, string type)
		{
			this._writer.WriteStartElement("lexicon");
			this._writer.WriteAttributeString("uri", uri.ToString());
			if (!string.IsNullOrEmpty(type))
			{
				this._writer.WriteAttributeString("type", type);
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001C738 File Offset: 0x0001A938
		public void EndElement()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001C745 File Offset: 0x0001A945
		public void EndSpeakElement()
		{
			if (this._closeSpeak)
			{
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001C75A File Offset: 0x0001A95A
		public void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader)
		{
			this._writer.WriteNode(reader, false);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001C76C File Offset: 0x0001A96C
		public void StartProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string sElement, List<SsmlXmlAttribute> extraAttributes)
		{
			foreach (SsmlXmlAttribute ssmlXmlAttribute in extraAttributes)
			{
				this._writer.WriteAttributeString(ssmlXmlAttribute._prefix, ssmlXmlAttribute._name, ssmlXmlAttribute._ns, ssmlXmlAttribute._value);
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string sElement, List<SsmlXmlAttribute> extraAttributes)
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		public void ContainsPexml(string pexmlPrefix)
		{
			this._pexmlPrefix = pexmlPrefix;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001C7E4 File Offset: 0x0001A9E4
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

		// Token: 0x060006CD RID: 1741 RVA: 0x0001C846 File Offset: 0x0001AA46
		public bool BeginPromptEngineOutput(object voice)
		{
			return this.ProcessPromptEngine("prompt_output", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001C85C File Offset: 0x0001AA5C
		public bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset)
		{
			return this.ProcessPromptEngine("database", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("fname", fname),
				new KeyValuePair<string, string>("delta", delta),
				new KeyValuePair<string, string>("idset", idset)
			});
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001C8B1 File Offset: 0x0001AAB1
		public bool ProcessPromptEngineDiv(object voice)
		{
			return this.ProcessPromptEngine("div", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001C8C4 File Offset: 0x0001AAC4
		public bool ProcessPromptEngineId(object voice, string id)
		{
			return this.ProcessPromptEngine("id", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("id", id)
			});
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001C8E9 File Offset: 0x0001AAE9
		public bool BeginPromptEngineTts(object voice)
		{
			return this.ProcessPromptEngine("tts", new KeyValuePair<string, string>[0]);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineTts(object voice)
		{
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001C8FC File Offset: 0x0001AAFC
		public bool BeginPromptEngineWithTag(object voice, string tag)
		{
			return this.ProcessPromptEngine("withtag", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("tag", tag)
			});
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineWithTag(object voice, string tag)
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001C921 File Offset: 0x0001AB21
		public bool BeginPromptEngineRule(object voice, string name)
		{
			return this.ProcessPromptEngine("rule", new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("name", name)
			});
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineRule(object voice, string name)
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineOutput(object voice)
		{
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00016269 File Offset: 0x00014469
		public string Ssml
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000534 RID: 1332
		private XmlTextWriter _writer;

		// Token: 0x04000535 RID: 1333
		private CultureInfo _culture;

		// Token: 0x04000536 RID: 1334
		private bool _closeSpeak;

		// Token: 0x04000537 RID: 1335
		private string _pexmlPrefix;

		// Token: 0x04000538 RID: 1336
		private const string xmlNamespacePrompt = "http://schemas.microsoft.com/Speech/2003/03/PromptEngine";
	}
}
