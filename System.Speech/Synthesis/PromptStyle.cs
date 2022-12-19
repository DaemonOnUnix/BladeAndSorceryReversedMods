using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000147 RID: 327
	[Serializable]
	public class PromptStyle
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0002740A File Offset: 0x0002640A
		public PromptStyle()
		{
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00027412 File Offset: 0x00026412
		public PromptStyle(PromptRate rate)
		{
			this.Rate = rate;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00027421 File Offset: 0x00026421
		public PromptStyle(PromptVolume volume)
		{
			this.Volume = volume;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00027430 File Offset: 0x00026430
		public PromptStyle(PromptEmphasis emphasis)
		{
			this.Emphasis = emphasis;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0002743F File Offset: 0x0002643F
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00027447 File Offset: 0x00026447
		public PromptRate Rate
		{
			get
			{
				return this._rate;
			}
			set
			{
				this._rate = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00027450 File Offset: 0x00026450
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00027458 File Offset: 0x00026458
		public PromptVolume Volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00027461 File Offset: 0x00026461
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00027469 File Offset: 0x00026469
		public PromptEmphasis Emphasis
		{
			get
			{
				return this._emphasis;
			}
			set
			{
				this._emphasis = value;
			}
		}

		// Token: 0x0400062A RID: 1578
		private PromptRate _rate;

		// Token: 0x0400062B RID: 1579
		private PromptVolume _volume;

		// Token: 0x0400062C RID: 1580
		private PromptEmphasis _emphasis;
	}
}
