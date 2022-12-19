using System;

namespace Mono.Cecil
{
	// Token: 0x02000130 RID: 304
	internal sealed class LinkedResource : Resource
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00022F39 File Offset: 0x00021139
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00022F41 File Offset: 0x00021141
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00022F49 File Offset: 0x00021149
		public string File
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00011F38 File Offset: 0x00010138
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Linked;
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00014AA6 File Offset: 0x00012CA6
		public LinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00022F52 File Offset: 0x00021152
		public LinkedResource(string name, ManifestResourceAttributes flags, string file)
			: base(name, flags)
		{
			this.file = file;
		}

		// Token: 0x04000317 RID: 791
		internal byte[] hash;

		// Token: 0x04000318 RID: 792
		private string file;
	}
}
