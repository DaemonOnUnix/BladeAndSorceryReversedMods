using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000208 RID: 520
	public sealed class GenericParameter : TypeReference, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x000276AD File Offset: 0x000258AD
		// (set) Token: 0x06000B30 RID: 2864 RVA: 0x000276B5 File Offset: 0x000258B5
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

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x000276BE File Offset: 0x000258BE
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x000276C6 File Offset: 0x000258C6
		public GenericParameterType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x000276CE File Offset: 0x000258CE
		public IGenericParameterProvider Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x000276D8 File Offset: 0x000258D8
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

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00027734 File Offset: 0x00025934
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

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x000277A3 File Offset: 0x000259A3
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

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x000277C8 File Offset: 0x000259C8
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x000277E6 File Offset: 0x000259E6
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00027826 File Offset: 0x00025A26
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00027833 File Offset: 0x00025A33
		public MethodReference DeclaringMethod
		{
			get
			{
				return this.owner as MethodReference;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00027840 File Offset: 0x00025A40
		public override ModuleDefinition Module
		{
			get
			{
				return this.module ?? this.owner.Module;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00027858 File Offset: 0x00025A58
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

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00025EB9 File Offset: 0x000240B9
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x000278A7 File Offset: 0x00025AA7
		public override string FullName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool ContainsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000278AF File Offset: 0x00025AAF
		public override MetadataType MetadataType
		{
			get
			{
				return (MetadataType)this.etype;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x000278B7 File Offset: 0x00025AB7
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x000278C6 File Offset: 0x00025AC6
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

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x000278DC File Offset: 0x00025ADC
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x000278EB File Offset: 0x00025AEB
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00027901 File Offset: 0x00025B01
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x00027910 File Offset: 0x00025B10
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00027926 File Offset: 0x00025B26
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x00027934 File Offset: 0x00025B34
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00027949 File Offset: 0x00025B49
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x00027957 File Offset: 0x00025B57
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002796C File Offset: 0x00025B6C
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0002797B File Offset: 0x00025B7B
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

		// Token: 0x06000B51 RID: 2897 RVA: 0x00027991 File Offset: 0x00025B91
		public GenericParameter(IGenericParameterProvider owner)
			: this(string.Empty, owner)
		{
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000279A0 File Offset: 0x00025BA0
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

		// Token: 0x06000B53 RID: 2899 RVA: 0x00027A00 File Offset: 0x00025C00
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

		// Token: 0x06000B54 RID: 2900 RVA: 0x00027A54 File Offset: 0x00025C54
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

		// Token: 0x06000B55 RID: 2901 RVA: 0x00027509 File Offset: 0x00025709
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x04000323 RID: 803
		internal int position;

		// Token: 0x04000324 RID: 804
		internal GenericParameterType type;

		// Token: 0x04000325 RID: 805
		internal IGenericParameterProvider owner;

		// Token: 0x04000326 RID: 806
		private ushort attributes;

		// Token: 0x04000327 RID: 807
		private GenericParameterConstraintCollection constraints;

		// Token: 0x04000328 RID: 808
		private Collection<CustomAttribute> custom_attributes;
	}
}
