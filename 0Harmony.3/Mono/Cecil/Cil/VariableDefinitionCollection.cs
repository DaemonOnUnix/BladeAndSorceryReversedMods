using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C6 RID: 454
	internal sealed class VariableDefinitionCollection : Collection<VariableDefinition>
	{
		// Token: 0x06000E96 RID: 3734 RVA: 0x0003211A File Offset: 0x0003031A
		internal VariableDefinitionCollection()
		{
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00032122 File Offset: 0x00030322
		internal VariableDefinitionCollection(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0003212B File Offset: 0x0003032B
		protected override void OnAdd(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00032134 File Offset: 0x00030334
		protected override void OnInsert(VariableDefinition item, int index)
		{
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0003212B File Offset: 0x0003032B
		protected override void OnSet(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0003216C File Offset: 0x0003036C
		protected override void OnRemove(VariableDefinition item, int index)
		{
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}
	}
}
