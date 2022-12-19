using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002B5 RID: 693
	public sealed class Document : DebugInformation
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00038ED9 File Offset: 0x000370D9
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x00038EE1 File Offset: 0x000370E1
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

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00038EEA File Offset: 0x000370EA
		// (set) Token: 0x0600117C RID: 4476 RVA: 0x00038EF7 File Offset: 0x000370F7
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

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00038F05 File Offset: 0x00037105
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x00038F0D File Offset: 0x0003710D
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

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x00038F16 File Offset: 0x00037116
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x00038F23 File Offset: 0x00037123
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

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00038F31 File Offset: 0x00037131
		// (set) Token: 0x06001182 RID: 4482 RVA: 0x00038F39 File Offset: 0x00037139
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

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00038F42 File Offset: 0x00037142
		// (set) Token: 0x06001184 RID: 4484 RVA: 0x00038F4F File Offset: 0x0003714F
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

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00038F5D File Offset: 0x0003715D
		// (set) Token: 0x06001186 RID: 4486 RVA: 0x00038F65 File Offset: 0x00037165
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

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x00038F6E File Offset: 0x0003716E
		// (set) Token: 0x06001188 RID: 4488 RVA: 0x00038F7B File Offset: 0x0003717B
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

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00038F89 File Offset: 0x00037189
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x00038F91 File Offset: 0x00037191
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

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00038F9A File Offset: 0x0003719A
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x00038FA2 File Offset: 0x000371A2
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

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00038FAB File Offset: 0x000371AB
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00038FB3 File Offset: 0x000371B3
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

		// Token: 0x0600118F RID: 4495 RVA: 0x00038FBC File Offset: 0x000371BC
		public Document(string url)
		{
			this.url = url;
			this.hash = Empty<byte>.Array;
			this.embedded_source = Empty<byte>.Array;
			this.token = new MetadataToken(TokenType.Document);
		}

		// Token: 0x040007D6 RID: 2006
		private string url;

		// Token: 0x040007D7 RID: 2007
		private Guid type;

		// Token: 0x040007D8 RID: 2008
		private Guid hash_algorithm;

		// Token: 0x040007D9 RID: 2009
		private Guid language;

		// Token: 0x040007DA RID: 2010
		private Guid language_vendor;

		// Token: 0x040007DB RID: 2011
		private byte[] hash;

		// Token: 0x040007DC RID: 2012
		private byte[] embedded_source;
	}
}
