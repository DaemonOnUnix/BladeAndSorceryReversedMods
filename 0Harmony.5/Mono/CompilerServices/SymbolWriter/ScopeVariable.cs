using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000309 RID: 777
	public struct ScopeVariable
	{
		// Token: 0x06001367 RID: 4967 RVA: 0x0003E81F File Offset: 0x0003CA1F
		public ScopeVariable(int scope, int index)
		{
			this.Scope = scope;
			this.Index = index;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0003E82F File Offset: 0x0003CA2F
		internal ScopeVariable(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.Index = reader.ReadLeb128();
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0003E849 File Offset: 0x0003CA49
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.WriteLeb128(this.Index);
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0003E863 File Offset: 0x0003CA63
		public override string ToString()
		{
			return string.Format("[ScopeVariable {0}:{1}]", this.Scope, this.Index);
		}

		// Token: 0x040009FC RID: 2556
		public readonly int Scope;

		// Token: 0x040009FD RID: 2557
		public readonly int Index;
	}
}
