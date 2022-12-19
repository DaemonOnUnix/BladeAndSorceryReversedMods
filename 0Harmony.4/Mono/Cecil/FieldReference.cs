using System;

namespace Mono.Cecil
{
	// Token: 0x02000203 RID: 515
	public class FieldReference : MemberReference
	{
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x000272E8 File Offset: 0x000254E8
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x000272F0 File Offset: 0x000254F0
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

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000272F9 File Offset: 0x000254F9
		public override string FullName
		{
			get
			{
				return this.field_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00027316 File Offset: 0x00025516
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.field_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002732D File Offset: 0x0002552D
		internal FieldReference()
		{
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00027345 File Offset: 0x00025545
		public FieldReference(string name, TypeReference fieldType)
			: base(name)
		{
			Mixin.CheckType(fieldType, Mixin.Argument.fieldType);
			this.field_type = fieldType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002736D File Offset: 0x0002556D
		public FieldReference(string name, TypeReference fieldType, TypeReference declaringType)
			: this(name, fieldType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00027386 File Offset: 0x00025586
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002738E File Offset: 0x0002558E
		public new virtual FieldDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x0400031C RID: 796
		private TypeReference field_type;
	}
}
