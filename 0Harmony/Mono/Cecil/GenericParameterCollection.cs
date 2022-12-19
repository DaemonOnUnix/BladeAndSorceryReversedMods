using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200020A RID: 522
	internal sealed class GenericParameterCollection : Collection<GenericParameter>
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00027A88 File Offset: 0x00025C88
		internal GenericParameterCollection(IGenericParameterProvider owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00027A97 File Offset: 0x00025C97
		internal GenericParameterCollection(IGenericParameterProvider owner, int capacity)
			: base(capacity)
		{
			this.owner = owner;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00027AA7 File Offset: 0x00025CA7
		protected override void OnAdd(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00027AB4 File Offset: 0x00025CB4
		protected override void OnInsert(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
			for (int i = index; i < this.size; i++)
			{
				this.items[i].position = i + 1;
			}
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00027AA7 File Offset: 0x00025CA7
		protected override void OnSet(GenericParameter item, int index)
		{
			this.UpdateGenericParameter(item, index);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00027AEA File Offset: 0x00025CEA
		private void UpdateGenericParameter(GenericParameter item, int index)
		{
			item.owner = this.owner;
			item.position = index;
			item.type = this.owner.GenericParameterType;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00027B10 File Offset: 0x00025D10
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

		// Token: 0x0400032C RID: 812
		private readonly IGenericParameterProvider owner;
	}
}
