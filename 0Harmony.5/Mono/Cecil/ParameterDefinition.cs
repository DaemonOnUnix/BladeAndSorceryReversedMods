using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200024F RID: 591
	public sealed class ParameterDefinition : ParameterReference, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x0002CF85 File Offset: 0x0002B185
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x0002CF8D File Offset: 0x0002B18D
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

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x0002CF96 File Offset: 0x0002B196
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0002CF9E File Offset: 0x0002B19E
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

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0002CFC6 File Offset: 0x0002B1C6
		// (set) Token: 0x06000E28 RID: 3624 RVA: 0x0002CFEF File Offset: 0x0002B1EF
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

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0002CFFF File Offset: 0x0002B1FF
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x0002D011 File Offset: 0x0002B211
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0002D01A File Offset: 0x0002B21A
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

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0002D044 File Offset: 0x0002B244
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.parameter_type.Module);
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0002D067 File Offset: 0x0002B267
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.parameter_type.Module);
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0002D084 File Offset: 0x0002B284
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0002D0A7 File Offset: 0x0002B2A7
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

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0002D0BE File Offset: 0x0002B2BE
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

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0002D0D3 File Offset: 0x0002B2D3
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0002D0E1 File Offset: 0x0002B2E1
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

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002D0F6 File Offset: 0x0002B2F6
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0002D104 File Offset: 0x0002B304
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

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002D119 File Offset: 0x0002B319
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x0002D127 File Offset: 0x0002B327
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

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0002D13C File Offset: 0x0002B33C
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0002D14B File Offset: 0x0002B34B
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

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0002D161 File Offset: 0x0002B361
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x0002D173 File Offset: 0x0002B373
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0002D18C File Offset: 0x0002B38C
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x0002D19E File Offset: 0x0002B39E
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

		// Token: 0x06000E3E RID: 3646 RVA: 0x0002D1B7 File Offset: 0x0002B3B7
		internal ParameterDefinition(TypeReference parameterType, IMethodSignature method)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
			this.method = method;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0002D1CD File Offset: 0x0002B3CD
		public ParameterDefinition(TypeReference parameterType)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0002D1DC File Offset: 0x0002B3DC
		public ParameterDefinition(string name, ParameterAttributes attributes, TypeReference parameterType)
			: base(name, parameterType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Param);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00017E2C File Offset: 0x0001602C
		public override ParameterDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000483 RID: 1155
		private ushort attributes;

		// Token: 0x04000484 RID: 1156
		internal IMethodSignature method;

		// Token: 0x04000485 RID: 1157
		private object constant = Mixin.NotResolved;

		// Token: 0x04000486 RID: 1158
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000487 RID: 1159
		private MarshalInfo marshal_info;
	}
}
