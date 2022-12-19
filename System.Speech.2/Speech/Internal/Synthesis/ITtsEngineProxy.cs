using System;
using System.Collections.Generic;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C7 RID: 199
	internal abstract class ITtsEngineProxy
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x0001C946 File Offset: 0x0001AB46
		internal ITtsEngineProxy(int lcid)
		{
			this._alphabetConverter = new AlphabetConverter(lcid);
		}

		// Token: 0x060006DA RID: 1754
		internal abstract IntPtr GetOutputFormat(IntPtr targetFormat);

		// Token: 0x060006DB RID: 1755
		internal abstract void AddLexicon(Uri lexicon, string mediaType);

		// Token: 0x060006DC RID: 1756
		internal abstract void RemoveLexicon(Uri lexicon);

		// Token: 0x060006DD RID: 1757
		internal abstract void Speak(List<TextFragment> frags, byte[] wfx);

		// Token: 0x060006DE RID: 1758
		internal abstract void ReleaseInterface();

		// Token: 0x060006DF RID: 1759
		internal abstract char[] ConvertPhonemes(char[] phones, AlphabetType alphabet);

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060006E0 RID: 1760
		internal abstract AlphabetType EngineAlphabet { get; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001C95A File Offset: 0x0001AB5A
		internal AlphabetConverter AlphabetConverter
		{
			get
			{
				return this._alphabetConverter;
			}
		}

		// Token: 0x04000539 RID: 1337
		protected AlphabetConverter _alphabetConverter;
	}
}
