using System;
using System.Globalization;
using System.Speech.Recognition;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000016 RID: 22
	internal class SpeechRecogWands : MonoBehaviour
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003DE4 File Offset: 0x00001FE4
		public void Start()
		{
			using (this.recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US")))
			{
				Choices spells = new Choices(new string[] { "Avada kedavra", "Stew pify", "Expelliarmus", "PetrificusTotallus" });
				this.findServices = new GrammarBuilder();
				this.findServices.Append(spells);
			}
			Grammar servicesGrammar = new Grammar(this.findServices);
			this.recognizer.LoadGrammarAsync(servicesGrammar);
			this.recognizer.SpeechRecognized += this.recognizer_SpeechRecognized;
			this.recognizer.SetInputToDefaultAudioDevice();
			this.recognizer.RecognizeAsync(RecognizeMode.Multiple);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003EB8 File Offset: 0x000020B8
		public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			bool flag = e.Result.Text != null;
			if (flag)
			{
				this.knownCurrent = e.Result.Text;
			}
		}

		// Token: 0x0400005B RID: 91
		private GrammarBuilder findServices;

		// Token: 0x0400005C RID: 92
		private SpeechRecognitionEngine recognizer;

		// Token: 0x0400005D RID: 93
		public string knownCurrent;

		// Token: 0x0400005E RID: 94
		private bool globalWaitFlag;
	}
}
