using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Internal;

namespace System.Speech.Synthesis
{
	// Token: 0x02000008 RID: 8
	[DebuggerDisplay("{_text}")]
	public class Prompt
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002468 File Offset: 0x00000668
		public Prompt(string textToSpeak)
			: this(textToSpeak, SynthesisTextFormat.Text)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002472 File Offset: 0x00000672
		public Prompt(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			this._text = promptBuilder.ToXml();
			this._media = SynthesisMediaType.Ssml;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002498 File Offset: 0x00000698
		public Prompt(string textToSpeak, SynthesisTextFormat media)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			this._media = (SynthesisMediaType)media;
			if (media <= SynthesisTextFormat.Ssml)
			{
				this._text = textToSpeak;
				return;
			}
			throw new ArgumentException(SR.Get(SRID.SynthesizerUnknownMediaType, new object[0]), "media");
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024E8 File Offset: 0x000006E8
		internal Prompt(Uri promptFile, SynthesisMediaType media)
		{
			Helpers.ThrowIfNull(promptFile, "promptFile");
			this._media = media;
			if (media > SynthesisMediaType.Ssml)
			{
				if (media != SynthesisMediaType.WaveAudio)
				{
					throw new ArgumentException(SR.Get(SRID.SynthesizerUnknownMediaType, new object[0]), "media");
				}
			}
			else
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
							return;
						}
					}
					finally
					{
						Prompt._resourceLoader.UnloadFile(text2);
					}
				}
			}
			this._text = promptFile.ToString();
			this._audio = promptFile;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000025C4 File Offset: 0x000007C4
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000025CC File Offset: 0x000007CC
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

		// Token: 0x17000006 RID: 6
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000025D5 File Offset: 0x000007D5
		internal object Synthesizer
		{
			set
			{
				if (value != null && (this._synthesizer != null || this._completed))
				{
					throw new ArgumentException(SR.Get(SRID.SynthesizerPromptInUse, new object[0]), "value");
				}
				this._synthesizer = value;
			}
		}

		// Token: 0x0400018D RID: 397
		internal string _text;

		// Token: 0x0400018E RID: 398
		internal Uri _audio;

		// Token: 0x0400018F RID: 399
		internal SynthesisMediaType _media;

		// Token: 0x04000190 RID: 400
		internal bool _syncSpeak;

		// Token: 0x04000191 RID: 401
		internal Exception _exception;

		// Token: 0x04000192 RID: 402
		private bool _completed;

		// Token: 0x04000193 RID: 403
		private object _synthesizer;

		// Token: 0x04000194 RID: 404
		private static ResourceLoader _resourceLoader = new ResourceLoader();
	}
}
