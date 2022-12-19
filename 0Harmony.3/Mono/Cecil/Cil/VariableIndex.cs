using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E0 RID: 480
	internal struct VariableIndex
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x000344F2 File Offset: 0x000326F2
		public int Index
		{
			get
			{
				if (this.variable != null)
				{
					return this.variable.Index;
				}
				if (this.index != null)
				{
					return this.index.Value;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00034526 File Offset: 0x00032726
		public VariableIndex(VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			this.variable = variable;
			this.index = null;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00034549 File Offset: 0x00032749
		public VariableIndex(int index)
		{
			this.variable = null;
			this.index = new int?(index);
		}

		// Token: 0x0400091F RID: 2335
		private readonly VariableDefinition variable;

		// Token: 0x04000920 RID: 2336
		private readonly int? index;
	}
}
