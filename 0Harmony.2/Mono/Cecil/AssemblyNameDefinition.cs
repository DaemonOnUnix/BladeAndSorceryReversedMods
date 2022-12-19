using System;

namespace Mono.Cecil
{
	// Token: 0x020001B4 RID: 436
	public sealed class AssemblyNameDefinition : AssemblyNameReference
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0001A94D File Offset: 0x00018B4D
		public override byte[] Hash
		{
			get
			{
				return Empty<byte>.Array;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001A954 File Offset: 0x00018B54
		internal AssemblyNameDefinition()
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001A96D File Offset: 0x00018B6D
		public AssemblyNameDefinition(string name, Version version)
			: base(name, version)
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}
	}
}
