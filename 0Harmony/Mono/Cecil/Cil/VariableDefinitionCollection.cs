using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002BB RID: 699
	internal sealed class VariableDefinitionCollection : Collection<VariableDefinition>
	{
		// Token: 0x060011FA RID: 4602 RVA: 0x00039B5A File Offset: 0x00037D5A
		internal VariableDefinitionCollection(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00039B69 File Offset: 0x00037D69
		internal VariableDefinitionCollection(MethodDefinition method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00039B79 File Offset: 0x00037D79
		protected override void OnAdd(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00039B82 File Offset: 0x00037D82
		protected override void OnInsert(VariableDefinition item, int index)
		{
			item.index = index;
			this.UpdateVariableIndices(index, 1, null);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00039B79 File Offset: 0x00037D79
		protected override void OnSet(VariableDefinition item, int index)
		{
			item.index = index;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00039B94 File Offset: 0x00037D94
		protected override void OnRemove(VariableDefinition item, int index)
		{
			this.UpdateVariableIndices(index + 1, -1, item);
			item.index = -1;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00039BA8 File Offset: 0x00037DA8
		private void UpdateVariableIndices(int startIndex, int offset, VariableDefinition variableToRemove = null)
		{
			for (int i = startIndex; i < this.size; i++)
			{
				this.items[i].index = i + offset;
			}
			MethodDebugInformation methodDebugInformation = ((this.method == null) ? null : this.method.debug_info);
			if (methodDebugInformation == null || methodDebugInformation.Scope == null)
			{
				return;
			}
			foreach (ScopeDebugInformation scopeDebugInformation in methodDebugInformation.GetScopes())
			{
				if (scopeDebugInformation.HasVariables)
				{
					Collection<VariableDebugInformation> variables = scopeDebugInformation.Variables;
					int num = -1;
					for (int j = 0; j < variables.Count; j++)
					{
						VariableDebugInformation variableDebugInformation = variables[j];
						if (variableToRemove != null && ((variableDebugInformation.index.IsResolved && variableDebugInformation.index.ResolvedVariable == variableToRemove) || (!variableDebugInformation.index.IsResolved && variableDebugInformation.Index == variableToRemove.Index)))
						{
							num = j;
						}
						else if (!variableDebugInformation.index.IsResolved && variableDebugInformation.Index >= startIndex)
						{
							variableDebugInformation.index = new VariableIndex(variableDebugInformation.Index + offset);
						}
					}
					if (num >= 0)
					{
						variables.RemoveAt(num);
					}
				}
			}
		}

		// Token: 0x040007F9 RID: 2041
		private readonly MethodDefinition method;
	}
}
