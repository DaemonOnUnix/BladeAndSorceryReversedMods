using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200016C RID: 364
	public sealed class SecurityDeclaration
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x000275EB File Offset: 0x000257EB
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x000275F3 File Offset: 0x000257F3
		public SecurityAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000275FC File Offset: 0x000257FC
		public bool HasSecurityAttributes
		{
			get
			{
				this.Resolve();
				return !this.security_attributes.IsNullOrEmpty<SecurityAttribute>();
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00027612 File Offset: 0x00025812
		public Collection<SecurityAttribute> SecurityAttributes
		{
			get
			{
				this.Resolve();
				if (this.security_attributes == null)
				{
					Interlocked.CompareExchange<Collection<SecurityAttribute>>(ref this.security_attributes, new Collection<SecurityAttribute>(), null);
				}
				return this.security_attributes;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0002763A File Offset: 0x0002583A
		internal bool HasImage
		{
			get
			{
				return this.module != null && this.module.HasImage;
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00027651 File Offset: 0x00025851
		internal SecurityDeclaration(SecurityAction action, uint signature, ModuleDefinition module)
		{
			this.action = action;
			this.signature = signature;
			this.module = module;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002766E File Offset: 0x0002586E
		public SecurityDeclaration(SecurityAction action)
		{
			this.action = action;
			this.resolved = true;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00027684 File Offset: 0x00025884
		public SecurityDeclaration(SecurityAction action, byte[] blob)
		{
			this.action = action;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000276A4 File Offset: 0x000258A4
		public byte[] GetBlob()
		{
			if (this.blob != null)
			{
				return this.blob;
			}
			if (!this.HasImage || this.signature == 0U)
			{
				throw new NotSupportedException();
			}
			return this.module.Read<SecurityDeclaration, byte[]>(ref this.blob, this, (SecurityDeclaration declaration, MetadataReader reader) => reader.ReadSecurityDeclarationBlob(declaration.signature));
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00027708 File Offset: 0x00025908
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			object syncRoot = this.module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.resolved)
				{
					this.module.Read<SecurityDeclaration>(this, delegate(SecurityDeclaration declaration, MetadataReader reader)
					{
						reader.ReadSecurityDeclarationSignature(declaration);
					});
					this.resolved = true;
				}
			}
		}

		// Token: 0x04000498 RID: 1176
		internal readonly uint signature;

		// Token: 0x04000499 RID: 1177
		private byte[] blob;

		// Token: 0x0400049A RID: 1178
		private readonly ModuleDefinition module;

		// Token: 0x0400049B RID: 1179
		internal bool resolved;

		// Token: 0x0400049C RID: 1180
		private SecurityAction action;

		// Token: 0x0400049D RID: 1181
		internal Collection<SecurityAttribute> security_attributes;
	}
}
