using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200015B RID: 347
	public sealed class ParameterDefinition : ParameterReference, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002691D File Offset: 0x00024B1D
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x00026925 File Offset: 0x00024B25
		public ParameterAttributes Attributes
		{
			get
			{
				return (ParameterAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002692E File Offset: 0x00024B2E
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00026936 File Offset: 0x00024B36
		public int Sequence
		{
			get
			{
				if (this.method == null)
				{
					return -1;
				}
				if (!this.method.HasImplicitThis())
				{
					return this.index;
				}
				return this.index + 1;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002695E File Offset: 0x00024B5E
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00026987 File Offset: 0x00024B87
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.parameter_type.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00026997 File Offset: 0x00024B97
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x000269A9 File Offset: 0x00024BA9
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x000269B2 File Offset: 0x00024BB2
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.parameter_type.Module);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x000269DC File Offset: 0x00024BDC
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.parameter_type.Module);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x000269FF File Offset: 0x00024BFF
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.parameter_type.Module);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00026A1C File Offset: 0x00024C1C
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00026A3F File Offset: 0x00024C3F
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.marshal_info ?? this.GetMarshalInfo(ref this.marshal_info, this.parameter_type.Module);
			}
			set
			{
				this.marshal_info = value;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00026A48 File Offset: 0x00024C48
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x00026A56 File Offset: 0x00024C56
		public bool IsIn
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00026A6B File Offset: 0x00024C6B
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00026A79 File Offset: 0x00024C79
		public bool IsOut
		{
			get
			{
				return this.attributes.GetAttributes(2);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2, value);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00026A8E File Offset: 0x00024C8E
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00026A9C File Offset: 0x00024C9C
		public bool IsLcid
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00026AB1 File Offset: 0x00024CB1
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x00026ABF File Offset: 0x00024CBF
		public bool IsReturnValue
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00026AD4 File Offset: 0x00024CD4
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x00026AE3 File Offset: 0x00024CE3
		public bool IsOptional
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

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00026AF9 File Offset: 0x00024CF9
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x00026B0B File Offset: 0x00024D0B
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(4096);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00026B24 File Offset: 0x00024D24
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x00026B36 File Offset: 0x00024D36
		public bool HasFieldMarshal
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00026B4F File Offset: 0x00024D4F
		internal ParameterDefinition(TypeReference parameterType, IMethodSignature method)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
			this.method = method;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00026B65 File Offset: 0x00024D65
		public ParameterDefinition(TypeReference parameterType)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00026B74 File Offset: 0x00024D74
		public ParameterDefinition(string name, ParameterAttributes attributes, TypeReference parameterType)
			: base(name, parameterType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Param);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override ParameterDefinition Resolve()
		{
			return this;
		}

		// Token: 0x0400044E RID: 1102
		private ushort attributes;

		// Token: 0x0400044F RID: 1103
		internal IMethodSignature method;

		// Token: 0x04000450 RID: 1104
		private object constant = Mixin.NotResolved;

		// Token: 0x04000451 RID: 1105
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000452 RID: 1106
		private MarshalInfo marshal_info;
	}
}
