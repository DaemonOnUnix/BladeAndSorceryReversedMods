using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200015C RID: 348
	internal sealed class ParameterDefinitionCollection : Collection<ParameterDefinition>
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x00026BA0 File Offset: 0x00024DA0
		internal ParameterDefinitionCollection(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00026BAF File Offset: 0x00024DAF
		internal ParameterDefinitionCollection(IMethodSignature method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00026BBF File Offset: 0x00024DBF
		protected override void OnAdd(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00026BD4 File Offset: 0x00024DD4
		protected override void OnInsert(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00026BBF File Offset: 0x00024DBF
		protected override void OnSet(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00026C18 File Offset: 0x00024E18
		protected override void OnRemove(ParameterDefinition item, int index)
		{
			item.method = null;
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}

		// Token: 0x04000453 RID: 1107
		private readonly IMethodSignature method;
	}
}
