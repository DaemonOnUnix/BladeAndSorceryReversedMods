using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Speech.Internal;
using System.Speech.Internal.Synthesis;
using System.Xml;

namespace System.Speech.Synthesis
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class PromptBuilder
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002618 File Offset: 0x00000818
		public PromptBuilder()
			: this(CultureInfo.CurrentUICulture)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002628 File Offset: 0x00000828
		public PromptBuilder(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			this._culture = culture;
			this.ClearContent();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000268E File Offset: 0x0000088E
		public void ClearContent()
		{
			this._elements.Clear();
			this._elementStack.Push(new PromptBuilder.StackElement(SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Lexicon | SsmlElement.Meta | SsmlElement.MetaData | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput, PromptBuilder.SsmlState.Header, this._culture));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026B7 File Offset: 0x000008B7
		public void AppendText(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.Text, textToSpeak));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026EC File Offset: 0x000008EC
		public void AppendText(string textToSpeak, PromptRate rate)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (rate < PromptRate.NotSet || rate > PromptRate.ExtraSlow)
			{
				throw new ArgumentOutOfRangeException("rate");
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Prosody, textToSpeak);
			this._elements.Add(element);
			string text = null;
			if (rate != PromptRate.NotSet)
			{
				if (rate != PromptRate.ExtraFast)
				{
					if (rate != PromptRate.ExtraSlow)
					{
						text = rate.ToString().ToLowerInvariant();
					}
					else
					{
						text = "x-slow";
					}
				}
				else
				{
					text = "x-fast";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("rate", text));
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000279C File Offset: 0x0000099C
		public void AppendText(string textToSpeak, PromptVolume volume)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (volume < PromptVolume.NotSet || volume > PromptVolume.Default)
			{
				throw new ArgumentOutOfRangeException("volume");
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Prosody, textToSpeak);
			this._elements.Add(element);
			string text = null;
			if (volume != PromptVolume.NotSet)
			{
				if (volume != PromptVolume.ExtraSoft)
				{
					if (volume != PromptVolume.ExtraLoud)
					{
						text = volume.ToString().ToLowerInvariant();
					}
					else
					{
						text = "x-loud";
					}
				}
				else
				{
					text = "x-soft";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("volume", text));
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000284C File Offset: 0x00000A4C
		public void AppendText(string textToSpeak, PromptEmphasis emphasis)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (emphasis < PromptEmphasis.NotSet || emphasis > PromptEmphasis.Reduced)
			{
				throw new ArgumentOutOfRangeException("emphasis");
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Emphasis, textToSpeak);
			this._elements.Add(element);
			if (emphasis != PromptEmphasis.NotSet)
			{
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("level", emphasis.ToString().ToLowerInvariant()));
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028D8 File Offset: 0x00000AD8
		public void StartStyle(PromptStyle style)
		{
			Helpers.ThrowIfNull(style, "style");
			PromptBuilder.StackElement stackElement = this._elementStack.Peek();
			PromptBuilder.ValidateElement(stackElement, SsmlElement.Prosody);
			PromptBuilder.SsmlState ssmlState = (PromptBuilder.SsmlState)0;
			SsmlElement ssmlElement = stackElement._possibleChildren;
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.StartStyle));
			if (style.Emphasis != PromptEmphasis.NotSet)
			{
				PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Emphasis);
				this._elements.Add(element);
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("level", style.Emphasis.ToString().ToLowerInvariant()));
				ssmlElement = SsmlElement.AudioMarkTextWithStyle;
				ssmlState = PromptBuilder.SsmlState.StyleEmphasis;
			}
			if (style.Rate != PromptRate.NotSet || style.Volume != PromptVolume.NotSet)
			{
				if (ssmlState != (PromptBuilder.SsmlState)0)
				{
					this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.StartStyle));
				}
				PromptBuilder.Element element2 = new PromptBuilder.Element(PromptBuilder.ElementType.Prosody);
				this._elements.Add(element2);
				if (style.Rate != PromptRate.NotSet)
				{
					PromptRate rate = style.Rate;
					string text;
					if (rate != PromptRate.ExtraFast)
					{
						if (rate != PromptRate.ExtraSlow)
						{
							text = style.Rate.ToString().ToLowerInvariant();
						}
						else
						{
							text = "x-slow";
						}
					}
					else
					{
						text = "x-fast";
					}
					element2._attributes = new Collection<PromptBuilder.AttributeItem>();
					element2._attributes.Add(new PromptBuilder.AttributeItem("rate", text));
				}
				if (style.Volume != PromptVolume.NotSet)
				{
					PromptVolume volume = style.Volume;
					string text2;
					if (volume != PromptVolume.ExtraSoft)
					{
						if (volume != PromptVolume.ExtraLoud)
						{
							text2 = style.Volume.ToString().ToLowerInvariant();
						}
						else
						{
							text2 = "x-loud";
						}
					}
					else
					{
						text2 = "x-soft";
					}
					if (element2._attributes == null)
					{
						element2._attributes = new Collection<PromptBuilder.AttributeItem>();
					}
					element2._attributes.Add(new PromptBuilder.AttributeItem("volume", text2));
				}
				ssmlElement = SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput;
				ssmlState |= PromptBuilder.SsmlState.StyleProsody;
			}
			this._elementStack.Push(new PromptBuilder.StackElement(ssmlElement, ssmlState, stackElement._culture));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002AC4 File Offset: 0x00000CC4
		public void EndStyle()
		{
			PromptBuilder.StackElement stackElement = this._elementStack.Pop();
			if (stackElement._state != (PromptBuilder.SsmlState)0)
			{
				if ((stackElement._state & (PromptBuilder.SsmlState)24) == (PromptBuilder.SsmlState)0)
				{
					throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchStyle, new object[0]));
				}
				this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndStyle));
				if (stackElement._state == (PromptBuilder.SsmlState)24)
				{
					this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndStyle));
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002B34 File Offset: 0x00000D34
		public void StartVoice(VoiceInfo voice)
		{
			Helpers.ThrowIfNull(voice, "voice");
			if (!VoiceInfo.ValidateGender(voice.Gender))
			{
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "VoiceGender" }), "voice");
			}
			if (!VoiceInfo.ValidateAge(voice.Age))
			{
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "VoiceAge" }), "voice");
			}
			PromptBuilder.StackElement stackElement = this._elementStack.Peek();
			PromptBuilder.ValidateElement(stackElement, SsmlElement.Voice);
			CultureInfo cultureInfo = ((voice.Culture == null) ? stackElement._culture : voice.Culture);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.StartVoice);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			this._elements.Add(element);
			if (!string.IsNullOrEmpty(voice.Name))
			{
				element._attributes.Add(new PromptBuilder.AttributeItem("name", voice.Name));
			}
			if (voice.Culture != null)
			{
				element._attributes.Add(new PromptBuilder.AttributeItem("xml", "lang", voice.Culture.Name));
			}
			if (voice.Gender != VoiceGender.NotSet)
			{
				element._attributes.Add(new PromptBuilder.AttributeItem("gender", voice.Gender.ToString().ToLowerInvariant()));
			}
			if (voice.Age != VoiceAge.NotSet)
			{
				element._attributes.Add(new PromptBuilder.AttributeItem("age", ((int)voice.Age).ToString(CultureInfo.InvariantCulture)));
			}
			if (voice.Variant >= 0)
			{
				element._attributes.Add(new PromptBuilder.AttributeItem("variant", voice.Variant.ToString(CultureInfo.InvariantCulture)));
			}
			this._elementStack.Push(new PromptBuilder.StackElement(SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput, PromptBuilder.SsmlState.Voice, cultureInfo));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002CF2 File Offset: 0x00000EF2
		public void StartVoice(string name)
		{
			Helpers.ThrowIfEmptyOrNull(name, "name");
			this.StartVoice(new VoiceInfo(name));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D0B File Offset: 0x00000F0B
		public void StartVoice(VoiceGender gender)
		{
			this.StartVoice(new VoiceInfo(gender));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D19 File Offset: 0x00000F19
		public void StartVoice(VoiceGender gender, VoiceAge age)
		{
			this.StartVoice(new VoiceInfo(gender, age));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D28 File Offset: 0x00000F28
		public void StartVoice(VoiceGender gender, VoiceAge age, int voiceAlternate)
		{
			this.StartVoice(new VoiceInfo(gender, age, voiceAlternate));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D38 File Offset: 0x00000F38
		public void StartVoice(CultureInfo culture)
		{
			this.StartVoice(new VoiceInfo(culture));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D46 File Offset: 0x00000F46
		public void EndVoice()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Voice)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchVoice, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndVoice));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002D84 File Offset: 0x00000F84
		public void StartParagraph()
		{
			this.StartParagraph(null);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D90 File Offset: 0x00000F90
		public void StartParagraph(CultureInfo culture)
		{
			PromptBuilder.StackElement stackElement = this._elementStack.Peek();
			PromptBuilder.ValidateElement(stackElement, SsmlElement.Paragraph);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.StartParagraph);
			this._elements.Add(element);
			if (culture != null)
			{
				if (culture.Equals(CultureInfo.InvariantCulture))
				{
					throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
				}
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("xml", "lang", culture.Name));
			}
			else
			{
				culture = stackElement._culture;
			}
			this._elementStack.Push(new PromptBuilder.StackElement(SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Sentence | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput, PromptBuilder.SsmlState.Paragraph, culture));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002E3C File Offset: 0x0000103C
		public void EndParagraph()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Paragraph)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchParagraph, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndParagraph));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002E79 File Offset: 0x00001079
		public void StartSentence()
		{
			this.StartSentence(null);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E84 File Offset: 0x00001084
		public void StartSentence(CultureInfo culture)
		{
			PromptBuilder.StackElement stackElement = this._elementStack.Peek();
			PromptBuilder.ValidateElement(stackElement, SsmlElement.Sentence);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.StartSentence);
			this._elements.Add(element);
			if (culture != null)
			{
				if (culture.Equals(CultureInfo.InvariantCulture))
				{
					throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
				}
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("xml", "lang", culture.Name));
			}
			else
			{
				culture = stackElement._culture;
			}
			this._elementStack.Push(new PromptBuilder.StackElement(SsmlElement.AudioMarkTextWithStyle, PromptBuilder.SsmlState.Sentence, culture));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002F2D File Offset: 0x0000112D
		public void EndSentence()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Sentence)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchSentence, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndSentence));
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F6C File Offset: 0x0000116C
		public void AppendTextWithHint(string textToSpeak, SayAs sayAs)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			if (sayAs < SayAs.SpellOut || sayAs > SayAs.Text)
			{
				throw new ArgumentOutOfRangeException("sayAs");
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			if (sayAs != SayAs.Text)
			{
				PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.SayAs, textToSpeak);
				this._elements.Add(element);
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				string text = null;
				string text2 = null;
				switch (sayAs)
				{
				case SayAs.SpellOut:
					text = "characters";
					break;
				case SayAs.NumberOrdinal:
					text = "ordinal";
					break;
				case SayAs.NumberCardinal:
					text = "cardinal";
					break;
				case SayAs.Date:
					text = "date";
					break;
				case SayAs.DayMonthYear:
					text = "date";
					text2 = "dmy";
					break;
				case SayAs.MonthDayYear:
					text = "date";
					text2 = "mdy";
					break;
				case SayAs.YearMonthDay:
					text = "date";
					text2 = "ymd";
					break;
				case SayAs.YearMonth:
					text = "date";
					text2 = "ym";
					break;
				case SayAs.MonthYear:
					text = "date";
					text2 = "my";
					break;
				case SayAs.MonthDay:
					text = "date";
					text2 = "md";
					break;
				case SayAs.DayMonth:
					text = "date";
					text2 = "dm";
					break;
				case SayAs.Year:
					text = "date";
					text2 = "y";
					break;
				case SayAs.Month:
					text = "date";
					text2 = "m";
					break;
				case SayAs.Day:
					text = "date";
					text2 = "d";
					break;
				case SayAs.Time:
					text = "time";
					break;
				case SayAs.Time24:
					text = "time";
					text2 = "hms24";
					break;
				case SayAs.Time12:
					text = "time";
					text2 = "hms12";
					break;
				case SayAs.Telephone:
					text = "telephone";
					break;
				}
				element._attributes.Add(new PromptBuilder.AttributeItem("interpret-as", text));
				if (!string.IsNullOrEmpty(text2))
				{
					element._attributes.Add(new PromptBuilder.AttributeItem("format", text2));
					return;
				}
			}
			else
			{
				this.AppendText(textToSpeak);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003154 File Offset: 0x00001354
		public void AppendTextWithHint(string textToSpeak, string sayAs)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Helpers.ThrowIfEmptyOrNull(sayAs, "sayAs");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.SayAs, textToSpeak);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("interpret-as", sayAs));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000031C4 File Offset: 0x000013C4
		public void AppendTextWithPronunciation(string textToSpeak, string pronunciation)
		{
			Helpers.ThrowIfEmptyOrNull(textToSpeak, "textToSpeak");
			Helpers.ThrowIfEmptyOrNull(pronunciation, "pronunciation");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PhonemeConverter.ValidateUpsIds(pronunciation);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Phoneme, textToSpeak);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("ph", pronunciation));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003238 File Offset: 0x00001438
		public void AppendTextWithAlias(string textToSpeak, string substitute)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Helpers.ThrowIfNull(substitute, "substitute");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Sub, textToSpeak);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("alias", substitute));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000032A5 File Offset: 0x000014A5
		public void AppendBreak()
		{
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Break);
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.Break));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000032D0 File Offset: 0x000014D0
		public void AppendBreak(PromptBreak strength)
		{
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Break);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Break);
			this._elements.Add(element);
			string text;
			switch (strength)
			{
			case PromptBreak.None:
				text = "none";
				break;
			case PromptBreak.ExtraSmall:
				text = "x-weak";
				break;
			case PromptBreak.Small:
				text = "weak";
				break;
			case PromptBreak.Medium:
				text = "medium";
				break;
			case PromptBreak.Large:
				text = "strong";
				break;
			case PromptBreak.ExtraLarge:
				text = "x-strong";
				break;
			default:
				throw new ArgumentNullException("strength");
			}
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("strength", text));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003384 File Offset: 0x00001584
		public void AppendBreak(TimeSpan duration)
		{
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Break);
			if (duration.Ticks < 0L)
			{
				throw new ArgumentOutOfRangeException("duration");
			}
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Break);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("time", duration.TotalMilliseconds + "ms"));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003408 File Offset: 0x00001608
		public void AppendAudio(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Uri uri;
			try
			{
				uri = new Uri(path, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Audio);
			this.AppendAudio(uri);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003464 File Offset: 0x00001664
		public void AppendAudio(Uri audioFile)
		{
			Helpers.ThrowIfNull(audioFile, "audioFile");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Audio);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Audio);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("src", audioFile.ToString()));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000034C8 File Offset: 0x000016C8
		public void AppendAudio(Uri audioFile, string alternateText)
		{
			Helpers.ThrowIfNull(audioFile, "audioFile");
			Helpers.ThrowIfNull(alternateText, "alternateText");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Audio);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Audio, alternateText);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("src", audioFile.ToString()));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003538 File Offset: 0x00001738
		public void AppendBookmark(string bookmarkName)
		{
			Helpers.ThrowIfEmptyOrNull(bookmarkName, "bookmarkName");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Mark);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Bookmark);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("name", bookmarkName));
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000359C File Offset: 0x0000179C
		public void AppendPromptBuilder(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			StringReader stringReader = new StringReader(promptBuilder.ToXml());
			XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
			this.AppendSsml(xmlTextReader);
			xmlTextReader.Close();
			stringReader.Close();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000035DA File Offset: 0x000017DA
		public void AppendSsml(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.AppendSsml(new Uri(path, UriKind.Relative));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000035F4 File Offset: 0x000017F4
		public void AppendSsml(Uri ssmlFile)
		{
			Helpers.ThrowIfNull(ssmlFile, "ssmlFile");
			string text;
			Uri uri;
			using (Stream stream = PromptBuilder._resourceLoader.LoadFile(ssmlFile, out text, out uri))
			{
				try
				{
					this.AppendSsml(new XmlTextReader(stream));
				}
				finally
				{
					PromptBuilder._resourceLoader.UnloadFile(text);
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000365C File Offset: 0x0000185C
		public void AppendSsml(XmlReader ssmlFile)
		{
			Helpers.ThrowIfNull(ssmlFile, "ssmlFile");
			this.AppendSsmlInternal(ssmlFile);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003670 File Offset: 0x00001870
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AppendSsmlMarkup(string ssmlMarkup)
		{
			Helpers.ThrowIfEmptyOrNull(ssmlMarkup, "ssmlMarkup");
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.SsmlMarkup, ssmlMarkup));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003690 File Offset: 0x00001890
		public string ToXml()
		{
			string text2;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
				{
					this.WriteXml(xmlTextWriter);
					PromptBuilder.SsmlState state = this._elementStack.Peek()._state;
					if (state != PromptBuilder.SsmlState.Header)
					{
						string text = SR.Get(SRID.PromptBuilderInvalideState, new object[0]);
						if (state <= PromptBuilder.SsmlState.StyleEmphasis)
						{
							if (state == PromptBuilder.SsmlState.Paragraph)
							{
								text += SR.Get(SRID.PromptBuilderStateParagraph, new object[0]);
								goto IL_F8;
							}
							if (state == PromptBuilder.SsmlState.Sentence)
							{
								text += SR.Get(SRID.PromptBuilderStateSentence, new object[0]);
								goto IL_F8;
							}
							if (state != PromptBuilder.SsmlState.StyleEmphasis)
							{
								goto IL_F2;
							}
						}
						else if (state <= (PromptBuilder.SsmlState)24)
						{
							if (state != PromptBuilder.SsmlState.StyleProsody && state != (PromptBuilder.SsmlState)24)
							{
								goto IL_F2;
							}
						}
						else
						{
							if (state == PromptBuilder.SsmlState.Voice)
							{
								text += SR.Get(SRID.PromptBuilderStateVoice, new object[0]);
								goto IL_F8;
							}
							if (state == PromptBuilder.SsmlState.Ended)
							{
								text += SR.Get(SRID.PromptBuilderStateEnded, new object[0]);
								goto IL_F8;
							}
							goto IL_F2;
						}
						text += SR.Get(SRID.PromptBuilderStateStyle, new object[0]);
						goto IL_F8;
						IL_F2:
						throw new NotSupportedException();
						IL_F8:
						throw new InvalidOperationException(text);
					}
					text2 = stringWriter.ToString();
				}
			}
			return text2;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000037F0 File Offset: 0x000019F0
		public bool IsEmpty
		{
			get
			{
				return this._elements.Count == 0;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003817 File Offset: 0x00001A17
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003800 File Offset: 0x00001A00
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._culture = value;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003820 File Offset: 0x00001A20
		private void WriteXml(XmlTextWriter writer)
		{
			writer.WriteStartElement("speak");
			writer.WriteAttributeString("version", "1.0");
			writer.WriteAttributeString("xmlns", "http://www.w3.org/2001/10/synthesis");
			writer.WriteAttributeString("xml", "lang", null, this._culture.Name);
			bool flag = false;
			foreach (PromptBuilder.Element element in this._elements)
			{
				flag = flag || element._type == PromptBuilder.ElementType.StartSentence || element._type == PromptBuilder.ElementType.StartParagraph || element._type == PromptBuilder.ElementType.StartStyle || element._type == PromptBuilder.ElementType.StartVoice;
				switch (element._type)
				{
				case PromptBuilder.ElementType.Prosody:
				case PromptBuilder.ElementType.Emphasis:
				case PromptBuilder.ElementType.SayAs:
				case PromptBuilder.ElementType.Phoneme:
				case PromptBuilder.ElementType.Sub:
				case PromptBuilder.ElementType.Break:
				case PromptBuilder.ElementType.Audio:
				case PromptBuilder.ElementType.Bookmark:
				case PromptBuilder.ElementType.StartVoice:
				case PromptBuilder.ElementType.StartParagraph:
				case PromptBuilder.ElementType.StartSentence:
					writer.WriteStartElement(PromptBuilder._promptBuilderElementName[(int)element._type]);
					if (element._attributes != null)
					{
						foreach (PromptBuilder.AttributeItem attributeItem in element._attributes)
						{
							if (attributeItem._namespace == null)
							{
								writer.WriteAttributeString(attributeItem._key, attributeItem._value);
							}
							else
							{
								writer.WriteAttributeString(attributeItem._namespace, attributeItem._key, null, attributeItem._value);
							}
						}
					}
					if (element._text != null)
					{
						writer.WriteString(element._text);
					}
					if (!flag)
					{
						writer.WriteEndElement();
					}
					flag = false;
					break;
				case PromptBuilder.ElementType.EndSentence:
				case PromptBuilder.ElementType.EndParagraph:
				case PromptBuilder.ElementType.EndStyle:
				case PromptBuilder.ElementType.EndVoice:
					writer.WriteEndElement();
					break;
				case PromptBuilder.ElementType.StartStyle:
					break;
				case PromptBuilder.ElementType.Text:
					writer.WriteString(element._text);
					break;
				case PromptBuilder.ElementType.SsmlMarkup:
					writer.WriteRaw(element._text);
					break;
				default:
					throw new NotSupportedException();
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003A44 File Offset: 0x00001C44
		private static void ValidateElement(PromptBuilder.StackElement stackElement, SsmlElement currentElement)
		{
			if ((stackElement._possibleChildren & currentElement) == (SsmlElement)0)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SR.Get(SRID.PromptBuilderInvalidElement, new object[0]), new object[]
				{
					currentElement.ToString(),
					stackElement._state.ToString()
				}));
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003AA8 File Offset: 0x00001CA8
		private void AppendSsmlInternal(XmlReader ssmlFile)
		{
			PromptBuilder.StackElement stackElement = this._elementStack.Peek();
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Voice);
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
				{
					TextWriterEngine textWriterEngine = new TextWriterEngine(xmlTextWriter, stackElement._culture);
					SsmlParser.Parse(ssmlFile, textWriterEngine, null);
				}
				this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.SsmlMarkup, stringWriter.ToString()));
			}
		}

		// Token: 0x0400019F RID: 415
		private Stack<PromptBuilder.StackElement> _elementStack = new Stack<PromptBuilder.StackElement>();

		// Token: 0x040001A0 RID: 416
		private CultureInfo _culture;

		// Token: 0x040001A1 RID: 417
		private List<PromptBuilder.Element> _elements = new List<PromptBuilder.Element>();

		// Token: 0x040001A2 RID: 418
		private static ResourceLoader _resourceLoader = new ResourceLoader();

		// Token: 0x040001A3 RID: 419
		private const string _xmlnsDefault = "http://www.w3.org/2001/10/synthesis";

		// Token: 0x040001A4 RID: 420
		private static readonly string[] _promptBuilderElementName = new string[]
		{
			"prosody", "emphasis", "say-as", "phoneme", "sub", "break", "audio", "mark", "voice", "p",
			"s"
		};

		// Token: 0x02000171 RID: 369
		internal enum SsmlState
		{
			// Token: 0x04000877 RID: 2167
			Header = 1,
			// Token: 0x04000878 RID: 2168
			Paragraph,
			// Token: 0x04000879 RID: 2169
			Sentence = 4,
			// Token: 0x0400087A RID: 2170
			StyleEmphasis = 8,
			// Token: 0x0400087B RID: 2171
			StyleProsody = 16,
			// Token: 0x0400087C RID: 2172
			Voice = 32,
			// Token: 0x0400087D RID: 2173
			Ended = 64
		}

		// Token: 0x02000172 RID: 370
		[Serializable]
		private struct StackElement
		{
			// Token: 0x06000B3A RID: 2874 RVA: 0x0002D198 File Offset: 0x0002B398
			internal StackElement(SsmlElement possibleChildren, PromptBuilder.SsmlState state, CultureInfo culture)
			{
				this._possibleChildren = possibleChildren;
				this._state = state;
				this._culture = culture;
			}

			// Token: 0x0400087E RID: 2174
			internal SsmlElement _possibleChildren;

			// Token: 0x0400087F RID: 2175
			internal PromptBuilder.SsmlState _state;

			// Token: 0x04000880 RID: 2176
			internal CultureInfo _culture;
		}

		// Token: 0x02000173 RID: 371
		private enum ElementType
		{
			// Token: 0x04000882 RID: 2178
			Prosody,
			// Token: 0x04000883 RID: 2179
			Emphasis,
			// Token: 0x04000884 RID: 2180
			SayAs,
			// Token: 0x04000885 RID: 2181
			Phoneme,
			// Token: 0x04000886 RID: 2182
			Sub,
			// Token: 0x04000887 RID: 2183
			Break,
			// Token: 0x04000888 RID: 2184
			Audio,
			// Token: 0x04000889 RID: 2185
			Bookmark,
			// Token: 0x0400088A RID: 2186
			StartVoice,
			// Token: 0x0400088B RID: 2187
			StartParagraph,
			// Token: 0x0400088C RID: 2188
			StartSentence,
			// Token: 0x0400088D RID: 2189
			EndSentence,
			// Token: 0x0400088E RID: 2190
			EndParagraph,
			// Token: 0x0400088F RID: 2191
			StartStyle,
			// Token: 0x04000890 RID: 2192
			EndStyle,
			// Token: 0x04000891 RID: 2193
			EndVoice,
			// Token: 0x04000892 RID: 2194
			Text,
			// Token: 0x04000893 RID: 2195
			SsmlMarkup
		}

		// Token: 0x02000174 RID: 372
		[Serializable]
		private struct AttributeItem
		{
			// Token: 0x06000B3B RID: 2875 RVA: 0x0002D1AF File Offset: 0x0002B3AF
			internal AttributeItem(string key, string value)
			{
				this._key = key;
				this._value = value;
				this._namespace = null;
			}

			// Token: 0x06000B3C RID: 2876 RVA: 0x0002D1C6 File Offset: 0x0002B3C6
			internal AttributeItem(string ns, string key, string value)
			{
				this = new PromptBuilder.AttributeItem(key, value);
				this._namespace = ns;
			}

			// Token: 0x04000894 RID: 2196
			internal string _key;

			// Token: 0x04000895 RID: 2197
			internal string _value;

			// Token: 0x04000896 RID: 2198
			internal string _namespace;
		}

		// Token: 0x02000175 RID: 373
		[Serializable]
		private class Element
		{
			// Token: 0x06000B3D RID: 2877 RVA: 0x0002D1D7 File Offset: 0x0002B3D7
			internal Element(PromptBuilder.ElementType type)
			{
				this._type = type;
			}

			// Token: 0x06000B3E RID: 2878 RVA: 0x0002D1E6 File Offset: 0x0002B3E6
			internal Element(PromptBuilder.ElementType type, string text)
				: this(type)
			{
				this._text = text;
			}

			// Token: 0x04000897 RID: 2199
			internal PromptBuilder.ElementType _type;

			// Token: 0x04000898 RID: 2200
			internal string _text;

			// Token: 0x04000899 RID: 2201
			internal Collection<PromptBuilder.AttributeItem> _attributes;
		}
	}
}
