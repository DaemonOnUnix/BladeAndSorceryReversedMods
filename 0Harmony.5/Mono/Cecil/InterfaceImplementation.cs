using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200026D RID: 621
	public sealed class InterfaceImplementation : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0002EAFF File Offset: 0x0002CCFF
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0002EB07 File Offset: 0x0002CD07
		public TypeReference InterfaceType
		{
			get
			{
				return this.interface_type;
			}
			set
			{
				this.interface_type = value;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002EB10 File Offset: 0x0002CD10
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.type != null && this.GetHasCustomAttributes(this.type.Module);
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				if (this.type == null)
				{
					if (this.custom_attributes == null)
					{
						Interlocked.CompareExchange<Collection<CustomAttribute>>(ref this.custom_attributes, new Collection<CustomAttribute>(), null);
					}
					return this.custom_attributes;
				}
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.type.Module);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002EB9B File Offset: 0x0002CD9B
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0002EBA3 File Offset: 0x0002CDA3
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

		// Token: 0x06000F48 RID: 3912 RVA: 0x0002EBAC File Offset: 0x0002CDAC
		internal InterfaceImplementation(TypeReference interfaceType, MetadataToken token)
		{
			this.interface_type = interfaceType;
			this.token = token;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0002EBC2 File Offset: 0x0002CDC2
		public InterfaceImplementation(TypeReference interfaceType)
		{
			Mixin.CheckType(interfaceType, Mixin.Argument.interfaceType);
			this.interface_type = interfaceType;
			this.token = new MetadataToken(TokenType.InterfaceImpl);
		}

		// Token: 0x04000539 RID: 1337
		internal TypeDefinition type;

		// Token: 0x0400053A RID: 1338
		internal MetadataToken token;

		// Token: 0x0400053B RID: 1339
		private TypeReference interface_type;

		// Token: 0x0400053C RID: 1340
		private Collection<CustomAttribute> custom_attributes;
	}
}
