using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002F2 RID: 754
	public sealed class VariableDefinition : VariableReference
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0003D22D File Offset: 0x0003B42D
		public bool IsPinned
		{
			get
			{
				return this.variable_type.IsPinned;
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0003D23A File Offset: 0x0003B43A
		public VariableDefinition(TypeReference variableType)
			: base(variableType)
		{
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00017E2C File Offset: 0x0001602C
		public override VariableDefinition Resolve()
		{
			return this;
		}
	}
}
