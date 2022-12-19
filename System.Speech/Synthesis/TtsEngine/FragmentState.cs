using System;
using System.ComponentModel;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000166 RID: 358
	[ImmutableObject(true)]
	public struct FragmentState : IEquatable<FragmentState>
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0002838E File Offset: 0x0002738E
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x00028396 File Offset: 0x00027396
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

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0002839F File Offset: 0x0002739F
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x000283A7 File Offset: 0x000273A7
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x000283B0 File Offset: 0x000273B0
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x000283B8 File Offset: 0x000273B8
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x000283C1 File Offset: 0x000273C1
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x000283C9 File Offset: 0x000273C9
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x000283D2 File Offset: 0x000273D2
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x000283DA File Offset: 0x000273DA
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

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x000283EE File Offset: 0x000273EE
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x000283F6 File Offset: 0x000273F6
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

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0002840A File Offset: 0x0002740A
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x00028412 File Offset: 0x00027412
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

		// Token: 0x0600093E RID: 2366 RVA: 0x00028426 File Offset: 0x00027426
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

		// Token: 0x0600093F RID: 2367 RVA: 0x00028460 File Offset: 0x00027460
		public static bool operator ==(FragmentState state1, FragmentState state2)
		{
			return state1.Action == state2.Action && state1.LangId == state2.LangId && state1.Emphasis == state2.Emphasis && state1.Duration == state2.Duration && state1.SayAs == state2.SayAs && state1.Prosody == state2.Prosody && object.Equals(state1.Phoneme, state2.Phoneme);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000284E2 File Offset: 0x000274E2
		public static bool operator !=(FragmentState state1, FragmentState state2)
		{
			return !(state1 == state2);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000284EE File Offset: 0x000274EE
		public bool Equals(FragmentState other)
		{
			return this == other;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000284FC File Offset: 0x000274FC
		public override bool Equals(object obj)
		{
			return obj is FragmentState && this.Equals((FragmentState)obj);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00028514 File Offset: 0x00027514
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040006D0 RID: 1744
		private TtsEngineAction _action;

		// Token: 0x040006D1 RID: 1745
		private int _langId;

		// Token: 0x040006D2 RID: 1746
		private int _emphasis;

		// Token: 0x040006D3 RID: 1747
		private int _duration;

		// Token: 0x040006D4 RID: 1748
		private SayAs _sayAs;

		// Token: 0x040006D5 RID: 1749
		private Prosody _prosody;

		// Token: 0x040006D6 RID: 1750
		private char[] _phoneme;
	}
}
