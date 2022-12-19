using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C9 RID: 201
	internal class TtsProxyCom : ITtsEngineProxy
	{
		// Token: 0x060006EA RID: 1770 RVA: 0x0001CA18 File Offset: 0x0001AC18
		internal TtsProxyCom(ITtsEngineSsml comEngine, IntPtr iSite, int lcid)
			: base(lcid)
		{
			this._iSite = iSite;
			this._comEngine = comEngine;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001CA30 File Offset: 0x0001AC30
		internal override IntPtr GetOutputFormat(IntPtr targetFormat)
		{
			IntPtr intPtr;
			this._comEngine.GetOutputFormat((targetFormat != IntPtr.Zero) ? SpeakOutputFormat.WaveFormat : SpeakOutputFormat.Text, targetFormat, out intPtr);
			return intPtr;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001CA5D File Offset: 0x0001AC5D
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
			this._comEngine.AddLexicon(lexicon.ToString(), mediaType, this._iSite);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001CA77 File Offset: 0x0001AC77
		internal override void RemoveLexicon(Uri lexicon)
		{
			this._comEngine.RemoveLexicon(lexicon.ToString(), this._iSite);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001CA90 File Offset: 0x0001AC90
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				Collection<IntPtr> collection = new Collection<IntPtr>();
				IntPtr intPtr2 = TextFragmentInterop.FragmentToPtr(frags, collection);
				try
				{
					this._comEngine.Speak(intPtr2, frags.Count, intPtr, this._iSite);
				}
				finally
				{
					foreach (IntPtr intPtr3 in collection)
					{
						Marshal.FreeCoTaskMem(intPtr3);
					}
				}
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001CB38 File Offset: 0x0001AD38
		internal override void ReleaseInterface()
		{
			Marshal.ReleaseComObject(this._comEngine);
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0000936B File Offset: 0x0000756B
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Ipa;
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001CA04 File Offset: 0x0001AC04
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return phones;
			}
			return this._alphabetConverter.SapiToIpa(phones);
		}

		// Token: 0x0400053C RID: 1340
		private ITtsEngineSsml _comEngine;

		// Token: 0x0400053D RID: 1341
		private IntPtr _iSite;
	}
}
