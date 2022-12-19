using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000BE RID: 190
	internal sealed class SpeakInfo
	{
		// Token: 0x06000653 RID: 1619 RVA: 0x00019090 File Offset: 0x00017290
		internal SpeakInfo(VoiceSynthesis voiceSynthesis, TTSVoice ttsVoice)
		{
			this._voiceSynthesis = voiceSynthesis;
			this._ttsVoice = ttsVoice;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000190B8 File Offset: 0x000172B8
		internal TTSVoice Voice
		{
			get
			{
				return this._ttsVoice;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000190C0 File Offset: 0x000172C0
		internal void SetVoice(string name, CultureInfo culture, VoiceGender gender, VoiceAge age, int variant)
		{
			TTSVoice engine = this._voiceSynthesis.GetEngine(name, culture, gender, age, variant, false);
			if (!engine.Equals(this._ttsVoice))
			{
				this._ttsVoice = engine;
				this._fNotInTextSeg = true;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000190FD File Offset: 0x000172FD
		internal void AddAudio(AudioData audio)
		{
			this.AddNewSeg(null, audio);
			this._fNotInTextSeg = true;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001910E File Offset: 0x0001730E
		internal void AddText(TTSVoice ttsVoice, TextFragment textFragment)
		{
			if (this._fNotInTextSeg || ttsVoice != this._ttsVoice)
			{
				this.AddNewSeg(ttsVoice, null);
				this._fNotInTextSeg = false;
			}
			this._lastSeg.AddFrag(textFragment);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001913C File Offset: 0x0001733C
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

		// Token: 0x06000659 RID: 1625 RVA: 0x00019174 File Offset: 0x00017374
		private void AddNewSeg(TTSVoice pCurrVoice, AudioData audio)
		{
			SpeechSeg speechSeg = new SpeechSeg(pCurrVoice, audio);
			this._listSeg.Add(speechSeg);
			this._lastSeg = speechSeg;
		}

		// Token: 0x040004EB RID: 1259
		private TTSVoice _ttsVoice;

		// Token: 0x040004EC RID: 1260
		private bool _fNotInTextSeg = true;

		// Token: 0x040004ED RID: 1261
		private List<SpeechSeg> _listSeg = new List<SpeechSeg>();

		// Token: 0x040004EE RID: 1262
		private SpeechSeg _lastSeg;

		// Token: 0x040004EF RID: 1263
		private VoiceSynthesis _voiceSynthesis;
	}
}
