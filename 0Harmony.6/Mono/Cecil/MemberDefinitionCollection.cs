using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000138 RID: 312
	internal sealed class MemberDefinitionCollection<T> : Collection<T> where T : IMemberDefinition
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x000230B4 File Offset: 0x000212B4
		internal MemberDefinitionCollection(TypeDefinition container)
		{
			this.container = container;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000230C3 File Offset: 0x000212C3
		internal MemberDefinitionCollection(TypeDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000230D3 File Offset: 0x000212D3
		protected override void OnAdd(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000230D3 File Offset: 0x000212D3
		protected sealed override void OnSet(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000230D3 File Offset: 0x000212D3
		protected sealed override void OnInsert(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000230DC File Offset: 0x000212DC
		protected sealed override void OnRemove(T item, int index)
		{
			MemberDefinitionCollection<T>.Detach(item);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000230E4 File Offset: 0x000212E4
		protected sealed override void OnClear()
		{
			foreach (T t in this)
			{
				MemberDefinitionCollection<T>.Detach(t);
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00023130 File Offset: 0x00021330
		private void Attach(T element)
		{
			if (element.DeclaringType == this.container)
			{
				return;
			}
			if (element.DeclaringType != null)
			{
				throw new ArgumentException("Member already attached");
			}
			element.DeclaringType = this.container;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00023180 File Offset: 0x00021380
		private static void Detach(T element)
		{
			element.DeclaringType = null;
		}

		// Token: 0x0400032A RID: 810
		private TypeDefinition container;
	}
}
