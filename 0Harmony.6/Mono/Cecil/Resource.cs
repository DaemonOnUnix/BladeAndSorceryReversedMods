using System;

namespace Mono.Cecil
{
	// Token: 0x02000168 RID: 360
	public abstract class Resource
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x000274E5 File Offset: 0x000256E5
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x000274ED File Offset: 0x000256ED
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

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x000274F6 File Offset: 0x000256F6
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x000274FE File Offset: 0x000256FE
		public ManifestResourceAttributes Attributes
		{
			get
			{
				return (ManifestResourceAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B6A RID: 2922
		public abstract ResourceType ResourceType { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00027507 File Offset: 0x00025707
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x00027516 File Offset: 0x00025716
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 1U, value);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0002752C File Offset: 0x0002572C
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0002753B File Offset: 0x0002573B
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 2U, value);
			}
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00027551 File Offset: 0x00025751
		internal Resource(string name, ManifestResourceAttributes attributes)
		{
			this.name = name;
			this.attributes = (uint)attributes;
		}

		// Token: 0x04000483 RID: 1155
		private string name;

		// Token: 0x04000484 RID: 1156
		private uint attributes;
	}
}
