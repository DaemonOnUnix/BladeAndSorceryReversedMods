using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017B RID: 379
	internal sealed class TypeDefinitionCollection : Collection<TypeDefinition>
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x000285B7 File Offset: 0x000267B7
		internal TypeDefinitionCollection(ModuleDefinition container)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(new RowEqualityComparer());
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000285D6 File Offset: 0x000267D6
		internal TypeDefinitionCollection(ModuleDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(capacity, new RowEqualityComparer());
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000285F7 File Offset: 0x000267F7
		protected override void OnAdd(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000285F7 File Offset: 0x000267F7
		protected override void OnSet(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000285F7 File Offset: 0x000267F7
		protected override void OnInsert(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00028600 File Offset: 0x00026800
		protected override void OnRemove(TypeDefinition item, int index)
		{
			this.Detach(item);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002860C File Offset: 0x0002680C
		protected override void OnClear()
		{
			foreach (TypeDefinition typeDefinition in this)
			{
				this.Detach(typeDefinition);
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002865C File Offset: 0x0002685C
		private void Attach(TypeDefinition type)
		{
			if (type.Module != null && type.Module != this.container)
			{
				throw new ArgumentException("Type already attached");
			}
			type.module = this.container;
			type.scope = this.container;
			this.name_cache[new Row<string, string>(type.Namespace, type.Name)] = type;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000286BF File Offset: 0x000268BF
		private void Detach(TypeDefinition type)
		{
			type.module = null;
			type.scope = null;
			this.name_cache.Remove(new Row<string, string>(type.Namespace, type.Name));
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000286EC File Offset: 0x000268EC
		public TypeDefinition GetType(string fullname)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(fullname, out text, out text2);
			return this.GetType(text, text2);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002870C File Offset: 0x0002690C
		public TypeDefinition GetType(string @namespace, string name)
		{
			TypeDefinition typeDefinition;
			if (this.name_cache.TryGetValue(new Row<string, string>(@namespace, name), out typeDefinition))
			{
				return typeDefinition;
			}
			return null;
		}

		// Token: 0x04000508 RID: 1288
		private readonly ModuleDefinition container;

		// Token: 0x04000509 RID: 1289
		private readonly Dictionary<Row<string, string>, TypeDefinition> name_cache;
	}
}
