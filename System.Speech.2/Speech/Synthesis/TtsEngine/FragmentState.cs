using System;
using System.ComponentModel;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000031 RID: 49
	[ImmutableObject(true)]
	public struct FragmentState : IEquatable<FragmentState>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004BDE File Offset: 0x00002DDE
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004BE6 File Offset: 0x00002DE6
		public TtsEngineAction Action
		{
			get
			{
				return this._action;
			}
			internal set
			{
				this._action = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004BEF File Offset: 0x00002DEF
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004BF7 File Offset: 0x00002DF7
		public int LangId
		{
			get
			{
				return this._langId;
			}
			internal set
			{
				this._langId = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004C00 File Offset: 0x00002E00
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004C08 File Offset: 0x00002E08
		public int Emphasis
		{
			get
			{
				return this._emphasis;
			}
			internal set
			{
				this._emphasis = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004C11 File Offset: 0x00002E11
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004C19 File Offset: 0x00002E19
		public int Duration
		{
			get
			{
				return this._duration;
			}
			internal set
			{
				this._duration = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004C22 File Offset: 0x00002E22
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004C2A File Offset: 0x00002E2A
		public SayAs SayAs
		{
			get
			{
				return this._sayAs;
			}
			internal set
			{
				Helpers.ThrowIfNull(value, "value");
				this._sayAs = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004C3E File Offset: 0x00002E3E
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004C46 File Offset: 0x00002E46
		public Prosody Prosody
		{
			get
			{
				return this._prosody;
			}
			internal set
			{
				Helpers.ThrowIfNull(value, "value");
				this._prosody = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004C5A File Offset: 0x00002E5A
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004C62 File Offset: 0x00002E62
		public char[] Phoneme
		{
			get
			{
				return this._phoneme;
			}
			internal set
			{
				Helpers.ThrowIfNull(value, "value");
				this._phoneme = value;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004C76 File Offset: 0x00002E76
		public FragmentState(TtsEngineAction action, int langId, int emphasis, int duration, SayAs sayAs, Prosody prosody, char[] phonemes)
		{
			this._action = action;
			this._langId = langId;
			this._emphasis = emphasis;
			this._duration = duration;
			this._sayAs = sayAs;
			this._prosody = prosody;
			this._phoneme = phonemes;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public static bool operator ==(FragmentState state1, FragmentState state2)
		{
			return state1.Action == state2.Action && state1.LangId == state2.LangId && state1.Emphasis == state2.Emphasis && state1.Duration == state2.Duration && state1.SayAs == state2.SayAs && state1.Prosody == state2.Prosody && object.Equals(state1.Phoneme, state2.Phoneme);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004D32 File Offset: 0x00002F32
		public static bool operator !=(FragmentState state1, FragmentState state2)
		{
			return !(state1 == state2);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004D3E File Offset: 0x00002F3E
		public bool Equals(FragmentState other)
		{
			return this == other;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004D4C File Offset: 0x00002F4C
		public override bool Equals(object obj)
		{
			return obj is FragmentState && this.Equals((FragmentState)obj);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004D64 File Offset: 0x00002F64
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000256 RID: 598
		private TtsEngineAction _action;

		// Token: 0x04000257 RID: 599
		private int _langId;

		// Token: 0x04000258 RID: 600
		private int _emphasis;

		// Token: 0x04000259 RID: 601
		private int _duration;

		// Token: 0x0400025A RID: 602
		private SayAs _sayAs;

		// Token: 0x0400025B RID: 603
		private Prosody _prosody;

		// Token: 0x0400025C RID: 604
		private char[] _phoneme;
	}
}
