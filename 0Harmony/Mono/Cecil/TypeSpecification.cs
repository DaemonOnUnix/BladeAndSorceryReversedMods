using System;

namespace Mono.Cecil
{
	// Token: 0x02000276 RID: 630
	internal abstract class TypeSpecification : TypeReference
	{
		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x000306FA File Offset: 0x0002E8FA
		public TypeReference ElementType
		{
			get
			{
				return this.element_type;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00030702 File Offset: 0x0002E902
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0001845A File Offset: 0x0001665A
		public override string Name
		{
			get
			{
				return this.element_type.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0003070F File Offset: 0x0002E90F
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0001845A File Offset: 0x0001665A
		public override string Namespace
		{
			get
			{
				return this.element_type.Namespace;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0003071C File Offset: 0x0002E91C
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0001845A File Offset: 0x0001665A
		public override IMetadataScope Scope
		{
			get
			{
				return this.element_type.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00030729 File Offset: 0x0002E929
		public override ModuleDefinition Module
		{
			get
			{
				return this.element_type.Module;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00030736 File Offset: 0x0002E936
		public override string FullName
		{
			get
			{
				return this.element_type.FullName;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00030743 File Offset: 0x0002E943
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.element_type.ContainsGenericParameter;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x000278AF File Offset: 0x00025AAF
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00030750 File Offset: 0x0002E950
		internal TypeSpecification(TypeReference type)
			: base(null, null)
		{
			this.element_type = type;
			this.token = new MetadataToken(TokenType.TypeSpec);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00030771 File Offset: 0x0002E971
		public override TypeReference GetElementType()
		{
			return this.element_type.GetElementType();
		}

		// Token: 0x04000575 RID: 1397
		private readonly TypeReference element_type;
	}
}
