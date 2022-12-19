using System;
using System.Collections.Generic;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000F5 RID: 245
	internal class SpeechSeg
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x00017F71 File Offset: 0x00016F71
		internal SpeechSeg(TTSVoice voice, AudioData audio)
		{
			this._voice = voice;
			this._audio = audio;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00017F92 File Offset: 0x00016F92
		internal List<TextFragment> FragmentList
		{
			get
			{
				return this._textFragments;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00017F9A File Offset: 0x00016F9A
		internal AudioData Audio
		{
			get
			{
				return this._audio;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00017FA2 File Offset: 0x00016FA2
		internal TTSVoice Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00017FAA File Offset: 0x00016FAA
		internal bool IsText
		{
			get
			{
				return this._audio == null;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00017FB5 File Offset: 0x00016FB5
		internal void AddFrag(TextFragment textFragment)
		{
			if (this._audio != null)
			{
				throw new InvalidOperationException();
			}
			this._textFragments.Add(textFragment);
		}

		// Token: 0x0400047C RID: 1148
		private TTSVoice _voice;

		// Token: 0x0400047D RID: 1149
		private List<TextFragment> _textFragments = new List<TextFragment>();

		// Token: 0x0400047E RID: 1150
		private AudioData _audio;
	}
}
