using System;
using System.Collections.Generic;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000FF RID: 255
	internal abstract class ITtsEngineProxy
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x0001BABE File Offset: 0x0001AABE
		internal ITtsEngineProxy(int lcid)
		{
			this._alphabetConverter = new AlphabetConverter(lcid);
		}

		// Token: 0x0600060D RID: 1549
		internal abstract IntPtr GetOutputFormat(IntPtr targetFormat);

		// Token: 0x0600060E RID: 1550
		internal abstract void AddLexicon(Uri lexicon, string mediaType);

		// Token: 0x0600060F RID: 1551
		internal abstract void RemoveLexicon(Uri lexicon);

		// Token: 0x06000610 RID: 1552
		internal abstract void Speak(List<TextFragment> frags, byte[] wfx);

		// Token: 0x06000611 RID: 1553
		internal abstract void ReleaseInterface();

		// Token: 0x06000612 RID: 1554
		internal abstract char[] ConvertPhonemes(char[] phones, AlphabetType alphabet);

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000613 RID: 1555
		internal abstract AlphabetType EngineAlphabet { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001BAD2 File Offset: 0x0001AAD2
		internal AlphabetConverter AlphabetConverter
		{
			get
			{
				return this._alphabetConverter;
			}
		}

		// Token: 0x040004C2 RID: 1218
		protected AlphabetConverter _alphabetConverter;
	}
}
