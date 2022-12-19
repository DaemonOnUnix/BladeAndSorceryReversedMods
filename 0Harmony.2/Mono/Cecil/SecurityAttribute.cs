using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200025F RID: 607
	[DebuggerDisplay("{AttributeType}")]
	public sealed class SecurityAttribute : ICustomAttribute
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0002DBCF File Offset: 0x0002BDCF
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x0002DBD7 File Offset: 0x0002BDD7
		public TypeReference AttributeType
		{
			get
			{
				return this.attribute_type;
			}
			set
			{
				this.attribute_type = value;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
		public bool HasFields
		{
			get
			{
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				if (this.fields == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.fields, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.fields;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0002DC12 File Offset: 0x0002BE12
		public bool HasProperties
		{
			get
			{
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0002DC22 File Offset: 0x0002BE22
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				if (this.properties == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.properties, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.properties;
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002DC44 File Offset: 0x0002BE44
		public SecurityAttribute(TypeReference attributeType)
		{
			this.attribute_type = attributeType;
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00017DC4 File Offset: 0x00015FC4
		bool ICustomAttribute.HasConstructorArguments
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00003A32 File Offset: 0x00001C32
		Collection<CustomAttributeArgument> ICustomAttribute.ConstructorArguments
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x040004CA RID: 1226
		private TypeReference attribute_type;

		// Token: 0x040004CB RID: 1227
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x040004CC RID: 1228
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
