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
	// Token: 0x02000140 RID: 320
	[Serializable]
	public class PromptBuilder
	{
		// Token: 0x0600086E RID: 2158 RVA: 0x00025DD0 File Offset: 0x00024DD0
		public PromptBuilder()
			: this(CultureInfo.CurrentUICulture)
		{
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00025DE0 File Offset: 0x00024DE0
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

		// Token: 0x06000870 RID: 2160 RVA: 0x00025E46 File Offset: 0x00024E46
		public void ClearContent()
		{
			this._elements.Clear();
			this._elementStack.Push(new PromptBuilder.StackElement(SsmlElement.Voice | SsmlElement.Audio | SsmlElement.Lexicon | SsmlElement.Meta | SsmlElement.MetaData | SsmlElement.Sentence | SsmlElement.Paragraph | SsmlElement.SayAs | SsmlElement.Phoneme | SsmlElement.Sub | SsmlElement.Emphasis | SsmlElement.Break | SsmlElement.Prosody | SsmlElement.Mark | SsmlElement.Text | SsmlElement.PromptEngineOutput, PromptBuilder.SsmlState.Header, this._culture));
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00025E6F File Offset: 0x00024E6F
		public void AppendText(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Text);
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.Text, textToSpeak));
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00025EA4 File Offset: 0x00024EA4
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
			switch (rate)
			{
			case PromptRate.NotSet:
				break;
			case PromptRate.ExtraFast:
				text = "x-fast";
				break;
			default:
				if (rate != PromptRate.ExtraSlow)
				{
					text = rate.ToString().ToLowerInvariant();
				}
				else
				{
					text = "x-slow";
				}
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("rate", text));
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00025F5C File Offset: 0x00024F5C
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
			switch (volume)
			{
			case PromptVolume.NotSet:
				goto IL_84;
			case PromptVolume.Silent:
				break;
			case PromptVolume.ExtraSoft:
				text = "x-soft";
				goto IL_84;
			default:
				if (volume == PromptVolume.ExtraLoud)
				{
					text = "x-loud";
					goto IL_84;
				}
				break;
			}
			text = volume.ToString().ToLowerInvariant();
			IL_84:
			if (!string.IsNullOrEmpty(text))
			{
				element._attributes = new Collection<PromptBuilder.AttributeItem>();
				element._attributes.Add(new PromptBuilder.AttributeItem("volume", text));
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00026018 File Offset: 0x00025018
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

		// Token: 0x06000875 RID: 2165 RVA: 0x000260A0 File Offset: 0x000250A0
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

		// Token: 0x06000876 RID: 2166 RVA: 0x00026280 File Offset: 0x00025280
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

		// Token: 0x06000877 RID: 2167 RVA: 0x000262F4 File Offset: 0x000252F4
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

		// Token: 0x06000878 RID: 2168 RVA: 0x000264B6 File Offset: 0x000254B6
		public void StartVoice(string name)
		{
			Helpers.ThrowIfEmptyOrNull(name, "name");
			this.StartVoice(new VoiceInfo(name));
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000264CF File Offset: 0x000254CF
		public void StartVoice(VoiceGender gender)
		{
			this.StartVoice(new VoiceInfo(gender));
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000264DD File Offset: 0x000254DD
		public void StartVoice(VoiceGender gender, VoiceAge age)
		{
			this.StartVoice(new VoiceInfo(gender, age));
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000264EC File Offset: 0x000254EC
		public void StartVoice(VoiceGender gender, VoiceAge age, int voiceAlternate)
		{
			this.StartVoice(new VoiceInfo(gender, age, voiceAlternate));
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000264FC File Offset: 0x000254FC
		public void StartVoice(CultureInfo culture)
		{
			this.StartVoice(new VoiceInfo(culture));
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002650A File Offset: 0x0002550A
		public void EndVoice()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Voice)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchVoice, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndVoice));
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00026548 File Offset: 0x00025548
		public void StartParagraph()
		{
			this.StartParagraph(null);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00026554 File Offset: 0x00025554
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

		// Token: 0x06000880 RID: 2176 RVA: 0x00026601 File Offset: 0x00025601
		public void EndParagraph()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Paragraph)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchParagraph, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndParagraph));
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0002663E File Offset: 0x0002563E
		public void StartSentence()
		{
			this.StartSentence(null);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00026648 File Offset: 0x00025648
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

		// Token: 0x06000883 RID: 2179 RVA: 0x000266F2 File Offset: 0x000256F2
		public void EndSentence()
		{
			if (this._elementStack.Pop()._state != PromptBuilder.SsmlState.Sentence)
			{
				throw new InvalidOperationException(SR.Get(SRID.PromptBuilderMismatchSentence, new object[0]));
			}
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.EndSentence));
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00026730 File Offset: 0x00025730
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

		// Token: 0x06000885 RID: 2181 RVA: 0x00026918 File Offset: 0x00025918
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

		// Token: 0x06000886 RID: 2182 RVA: 0x00026988 File Offset: 0x00025988
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

		// Token: 0x06000887 RID: 2183 RVA: 0x000269FC File Offset: 0x000259FC
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

		// Token: 0x06000888 RID: 2184 RVA: 0x00026A69 File Offset: 0x00025A69
		public void AppendBreak()
		{
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Break);
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.Break));
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00026A94 File Offset: 0x00025A94
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

		// Token: 0x0600088A RID: 2186 RVA: 0x00026B4C File Offset: 0x00025B4C
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

		// Token: 0x0600088B RID: 2187 RVA: 0x00026BD0 File Offset: 0x00025BD0
		public void AppendAudio(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Uri uri;
			try
			{
				uri = new Uri(path, 0);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Audio);
			this.AppendAudio(uri);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00026C2C File Offset: 0x00025C2C
		public void AppendAudio(Uri audioFile)
		{
			Helpers.ThrowIfNull(audioFile, "audioFile");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Audio);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Audio);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("src", audioFile.ToString()));
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00026C90 File Offset: 0x00025C90
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

		// Token: 0x0600088E RID: 2190 RVA: 0x00026D00 File Offset: 0x00025D00
		public void AppendBookmark(string bookmarkName)
		{
			Helpers.ThrowIfEmptyOrNull(bookmarkName, "bookmarkName");
			PromptBuilder.ValidateElement(this._elementStack.Peek(), SsmlElement.Mark);
			PromptBuilder.Element element = new PromptBuilder.Element(PromptBuilder.ElementType.Bookmark);
			this._elements.Add(element);
			element._attributes = new Collection<PromptBuilder.AttributeItem>();
			element._attributes.Add(new PromptBuilder.AttributeItem("name", bookmarkName));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00026D64 File Offset: 0x00025D64
		public void AppendPromptBuilder(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			StringReader stringReader = new StringReader(promptBuilder.ToXml());
			XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
			this.AppendSsml(xmlTextReader);
			xmlTextReader.Close();
			stringReader.Close();
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00026DA2 File Offset: 0x00025DA2
		public void AppendSsml(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.AppendSsml(new Uri(path, 2));
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00026DBC File Offset: 0x00025DBC
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

		// Token: 0x06000892 RID: 2194 RVA: 0x00026E28 File Offset: 0x00025E28
		public void AppendSsml(XmlReader ssmlFile)
		{
			Helpers.ThrowIfNull(ssmlFile, "ssmlFile");
			this.AppendSsmlInternal(ssmlFile);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00026E3C File Offset: 0x00025E3C
		[EditorBrowsable(2)]
		public void AppendSsmlMarkup(string ssmlMarkup)
		{
			Helpers.ThrowIfEmptyOrNull(ssmlMarkup, "ssmlMarkup");
			this._elements.Add(new PromptBuilder.Element(PromptBuilder.ElementType.SsmlMarkup, ssmlMarkup));
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00026E5C File Offset: 0x00025E5C
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
						PromptBuilder.SsmlState ssmlState = state;
						if (ssmlState <= PromptBuilder.SsmlState.StyleProsody)
						{
							switch (ssmlState)
							{
							case PromptBuilder.SsmlState.Paragraph:
								text += SR.Get(SRID.PromptBuilderStateParagraph, new object[0]);
								goto IL_105;
							case (PromptBuilder.SsmlState)3:
								goto IL_FF;
							case PromptBuilder.SsmlState.Sentence:
								text += SR.Get(SRID.PromptBuilderStateSentence, new object[0]);
								goto IL_105;
							default:
								if (ssmlState != PromptBuilder.SsmlState.StyleEmphasis && ssmlState != PromptBuilder.SsmlState.StyleProsody)
								{
									goto IL_FF;
								}
								break;
							}
						}
						else if (ssmlState != (PromptBuilder.SsmlState)24)
						{
							if (ssmlState == PromptBuilder.SsmlState.Voice)
							{
								text += SR.Get(SRID.PromptBuilderStateVoice, new object[0]);
								goto IL_105;
							}
							if (ssmlState == PromptBuilder.SsmlState.Ended)
							{
								text += SR.Get(SRID.PromptBuilderStateEnded, new object[0]);
								goto IL_105;
							}
							goto IL_FF;
						}
						text += SR.Get(SRID.PromptBuilderStateStyle, new object[0]);
						goto IL_105;
						IL_FF:
						throw new NotSupportedException();
						IL_105:
						throw new InvalidOperationException(text);
					}
					text2 = stringWriter.ToString();
				}
			}
			return text2;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00026FCC File Offset: 0x00025FCC
		public bool IsEmpty
		{
			get
			{
				return this._elements.Count == 0;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00026FF3 File Offset: 0x00025FF3
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x00026FDC File Offset: 0x00025FDC
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

		// Token: 0x06000898 RID: 2200 RVA: 0x00026FFC File Offset: 0x00025FFC
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

		// Token: 0x06000899 RID: 2201 RVA: 0x00027220 File Offset: 0x00026220
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

		// Token: 0x0600089A RID: 2202 RVA: 0x00027284 File Offset: 0x00026284
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

		// Token: 0x04000600 RID: 1536
		private const string _xmlnsDefault = "http://www.w3.org/2001/10/synthesis";

		// Token: 0x04000601 RID: 1537
		private Stack<PromptBuilder.StackElement> _elementStack = new Stack<PromptBuilder.StackElement>();

		// Token: 0x04000602 RID: 1538
		private CultureInfo _culture;

		// Token: 0x04000603 RID: 1539
		private List<PromptBuilder.Element> _elements = new List<PromptBuilder.Element>();

		// Token: 0x04000604 RID: 1540
		private static ResourceLoader _resourceLoader = new ResourceLoader();

		// Token: 0x04000605 RID: 1541
		private static readonly string[] _promptBuilderElementName = new string[]
		{
			"prosody", "emphasis", "say-as", "phoneme", "sub", "break", "audio", "mark", "voice", "p",
			"s"
		};

		// Token: 0x02000141 RID: 321
		internal enum SsmlState
		{
			// Token: 0x04000607 RID: 1543
			Header = 1,
			// Token: 0x04000608 RID: 1544
			Paragraph,
			// Token: 0x04000609 RID: 1545
			Sentence = 4,
			// Token: 0x0400060A RID: 1546
			StyleEmphasis = 8,
			// Token: 0x0400060B RID: 1547
			StyleProsody = 16,
			// Token: 0x0400060C RID: 1548
			Voice = 32,
			// Token: 0x0400060D RID: 1549
			Ended = 64
		}

		// Token: 0x02000142 RID: 322
		[Serializable]
		private struct StackElement
		{
			// Token: 0x0600089C RID: 2204 RVA: 0x000273A3 File Offset: 0x000263A3
			internal StackElement(SsmlElement possibleChildren, PromptBuilder.SsmlState state, CultureInfo culture)
			{
				this._possibleChildren = possibleChildren;
				this._state = state;
				this._culture = culture;
			}

			// Token: 0x0400060E RID: 1550
			internal SsmlElement _possibleChildren;

			// Token: 0x0400060F RID: 1551
			internal PromptBuilder.SsmlState _state;

			// Token: 0x04000610 RID: 1552
			internal CultureInfo _culture;
		}

		// Token: 0x02000143 RID: 323
		private enum ElementType
		{
			// Token: 0x04000612 RID: 1554
			Prosody,
			// Token: 0x04000613 RID: 1555
			Emphasis,
			// Token: 0x04000614 RID: 1556
			SayAs,
			// Token: 0x04000615 RID: 1557
			Phoneme,
			// Token: 0x04000616 RID: 1558
			Sub,
			// Token: 0x04000617 RID: 1559
			Break,
			// Token: 0x04000618 RID: 1560
			Audio,
			// Token: 0x04000619 RID: 1561
			Bookmark,
			// Token: 0x0400061A RID: 1562
			StartVoice,
			// Token: 0x0400061B RID: 1563
			StartParagraph,
			// Token: 0x0400061C RID: 1564
			StartSentence,
			// Token: 0x0400061D RID: 1565
			EndSentence,
			// Token: 0x0400061E RID: 1566
			EndParagraph,
			// Token: 0x0400061F RID: 1567
			StartStyle,
			// Token: 0x04000620 RID: 1568
			EndStyle,
			// Token: 0x04000621 RID: 1569
			EndVoice,
			// Token: 0x04000622 RID: 1570
			Text,
			// Token: 0x04000623 RID: 1571
			SsmlMarkup
		}

		// Token: 0x02000144 RID: 324
		[Serializable]
		private struct AttributeItem
		{
			// Token: 0x0600089D RID: 2205 RVA: 0x000273BA File Offset: 0x000263BA
			internal AttributeItem(string key, string value)
			{
				this._key = key;
				this._value = value;
				this._namespace = null;
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x000273D1 File Offset: 0x000263D1
			internal AttributeItem(string ns, string key, string value)
			{
				this = new PromptBuilder.AttributeItem(key, value);
				this._namespace = ns;
			}

			// Token: 0x04000624 RID: 1572
			internal string _key;

			// Token: 0x04000625 RID: 1573
			internal string _value;

			// Token: 0x04000626 RID: 1574
			internal string _namespace;
		}

		// Token: 0x02000145 RID: 325
		[Serializable]
		private class Element
		{
			// Token: 0x0600089F RID: 2207 RVA: 0x000273E2 File Offset: 0x000263E2
			internal Element(PromptBuilder.ElementType type)
			{
				this._type = type;
			}

			// Token: 0x060008A0 RID: 2208 RVA: 0x000273F1 File Offset: 0x000263F1
			internal Element(PromptBuilder.ElementType type, string text)
				: this(type)
			{
				this._text = text;
			}

			// Token: 0x04000627 RID: 1575
			internal PromptBuilder.ElementType _type;

			// Token: 0x04000628 RID: 1576
			internal string _text;

			// Token: 0x04000629 RID: 1577
			internal Collection<PromptBuilder.AttributeItem> _attributes;
		}
	}
}
