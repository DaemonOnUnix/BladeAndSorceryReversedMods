using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200020B RID: 523
	public sealed class GenericParameterConstraint : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00027B55 File Offset: 0x00025D55
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x00027B5D File Offset: 0x00025D5D
		public TypeReference ConstraintType
		{
			get
			{
				return this.constraint_type;
			}
			set
			{
				this.constraint_type = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00027B66 File Offset: 0x00025D66
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.generic_parameter != null && this.GetHasCustomAttributes(this.generic_parameter.Module);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00027B9C File Offset: 0x00025D9C
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				if (this.generic_parameter == null)
				{
					if (this.custom_attributes == null)
					{
						Interlocked.CompareExchange<Collection<CustomAttribute>>(ref this.custom_attributes, new Collection<CustomAttribute>(), null);
					}
					return this.custom_attributes;
				}
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.generic_parameter.Module);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00027BF3 File Offset: 0x00025DF3
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x00027BFB File Offset: 0x00025DFB
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00027C04 File Offset: 0x00025E04
		internal GenericParameterConstraint(TypeReference constraintType, MetadataToken token)
		{
			this.constraint_type = constraintType;
			this.token = token;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00027C1A File Offset: 0x00025E1A
		public GenericParameterConstraint(TypeReference constraintType)
		{
			Mixin.CheckType(constraintType, Mixin.Argument.constraintType);
			this.constraint_type = constraintType;
			this.token = new MetadataToken(TokenType.GenericParamConstraint);
		}

		// Token: 0x0400032D RID: 813
		internal GenericParameter generic_parameter;

		// Token: 0x0400032E RID: 814
		internal MetadataToken token;

		// Token: 0x0400032F RID: 815
		private TypeReference constraint_type;

		// Token: 0x04000330 RID: 816
		private Collection<CustomAttribute> custom_attributes;
	}
}
