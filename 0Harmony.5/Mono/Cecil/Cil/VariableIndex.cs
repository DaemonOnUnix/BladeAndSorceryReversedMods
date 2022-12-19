using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D6 RID: 726
	internal struct VariableIndex
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0003C368 File Offset: 0x0003A568
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

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x0003C39C File Offset: 0x0003A59C
		internal bool IsResolved
		{
			get
			{
				return this.variable != null;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0003C3A7 File Offset: 0x0003A5A7
		internal VariableDefinition ResolvedVariable
		{
			get
			{
				return this.variable;
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0003C3AF File Offset: 0x0003A5AF
		public VariableIndex(VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			this.variable = variable;
			this.index = null;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0003C3D2 File Offset: 0x0003A5D2
		public VariableIndex(int index)
		{
			this.variable = null;
			this.index = new int?(index);
		}

		// Token: 0x0400095B RID: 2395
		private readonly VariableDefinition variable;

		// Token: 0x0400095C RID: 2396
		private readonly int? index;
	}
}
