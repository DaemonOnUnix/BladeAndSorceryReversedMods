using System;

namespace HarmonyLib
{
	// Token: 0x0200005A RID: 90
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public class HarmonyArgument : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000AD51 File Offset: 0x00008F51
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000AD59 File Offset: 0x00008F59
		public string OriginalName { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000AD62 File Offset: 0x00008F62
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000AD6A File Offset: 0x00008F6A
		public int Index { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000AD73 File Offset: 0x00008F73
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000AD7B File Offset: 0x00008F7B
		public string NewName { get; private set; }

		// Token: 0x06000199 RID: 409 RVA: 0x0000AD84 File Offset: 0x00008F84
		public HarmonyArgument(string originalName)
			: this(originalName, null)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000AD8E File Offset: 0x00008F8E
		public HarmonyArgument(int index)
			: this(index, null)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000AD98 File Offset: 0x00008F98
		public HarmonyArgument(string originalName, string newName)
		{
			this.OriginalName = originalName;
			this.Index = -1;
			this.NewName = newName;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000ADB5 File Offset: 0x00008FB5
		public HarmonyArgument(int index, string name)
		{
			this.OriginalName = null;
			this.Index = index;
			this.NewName = name;
		}
	}
}
