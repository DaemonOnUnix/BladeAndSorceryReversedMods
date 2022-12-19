using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E4 RID: 740
	internal sealed class EmbeddedSourceDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0003C80B File Offset: 0x0003AA0B
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0003C821 File Offset: 0x0003AA21
		public byte[] Content
		{
			get
			{
				if (!this.resolved)
				{
					this.Resolve();
				}
				return this.content;
			}
			set
			{
				this.content = value;
				this.resolved = true;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0003C831 File Offset: 0x0003AA31
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0003C847 File Offset: 0x0003AA47
		public bool Compress
		{
			get
			{
				if (!this.resolved)
				{
					this.Resolve();
				}
				return this.compress;
			}
			set
			{
				this.compress = value;
				this.resolved = true;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0003C857 File Offset: 0x0003AA57
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.EmbeddedSource;
			}
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0003C85A File Offset: 0x0003AA5A
		internal EmbeddedSourceDebugInformation(uint index, MetadataReader debug_reader)
			: base(EmbeddedSourceDebugInformation.KindIdentifier)
		{
			this.index = index;
			this.debug_reader = debug_reader;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0003C875 File Offset: 0x0003AA75
		public EmbeddedSourceDebugInformation(byte[] content, bool compress)
			: base(EmbeddedSourceDebugInformation.KindIdentifier)
		{
			this.resolved = true;
			this.content = content;
			this.compress = compress;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0003C897 File Offset: 0x0003AA97
		internal byte[] ReadRawEmbeddedSourceDebugInformation()
		{
			if (this.debug_reader == null)
			{
				throw new InvalidOperationException();
			}
			return this.debug_reader.ReadRawEmbeddedSourceDebugInformation(this.index);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
		private void Resolve()
		{
			if (this.resolved)
			{
				return;
			}
			if (this.debug_reader == null)
			{
				throw new InvalidOperationException();
			}
			Row<byte[], bool> row = this.debug_reader.ReadEmbeddedSourceDebugInformation(this.index);
			this.content = row.Col1;
			this.compress = row.Col2;
			this.resolved = true;
		}

		// Token: 0x04000989 RID: 2441
		internal uint index;

		// Token: 0x0400098A RID: 2442
		internal MetadataReader debug_reader;

		// Token: 0x0400098B RID: 2443
		internal bool resolved;

		// Token: 0x0400098C RID: 2444
		internal byte[] content;

		// Token: 0x0400098D RID: 2445
		internal bool compress;

		// Token: 0x0400098E RID: 2446
		public static Guid KindIdentifier = new Guid("{0E8A571B-6926-466E-B4AD-8AB04611F5FE}");
	}
}
