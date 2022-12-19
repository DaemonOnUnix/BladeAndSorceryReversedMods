using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200026F RID: 623
	internal sealed class TypeDefinitionCollection : Collection<TypeDefinition>
	{
		// Token: 0x06000F50 RID: 3920 RVA: 0x0002EC1F File Offset: 0x0002CE1F
		internal TypeDefinitionCollection(ModuleDefinition container)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(new RowEqualityComparer());
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0002EC3E File Offset: 0x0002CE3E
		internal TypeDefinitionCollection(ModuleDefinition container, int capacity)
			: base(capacity)
		{
			this.container = container;
			this.name_cache = new Dictionary<Row<string, string>, TypeDefinition>(capacity, new RowEqualityComparer());
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0002EC5F File Offset: 0x0002CE5F
		protected override void OnAdd(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0002EC5F File Offset: 0x0002CE5F
		protected override void OnSet(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0002EC5F File Offset: 0x0002CE5F
		protected override void OnInsert(TypeDefinition item, int index)
		{
			this.Attach(item);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0002EC68 File Offset: 0x0002CE68
		protected override void OnRemove(TypeDefinition item, int index)
		{
			this.Detach(item);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0002EC74 File Offset: 0x0002CE74
		protected override void OnClear()
		{
			foreach (TypeDefinition typeDefinition in this)
			{
				this.Detach(typeDefinition);
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
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

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002ED27 File Offset: 0x0002CF27
		private void Detach(TypeDefinition type)
		{
			type.module = null;
			type.scope = null;
			this.name_cache.Remove(new Row<string, string>(type.Namespace, type.Name));
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0002ED54 File Offset: 0x0002CF54
		public TypeDefinition GetType(string fullname)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(fullname, out text, out text2);
			return this.GetType(text, text2);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0002ED74 File Offset: 0x0002CF74
		public TypeDefinition GetType(string @namespace, string name)
		{
			TypeDefinition typeDefinition;
			if (this.name_cache.TryGetValue(new Row<string, string>(@namespace, name), out typeDefinition))
			{
				return typeDefinition;
			}
			return null;
		}

		// Token: 0x0400053E RID: 1342
		private readonly ModuleDefinition container;

		// Token: 0x0400053F RID: 1343
		private readonly Dictionary<Row<string, string>, TypeDefinition> name_cache;
	}
}
