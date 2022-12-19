using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000119 RID: 281
	public sealed class GenericParameterConstraint : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00021C6D File Offset: 0x0001FE6D
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x00021C75 File Offset: 0x0001FE75
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00021C7E File Offset: 0x0001FE7E
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00021CB4 File Offset: 0x0001FEB4
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00021D0B File Offset: 0x0001FF0B
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x00021D13 File Offset: 0x0001FF13
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

		// Token: 0x0600082D RID: 2093 RVA: 0x00021D1C File Offset: 0x0001FF1C
		internal GenericParameterConstraint(TypeReference constraintType, MetadataToken token)
		{
			this.constraint_type = constraintType;
			this.token = token;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00021D32 File Offset: 0x0001FF32
		public GenericParameterConstraint(TypeReference constraintType)
		{
			Mixin.CheckType(constraintType, Mixin.Argument.constraintType);
			this.constraint_type = constraintType;
			this.token = new MetadataToken(TokenType.GenericParamConstraint);
		}

		// Token: 0x040002FB RID: 763
		internal GenericParameter generic_parameter;

		// Token: 0x040002FC RID: 764
		internal MetadataToken token;

		// Token: 0x040002FD RID: 765
		private TypeReference constraint_type;

		// Token: 0x040002FE RID: 766
		private Collection<CustomAttribute> custom_attributes;
	}
}
