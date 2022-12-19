using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class PromptStyle
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003BF5 File Offset: 0x00001DF5
		public PromptStyle()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003BFD File Offset: 0x00001DFD
		public PromptStyle(PromptRate rate)
		{
			this.Rate = rate;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C0C File Offset: 0x00001E0C
		public PromptStyle(PromptVolume volume)
		{
			this.Volume = volume;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003C1B File Offset: 0x00001E1B
		public PromptStyle(PromptEmphasis emphasis)
		{
			this.Emphasis = emphasis;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003C2A File Offset: 0x00001E2A
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003C32 File Offset: 0x00001E32
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003C3B File Offset: 0x00001E3B
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00003C43 File Offset: 0x00001E43
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003C4C File Offset: 0x00001E4C
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003C54 File Offset: 0x00001E54
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

		// Token: 0x040001A6 RID: 422
		private PromptRate _rate;

		// Token: 0x040001A7 RID: 423
		private PromptVolume _volume;

		// Token: 0x040001A8 RID: 424
		private PromptEmphasis _emphasis;
	}
}
