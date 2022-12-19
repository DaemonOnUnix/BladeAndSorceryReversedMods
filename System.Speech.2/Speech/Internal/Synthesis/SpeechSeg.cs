using System;
using System.Collections.Generic;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C1 RID: 193
	internal class SpeechSeg
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x0001926D File Offset: 0x0001746D
		internal SpeechSeg(TTSVoice voice, AudioData audio)
		{
			this._voice = voice;
			this._audio = audio;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001928E File Offset: 0x0001748E
		internal List<TextFragment> FragmentList
		{
			get
			{
				return this._textFragments;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00019296 File Offset: 0x00017496
		internal AudioData Audio
		{
			get
			{
				return this._audio;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001929E File Offset: 0x0001749E
		internal TTSVoice Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x000192A6 File Offset: 0x000174A6
		internal bool IsText
		{
			get
			{
				return this._audio == null;
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000192B1 File Offset: 0x000174B1
		internal void AddFrag(TextFragment textFragment)
		{
			if (this._audio != null)
			{
				throw new InvalidOperationException();
			}
			this._textFragments.Add(textFragment);
		}

		// Token: 0x040004FB RID: 1275
		private TTSVoice _voice;

		// Token: 0x040004FC RID: 1276
		private List<TextFragment> _textFragments = new List<TextFragment>();

		// Token: 0x040004FD RID: 1277
		private AudioData _audio;
	}
}
