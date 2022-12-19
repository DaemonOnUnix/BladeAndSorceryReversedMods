using System;

namespace Mono.Cecil
{
	// Token: 0x02000158 RID: 344
	public class ModuleReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000268D4 File Offset: 0x00024AD4
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x000268DC File Offset: 0x00024ADC
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00012561 File Offset: 0x00010761
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleReference;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x000268E5 File Offset: 0x00024AE5
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x000268ED File Offset: 0x00024AED
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

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000268F6 File Offset: 0x00024AF6
		internal ModuleReference()
		{
			this.token = new MetadataToken(TokenType.ModuleRef);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002690E File Offset: 0x00024B0E
		public ModuleReference(string name)
			: this()
		{
			this.name = name;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000268D4 File Offset: 0x00024AD4
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400041C RID: 1052
		private string name;

		// Token: 0x0400041D RID: 1053
		internal MetadataToken token;
	}
}
