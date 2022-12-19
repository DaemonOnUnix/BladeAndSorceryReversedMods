using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200016B RID: 363
	[DebuggerDisplay("{AttributeType}")]
	public sealed class SecurityAttribute : ICustomAttribute
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00027567 File Offset: 0x00025767
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0002756F File Offset: 0x0002576F
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

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00027578 File Offset: 0x00025778
		public bool HasFields
		{
			get
			{
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x00027588 File Offset: 0x00025788
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

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x000275AA File Offset: 0x000257AA
		public bool HasProperties
		{
			get
			{
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x000275BA File Offset: 0x000257BA
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

		// Token: 0x06000B78 RID: 2936 RVA: 0x000275DC File Offset: 0x000257DC
		public SecurityAttribute(TypeReference attributeType)
		{
			this.attribute_type = attributeType;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00011F38 File Offset: 0x00010138
		bool ICustomAttribute.HasConstructorArguments
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x000039F6 File Offset: 0x00001BF6
		Collection<CustomAttributeArgument> ICustomAttribute.ConstructorArguments
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x04000495 RID: 1173
		private TypeReference attribute_type;

		// Token: 0x04000496 RID: 1174
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x04000497 RID: 1175
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
