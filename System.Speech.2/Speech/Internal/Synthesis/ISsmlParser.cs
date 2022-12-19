using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000AF RID: 175
	internal interface ISsmlParser
	{
		// Token: 0x060005D9 RID: 1497
		object ProcessSpeak(string sVersion, string sBaseUri, CultureInfo culture, List<SsmlXmlAttribute> extraNamespace);

		// Token: 0x060005DA RID: 1498
		void ProcessText(string text, object voice, ref FragmentState fragmentState, int position, bool fIgnore);

		// Token: 0x060005DB RID: 1499
		void ProcessAudio(object voice, string sUri, string baseUri, bool fIgnore);

		// Token: 0x060005DC RID: 1500
		void ProcessBreak(object voice, ref FragmentState fragmentState, EmphasisBreak eBreak, int time, bool fIgnore);

		// Token: 0x060005DD RID: 1501
		void ProcessDesc(CultureInfo culture);

		// Token: 0x060005DE RID: 1502
		void ProcessEmphasis(bool noLevel, EmphasisWord word);

		// Token: 0x060005DF RID: 1503
		void ProcessMark(object voice, ref FragmentState fragmentState, string name, bool fIgnore);

		// Token: 0x060005E0 RID: 1504
		object ProcessTextBlock(bool isParagraph, object voice, ref FragmentState fragmentState, CultureInfo culture, bool newCulture, VoiceGender gender, VoiceAge age);

		// Token: 0x060005E1 RID: 1505
		void EndProcessTextBlock(bool isParagraph);

		// Token: 0x060005E2 RID: 1506
		void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds);

		// Token: 0x060005E3 RID: 1507
		void ProcessProsody(string pitch, string range, string rate, string volume, string duration, string points);

		// Token: 0x060005E4 RID: 1508
		void ProcessSayAs(string interpretAs, string format, string detail);

		// Token: 0x060005E5 RID: 1509
		void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore);

		// Token: 0x060005E6 RID: 1510
		object ProcessVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool fNewCulture, List<SsmlXmlAttribute> extraNamespace);

		// Token: 0x060005E7 RID: 1511
		void ProcessLexicon(Uri uri, string type);

		// Token: 0x060005E8 RID: 1512
		void EndElement();

		// Token: 0x060005E9 RID: 1513
		void EndSpeakElement();

		// Token: 0x060005EA RID: 1514
		void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader);

		// Token: 0x060005EB RID: 1515
		void StartProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes);

		// Token: 0x060005EC RID: 1516
		void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes);

		// Token: 0x060005ED RID: 1517
		void ContainsPexml(string pexmlPrefix);

		// Token: 0x060005EE RID: 1518
		bool BeginPromptEngineOutput(object voice);

		// Token: 0x060005EF RID: 1519
		void EndPromptEngineOutput(object voice);

		// Token: 0x060005F0 RID: 1520
		bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset);

		// Token: 0x060005F1 RID: 1521
		bool ProcessPromptEngineDiv(object voice);

		// Token: 0x060005F2 RID: 1522
		bool ProcessPromptEngineId(object voice, string id);

		// Token: 0x060005F3 RID: 1523
		bool BeginPromptEngineTts(object voice);

		// Token: 0x060005F4 RID: 1524
		void EndPromptEngineTts(object voice);

		// Token: 0x060005F5 RID: 1525
		bool BeginPromptEngineWithTag(object voice, string tag);

		// Token: 0x060005F6 RID: 1526
		void EndPromptEngineWithTag(object voice, string tag);

		// Token: 0x060005F7 RID: 1527
		bool BeginPromptEngineRule(object voice, string name);

		// Token: 0x060005F8 RID: 1528
		void EndPromptEngineRule(object voice, string name);

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005F9 RID: 1529
		string Ssml { get; }
	}
}
