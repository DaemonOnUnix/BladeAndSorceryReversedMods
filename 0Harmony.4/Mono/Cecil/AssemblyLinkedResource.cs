using System;

namespace Mono.Cecil
{
	// Token: 0x020001B3 RID: 435
	internal sealed class AssemblyLinkedResource : Resource
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001A91E File Offset: 0x00018B1E
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0001A926 File Offset: 0x00018B26
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001A92F File Offset: 0x00018B2F
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.AssemblyLinked;
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001A932 File Offset: 0x00018B32
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001A93C File Offset: 0x00018B3C
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags, AssemblyNameReference reference)
			: base(name, flags)
		{
			this.reference = reference;
		}

		// Token: 0x04000277 RID: 631
		private AssemblyNameReference reference;
	}
}
