using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000213 RID: 531
	public struct ScopeVariable
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x000368D3 File Offset: 0x00034AD3
		public ScopeVariable(int scope, int index)
		{
			this.Scope = scope;
			this.Index = index;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000368E3 File Offset: 0x00034AE3
		internal ScopeVariable(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.Index = reader.ReadLeb128();
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x000368FD File Offset: 0x00034AFD
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.WriteLeb128(this.Index);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00036917 File Offset: 0x00034B17
		public override string ToString()
		{
			return string.Format("[ScopeVariable {0}:{1}]", this.Scope, this.Index);
		}

		// Token: 0x040009BD RID: 2493
		public readonly int Scope;

		// Token: 0x040009BE RID: 2494
		public readonly int Index;
	}
}
