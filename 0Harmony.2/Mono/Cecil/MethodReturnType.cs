using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200023A RID: 570
	public sealed class MethodReturnType : IConstantProvider, IMetadataTokenProvider, ICustomAttributeProvider, IMarshalInfoProvider
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0002B5E2 File Offset: 0x000297E2
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0002B5EA File Offset: 0x000297EA
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x0002B5F2 File Offset: 0x000297F2
		public TypeReference ReturnType
		{
			get
			{
				return this.return_type;
			}
			set
			{
				this.return_type = value;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0002B5FB File Offset: 0x000297FB
		internal ParameterDefinition Parameter
		{
			get
			{
				if (this.parameter == null)
				{
					Interlocked.CompareExchange<ParameterDefinition>(ref this.parameter, new ParameterDefinition(this.return_type, this.method), null);
				}
				return this.parameter;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0002B629 File Offset: 0x00029829
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x0002B636 File Offset: 0x00029836
		public MetadataToken MetadataToken
		{
			get
			{
				return this.Parameter.MetadataToken;
			}
			set
			{
				this.Parameter.MetadataToken = value;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0002B644 File Offset: 0x00029844
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x0002B651 File Offset: 0x00029851
		public ParameterAttributes Attributes
		{
			get
			{
				return this.Parameter.Attributes;
			}
			set
			{
				this.Parameter.Attributes = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0002B65F File Offset: 0x0002985F
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x0002B66C File Offset: 0x0002986C
		public string Name
		{
			get
			{
				return this.Parameter.Name;
			}
			set
			{
				this.Parameter.Name = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0002B67A File Offset: 0x0002987A
		public bool HasCustomAttributes
		{
			get
			{
				return this.parameter != null && this.parameter.HasCustomAttributes;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0002B691 File Offset: 0x00029891
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.Parameter.CustomAttributes;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0002B69E File Offset: 0x0002989E
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x0002B6B5 File Offset: 0x000298B5
		public bool HasDefault
		{
			get
			{
				return this.parameter != null && this.parameter.HasDefault;
			}
			set
			{
				this.Parameter.HasDefault = value;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0002B6C3 File Offset: 0x000298C3
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x0002B6DA File Offset: 0x000298DA
		public bool HasConstant
		{
			get
			{
				return this.parameter != null && this.parameter.HasConstant;
			}
			set
			{
				this.Parameter.HasConstant = value;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0002B6E8 File Offset: 0x000298E8
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x0002B6F5 File Offset: 0x000298F5
		public object Constant
		{
			get
			{
				return this.Parameter.Constant;
			}
			set
			{
				this.Parameter.Constant = value;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0002B703 File Offset: 0x00029903
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x0002B71A File Offset: 0x0002991A
		public bool HasFieldMarshal
		{
			get
			{
				return this.parameter != null && this.parameter.HasFieldMarshal;
			}
			set
			{
				this.Parameter.HasFieldMarshal = value;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0002B728 File Offset: 0x00029928
		public bool HasMarshalInfo
		{
			get
			{
				return this.parameter != null && this.parameter.HasMarshalInfo;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0002B73F File Offset: 0x0002993F
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x0002B74C File Offset: 0x0002994C
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.Parameter.MarshalInfo;
			}
			set
			{
				this.Parameter.MarshalInfo = value;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0002B75A File Offset: 0x0002995A
		public MethodReturnType(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x040003CD RID: 973
		internal IMethodSignature method;

		// Token: 0x040003CE RID: 974
		internal ParameterDefinition parameter;

		// Token: 0x040003CF RID: 975
		private TypeReference return_type;
	}
}
