using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C8 RID: 200
	internal class TtsProxySsml : ITtsEngineProxy
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001C962 File Offset: 0x0001AB62
		internal TtsProxySsml(TtsEngineSsml ssmlEngine, ITtsEngineSite site, int lcid)
			: base(lcid)
		{
			this._ssmlEngine = ssmlEngine;
			this._site = site;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001C979 File Offset: 0x0001AB79
		internal override IntPtr GetOutputFormat(IntPtr targetFormat)
		{
			return this._ssmlEngine.GetOutputFormat(SpeakOutputFormat.WaveFormat, targetFormat);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001C988 File Offset: 0x0001AB88
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
			this._ssmlEngine.AddLexicon(lexicon, mediaType, this._site);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001C99D File Offset: 0x0001AB9D
		internal override void RemoveLexicon(Uri lexicon)
		{
			this._ssmlEngine.RemoveLexicon(lexicon, this._site);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				this._ssmlEngine.Speak(frags.ToArray(), intPtr, this._site);
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001CA04 File Offset: 0x0001AC04
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return phones;
			}
			return this._alphabetConverter.SapiToIpa(phones);
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0000936B File Offset: 0x0000756B
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Ipa;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal override void ReleaseInterface()
		{
		}

		// Token: 0x0400053A RID: 1338
		private TtsEngineSsml _ssmlEngine;

		// Token: 0x0400053B RID: 1339
		private ITtsEngineSite _site;
	}
}
