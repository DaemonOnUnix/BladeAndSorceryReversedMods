using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C0 RID: 448
	public sealed class Document : DebugInformation
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x000314AD File Offset: 0x0002F6AD
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x000314B5 File Offset: 0x0002F6B5
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x000314BE File Offset: 0x0002F6BE
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x000314CB File Offset: 0x0002F6CB
		public DocumentType Type
		{
			get
			{
				return this.type.ToType();
			}
			set
			{
				this.type = value.ToGuid();
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x000314D9 File Offset: 0x0002F6D9
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x000314E1 File Offset: 0x0002F6E1
		public Guid TypeGuid
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x000314EA File Offset: 0x0002F6EA
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x000314F7 File Offset: 0x0002F6F7
		public DocumentHashAlgorithm HashAlgorithm
		{
			get
			{
				return this.hash_algorithm.ToHashAlgorithm();
			}
			set
			{
				this.hash_algorithm = value.ToGuid();
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00031505 File Offset: 0x0002F705
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0003150D File Offset: 0x0002F70D
		public Guid HashAlgorithmGuid
		{
			get
			{
				return this.hash_algorithm;
			}
			set
			{
				this.hash_algorithm = value;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00031516 File Offset: 0x0002F716
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x00031523 File Offset: 0x0002F723
		public DocumentLanguage Language
		{
			get
			{
				return this.language.ToLanguage();
			}
			set
			{
				this.language = value.ToGuid();
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00031531 File Offset: 0x0002F731
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x00031539 File Offset: 0x0002F739
		public Guid LanguageGuid
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00031542 File Offset: 0x0002F742
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0003154F File Offset: 0x0002F74F
		public DocumentLanguageVendor LanguageVendor
		{
			get
			{
				return this.language_vendor.ToVendor();
			}
			set
			{
				this.language_vendor = value.ToGuid();
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0003155D File Offset: 0x0002F75D
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00031565 File Offset: 0x0002F765
		public Guid LanguageVendorGuid
		{
			get
			{
				return this.language_vendor;
			}
			set
			{
				this.language_vendor = value;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0003156E File Offset: 0x0002F76E
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00031576 File Offset: 0x0002F776
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0003157F File Offset: 0x0002F77F
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00031587 File Offset: 0x0002F787
		public byte[] EmbeddedSource
		{
			get
			{
				return this.embedded_source;
			}
			set
			{
				this.embedded_source = value;
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00031590 File Offset: 0x0002F790
		public Document(string url)
		{
			this.url = url;
			this.hash = Empty<byte>.Array;
			this.embedded_source = Empty<byte>.Array;
			this.token = new MetadataToken(TokenType.Document);
		}

		// Token: 0x0400079E RID: 1950
		private string url;

		// Token: 0x0400079F RID: 1951
		private Guid type;

		// Token: 0x040007A0 RID: 1952
		private Guid hash_algorithm;

		// Token: 0x040007A1 RID: 1953
		private Guid language;

		// Token: 0x040007A2 RID: 1954
		private Guid language_vendor;

		// Token: 0x040007A3 RID: 1955
		private byte[] hash;

		// Token: 0x040007A4 RID: 1956
		private byte[] embedded_source;
	}
}
