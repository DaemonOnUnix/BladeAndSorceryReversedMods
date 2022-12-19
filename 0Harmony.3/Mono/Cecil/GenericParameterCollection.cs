using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000118 RID: 280
	internal sealed class GenericParameterCollection : Collection<GenericParameter>
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x00021BA0 File Offset: 0x0001FDA0
		internal GenericParameterCollection(IGenericParameterProvider owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00021BAF File Offset: 0x0001FDAF
		internal GenericParameterCollection(IGenericParameterProvider owner, int capacity)
			: base(capacity)
		{
			this.owner = owner;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00021BBF File Offset: 0x0001FDBF
		protected override void OnAdd(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00021BCC File Offset: 0x0001FDCC
		protected override void OnInsert(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
			for (int i = index; i < this.size; i++)
			{
				this.items[i].position = i + 1;
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00021BBF File Offset: 0x0001FDBF
		protected override void OnSet(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00021C02 File Offset: 0x0001FE02
		private void UpdateGenericParameter(GenericParameter item, int index)
		{
			item.owner = this.owner;
			item.position = index;
			item.type = this.owner.GenericParameterType;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00021C28 File Offset: 0x0001FE28
		protected override void OnRemove(GenericParameter item, int index)
		{
			item.owner = null;
			item.position = -1;
			item.type = GenericParameterType.Type;
			for (int i = index + 1; i < this.size; i++)
			{
				this.items[i].position = i - 1;
			}
		}

		// Token: 0x040002FA RID: 762
		private readonly IGenericParameterProvider owner;
	}
}
