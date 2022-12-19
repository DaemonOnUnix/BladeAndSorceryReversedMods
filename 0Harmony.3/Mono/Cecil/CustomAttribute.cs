using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000105 RID: 261
	[DebuggerDisplay("{AttributeType}")]
	public sealed class CustomAttribute : ICustomAttribute
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00020125 File Offset: 0x0001E325
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0002012D File Offset: 0x0001E32D
		public MethodReference Constructor
		{
			get
			{
				return this.constructor;
			}
			set
			{
				this.constructor = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00020136 File Offset: 0x0001E336
		public TypeReference AttributeType
		{
			get
			{
				return this.constructor.DeclaringType;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00020143 File Offset: 0x0001E343
		public bool IsResolved
		{
			get
			{
				return this.resolved;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0002014B File Offset: 0x0001E34B
		public bool HasConstructorArguments
		{
			get
			{
				this.Resolve();
				return !this.arguments.IsNullOrEmpty<CustomAttributeArgument>();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00020161 File Offset: 0x0001E361
		public Collection<CustomAttributeArgument> ConstructorArguments
		{
			get
			{
				this.Resolve();
				if (this.arguments == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeArgument>>(ref this.arguments, new Collection<CustomAttributeArgument>(), null);
				}
				return this.arguments;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00020189 File Offset: 0x0001E389
		public bool HasFields
		{
			get
			{
				this.Resolve();
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0002019F File Offset: 0x0001E39F
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				this.Resolve();
				if (this.fields == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.fields, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.fields;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000201C7 File Offset: 0x0001E3C7
		public bool HasProperties
		{
			get
			{
				this.Resolve();
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000201DD File Offset: 0x0001E3DD
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				this.Resolve();
				if (this.properties == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.properties, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.properties;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00020205 File Offset: 0x0001E405
		internal bool HasImage
		{
			get
			{
				return this.constructor != null && this.constructor.HasImage;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0002021C File Offset: 0x0001E41C
		internal ModuleDefinition Module
		{
			get
			{
				return this.constructor.Module;
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00020229 File Offset: 0x0001E429
		internal CustomAttribute(uint signature, MethodReference constructor)
		{
			this.signature = signature;
			this.constructor = constructor;
			this.resolved = false;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00020246 File Offset: 0x0001E446
		public CustomAttribute(MethodReference constructor)
		{
			this.constructor = constructor;
			this.resolved = true;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002025C File Offset: 0x0001E45C
		public CustomAttribute(MethodReference constructor, byte[] blob)
		{
			this.constructor = constructor;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002027C File Offset: 0x0001E47C
		public byte[] GetBlob()
		{
			if (this.blob != null)
			{
				return this.blob;
			}
			if (!this.HasImage)
			{
				throw new NotSupportedException();
			}
			return this.Module.Read<CustomAttribute, byte[]>(ref this.blob, this, (CustomAttribute attribute, MetadataReader reader) => reader.ReadCustomAttributeBlob(attribute.signature));
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000202D8 File Offset: 0x0001E4D8
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.resolved)
				{
					this.Module.Read<CustomAttribute>(this, delegate(CustomAttribute attribute, MetadataReader reader)
					{
						try
						{
							reader.ReadCustomAttributeSignature(attribute);
							this.resolved = true;
						}
						catch (ResolutionException)
						{
							if (this.arguments != null)
							{
								this.arguments.Clear();
							}
							if (this.fields != null)
							{
								this.fields.Clear();
							}
							if (this.properties != null)
							{
								this.properties.Clear();
							}
							this.resolved = false;
						}
					});
				}
			}
		}

		// Token: 0x040002A9 RID: 681
		internal CustomAttributeValueProjection projection;

		// Token: 0x040002AA RID: 682
		internal readonly uint signature;

		// Token: 0x040002AB RID: 683
		internal bool resolved;

		// Token: 0x040002AC RID: 684
		private MethodReference constructor;

		// Token: 0x040002AD RID: 685
		private byte[] blob;

		// Token: 0x040002AE RID: 686
		internal Collection<CustomAttributeArgument> arguments;

		// Token: 0x040002AF RID: 687
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x040002B0 RID: 688
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
