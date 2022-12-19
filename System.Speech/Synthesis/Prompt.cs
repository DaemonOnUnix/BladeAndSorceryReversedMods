using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Internal;

namespace System.Speech.Synthesis
{
	// Token: 0x0200013A RID: 314
	[DebuggerDisplay("{_text}")]
	public class Prompt
	{
		// Token: 0x0600085B RID: 2139 RVA: 0x000259C5 File Offset: 0x000249C5
		public Prompt(string textToSpeak)
			: this(textToSpeak, SynthesisTextFormat.Text)
		{
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000259CF File Offset: 0x000249CF
		public Prompt(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			this._text = promptBuilder.ToXml();
			this._media = SynthesisMediaType.Ssml;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000259F8 File Offset: 0x000249F8
		public Prompt(string textToSpeak, SynthesisTextFormat media)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			this._media = (SynthesisMediaType)media;
			switch (media)
			{
			case SynthesisTextFormat.Text:
			case SynthesisTextFormat.Ssml:
				this._text = textToSpeak;
				return;
			default:
				throw new ArgumentException(SR.Get(SRID.SynthesizerUnknownMediaType, new object[0]), "media");
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00025A54 File Offset: 0x00024A54
		internal Prompt(Uri promptFile, SynthesisMediaType media)
		{
			Helpers.ThrowIfNull(promptFile, "promptFile");
			this._media = media;
			switch (media)
			{
			case SynthesisMediaType.Text:
			case SynthesisMediaType.Ssml:
			{
				string text;
				Uri uri;
				string text2;
				using (Stream stream = Prompt._resourceLoader.LoadFile(promptFile, out text, out uri, out text2))
				{
					try
					{
						using (TextReader textReader = new StreamReader(stream))
						{
							this._text = textReader.ReadToEnd();
						}
					}
					finally
					{
						Prompt._resourceLoader.UnloadFile(text2);
					}
					return;
				}
				break;
			}
			case SynthesisMediaType.WaveAudio:
				break;
			default:
				throw new ArgumentException(SR.Get(SRID.SynthesizerUnknownMediaType, new object[0]), "media");
			}
			this._text = promptFile.ToString();
			this._audio = promptFile;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00025B3C File Offset: 0x00024B3C
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00025B44 File Offset: 0x00024B44
		public bool IsCompleted
		{
			get
			{
				return this._completed;
			}
			internal set
			{
				this._completed = value;
			}
		}

		// Token: 0x17000179 RID: 377
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x00025B4D File Offset: 0x00024B4D
		internal object Synthesizer
		{
			set
			{
				if (value != null && (this._synthesizer != null || this._completed))
				{
					throw new ArgumentException(SR.Get(SRID.SynthesizerPromptInUse, new object[0]), "synthesizer");
				}
				this._synthesizer = value;
			}
		}

		// Token: 0x040005EB RID: 1515
		internal string _text;

		// Token: 0x040005EC RID: 1516
		internal Uri _audio;

		// Token: 0x040005ED RID: 1517
		internal SynthesisMediaType _media;

		// Token: 0x040005EE RID: 1518
		internal bool _syncSpeak;

		// Token: 0x040005EF RID: 1519
		internal Exception _exception;

		// Token: 0x040005F0 RID: 1520
		private bool _completed;

		// Token: 0x040005F1 RID: 1521
		private object _synthesizer;

		// Token: 0x040005F2 RID: 1522
		private static ResourceLoader _resourceLoader = new ResourceLoader();
	}
}
