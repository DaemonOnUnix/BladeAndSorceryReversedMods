using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000179 RID: 377
	public sealed class InterfaceImplementation : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00028497 File Offset: 0x00026697
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0002849F File Offset: 0x0002669F
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

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000284A8 File Offset: 0x000266A8
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

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x000284DC File Offset: 0x000266DC
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

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00028533 File Offset: 0x00026733
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0002853B File Offset: 0x0002673B
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

		// Token: 0x06000BFE RID: 3070 RVA: 0x00028544 File Offset: 0x00026744
		internal InterfaceImplementation(TypeReference interfaceType, MetadataToken token)
		{
			this.interface_type = interfaceType;
			this.token = token;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002855A File Offset: 0x0002675A
		public InterfaceImplementation(TypeReference interfaceType)
		{
			Mixin.CheckType(interfaceType, Mixin.Argument.interfaceType);
			this.interface_type = interfaceType;
			this.token = new MetadataToken(TokenType.InterfaceImpl);
		}

		// Token: 0x04000503 RID: 1283
		internal TypeDefinition type;

		// Token: 0x04000504 RID: 1284
		internal MetadataToken token;

		// Token: 0x04000505 RID: 1285
		private TypeReference interface_type;

		// Token: 0x04000506 RID: 1286
		private Collection<CustomAttribute> custom_attributes;
	}
}
