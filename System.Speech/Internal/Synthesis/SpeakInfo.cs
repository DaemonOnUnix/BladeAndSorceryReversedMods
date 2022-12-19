using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000F2 RID: 242
	internal sealed class SpeakInfo
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x00017D91 File Offset: 0x00016D91
		internal SpeakInfo(VoiceSynthesis voiceSynthesis, TTSVoice ttsVoice)
		{
			this._voiceSynthesis = voiceSynthesis;
			this._ttsVoice = ttsVoice;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00017DB9 File Offset: 0x00016DB9
		internal TTSVoice Voice
		{
			get
			{
				return this._ttsVoice;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00017DC4 File Offset: 0x00016DC4
		internal void SetVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant)
		{
			TTSVoice engine = this._voiceSynthesis.GetEngine(name, culture, gender, age, variant, false);
			if (!engine.Equals(this._ttsVoice))
			{
				this._ttsVoice = engine;
				this._fNotInTextSeg = true;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00017E01 File Offset: 0x00016E01
		internal void AddAudio(AudioData audio)
		{
			this.AddNewSeg(null, audio);
			this._fNotInTextSeg = true;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00017E12 File Offset: 0x00016E12
		internal void AddText(TTSVoice ttsVoice, TextFragment textFragment)
		{
			if (this._fNotInTextSeg || ttsVoice != this._ttsVoice)
			{
				this.AddNewSeg(ttsVoice, null);
				this._fNotInTextSeg = false;
			}
			this._lastSeg.AddFrag(textFragment);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00017E40 File Offset: 0x00016E40
		internal SpeechSeg RemoveFirst()
		{
			SpeechSeg speechSeg = null;
			if (this._listSeg.Count > 0)
			{
				speechSeg = this._listSeg[0];
				this._listSeg.RemoveAt(0);
			}
			return speechSeg;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00017E78 File Offset: 0x00016E78
		private void AddNewSeg(TTSVoice pCurrVoice, AudioData audio)
		{
			SpeechSeg speechSeg = new SpeechSeg(pCurrVoice, audio);
			this._listSeg.Add(speechSeg);
			this._lastSeg = speechSeg;
		}

		// Token: 0x0400046C RID: 1132
		private TTSVoice _ttsVoice;

		// Token: 0x0400046D RID: 1133
		private bool _fNotInTextSeg = true;

		// Token: 0x0400046E RID: 1134
		private List<SpeechSeg> _listSeg = new List<SpeechSeg>();

		// Token: 0x0400046F RID: 1135
		private SpeechSeg _lastSeg;

		// Token: 0x04000470 RID: 1136
		private VoiceSynthesis _voiceSynthesis;
	}
}
