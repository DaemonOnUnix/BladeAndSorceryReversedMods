using System;

namespace Mono.Cecil
{
	// Token: 0x02000180 RID: 384
	internal abstract class TypeSpecification : TypeReference
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00029429 File Offset: 0x00027629
		public TypeReference ElementType
		{
			get
			{
				return this.element_type;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00029431 File Offset: 0x00027631
		// (set) Token: 0x06000C5A RID: 3162 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0002943E File Offset: 0x0002763E
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0002944B File Offset: 0x0002764B
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00029458 File Offset: 0x00027658
		public override ModuleDefinition Module
		{
			get
			{
				return this.element_type.Module;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00029465 File Offset: 0x00027665
		public override string FullName
		{
			get
			{
				return this.element_type.FullName;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00029472 File Offset: 0x00027672
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.element_type.ContainsGenericParameter;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000219C7 File Offset: 0x0001FBC7
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0002947F File Offset: 0x0002767F
		internal TypeSpecification(TypeReference type)
			: base(null, null)
		{
			this.element_type = type;
			this.token = new MetadataToken(TokenType.TypeSpec);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000294A0 File Offset: 0x000276A0
		public override TypeReference GetElementType()
		{
			return this.element_type.GetElementType();
		}

		// Token: 0x0400053D RID: 1341
		private readonly TypeReference element_type;
	}
}
