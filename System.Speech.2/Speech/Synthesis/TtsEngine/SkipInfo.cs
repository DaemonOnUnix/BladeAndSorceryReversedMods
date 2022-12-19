using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002F RID: 47
	public class SkipInfo
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00004ACA File Offset: 0x00002CCA
		internal SkipInfo(int type, int count)
		{
			this._type = type;
			this._count = count;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004AE9 File Offset: 0x00002CE9
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004AE0 File Offset: 0x00002CE0
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004AFA File Offset: 0x00002CFA
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004AF1 File Offset: 0x00002CF1
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

		// Token: 0x060000E2 RID: 226 RVA: 0x00003BF5 File Offset: 0x00001DF5
		public SkipInfo()
		{
		}

		// Token: 0x04000250 RID: 592
		private int _type;

		// Token: 0x04000251 RID: 593
		private int _count;
	}
}
