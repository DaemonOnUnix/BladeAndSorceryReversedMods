using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000101 RID: 257
	internal class TtsProxyCom : ITtsEngineProxy
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x0001BB95 File Offset: 0x0001AB95
		internal TtsProxyCom(ITtsEngineSsml comEngine, IntPtr iSite, int lcid)
			: base(lcid)
		{
			this._iSite = iSite;
			this._comEngine = comEngine;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001BBAC File Offset: 0x0001ABAC
		internal override IntPtr GetOutputFormat(IntPtr targetFormat)
		{
			IntPtr intPtr;
			this._comEngine.GetOutputFormat((targetFormat != IntPtr.Zero) ? SpeakOutputFormat.WaveFormat : SpeakOutputFormat.Text, targetFormat, out intPtr);
			return intPtr;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001BBD9 File Offset: 0x0001ABD9
		internal override void AddLexicon(Uri lexicon, string mediaType)
		{
			this._comEngine.AddLexicon(lexicon.ToString(), mediaType, this._iSite);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001BBF3 File Offset: 0x0001ABF3
		internal override void RemoveLexicon(Uri lexicon)
		{
			this._comEngine.RemoveLexicon(lexicon.ToString(), this._iSite);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001BC0C File Offset: 0x0001AC0C
		internal override void Speak(List<TextFragment> frags, byte[] wfx)
		{
			GCHandle gchandle = GCHandle.Alloc(wfx, 3);
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

		// Token: 0x06000622 RID: 1570 RVA: 0x0001BCB8 File Offset: 0x0001ACB8
		internal override void ReleaseInterface()
		{
			Marshal.ReleaseComObject(this._comEngine);
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001BCC6 File Offset: 0x0001ACC6
		internal override AlphabetType EngineAlphabet
		{
			get
			{
				return AlphabetType.Ipa;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001BCC9 File Offset: 0x0001ACC9
		internal override char[] ConvertPhonemes(char[] phones, AlphabetType alphabet)
		{
			if (alphabet == AlphabetType.Ipa)
			{
				return phones;
			}
			return this._alphabetConverter.SapiToIpa(phones);
		}

		// Token: 0x040004C5 RID: 1221
		private ITtsEngineSsml _comEngine;

		// Token: 0x040004C6 RID: 1222
		private IntPtr _iSite;
	}
}
