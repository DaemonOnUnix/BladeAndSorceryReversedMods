using System;
using System.Diagnostics;

namespace System.Speech.Recognition
{
	// Token: 0x02000053 RID: 83
	[DebuggerDisplay("Text: {Text}")]
	[Serializable]
	public class RecognizedWordUnit
	{
		// Token: 0x060001CC RID: 460 RVA: 0x000090D8 File Offset: 0x000072D8
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000916E File Offset: 0x0000736E
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009176 File Offset: 0x00007376
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000917E File Offset: 0x0000737E
		public string Pronunciation
		{
			get
			{
				return this._pronunciation;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00009186 File Offset: 0x00007386
		public string LexicalForm
		{
			get
			{
				return this._lexicalForm;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000918E File Offset: 0x0000738E
		public DisplayAttributes DisplayAttributes
		{
			get
			{
				return this._displayAttributes;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009196 File Offset: 0x00007396
		internal static byte DisplayAttributesToSapiAttributes(DisplayAttributes displayAttributes)
		{
			return (byte)(displayAttributes >> 1);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000919C File Offset: 0x0000739C
		internal static DisplayAttributes SapiAttributesToDisplayAttributes(byte sapiAttributes)
		{
			return (DisplayAttributes)(sapiAttributes << 1);
		}

		// Token: 0x0400031C RID: 796
		internal TimeSpan _audioPosition;

		// Token: 0x0400031D RID: 797
		internal TimeSpan _audioDuration;

		// Token: 0x0400031E RID: 798
		private string _text;

		// Token: 0x0400031F RID: 799
		private string _lexicalForm;

		// Token: 0x04000320 RID: 800
		private float _confidence;

		// Token: 0x04000321 RID: 801
		private string _pronunciation;

		// Token: 0x04000322 RID: 802
		private DisplayAttributes _displayAttributes;
	}
}
