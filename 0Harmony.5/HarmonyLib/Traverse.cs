using System;

namespace HarmonyLib
{
	// Token: 0x02000195 RID: 405
	public class Traverse<T>
	{
		// Token: 0x060006A1 RID: 1697 RVA: 0x00002AED File Offset: 0x00000CED
		private Traverse()
		{
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00016D9B File Offset: 0x00014F9B
		public Traverse(Traverse traverse)
		{
			this.traverse = traverse;
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00016DAA File Offset: 0x00014FAA
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x00016DB7 File Offset: 0x00014FB7
		public T Value
		{
			get
			{
				return this.traverse.GetValue<T>();
			}
			set
			{
				this.traverse.SetValue(value);
			}
		}

		// Token: 0x0400020C RID: 524
		private readonly Traverse traverse;
	}
}
