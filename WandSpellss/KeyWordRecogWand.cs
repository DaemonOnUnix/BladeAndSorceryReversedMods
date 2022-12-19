using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace WandSpellss
{
	// Token: 0x02000010 RID: 16
	internal class KeyWordRecogWand : MonoBehaviour
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00003622 File Offset: 0x00001822
		public void Start()
		{
			this.hasRecognizedWord = false;
			this.m_recognizer = new KeywordRecognizer(this.m_keywords);
			this.m_recognizer.OnPhraseRecognized += new PhraseRecognizer.PhraseRecognizedDelegate(this.M_recognizer_OnPhraseRecognized);
			this.m_recognizer.Start();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003664 File Offset: 0x00001864
		private void M_recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
		{
			StringBuilder builder = new StringBuilder();
			this.hasRecognizedWord = true;
			builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
			builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
			StringBuilder stringBuilder = builder;
			string text = "\tDuration: {0} seconds{1}";
			TimeSpan phraseDuration = args.phraseDuration;
			stringBuilder.AppendFormat(text, phraseDuration.TotalSeconds, Environment.NewLine);
			this.knownCurrent = builder.ToString();
		}

		// Token: 0x04000041 RID: 65
		internal bool hasRecognizedWord;

		// Token: 0x04000042 RID: 66
		private string[] m_keywords = new string[]
		{
			"Stewpify", "Expelliarmus", "Ahvahduhkuhdahvra", "PetrificusTotalus", "Levicorpus", "Liberacorpus", "Protego", "Lumos", "Assio", "Engorgio",
			"Evanesco", "Geminio", "Sectumsempra", "Nox", "Ascendio", "Vincere mortem", "Morsmordre"
		};

		// Token: 0x04000043 RID: 67
		private KeywordRecognizer m_recognizer;

		// Token: 0x04000044 RID: 68
		internal string knownCurrent;

		// Token: 0x04000045 RID: 69
		private bool isListening;

		// Token: 0x04000046 RID: 70
		private Coroutine attentionSpan;
	}
}
