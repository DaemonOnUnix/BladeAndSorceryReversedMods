using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Recognition;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	[StructLayout(0)]
	internal class SPPHRASE
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00009100 File Offset: 0x00008100
		internal static ISpPhrase CreatePhraseFromText(string phrase, CultureInfo culture, out GCHandle[] memHandles, out IntPtr coMem)
		{
			string[] array = phrase.Split(new char[0], 1);
			RecognizedWordUnit[] array2 = new RecognizedWordUnit[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = new RecognizedWordUnit(null, 1f, null, array[i], DisplayAttributes.OneTrailingSpace, TimeSpan.Zero, TimeSpan.Zero);
			}
			return SPPHRASE.CreatePhraseFromWordUnits(array2, culture, out memHandles, out coMem);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009158 File Offset: 0x00008158
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
						GCHandle gchandle = GCHandle.Alloc(recognizedWordUnit.Text, 3);
						list.Add(gchandle);
						array[i].pszDisplayText = gchandle.AddrOfPinnedObject();
					}
					if (recognizedWordUnit.Text == null || recognizedWordUnit.LexicalForm != recognizedWordUnit.Text)
					{
						GCHandle gchandle2 = GCHandle.Alloc(recognizedWordUnit.LexicalForm, 3);
						list.Add(gchandle2);
						array[i].pszLexicalForm = gchandle2.AddrOfPinnedObject();
					}
					else
					{
						array[i].pszLexicalForm = array[i].pszDisplayText;
					}
					if (!string.IsNullOrEmpty(recognizedWordUnit.Pronunciation))
					{
						GCHandle gchandle3 = GCHandle.Alloc(recognizedWordUnit.Pronunciation, 3);
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

		// Token: 0x04000236 RID: 566
		internal uint cbSize;

		// Token: 0x04000237 RID: 567
		internal ushort LangID;

		// Token: 0x04000238 RID: 568
		internal ushort wReserved;

		// Token: 0x04000239 RID: 569
		internal ulong ullGrammarID;

		// Token: 0x0400023A RID: 570
		internal ulong ftStartTime;

		// Token: 0x0400023B RID: 571
		internal ulong ullAudioStreamPosition;

		// Token: 0x0400023C RID: 572
		internal uint ulAudioSizeBytes;

		// Token: 0x0400023D RID: 573
		internal uint ulRetainedSizeBytes;

		// Token: 0x0400023E RID: 574
		internal uint ulAudioSizeTime;

		// Token: 0x0400023F RID: 575
		internal SPPHRASERULE Rule;

		// Token: 0x04000240 RID: 576
		internal IntPtr pProperties;

		// Token: 0x04000241 RID: 577
		internal IntPtr pElements;

		// Token: 0x04000242 RID: 578
		internal uint cReplacements;

		// Token: 0x04000243 RID: 579
		internal IntPtr pReplacements;

		// Token: 0x04000244 RID: 580
		internal Guid SREngineID;

		// Token: 0x04000245 RID: 581
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x04000246 RID: 582
		internal IntPtr pSREnginePrivateData;
	}
}
