using System;

namespace HarmonyLib
{
	// Token: 0x02000059 RID: 89
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public class HarmonyArgument : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009ED5 File Offset: 0x000080D5
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00009EDD File Offset: 0x000080DD
		public string OriginalName { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009EE6 File Offset: 0x000080E6
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00009EEE File Offset: 0x000080EE
		public int Index { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009EF7 File Offset: 0x000080F7
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00009EFF File Offset: 0x000080FF
		public string NewName { get; private set; }

		// Token: 0x06000189 RID: 393 RVA: 0x00009F08 File Offset: 0x00008108
		public HarmonyArgument(string originalName)
			: this(originalName, null)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00009F12 File Offset: 0x00008112
		public HarmonyArgument(int index)
			: this(index, null)
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009F1C File Offset: 0x0000811C
		public HarmonyArgument(string originalName, string newName)
		{
			this.OriginalName = originalName;
			this.Index = -1;
			this.NewName = newName;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009F39 File Offset: 0x00008139
		public HarmonyArgument(int index, string name)
		{
			this.OriginalName = null;
			this.Index = index;
			this.NewName = name;
		}
	}
}
