using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200026E RID: 622
	internal class InterfaceImplementationCollection : Collection<InterfaceImplementation>
	{
		// Token: 0x06000F4A RID: 3914 RVA: 0x0002EBE9 File Offset: 0x0002CDE9
		internal InterfaceImplementationCollection(TypeDefinition type)
		{
			this.type = type;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
		internal InterfaceImplementationCollection(TypeDefinition type, int length)
			: base(length)
		{
			this.type = type;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0002EC08 File Offset: 0x0002CE08
		protected override void OnAdd(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0002EC08 File Offset: 0x0002CE08
		protected override void OnInsert(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0002EC08 File Offset: 0x0002CE08
		protected override void OnSet(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002EC16 File Offset: 0x0002CE16
		protected override void OnRemove(InterfaceImplementation item, int index)
		{
			item.type = null;
		}

		// Token: 0x0400053D RID: 1341
		private readonly TypeDefinition type;
	}
}
