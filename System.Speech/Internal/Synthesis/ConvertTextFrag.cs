using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DC RID: 220
	internal static class ConvertTextFrag
	{
		// Token: 0x060004E7 RID: 1255 RVA: 0x00016238 File Offset: 0x00015238
		internal static bool ToSapi(List<TextFragment> ssmlFrags, ref GCHandle sapiFragLast)
		{
			bool flag = true;
			for (int i = ssmlFrags.Count - 1; i >= 0; i--)
			{
				TextFragment textFragment = ssmlFrags[i];
				if (textFragment.State.Action != TtsEngineAction.StartParagraph && textFragment.State.Action != TtsEngineAction.StartSentence)
				{
					SPVTEXTFRAG spvtextfrag = new SPVTEXTFRAG();
					spvtextfrag.gcNext = (flag ? default(GCHandle) : sapiFragLast);
					spvtextfrag.pNext = (flag ? IntPtr.Zero : sapiFragLast.AddrOfPinnedObject());
					spvtextfrag.gcText = GCHandle.Alloc(textFragment.TextToSpeak, 3);
					spvtextfrag.pTextStart = spvtextfrag.gcText.AddrOfPinnedObject();
					spvtextfrag.ulTextSrcOffset = textFragment.TextOffset;
					spvtextfrag.ulTextLen = textFragment.TextLength;
					SPVSTATE spvstate = default(SPVSTATE);
					FragmentState state = textFragment.State;
					spvstate.eAction = (SPVACTIONS)state.Action;
					spvstate.LangID = (short)state.LangId;
					spvstate.EmphAdj = ((state.Emphasis != 1) ? 0 : 1);
					if (state.Prosody != null)
					{
						spvstate.RateAdj = ConvertTextFrag.SapiRate(state.Prosody.Rate);
						spvstate.Volume = ConvertTextFrag.SapiVolume(state.Prosody.Volume);
						spvstate.PitchAdj.MiddleAdj = ConvertTextFrag.SapiPitch(state.Prosody.Pitch);
					}
					else
					{
						spvstate.Volume = 100;
					}
					spvstate.ePartOfSpeech = SPPARTOFSPEECH.SPPS_Unknown;
					if (spvstate.eAction == SPVACTIONS.SPVA_Silence)
					{
						spvstate.SilenceMSecs = ConvertTextFrag.SapiSilence(state.Duration, (EmphasisBreak)state.Emphasis);
					}
					if (state.Phoneme != null)
					{
						spvstate.eAction = SPVACTIONS.SPVA_Pronounce;
						spvtextfrag.gcPhoneme = GCHandle.Alloc(state.Phoneme, 3);
						spvstate.pPhoneIds = spvtextfrag.gcPhoneme.AddrOfPinnedObject();
					}
					else
					{
						spvtextfrag.gcPhoneme = default(GCHandle);
						spvstate.pPhoneIds = IntPtr.Zero;
					}
					if (state.SayAs != null)
					{
						string format = state.SayAs.Format;
						string text2;
						string text;
						if ((text = (text2 = state.SayAs.InterpretAs)) != null)
						{
							if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x60004d9-1 == null)
							{
								Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
								dictionary.Add("spellout", 0);
								dictionary.Add("spell-out", 1);
								dictionary.Add("characters", 2);
								dictionary.Add("letters", 3);
								dictionary.Add("time", 4);
								dictionary.Add("date", 5);
								<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x60004d9-1 = dictionary;
							}
							int num;
							if (<PrivateImplementationDetails>{BEF38D8F-C35A-4027-B811-2533645D828C}.$$method0x60004d9-1.TryGetValue(text, ref num))
							{
								switch (num)
								{
								case 0:
								case 1:
								case 2:
								case 3:
									spvstate.eAction = SPVACTIONS.SPVA_SpellOut;
									goto IL_2E7;
								case 4:
								case 5:
									if (!string.IsNullOrEmpty(format))
									{
										text2 = text2 + ':' + format;
									}
									spvstate.Context.pCategory = ConvertTextFrag.SapiCategory(spvtextfrag, text2, null);
									goto IL_2E7;
								}
							}
						}
						spvstate.Context.pCategory = ConvertTextFrag.SapiCategory(spvtextfrag, text2, format);
					}
					IL_2E7:
					spvtextfrag.State = spvstate;
					sapiFragLast = GCHandle.Alloc(spvtextfrag, 3);
					flag = false;
				}
			}
			return !flag;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00016554 File Offset: 0x00015554
		private static IntPtr SapiCategory(SPVTEXTFRAG sapiFrag, string interpretAs, string format)
		{
			int num = Array.BinarySearch<string>(ConvertTextFrag._asSayAsFormat, interpretAs);
			string text = ((num >= 0) ? ConvertTextFrag._asContextFormat[num] : format);
			sapiFrag.gcSayAsCategory = GCHandle.Alloc(text, 3);
			return sapiFrag.gcSayAsCategory.AddrOfPinnedObject();
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00016594 File Offset: 0x00015594
		internal static void FreeTextSegment(ref GCHandle fragment)
		{
			SPVTEXTFRAG spvtextfrag = (SPVTEXTFRAG)fragment.Target;
			if (spvtextfrag.gcNext.IsAllocated)
			{
				ConvertTextFrag.FreeTextSegment(ref spvtextfrag.gcNext);
				spvtextfrag.gcNext.Free();
			}
			if (spvtextfrag.gcPhoneme.IsAllocated)
			{
				spvtextfrag.gcPhoneme.Free();
			}
			if (spvtextfrag.gcSayAsCategory.IsAllocated)
			{
				spvtextfrag.gcSayAsCategory.Free();
			}
			spvtextfrag.gcText.Free();
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001660C File Offset: 0x0001560C
		private static int SapiVolume(ProsodyNumber volume)
		{
			int num = 100;
			if (volume.SsmlAttributeId != 2147483647)
			{
				switch (volume.SsmlAttributeId)
				{
				case -7:
					num = 100;
					break;
				case -6:
					num = 80;
					break;
				case -5:
					num = 60;
					break;
				case -4:
					num = 40;
					break;
				case -3:
					num = 20;
					break;
				case -2:
					num = 0;
					break;
				}
				num = (int)((double)(volume.IsNumberPercent ? ((float)num * volume.Number) : volume.Number) + 0.5);
			}
			else
			{
				num = (int)((double)volume.Number + 0.5);
			}
			if (num > 100)
			{
				num = 100;
			}
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000166C0 File Offset: 0x000156C0
		private static int SapiSilence(int duration, EmphasisBreak emphasis)
		{
			int num = 1000;
			if (duration > 0)
			{
				num = duration;
			}
			else
			{
				switch (emphasis)
				{
				case EmphasisBreak.ExtraStrong:
					num = 3000;
					break;
				case EmphasisBreak.Strong:
					num = 1750;
					break;
				case EmphasisBreak.Medium:
					num = 1000;
					break;
				case EmphasisBreak.Weak:
					num = 250;
					break;
				case EmphasisBreak.ExtraWeak:
					num = 125;
					break;
				case EmphasisBreak.None:
					num = 10;
					break;
				}
			}
			if (num < 0 || num > 65535)
			{
				num = 1000;
			}
			return num;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001673C File Offset: 0x0001573C
		private static int SapiRate(ProsodyNumber rate)
		{
			int num = 0;
			if (rate.SsmlAttributeId != 2147483647)
			{
				switch (rate.SsmlAttributeId)
				{
				case 1:
					num = -9;
					break;
				case 2:
					num = -4;
					break;
				case 4:
					num = 4;
					break;
				case 5:
					num = 9;
					break;
				}
				num = (int)((double)(rate.IsNumberPercent ? ConvertTextFrag.ScaleNumber(rate.Number, num, 10) : num) + 0.5);
			}
			else
			{
				num = ConvertTextFrag.ScaleNumber(rate.Number, 0, 10);
			}
			if (num > 10)
			{
				num = 10;
			}
			if (num < -10)
			{
				num = -10;
			}
			return num;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000167DC File Offset: 0x000157DC
		private static int SapiPitch(ProsodyNumber pitch)
		{
			int num = 0;
			if (pitch.SsmlAttributeId != 2147483647)
			{
				switch (pitch.SsmlAttributeId)
				{
				case 1:
					num = -9;
					break;
				case 2:
					num = -4;
					break;
				case 4:
					num = 4;
					break;
				case 5:
					num = 9;
					break;
				}
				num = (int)((double)(pitch.IsNumberPercent ? ((float)num * pitch.Number) : pitch.Number) + 0.5);
			}
			if (num > 10)
			{
				num = 10;
			}
			if (num < -10)
			{
				num = -10;
			}
			return num;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001686C File Offset: 0x0001586C
		private static int ScaleNumber(float value, int currentValue, int max)
		{
			int num;
			if ((double)value >= 0.01)
			{
				num = (int)(Math.Log((double)value) / Math.Log(3.0) * (double)max + 0.5);
				num += currentValue;
				if (num > max)
				{
					num = max;
				}
				else if (num < -max)
				{
					num = -max;
				}
			}
			else
			{
				num = -max;
			}
			return num;
		}

		// Token: 0x040003FF RID: 1023
		private static readonly string[] _asSayAsFormat = new string[]
		{
			"acronym", "address", "cardinal", "currency", "date", "date:d", "date:dm", "date:dmy", "date:m", "date:md",
			"date:mdy", "date:my", "date:ym", "date:ymd", "date:y", "digits", "name", "net", "net:email", "net:uri",
			"ordinal", "spellout", "telephone", "time", "time:hms12", "time:hms24"
		};

		// Token: 0x04000400 RID: 1024
		private static readonly string[] _asContextFormat = new string[]
		{
			"name", "address", "number_cardinal", "currency", "date_md", "date_dm", "date_dm", "date_dmy", "date_md", "date_md",
			"date_mdy", "date_my", "date_ym", "date_ymd", "date_year", "number_digit", "name", "web_url", "E-mail_address", "web_url",
			"number_ordinal", "", "phone_number", "time", "time", "time"
		};
	}
}
