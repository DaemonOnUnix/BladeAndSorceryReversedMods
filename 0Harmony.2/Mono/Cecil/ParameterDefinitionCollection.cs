using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000250 RID: 592
	internal sealed class ParameterDefinitionCollection : Collection<ParameterDefinition>
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x0002D208 File Offset: 0x0002B408
		internal ParameterDefinitionCollection(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0002D217 File Offset: 0x0002B417
		internal ParameterDefinitionCollection(IMethodSignature method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0002D227 File Offset: 0x0002B427
		protected override void OnAdd(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0002D23C File Offset: 0x0002B43C
		protected override void OnInsert(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
			for (int i = index; i < this.size; i++)
			{
				this.items[i].index = i + 1;
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0002D227 File Offset: 0x0002B427
		protected override void OnSet(ParameterDefinition item, int index)
		{
			item.method = this.method;
			item.index = index;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002D280 File Offset: 0x0002B480
		protected override void OnRemove(ParameterDefinition item, int index)
		{
			item.method = null;
			item.index = -1;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].index = i - 1;
			}
		}

		// Token: 0x04000488 RID: 1160
		private readonly IMethodSignature method;
	}
}
