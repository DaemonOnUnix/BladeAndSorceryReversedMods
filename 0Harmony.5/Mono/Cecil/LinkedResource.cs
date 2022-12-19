using System;

namespace Mono.Cecil
{
	// Token: 0x02000223 RID: 547
	internal sealed class LinkedResource : Resource
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00029211 File Offset: 0x00027411
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00029219 File Offset: 0x00027419
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00029221 File Offset: 0x00027421
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

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Linked;
			}
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0001A932 File Offset: 0x00018B32
		public LinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002922A File Offset: 0x0002742A
		public LinkedResource(string name, ManifestResourceAttributes flags, string file)
			: base(name, flags)
		{
			this.file = file;
		}

		// Token: 0x04000349 RID: 841
		internal byte[] hash;

		// Token: 0x0400034A RID: 842
		private string file;
	}
}
