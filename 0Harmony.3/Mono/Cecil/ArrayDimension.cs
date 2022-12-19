using System;

namespace Mono.Cecil
{
	// Token: 0x020000BB RID: 187
	internal struct ArrayDimension
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001456F File Offset: 0x0001276F
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00014577 File Offset: 0x00012777
		public int? LowerBound
		{
			get
			{
				return this.lower_bound;
			}
			set
			{
				this.lower_bound = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00014580 File Offset: 0x00012780
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00014588 File Offset: 0x00012788
		public int? UpperBound
		{
			get
			{
				return this.upper_bound;
			}
			set
			{
				this.upper_bound = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00014591 File Offset: 0x00012791
		public bool IsSized
		{
			get
			{
				return this.lower_bound != null || this.upper_bound != null;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000145AD File Offset: 0x000127AD
		public ArrayDimension(int? lowerBound, int? upperBound)
		{
			this.lower_bound = lowerBound;
			this.upper_bound = upperBound;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000145C0 File Offset: 0x000127C0
		public override string ToString()
		{
			if (this.IsSized)
			{
				int? num = this.lower_bound;
				string text = num.ToString();
				string text2 = "...";
				num = this.upper_bound;
				return text + text2 + num.ToString();
			}
			return string.Empty;
		}

		// Token: 0x04000230 RID: 560
		private int? lower_bound;

		// Token: 0x04000231 RID: 561
		private int? upper_bound;
	}
}
