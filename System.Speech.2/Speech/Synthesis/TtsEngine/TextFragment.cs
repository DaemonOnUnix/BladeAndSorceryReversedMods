using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000030 RID: 48
	[StructLayout(LayoutKind.Sequential)]
	public class TextFragment
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00004B02 File Offset: 0x00002D02
		public TextFragment()
		{
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004B15 File Offset: 0x00002D15
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004B1D File Offset: 0x00002D1D
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004B26 File Offset: 0x00002D26
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004B2E File Offset: 0x00002D2E
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004B42 File Offset: 0x00002D42
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004B4A File Offset: 0x00002D4A
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004B53 File Offset: 0x00002D53
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004B5B File Offset: 0x00002D5B
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

		// Token: 0x060000EC RID: 236 RVA: 0x00004B64 File Offset: 0x00002D64
		internal TextFragment(FragmentState fragState)
			: this(fragState, null, null, 0, 0)
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004B71 File Offset: 0x00002D71
		internal TextFragment(FragmentState fragState, string textToSpeak)
			: this(fragState, textToSpeak, textToSpeak, 0, textToSpeak.Length)
		{
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004B84 File Offset: 0x00002D84
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

		// Token: 0x04000252 RID: 594
		private FragmentState _state;

		// Token: 0x04000253 RID: 595
		[MarshalAs(UnmanagedType.LPWStr)]
		private string _textToSpeak = string.Empty;

		// Token: 0x04000254 RID: 596
		private int _textOffset;

		// Token: 0x04000255 RID: 597
		private int _textLength;
	}
}
