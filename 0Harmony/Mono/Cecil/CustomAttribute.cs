using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001F7 RID: 503
	[DebuggerDisplay("{AttributeType}")]
	public sealed class CustomAttribute : ICustomAttribute
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00025FCD File Offset: 0x000241CD
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00025FD5 File Offset: 0x000241D5
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

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00025FDE File Offset: 0x000241DE
		public TypeReference AttributeType
		{
			get
			{
				return this.constructor.DeclaringType;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00025FEB File Offset: 0x000241EB
		public bool IsResolved
		{
			get
			{
				return this.resolved;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00025FF3 File Offset: 0x000241F3
		public bool HasConstructorArguments
		{
			get
			{
				this.Resolve();
				return !this.arguments.IsNullOrEmpty<CustomAttributeArgument>();
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00026009 File Offset: 0x00024209
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00026031 File Offset: 0x00024231
		public bool HasFields
		{
			get
			{
				this.Resolve();
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00026047 File Offset: 0x00024247
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

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0002606F File Offset: 0x0002426F
		public bool HasProperties
		{
			get
			{
				this.Resolve();
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00026085 File Offset: 0x00024285
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x000260AD File Offset: 0x000242AD
		internal bool HasImage
		{
			get
			{
				return this.constructor != null && this.constructor.HasImage;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x000260C4 File Offset: 0x000242C4
		internal ModuleDefinition Module
		{
			get
			{
				return this.constructor.Module;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000260D1 File Offset: 0x000242D1
		internal CustomAttribute(uint signature, MethodReference constructor)
		{
			this.signature = signature;
			this.constructor = constructor;
			this.resolved = false;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x000260EE File Offset: 0x000242EE
		public CustomAttribute(MethodReference constructor)
		{
			this.constructor = constructor;
			this.resolved = true;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00026104 File Offset: 0x00024304
		public CustomAttribute(MethodReference constructor, byte[] blob)
		{
			this.constructor = constructor;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00026124 File Offset: 0x00024324
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

		// Token: 0x06000A49 RID: 2633 RVA: 0x00026180 File Offset: 0x00024380
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

		// Token: 0x040002DB RID: 731
		internal CustomAttributeValueProjection projection;

		// Token: 0x040002DC RID: 732
		internal readonly uint signature;

		// Token: 0x040002DD RID: 733
		internal bool resolved;

		// Token: 0x040002DE RID: 734
		private MethodReference constructor;

		// Token: 0x040002DF RID: 735
		private byte[] blob;

		// Token: 0x040002E0 RID: 736
		internal Collection<CustomAttributeArgument> arguments;

		// Token: 0x040002E1 RID: 737
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x040002E2 RID: 738
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
