using System;
using System.Diagnostics;

namespace System.Speech.Recognition
{
	// Token: 0x02000134 RID: 308
	[DebuggerDisplay("Text: {Text}")]
	[Serializable]
	public class RecognizedWordUnit
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x00025588 File Offset: 0x00024588
		public RecognizedWordUnit(string text, float confidence, string pronunciation, string lexicalForm, DisplayAttributes displayAttributes, TimeSpan audioPosition, TimeSpan audioDuration)
		{
			if (lexicalForm == null)
			{
				throw new ArgumentNullException("lexicalForm");
			}
			if (confidence < 0f || confidence > 1f)
			{
				throw new ArgumentOutOfRangeException(SR.Get(SRID.InvalidConfidence, new object[0]));
			}
			this._text = ((text == null || text.Length == 0) ? null : text);
			this._confidence = confidence;
			this._pronunciation = ((pronunciation == null || pronunciation.Length == 0) ? null : pronunciation);
			this._lexicalForm = lexicalForm;
			this._displayAttributes = displayAttributes;
			this._audioPosition = audioPosition;
			this._audioDuration = audioDuration;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0002561E File Offset: 0x0002461E
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00025626 File Offset: 0x00024626
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x0002562E File Offset: 0x0002462E
		public string Pronunciation
		{
			get
			{
				return this._pronunciation;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00025636 File Offset: 0x00024636
		public string LexicalForm
		{
			get
			{
				return this._lexicalForm;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0002563E File Offset: 0x0002463E
		public DisplayAttributes DisplayAttributes
		{
			get
			{
				return this._displayAttributes;
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00025646 File Offset: 0x00024646
		internal static byte DisplayAttributesToSapiAttributes(DisplayAttributes displayAttributes)
		{
			return (byte)(displayAttributes >> 1);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002564C File Offset: 0x0002464C
		internal static DisplayAttributes SapiAttributesToDisplayAttributes(byte sapiAttributes)
		{
			return (DisplayAttributes)(sapiAttributes << 1);
		}

		// Token: 0x040005D2 RID: 1490
		internal TimeSpan _audioPosition;

		// Token: 0x040005D3 RID: 1491
		internal TimeSpan _audioDuration;

		// Token: 0x040005D4 RID: 1492
		private string _text;

		// Token: 0x040005D5 RID: 1493
		private string _lexicalForm;

		// Token: 0x040005D6 RID: 1494
		private float _confidence;

		// Token: 0x040005D7 RID: 1495
		private string _pronunciation;

		// Token: 0x040005D8 RID: 1496
		private DisplayAttributes _displayAttributes;
	}
}
