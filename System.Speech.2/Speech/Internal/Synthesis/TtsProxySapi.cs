using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000CA RID: 202
	internal class TtsProxySapi : ITtsEngineProxy
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x0001CB46 File Offset: 0x0001AD46
		internal TtsProxySapi(ITtsEngine sapiEngine, IntPtr iSite, int lcid)
			: base(lcid)
		{
			this._iSite = iSite;
			this._sapiEngine = sapiEngine;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001CB60 File Offset: 0x0001AD60
		internal override IntPtr GetOutputFormat(IntPtr preferedFormat)
		{
			Guid spdfid_WaveFormatEx = SAPIGuids.SPDFID_WaveFormatEx;
			Guid guid = default(Guid);
			IntPtr zero = IntPtr.Zero;
			this._sapiEngine.GetOutputFormat(ref spdfid_WaveFormatEx, preferedFormat, out guid, out zero);
			return zero;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal override void RemoveLexicon(Uri lexicon)
		{
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001CB94 File Offset: 0x0001AD94
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				GCHandle gchandle2 = default(GCHandle);
				if (ConvertTextFrag.ToSapi(frags, ref gchandle2))
				{
					Guid spdfid_WaveFormatEx = SAPIGuids.SPDFID_WaveFormatEx;
					try
					{
						this._sapiEngine.Speak(SPEAKFLAGS.SPF_DEFAULT, ref spdfid_WaveFormatEx, intPtr, gchandle2.AddrOfPinnedObject(), this._iSite);
					}
					finally
					{
						ConvertTextFrag.FreeTextSegment(ref gchandle2);
					}
				}
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000142DD File Offset: 0x000124DD
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Sapi;
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001CC18 File Offset: 0x0001AE18
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return this._alphabetConverter.IpaToSapi(phones);
			}
			return phones;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001CC2C File Offset: 0x0001AE2C
		internal override void ReleaseInterface()
		{
			Marshal.ReleaseComObject(this._sapiEngine);
		}

		// Token: 0x0400053E RID: 1342
		private ITtsEngine _sapiEngine;

		// Token: 0x0400053F RID: 1343
		private IntPtr _iSite;
	}
}
