using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FC RID: 508
	public sealed class VariableDefinition : VariableReference
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x000352E1 File Offset: 0x000334E1
		public bool IsPinned
		{
			get
			{
				return this.variable_type.IsPinned;
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x000352EE File Offset: 0x000334EE
		public VariableDefinition(TypeReference variableType)
			: base(variableType)
		{
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override VariableDefinition Resolve()
		{
			return this;
		}
	}
}
