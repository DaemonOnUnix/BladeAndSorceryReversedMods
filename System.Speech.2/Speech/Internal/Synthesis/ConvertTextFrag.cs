using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000AE RID: 174
	internal static class ConvertTextFrag
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x000175D4 File Offset: 0x000157D4
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
					spvtextfrag.gcText = GCHandle.Alloc(textFragment.TextToSpeak, GCHandleType.Pinned);
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
						spvtextfrag.gcPhoneme = GCHandle.Alloc(state.Phoneme, GCHandleType.Pinned);
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
						string interpretAs;
						string text = (interpretAs = state.SayAs.InterpretAs);
						if (!(interpretAs == "spellout") && !(interpretAs == "spell-out") && !(interpretAs == "characters") && !(interpretAs == "letters"))
						{
							if (!(interpretAs == "time") && !(interpretAs == "date"))
							{
								spvstate.Context.pCategory = ConvertTextFrag.SapiCategory(spvtextfrag, text, format);
							}
							else
							{
								if (!string.IsNullOrEmpty(format))
								{
									text = text + ":" + format;
								}
								spvstate.Context.pCategory = ConvertTextFrag.SapiCategory(spvtextfrag, text, null);
							}
						}
						else
						{
							spvstate.eAction = SPVACTIONS.SPVA_SpellOut;
						}
					}
					spvtextfrag.State = spvstate;
					sapiFragLast = GCHandle.Alloc(spvtextfrag, GCHandleType.Pinned);
					flag = false;
				}
			}
			return !flag;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000178AC File Offset: 0x00015AAC
		private static IntPtr SapiCategory(SPVTEXTFRAG sapiFrag, string interpretAs, string format)
		{
			int num = Array.BinarySearch<string>(ConvertTextFrag._asSayAsFormat, interpretAs);
			string text = ((num >= 0) ? ConvertTextFrag._asContextFormat[num] : format);
			sapiFrag.gcSayAsCategory = GCHandle.Alloc(text, GCHandleType.Pinned);
			return sapiFrag.gcSayAsCategory.AddrOfPinnedObject();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000178EC File Offset: 0x00015AEC
		internal static void FreeTextSegment(ref GCHandle fragment)
		{
			SPVTEXTFRAG spvtextfrag = (SPVTEXTFRAG)fragment.Target;
			if (spvtextfrag.gcNext.IsAllocated)
			{
				ConvertTextFrag.FreeTextSegment(ref spvtextfrag.gcNext);
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
			fragment.Free();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00017960 File Offset: 0x00015B60
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

		// Token: 0x060005D4 RID: 1492 RVA: 0x00017A14 File Offset: 0x00015C14
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

		// Token: 0x060005D5 RID: 1493 RVA: 0x00017A90 File Offset: 0x00015C90
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x00017B30 File Offset: 0x00015D30
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

		// Token: 0x060005D7 RID: 1495 RVA: 0x00017BC0 File Offset: 0x00015DC0
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

		// Token: 0x04000492 RID: 1170
		private static readonly string[] _asSayAsFormat = new string[]
		{
			"acronym", "address", "cardinal", "currency", "date", "date:d", "date:dm", "date:dmy", "date:m", "date:md",
			"date:mdy", "date:my", "date:ym", "date:ymd", "date:y", "digits", "name", "net", "net:email", "net:uri",
			"ordinal", "spellout", "telephone", "time", "time:hms12", "time:hms24"
		};

		// Token: 0x04000493 RID: 1171
		private static readonly string[] _asContextFormat = new string[]
		{
			"name", "address", "number_cardinal", "currency", "date_md", "date_dm", "date_dm", "date_dmy", "date_md", "date_md",
			"date_mdy", "date_my", "date_ym", "date_ymd", "date_year", "number_digit", "name", "web_url", "E-mail_address", "web_url",
			"number_ordinal", "", "phone_number", "time", "time", "time"
		};
	}
}
