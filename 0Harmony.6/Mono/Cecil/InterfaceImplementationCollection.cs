using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017A RID: 378
	internal class InterfaceImplementationCollection : Collection<InterfaceImplementation>
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x00028581 File Offset: 0x00026781
		internal InterfaceImplementationCollection(TypeDefinition type)
		{
			this.type = type;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00028590 File Offset: 0x00026790
		internal InterfaceImplementationCollection(TypeDefinition type, int length)
			: base(length)
		{
			this.type = type;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000285A0 File Offset: 0x000267A0
		protected override void OnAdd(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000285A0 File Offset: 0x000267A0
		protected override void OnInsert(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000285A0 File Offset: 0x000267A0
		protected override void OnSet(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000285AE File Offset: 0x000267AE
		protected override void OnRemove(InterfaceImplementation item, int index)
		{
			item.type = null;
		}

		// Token: 0x04000507 RID: 1287
		private readonly TypeDefinition type;
	}
}
