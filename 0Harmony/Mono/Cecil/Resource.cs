using System;

namespace Mono.Cecil
{
	// Token: 0x0200025C RID: 604
	public abstract class Resource
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0002DB4D File Offset: 0x0002BD4D
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x0002DB55 File Offset: 0x0002BD55
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

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0002DB5E File Offset: 0x0002BD5E
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x0002DB66 File Offset: 0x0002BD66
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

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000EB4 RID: 3764
		public abstract ResourceType ResourceType { get; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002DB6F File Offset: 0x0002BD6F
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0002DB7E File Offset: 0x0002BD7E
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

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002DB94 File Offset: 0x0002BD94
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0002DBA3 File Offset: 0x0002BDA3
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

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0002DBB9 File Offset: 0x0002BDB9
		internal Resource(string name, ManifestResourceAttributes attributes)
		{
			this.name = name;
			this.attributes = (uint)attributes;
		}

		// Token: 0x040004B8 RID: 1208
		private string name;

		// Token: 0x040004B9 RID: 1209
		private uint attributes;
	}
}
