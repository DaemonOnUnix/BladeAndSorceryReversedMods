using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Xml;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DD RID: 221
	internal interface ISsmlParser
	{
		// Token: 0x060004F0 RID: 1264
		object ProcessSpeak(string sVersion, string sBaseUri, CultureInfo culture, List<SsmlXmlAttribute> extraNamespace);

		// Token: 0x060004F1 RID: 1265
		void ProcessText(string text, object voice, ref FragmentState fragmentState, int position, bool fIgnore);

		// Token: 0x060004F2 RID: 1266
		void ProcessAudio(object voice, string sUri, string baseUri, bool fIgnore);

		// Token: 0x060004F3 RID: 1267
		void ProcessBreak(object voice, ref FragmentState fragmentState, EmphasisBreak eBreak, int time, bool fIgnore);

		// Token: 0x060004F4 RID: 1268
		void ProcessDesc(CultureInfo culture);

		// Token: 0x060004F5 RID: 1269
		void ProcessEmphasis(bool noLevel, EmphasisWord word);

		// Token: 0x060004F6 RID: 1270
		void ProcessMark(object voice, ref FragmentState fragmentState, string name, bool fIgnore);

		// Token: 0x060004F7 RID: 1271
		object ProcessTextBlock(bool isParagraph, object voice, ref FragmentState fragmentState, CultureInfo culture, bool newCulture, VoiceGender gender, VoiceAge age);

		// Token: 0x060004F8 RID: 1272
		void EndProcessTextBlock(bool isParagraph);

		// Token: 0x060004F9 RID: 1273
		void ProcessPhoneme(ref FragmentState fragmentState, AlphabetType alphabet, string ph, char[] phoneIds);

		// Token: 0x060004FA RID: 1274
		void ProcessProsody(string pitch, string range, string rate, string volume, string duration, string points);

		// Token: 0x060004FB RID: 1275
		void ProcessSayAs(string interpretAs, string format, string detail);

		// Token: 0x060004FC RID: 1276
		void ProcessSub(string alias, object voice, ref FragmentState fragmentState, int position, bool fIgnore);

		// Token: 0x060004FD RID: 1277
		object ProcessVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant, bool fNewCulture, List<SsmlXmlAttribute> extraNamespace);

		// Token: 0x060004FE RID: 1278
		void ProcessLexicon(Uri uri, string type);

		// Token: 0x060004FF RID: 1279
		void EndElement();

		// Token: 0x06000500 RID: 1280
		void EndSpeakElement();

		// Token: 0x06000501 RID: 1281
		void ProcessUnknownElement(object voice, ref FragmentState fragmentState, XmlReader reader);

		// Token: 0x06000502 RID: 1282
		void StartProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes);

		// Token: 0x06000503 RID: 1283
		void EndProcessUnknownAttributes(object voice, ref FragmentState fragmentState, string element, List<SsmlXmlAttribute> extraAttributes);

		// Token: 0x06000504 RID: 1284
		void ContainsPexml(string pexmlPrefix);

		// Token: 0x06000505 RID: 1285
		bool BeginPromptEngineOutput(object voice);

		// Token: 0x06000506 RID: 1286
		void EndPromptEngineOutput(object voice);

		// Token: 0x06000507 RID: 1287
		bool ProcessPromptEngineDatabase(object voice, string fname, string delta, string idset);

		// Token: 0x06000508 RID: 1288
		bool ProcessPromptEngineDiv(object voice);

		// Token: 0x06000509 RID: 1289
		bool ProcessPromptEngineId(object voice, string id);

		// Token: 0x0600050A RID: 1290
		bool BeginPromptEngineTts(object voice);

		// Token: 0x0600050B RID: 1291
		void EndPromptEngineTts(object voice);

		// Token: 0x0600050C RID: 1292
		bool BeginPromptEngineWithTag(object voice, string tag);

		// Token: 0x0600050D RID: 1293
		void EndPromptEngineWithTag(object voice, string tag);

		// Token: 0x0600050E RID: 1294
		bool BeginPromptEngineRule(object voice, string name);

		// Token: 0x0600050F RID: 1295
		void EndPromptEngineRule(object voice, string name);

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000510 RID: 1296
		string Ssml { get; }
	}
}
