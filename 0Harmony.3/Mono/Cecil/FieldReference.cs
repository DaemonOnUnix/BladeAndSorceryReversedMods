using System;

namespace Mono.Cecil
{
	// Token: 0x02000111 RID: 273
	public class FieldReference : MemberReference
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00021401 File Offset: 0x0001F601
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x00021409 File Offset: 0x0001F609
		public TypeReference FieldType
		{
			get
			{
				return this.field_type;
			}
			set
			{
				this.field_type = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00021412 File Offset: 0x0001F612
		public override string FullName
		{
			get
			{
				return this.field_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0002142F File Offset: 0x0001F62F
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.field_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00021446 File Offset: 0x0001F646
		internal FieldReference()
		{
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002145E File Offset: 0x0001F65E
		public FieldReference(string name, TypeReference fieldType)
			: base(name)
		{
			Mixin.CheckType(fieldType, Mixin.Argument.fieldType);
			this.field_type = fieldType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00021486 File Offset: 0x0001F686
		public FieldReference(string name, TypeReference fieldType, TypeReference declaringType)
			: this(name, fieldType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002149F File Offset: 0x0001F69F
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000214A7 File Offset: 0x0001F6A7
		public new virtual FieldDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x040002EA RID: 746
		private TypeReference field_type;
	}
}
