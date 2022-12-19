using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000116 RID: 278
	public sealed class GenericParameter : TypeReference, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000217C5 File Offset: 0x0001F9C5
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x000217CD File Offset: 0x0001F9CD
		public GenericParameterAttributes Attributes
		{
			get
			{
				return (GenericParameterAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000217D6 File Offset: 0x0001F9D6
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000217DE File Offset: 0x0001F9DE
		public GenericParameterType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000217E6 File Offset: 0x0001F9E6
		public IGenericParameterProvider Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x000217F0 File Offset: 0x0001F9F0
		public bool HasConstraints
		{
			get
			{
				if (this.constraints != null)
				{
					return this.constraints.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<GenericParameter, bool>(this, (GenericParameter generic_parameter, MetadataReader reader) => reader.HasGenericConstraints(generic_parameter));
				}
				return false;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002184C File Offset: 0x0001FA4C
		public Collection<GenericParameterConstraint> Constraints
		{
			get
			{
				if (this.constraints != null)
				{
					return this.constraints;
				}
				if (base.HasImage)
				{
					return this.Module.Read<GenericParameter, GenericParameterConstraintCollection>(ref this.constraints, this, (GenericParameter generic_parameter, MetadataReader reader) => reader.ReadGenericConstraints(generic_parameter));
				}
				Interlocked.CompareExchange<GenericParameterConstraintCollection>(ref this.constraints, new GenericParameterConstraintCollection(this), null);
				return this.constraints;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x000218BB File Offset: 0x0001FABB
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x000218E0 File Offset: 0x0001FAE0
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x000218FE File Offset: 0x0001FAFE
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x000125CE File Offset: 0x000107CE
		public override IMetadataScope Scope
		{
			get
			{
				if (this.owner == null)
				{
					return null;
				}
				if (this.owner.GenericParameterType != GenericParameterType.Method)
				{
					return ((TypeReference)this.owner).Scope;
				}
				return ((MethodReference)this.owner).DeclaringType.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0002193E File Offset: 0x0001FB3E
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x000125CE File Offset: 0x000107CE
		public override TypeReference DeclaringType
		{
			get
			{
				return this.owner as TypeReference;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0002194B File Offset: 0x0001FB4B
		public MethodReference DeclaringMethod
		{
			get
			{
				return this.owner as MethodReference;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00021958 File Offset: 0x0001FB58
		public override ModuleDefinition Module
		{
			get
			{
				return this.module ?? this.owner.Module;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00021970 File Offset: 0x0001FB70
		public override string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(base.Name))
				{
					return base.Name;
				}
				return base.Name = ((this.type == GenericParameterType.Method) ? "!!" : "!") + this.position.ToString();
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00020011 File Offset: 0x0001E211
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x000125CE File Offset: 0x000107CE
		public override string Namespace
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x000219BF File Offset: 0x0001FBBF
		public override string FullName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00012561 File Offset: 0x00010761
		public override bool ContainsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000219C7 File Offset: 0x0001FBC7
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000219CF File Offset: 0x0001FBCF
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x000219DE File Offset: 0x0001FBDE
		public bool IsNonVariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 0U, value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x000219F4 File Offset: 0x0001FBF4
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x00021A03 File Offset: 0x0001FC03
		public bool IsCovariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 1U, value);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00021A19 File Offset: 0x0001FC19
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x00021A28 File Offset: 0x0001FC28
		public bool IsContravariant
		{
			get
			{
				return this.attributes.GetMaskedAttributes(3, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(3, 2U, value);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00021A3E File Offset: 0x0001FC3E
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x00021A4C File Offset: 0x0001FC4C
		public bool HasReferenceTypeConstraint
		{
			get
			{
				return this.attributes.GetAttributes(4);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4, value);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00021A61 File Offset: 0x0001FC61
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x00021A6F File Offset: 0x0001FC6F
		public bool HasNotNullableValueTypeConstraint
		{
			get
			{
				return this.attributes.GetAttributes(8);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00021A84 File Offset: 0x0001FC84
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x00021A93 File Offset: 0x0001FC93
		public bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00021AA9 File Offset: 0x0001FCA9
		public GenericParameter(IGenericParameterProvider owner)
			: this(string.Empty, owner)
		{
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00021AB8 File Offset: 0x0001FCB8
		public GenericParameter(string name, IGenericParameterProvider owner)
			: base(string.Empty, name)
		{
			if (owner == null)
			{
				throw new ArgumentNullException();
			}
			this.position = -1;
			this.owner = owner;
			this.type = owner.GenericParameterType;
			this.etype = GenericParameter.ConvertGenericParameterType(this.type);
			this.token = new MetadataToken(TokenType.GenericParam);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00021B18 File Offset: 0x0001FD18
		internal GenericParameter(int position, GenericParameterType type, ModuleDefinition module)
			: base(string.Empty, string.Empty)
		{
			Mixin.CheckModule(module);
			this.position = position;
			this.type = type;
			this.etype = GenericParameter.ConvertGenericParameterType(type);
			this.module = module;
			this.token = new MetadataToken(TokenType.GenericParam);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00021B6C File Offset: 0x0001FD6C
		private static ElementType ConvertGenericParameterType(GenericParameterType type)
		{
			if (type == GenericParameterType.Type)
			{
				return ElementType.Var;
			}
			if (type != GenericParameterType.Method)
			{
				throw new ArgumentOutOfRangeException();
			}
			return ElementType.MVar;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00021621 File Offset: 0x0001F821
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x040002F1 RID: 753
		internal int position;

		// Token: 0x040002F2 RID: 754
		internal GenericParameterType type;

		// Token: 0x040002F3 RID: 755
		internal IGenericParameterProvider owner;

		// Token: 0x040002F4 RID: 756
		private ushort attributes;

		// Token: 0x040002F5 RID: 757
		private GenericParameterConstraintCollection constraints;

		// Token: 0x040002F6 RID: 758
		private Collection<CustomAttribute> custom_attributes;
	}
}
