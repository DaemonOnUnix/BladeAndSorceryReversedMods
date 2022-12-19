using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Text;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C5 RID: 197
	internal class TextFragmentEngine : ISsmlParser
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x0001BC9D File Offset: 0x00019E9D
		internal TextFragmentEngine(SpeakInfo speakInfo, string ssmlText, bool pexml, ResourceLoader resourceLoader, List<LexiconEntry> lexicons)
		{
			this._lexicons = lexicons;
			this._ssmlText = ssmlText;
			this._speakInfo = speakInfo;
			this._resourceLoader = resourceLoader;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001BCD1 File Offset: 0x00019ED1
		public object ProcessSpeak(string sVersion, string sBaseUri, CultureInfo culture, List<SsmlXmlAttribute> extraNamespace)
		{
			this._speakInfo.SetVoice(null, culture, VoiceGender.NotSet, VoiceAge.NotSet, 1);
			return this._speakInfo.Voice;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001BCF0 File Offset: 0x00019EF0
		public void ProcessText(string text, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			if (!fIgnore)
			{
				TtsEngineAction action = fragmentState.Action;
				if (this._paragraphStarted)
				{
					fragmentState.Action = TtsEngineAction.StartParagraph;
					this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState));
					this._paragraphStarted = false;
					this._sentenceStarted = true;
				}
				if (this._sentenceStarted)
				{
					fragmentState.Action = TtsEngineAction.StartSentence;
					this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState));
					this._sentenceStarted = false;
				}
				fragmentState.Action = TextFragmentEngine.ActionTextFragment(action);
				this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState, text, this._ssmlText, position, text.Length));
				fragmentState.Action = action;
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public void ProcessAudio(object voice, string sUri, string baseUri, bool fIgnore)
		{
			if (!fIgnore)
			{
				Uri uri = new Uri(sUri, UriKind.RelativeOrAbsolute);
				if (!uri.IsAbsoluteUri && !string.IsNullOrEmpty(baseUri))
				{
					if (baseUri[baseUri.Length - 1] != '/' && baseUri[baseUri.Length - 1] != '\\')
					{
						int num = baseUri.LastIndexOf('/');
						if (num < 0)
						{
							num = baseUri.LastIndexOf('\\');
						}
						if (num >= 0)
						{
							baseUri = baseUri.Substring(0, num);
						}
						baseUri += "/";
					}
					StringBuilder stringBuilder = new StringBuilder(baseUri);
					stringBuilder.Append(sUri);
					uri = new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
				}
				this._speakInfo.AddAudio(new AudioData(uri, this._resourceLoader));
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001BE68 File Offset: 0x0001A068
		public void ProcessBreak(object voice, ref FragmentState fragmentState, EmphasisBreak eBreak, int time, bool fIgnore)
		{
			if (!fIgnore)
			{
				TtsEngineAction action = fragmentState.Action;
				fragmentState.Action = TextFragmentEngine.ActionTextFragment(fragmentState.Action);
				this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState));
				fragmentState.Action = action;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void ProcessDesc(CultureInfo culture)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void ProcessEmphasis(bool noLevel, EmphasisWord word)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		public void ProcessMark(object voice, ref FragmentState fragmentState, string name, bool fIgnore)
		{
			if (!fIgnore)
			{
				TtsEngineAction action = fragmentState.Action;
				fragmentState.Action = TextFragmentEngine.ActionTextFragment(fragmentState.Action);
				this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState, name));
				fragmentState.Action = action;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001BF01 File Offset: 0x0001A101
		public object ProcessTextBlock(bool isParagraph, object voice, ref FragmentState fragmentState, CultureInfo culture, bool newCulture, VoiceGender gender, VoiceAge age)
		{
			if (culture != null && newCulture)
			{
				this._speakInfo.SetVoice(null, culture, gender, age, 1);
			}
			if (isParagraph)
			{
				this._paragraphStarted = true;
			}
			else
			{
				this._sentenceStarted = true;
			}
			return this._speakInfo.Voice;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001BF3E File Offset: 0x0001A13E
		public void EndProcessTextBlock(bool isParagraph)
		{
			if (isParagraph)
			{
				this._paragraphStarted = true;
				return;
			}
			this._sentenceStarted = true;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001BF52 File Offset: 0x0001A152
		public void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds)
		{
			fragmentState.Action = TtsEngineAction.Pronounce;
			fragmentState.Phoneme = this._speakInfo.Voice.TtsEngine.ConvertPhonemes(phoneIds, alphabet);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void ProcessProsody(string pitch, string range, string rate, string volume, string duration, string points)
		{
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void ProcessSayAs(string interpretAs, string format, string detail)
		{
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001BF79 File Offset: 0x0001A179
		public void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this.ProcessText(alias, voice, ref fragmentState, position, fIgnore);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001BF88 File Offset: 0x0001A188
		public object ProcessVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool fNewCulture, List<SsmlXmlAttribute> extraNamespace)
		{
			this._speakInfo.SetVoice(name, culture, gender, age, variant);
			return this._speakInfo.Voice;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001BFA7 File Offset: 0x0001A1A7
		public void ProcessLexicon(Uri uri, string type)
		{
			this._lexicons.Add(new LexiconEntry(uri, type));
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		public void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteNode(reader, false);
			xmlTextWriter.Close();
			string text = stringWriter.ToString();
			this.AddParseUnknownFragment(voice, ref fragmentState, text);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001BFFC File Offset: 0x0001A1FC
		public void StartProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "<{0}", new object[] { element });
			foreach (SsmlXmlAttribute ssmlXmlAttribute in extraAttributes)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}:{1}=\"{2}\" xmlns:{3}=\"{4}\"", new object[] { ssmlXmlAttribute._prefix, ssmlXmlAttribute._name, ssmlXmlAttribute._value, ssmlXmlAttribute._prefix, ssmlXmlAttribute._ns });
			}
			stringBuilder.Append(">");
			this.AddParseUnknownFragment(voice, ref fragmentState, stringBuilder.ToString());
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
		public void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes)
		{
			this.AddParseUnknownFragment(voice, ref fragmentState, string.Format(CultureInfo.InvariantCulture, "</{0}>", new object[] { element }));
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void ContainsPexml(string pexmlPrefix)
		{
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000142DD File Offset: 0x000124DD
		public bool BeginPromptEngineOutput(object voice)
		{
			return false;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineOutput(object voice)
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000142DD File Offset: 0x000124DD
		public bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset)
		{
			return false;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000142DD File Offset: 0x000124DD
		public bool ProcessPromptEngineDiv(object voice)
		{
			return false;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000142DD File Offset: 0x000124DD
		public bool ProcessPromptEngineId(object voice, string id)
		{
			return false;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000142DD File Offset: 0x000124DD
		public bool BeginPromptEngineTts(object voice)
		{
			return false;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineTts(object voice)
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000142DD File Offset: 0x000124DD
		public bool BeginPromptEngineWithTag(object voice, string tag)
		{
			return false;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineWithTag(object voice, string tag)
		{
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000142DD File Offset: 0x000124DD
		public bool BeginPromptEngineRule(object voice, string name)
		{
			return false;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndPromptEngineRule(object voice, string name)
		{
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndElement()
		{
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0000BB6D File Offset: 0x00009D6D
		public void EndSpeakElement()
		{
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001C0F2 File Offset: 0x0001A2F2
		public string Ssml
		{
			get
			{
				return this._ssmlText;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001C0FA File Offset: 0x0001A2FA
		private static TtsEngineAction ActionTextFragment(TtsEngineAction action)
		{
			return action;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001C100 File Offset: 0x0001A300
		private void AddParseUnknownFragment(object voice, ref FragmentState fragmentState, string text)
		{
			TtsEngineAction action = fragmentState.Action;
			fragmentState.Action = TtsEngineAction.ParseUnknownTag;
			this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState, text));
			fragmentState.Action = action;
		}

		// Token: 0x0400052E RID: 1326
		private List<LexiconEntry> _lexicons;

		// Token: 0x0400052F RID: 1327
		private SpeakInfo _speakInfo;

		// Token: 0x04000530 RID: 1328
		private string _ssmlText;

		// Token: 0x04000531 RID: 1329
		private bool _paragraphStarted = true;

		// Token: 0x04000532 RID: 1330
		private bool _sentenceStarted = true;

		// Token: 0x04000533 RID: 1331
		private ResourceLoader _resourceLoader;
	}
}
