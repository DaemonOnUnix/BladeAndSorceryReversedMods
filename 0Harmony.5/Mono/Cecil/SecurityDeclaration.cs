using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000260 RID: 608
	public sealed class SecurityDeclaration
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0002DC53 File Offset: 0x0002BE53
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0002DC5B File Offset: 0x0002BE5B
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

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002DC64 File Offset: 0x0002BE64
		public bool HasSecurityAttributes
		{
			get
			{
				this.Resolve();
				return !this.security_attributes.IsNullOrEmpty<SecurityAttribute>();
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0002DC7A File Offset: 0x0002BE7A
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

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002DCA2 File Offset: 0x0002BEA2
		internal bool HasImage
		{
			get
			{
				return this.module != null && this.module.HasImage;
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0002DCB9 File Offset: 0x0002BEB9
		internal SecurityDeclaration(SecurityAction action, uint signature, ModuleDefinition module)
		{
			this.action = action;
			this.signature = signature;
			this.module = module;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0002DCD6 File Offset: 0x0002BED6
		public SecurityDeclaration(SecurityAction action)
		{
			this.action = action;
			this.resolved = true;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0002DCEC File Offset: 0x0002BEEC
		public SecurityDeclaration(SecurityAction action, byte[] blob)
		{
			this.action = action;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0002DD0C File Offset: 0x0002BF0C
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

		// Token: 0x06000ECE RID: 3790 RVA: 0x0002DD70 File Offset: 0x0002BF70
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

		// Token: 0x040004CD RID: 1229
		internal readonly uint signature;

		// Token: 0x040004CE RID: 1230
		private byte[] blob;

		// Token: 0x040004CF RID: 1231
		private readonly ModuleDefinition module;

		// Token: 0x040004D0 RID: 1232
		internal bool resolved;

		// Token: 0x040004D1 RID: 1233
		private SecurityAction action;

		// Token: 0x040004D2 RID: 1234
		internal Collection<SecurityAttribute> security_attributes;
	}
}
