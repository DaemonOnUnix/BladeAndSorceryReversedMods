using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000165 RID: 357
	[StructLayout(0)]
	public class TextFragment
	{
		// Token: 0x06000924 RID: 2340 RVA: 0x000282B2 File Offset: 0x000272B2
		public TextFragment()
		{
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x000282C5 File Offset: 0x000272C5
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x000282CD File Offset: 0x000272CD
		public FragmentState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x000282D6 File Offset: 0x000272D6
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x000282DE File Offset: 0x000272DE
		public string TextToSpeak
		{
			get
			{
				return this._textToSpeak;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._textToSpeak = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000282F2 File Offset: 0x000272F2
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x000282FA File Offset: 0x000272FA
		public int TextOffset
		{
			get
			{
				return this._textOffset;
			}
			set
			{
				this._textOffset = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00028303 File Offset: 0x00027303
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0002830B File Offset: 0x0002730B
		public int TextLength
		{
			get
			{
				return this._textLength;
			}
			set
			{
				this._textLength = value;
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00028314 File Offset: 0x00027314
		internal TextFragment(FragmentState fragState)
			: this(fragState, null, null, 0, 0)
		{
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00028321 File Offset: 0x00027321
		internal TextFragment(FragmentState fragState, string textToSpeak)
			: this(fragState, textToSpeak, textToSpeak, 0, textToSpeak.Length)
		{
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00028334 File Offset: 0x00027334
		internal TextFragment(FragmentState fragState, string textToSpeak, string textFrag, int offset, int length)
		{
			if (fragState.Action == TtsEngineAction.Speak || fragState.Action == TtsEngineAction.Pronounce)
			{
				textFrag = textToSpeak;
			}
			if (!string.IsNullOrEmpty(textFrag))
			{
				this.TextToSpeak = textFrag;
			}
			this.State = fragState;
			this.TextOffset = offset;
			this.TextLength = length;
		}

		// Token: 0x040006CC RID: 1740
		private FragmentState _state;

		// Token: 0x040006CD RID: 1741
		[MarshalAs(21)]
		private string _textToSpeak = string.Empty;

		// Token: 0x040006CE RID: 1742
		private int _textOffset;

		// Token: 0x040006CF RID: 1743
		private int _textLength;
	}
}
