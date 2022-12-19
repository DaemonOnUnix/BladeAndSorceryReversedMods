using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000100 RID: 256
	internal class TtsProxySsml : ITtsEngineProxy
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x0001BADA File Offset: 0x0001AADA
		internal TtsProxySsml(TtsEngineSsml ssmlEngine, ITtsEngineSite site, int lcid)
			: base(lcid)
		{
			this._ssmlEngine = ssmlEngine;
			this._site = site;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001BAF1 File Offset: 0x0001AAF1
		internal override IntPtr GetOutputFormat(IntPtr targetFormat)
		{
			return this._ssmlEngine.GetOutputFormat(SpeakOutputFormat.WaveFormat, targetFormat);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001BB00 File Offset: 0x0001AB00
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
			this._ssmlEngine.AddLexicon(lexicon, mediaType, this._site);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001BB15 File Offset: 0x0001AB15
		internal override void RemoveLexicon(Uri lexicon)
		{
			this._ssmlEngine.RemoveLexicon(lexicon, this._site);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001BB2C File Offset: 0x0001AB2C
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, 3);
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

		// Token: 0x0600061A RID: 1562 RVA: 0x0001BB7C File Offset: 0x0001AB7C
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return phones;
			}
			return this._alphabetConverter.SapiToIpa(phones);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001BB90 File Offset: 0x0001AB90
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Ipa;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001BB93 File Offset: 0x0001AB93
		internal override void ReleaseInterface()
		{
		}

		// Token: 0x040004C3 RID: 1219
		private TtsEngineSsml _ssmlEngine;

		// Token: 0x040004C4 RID: 1220
		private ITtsEngineSite _site;
	}
}
