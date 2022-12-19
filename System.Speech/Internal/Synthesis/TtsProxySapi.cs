using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000102 RID: 258
	internal class TtsProxySapi : ITtsEngineProxy
	{
		// Token: 0x06000625 RID: 1573 RVA: 0x0001BCDD File Offset: 0x0001ACDD
		internal TtsProxySapi(ITtsEngine sapiEngine, IntPtr iSite, int lcid)
			: base(lcid)
		{
			this._iSite = iSite;
			this._sapiEngine = sapiEngine;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001BCF4 File Offset: 0x0001ACF4
		internal override IntPtr GetOutputFormat(IntPtr preferedFormat)
		{
			Guid spdfid_WaveFormatEx = SAPIGuids.SPDFID_WaveFormatEx;
			Guid guid = default(Guid);
			IntPtr zero = IntPtr.Zero;
			this._sapiEngine.GetOutputFormat(ref spdfid_WaveFormatEx, preferedFormat, out guid, out zero);
			return zero;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001BD29 File Offset: 0x0001AD29
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001BD2B File Offset: 0x0001AD2B
		internal override void RemoveLexicon(Uri lexicon)
		{
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001BD30 File Offset: 0x0001AD30
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, 3);
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001BDB4 File Offset: 0x0001ADB4
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Sapi;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001BDB7 File Offset: 0x0001ADB7
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return this._alphabetConverter.IpaToSapi(phones);
			}
			return phones;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001BDCB File Offset: 0x0001ADCB
		internal override void ReleaseInterface()
		{
			Marshal.ReleaseComObject(this._sapiEngine);
		}

		// Token: 0x040004C7 RID: 1223
		private ITtsEngine _sapiEngine;

		// Token: 0x040004C8 RID: 1224
		private IntPtr _iSite;
	}
}
