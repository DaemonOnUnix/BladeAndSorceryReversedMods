using System;

namespace Mono.Cecil
{
	// Token: 0x020000C1 RID: 193
	internal sealed class AssemblyLinkedResource : Resource
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00014A92 File Offset: 0x00012C92
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x00014A9A File Offset: 0x00012C9A
		public AssemblyNameReference Assembly
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00014AA3 File Offset: 0x00012CA3
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.AssemblyLinked;
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00014AA6 File Offset: 0x00012CA6
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00014AB0 File Offset: 0x00012CB0
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags, AssemblyNameReference reference)
			: base(name, flags)
		{
			this.reference = reference;
		}

		// Token: 0x04000245 RID: 581
		private AssemblyNameReference reference;
	}
}
