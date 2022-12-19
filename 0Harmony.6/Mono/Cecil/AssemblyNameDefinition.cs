using System;

namespace Mono.Cecil
{
	// Token: 0x020000C2 RID: 194
	public sealed class AssemblyNameDefinition : AssemblyNameReference
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00014AC1 File Offset: 0x00012CC1
		public override byte[] Hash
		{
			get
			{
				return Empty<byte>.Array;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00014AC8 File Offset: 0x00012CC8
		internal AssemblyNameDefinition()
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014AE1 File Offset: 0x00012CE1
		public AssemblyNameDefinition(string name, Version version)
			: base(name, version)
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}
	}
}
