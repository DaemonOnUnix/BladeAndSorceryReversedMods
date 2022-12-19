using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000164 RID: 356
	public class SkipInfo
	{
		// Token: 0x0600091E RID: 2334 RVA: 0x00028272 File Offset: 0x00027272
		internal SkipInfo(int type, int count)
		{
			this._type = type;
			this._count = count;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00028291 File Offset: 0x00027291
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00028288 File Offset: 0x00027288
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000282A2 File Offset: 0x000272A2
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00028299 File Offset: 0x00027299
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000282AA File Offset: 0x000272AA
		public SkipInfo()
		{
		}

		// Token: 0x040006CA RID: 1738
		private int _type;

		// Token: 0x040006CB RID: 1739
		private int _count;
	}
}
