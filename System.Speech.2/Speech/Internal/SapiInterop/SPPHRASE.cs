using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Recognition;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000146 RID: 326
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPPHRASE
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x0002B618 File Offset: 0x00029818
		internal static ISpPhrase CreatePhraseFromText(string phrase, CultureInfo culture, out GCHandle[] memHandles, out IntPtr coMem)
		{
			string[] array = phrase.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			RecognizedWordUnit[] array2 = new RecognizedWordUnit[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = new RecognizedWordUnit(null, 1f, null, array[i], DisplayAttributes.OneTrailingSpace, TimeSpan.Zero, TimeSpan.Zero);
			}
			return SPPHRASE.CreatePhraseFromWordUnits(array2, culture, out memHandles, out coMem);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0002B670 File Offset: 0x00029870
		internal static ISpPhrase CreatePhraseFromWordUnits(RecognizedWordUnit[] words, CultureInfo culture, out GCHandle[] memHandles, out IntPtr coMem)
		{
			SPPHRASEELEMENT[] array = new SPPHRASEELEMENT[words.Length];
			int num = Marshal.SizeOf(typeof(SPPHRASEELEMENT));
			List<GCHandle> list = new List<GCHandle>();
			coMem = Marshal.AllocCoTaskMem(num * array.Length);
			try
			{
				for (int i = 0; i < words.Length; i++)
				{
					RecognizedWordUnit recognizedWordUnit = words[i];
					array[i] = new SPPHRASEELEMENT();
					array[i].bDisplayAttributes = RecognizedWordUnit.DisplayAttributesToSapiAttributes((recognizedWordUnit.DisplayAttributes == DisplayAttributes.None) ? DisplayAttributes.OneTrailingSpace : recognizedWordUnit.DisplayAttributes);
					array[i].SREngineConfidence = recognizedWordUnit.Confidence;
					array[i].ulAudioTimeOffset = (uint)(recognizedWordUnit._audioPosition.Ticks * 10000L / 10000L);
					array[i].ulAudioSizeTime = (uint)(recognizedWordUnit._audioDuration.Ticks * 10000L / 10000L);
					if (recognizedWordUnit.Text != null)
					{
						GCHandle gchandle = GCHandle.Alloc(recognizedWordUnit.Text, GCHandleType.Pinned);
						list.Add(gchandle);
						array[i].pszDisplayText = gchandle.AddrOfPinnedObject();
					}
					if (recognizedWordUnit.Text == null || recognizedWordUnit.LexicalForm != recognizedWordUnit.Text)
					{
						GCHandle gchandle2 = GCHandle.Alloc(recognizedWordUnit.LexicalForm, GCHandleType.Pinned);
						list.Add(gchandle2);
						array[i].pszLexicalForm = gchandle2.AddrOfPinnedObject();
					}
					else
					{
						array[i].pszLexicalForm = array[i].pszDisplayText;
					}
					if (!string.IsNullOrEmpty(recognizedWordUnit.Pronunciation))
					{
						GCHandle gchandle3 = GCHandle.Alloc(recognizedWordUnit.Pronunciation, GCHandleType.Pinned);
						list.Add(gchandle3);
						array[i].pszPronunciation = gchandle3.AddrOfPinnedObject();
					}
					Marshal.StructureToPtr(array[i], new IntPtr((long)coMem + (long)(num * i)), false);
				}
			}
			finally
			{
				memHandles = list.ToArray();
			}
			SPPHRASE spphrase = new SPPHRASE();
			spphrase.cbSize = (uint)Marshal.SizeOf(spphrase.GetType());
			spphrase.LangID = (ushort)culture.LCID;
			spphrase.Rule = new SPPHRASERULE();
			spphrase.Rule.ulCountOfElements = (uint)words.Length;
			spphrase.pElements = coMem;
			SpPhraseBuilder spPhraseBuilder = new SpPhraseBuilder();
			((ISpPhraseBuilder)spPhraseBuilder).InitFromPhrase(spphrase);
			return (ISpPhrase)spPhraseBuilder;
		}

		// Token: 0x040007A8 RID: 1960
		internal uint cbSize;

		// Token: 0x040007A9 RID: 1961
		internal ushort LangID;

		// Token: 0x040007AA RID: 1962
		internal ushort wReserved;

		// Token: 0x040007AB RID: 1963
		internal ulong ullGrammarID;

		// Token: 0x040007AC RID: 1964
		internal ulong ftStartTime;

		// Token: 0x040007AD RID: 1965
		internal ulong ullAudioStreamPosition;

		// Token: 0x040007AE RID: 1966
		internal uint ulAudioSizeBytes;

		// Token: 0x040007AF RID: 1967
		internal uint ulRetainedSizeBytes;

		// Token: 0x040007B0 RID: 1968
		internal uint ulAudioSizeTime;

		// Token: 0x040007B1 RID: 1969
		internal SPPHRASERULE Rule;

		// Token: 0x040007B2 RID: 1970
		internal IntPtr pProperties;

		// Token: 0x040007B3 RID: 1971
		internal IntPtr pElements;

		// Token: 0x040007B4 RID: 1972
		internal uint cReplacements;

		// Token: 0x040007B5 RID: 1973
		internal IntPtr pReplacements;

		// Token: 0x040007B6 RID: 1974
		internal Guid SREngineID;

		// Token: 0x040007B7 RID: 1975
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x040007B8 RID: 1976
		internal IntPtr pSREnginePrivateData;
	}
}
