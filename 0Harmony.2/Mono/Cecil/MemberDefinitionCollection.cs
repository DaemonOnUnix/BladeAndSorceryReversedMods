using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200022B RID: 555
	internal sealed class MemberDefinitionCollection<T> : Collection<T> where T : IMemberDefinition
	{
		// Token: 0x06000C0A RID: 3082 RVA: 0x0002938C File Offset: 0x0002758C
		internal MemberDefinitionCollection(TypeDefinition container)
		{
			this.container = container;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002939B File Offset: 0x0002759B
		internal MemberDefinitionCollection(TypeDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000293AB File Offset: 0x000275AB
		protected override void OnAdd(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000293AB File Offset: 0x000275AB
		protected sealed override void OnSet(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000293AB File Offset: 0x000275AB
		protected sealed override void OnInsert(T item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000293B4 File Offset: 0x000275B4
		protected sealed override void OnRemove(T item, int index)
		{
			MemberDefinitionCollection<T>.Detach(item);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000293BC File Offset: 0x000275BC
		protected sealed override void OnClear()
		{
			foreach (T t in this)
			{
				MemberDefinitionCollection<T>.Detach(t);
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00029408 File Offset: 0x00027608
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

		// Token: 0x06000C12 RID: 3090 RVA: 0x00029458 File Offset: 0x00027658
		private static void Detach(T element)
		{
			element.DeclaringType = null;
		}

		// Token: 0x0400035C RID: 860
		private TypeDefinition container;
	}
}
