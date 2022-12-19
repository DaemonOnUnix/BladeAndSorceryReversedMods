using System;

namespace Mono.Cecil
{
	// Token: 0x020001AD RID: 429
	internal struct ArrayDimension
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001A3FB File Offset: 0x000185FB
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0001A403 File Offset: 0x00018603
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

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0001A40C File Offset: 0x0001860C
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0001A414 File Offset: 0x00018614
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

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0001A41D File Offset: 0x0001861D
		public bool IsSized
		{
			get
			{
				return this.lower_bound != null || this.upper_bound != null;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001A439 File Offset: 0x00018639
		public ArrayDimension(int? lowerBound, int? upperBound)
		{
			this.lower_bound = lowerBound;
			this.upper_bound = upperBound;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001A44C File Offset: 0x0001864C
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

		// Token: 0x0400025E RID: 606
		private int? lower_bound;

		// Token: 0x0400025F RID: 607
		private int? upper_bound;
	}
}
