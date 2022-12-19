using System;

namespace HarmonyLib
{
	// Token: 0x020000A3 RID: 163
	public class Traverse<T>
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00002AED File Offset: 0x00000CED
		private Traverse()
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00010F10 File Offset: 0x0000F110
		public Traverse(Traverse traverse)
		{
			this.traverse = traverse;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00010F1F File Offset: 0x0000F11F
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00010F2C File Offset: 0x0000F12C
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

		// Token: 0x040001DE RID: 478
		private readonly Traverse traverse;
	}
}
