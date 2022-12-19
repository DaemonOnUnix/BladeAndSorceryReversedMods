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
	// Token: 0x020000FD RID: 253
	internal class TextFragmentEngine : ISsmlParser
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x0001AD7F File Offset: 0x00019D7F
		internal TextFragmentEngine(SpeakInfo speakInfo, string ssmlText, bool pexml, ResourceLoader resourceLoader, List<LexiconEntry> lexicons)
		{
			this._lexicons = lexicons;
			this._ssmlText = ssmlText;
			this._speakInfo = speakInfo;
			this._resourceLoader = resourceLoader;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001ADB3 File Offset: 0x00019DB3
		public object ProcessSpeak(string sVersion, string sBaseUri, CultureInfo culture, List<SsmlXmlAttribute> extraNamespace)
		{
			this._speakInfo.SetVoice(null, culture, VoiceGender.NotSet, VoiceAge.NotSet, 1);
			return this._speakInfo.Voice;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001ADD0 File Offset: 0x00019DD0
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

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001AE94 File Offset: 0x00019E94
		public void ProcessAudio(object voice, string sUri, string baseUri, bool fIgnore)
		{
			if (!fIgnore)
			{
				Uri uri = new Uri(sUri, 0);
				if (!uri.IsAbsoluteUri && !string.IsNullOrEmpty(baseUri))
				{
					if (baseUri.get_Chars(baseUri.Length - 1) != '/' && baseUri.get_Chars(baseUri.Length - 1) != '\\')
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
						baseUri += '/';
					}
					StringBuilder stringBuilder = new StringBuilder(baseUri);
					stringBuilder.Append(sUri);
					uri = new Uri(stringBuilder.ToString(), 0);
				}
				this._speakInfo.AddAudio(new AudioData(uri, this._resourceLoader));
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001AF4C File Offset: 0x00019F4C
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

		// Token: 0x060005CA RID: 1482 RVA: 0x0001AF98 File Offset: 0x00019F98
		public void ProcessDesc(CultureInfo culture)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001AF9A File Offset: 0x00019F9A
		public void ProcessEmphasis(bool noLevel, EmphasisWord word)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001AF9C File Offset: 0x00019F9C
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

		// Token: 0x060005CD RID: 1485 RVA: 0x0001AFE9 File Offset: 0x00019FE9
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

		// Token: 0x060005CE RID: 1486 RVA: 0x0001B024 File Offset: 0x0001A024
		public void EndProcessTextBlock(bool isParagraph)
		{
			if (isParagraph)
			{
				this._paragraphStarted = true;
				return;
			}
			this._sentenceStarted = true;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001B038 File Offset: 0x0001A038
		public void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds)
		{
			fragmentState.Action = TtsEngineAction.Pronounce;
			fragmentState.Phoneme = this._speakInfo.Voice.TtsEngine.ConvertPhonemes(phoneIds, alphabet);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001B05F File Offset: 0x0001A05F
		public void ProcessProsody(string pitch, string range, string rate, string volume, string duration, string points)
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001B061 File Offset: 0x0001A061
		public void ProcessSayAs(string interpretAs, string format, string detail)
		{
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001B063 File Offset: 0x0001A063
		public void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore)
		{
			this.ProcessText(alias, voice, ref fragmentState, position, fIgnore);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001B072 File Offset: 0x0001A072
		public object ProcessVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool fNewCulture, List<SsmlXmlAttribute> extraNamespace)
		{
			this._speakInfo.SetVoice(name, culture, gender, age, variant);
			return this._speakInfo.Voice;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001B091 File Offset: 0x0001A091
		public void ProcessLexicon(Uri uri, string type)
		{
			this._lexicons.Add(new LexiconEntry(uri, type));
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001B0A8 File Offset: 0x0001A0A8
		public void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteNode(reader, false);
			xmlTextWriter.Close();
			string text = stringWriter.ToString();
			this.AddParseUnknownFragment(voice, ref fragmentState, text);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001B0E8 File Offset: 0x0001A0E8
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

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001B1BC File Offset: 0x0001A1BC
		public void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes)
		{
			this.AddParseUnknownFragment(voice, ref fragmentState, string.Format(CultureInfo.InvariantCulture, "</{0}>", new object[] { element }));
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001B1EC File Offset: 0x0001A1EC
		public void ContainsPexml(string pexmlPrefix)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001B1EE File Offset: 0x0001A1EE
		public bool BeginPromptEngineOutput(object voice)
		{
			return false;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001B1F1 File Offset: 0x0001A1F1
		public void EndPromptEngineOutput(object voice)
		{
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001B1F3 File Offset: 0x0001A1F3
		public bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset)
		{
			return false;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001B1F6 File Offset: 0x0001A1F6
		public bool ProcessPromptEngineDiv(object voice)
		{
			return false;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001B1F9 File Offset: 0x0001A1F9
		public bool ProcessPromptEngineId(object voice, string id)
		{
			return false;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001B1FC File Offset: 0x0001A1FC
		public bool BeginPromptEngineTts(object voice)
		{
			return false;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001B1FF File Offset: 0x0001A1FF
		public void EndPromptEngineTts(object voice)
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001B201 File Offset: 0x0001A201
		public bool BeginPromptEngineWithTag(object voice, string tag)
		{
			return false;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001B204 File Offset: 0x0001A204
		public void EndPromptEngineWithTag(object voice, string tag)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001B206 File Offset: 0x0001A206
		public bool BeginPromptEngineRule(object voice, string name)
		{
			return false;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001B209 File Offset: 0x0001A209
		public void EndPromptEngineRule(object voice, string name)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001B20B File Offset: 0x0001A20B
		public void EndElement()
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001B20D File Offset: 0x0001A20D
		public void EndSpeakElement()
		{
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0001B20F File Offset: 0x0001A20F
		public string Ssml
		{
			get
			{
				return this._ssmlText;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001B217 File Offset: 0x0001A217
		private static TtsEngineAction ActionTextFragment(TtsEngineAction action)
		{
			return action;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001B21C File Offset: 0x0001A21C
		private void AddParseUnknownFragment(object voice, ref FragmentState fragmentState, string text)
		{
			TtsEngineAction action = fragmentState.Action;
			fragmentState.Action = TtsEngineAction.ParseUnknownTag;
			this._speakInfo.AddText((TTSVoice)voice, new TextFragment(fragmentState, text));
			fragmentState.Action = action;
		}

		// Token: 0x040004B7 RID: 1207
		private List<LexiconEntry> _lexicons;

		// Token: 0x040004B8 RID: 1208
		private SpeakInfo _speakInfo;

		// Token: 0x040004B9 RID: 1209
		private string _ssmlText;

		// Token: 0x040004BA RID: 1210
		private bool _paragraphStarted = true;

		// Token: 0x040004BB RID: 1211
		private bool _sentenceStarted = true;

		// Token: 0x040004BC RID: 1212
		private ResourceLoader _resourceLoader;
	}
}
