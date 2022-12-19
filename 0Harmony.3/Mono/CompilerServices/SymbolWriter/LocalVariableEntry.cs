using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020F RID: 527
	public struct LocalVariableEntry
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0003675F File Offset: 0x0003495F
		public LocalVariableEntry(int index, string name, int block)
		{
			this.Index = index;
			this.Name = name;
			this.BlockIndex = block;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00036776 File Offset: 0x00034976
		internal LocalVariableEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Index = reader.ReadLeb128();
			this.Name = reader.ReadString();
			this.BlockIndex = reader.ReadLeb128();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003679C File Offset: 0x0003499C
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Index);
			bw.Write(this.Name);
			bw.WriteLeb128(this.BlockIndex);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x000367C2 File Offset: 0x000349C2
		public override string ToString()
		{
			return string.Format("[LocalVariable {0}:{1}:{2}]", this.Name, this.Index, this.BlockIndex - 1);
		}

		// Token: 0x040009B1 RID: 2481
		public readonly int Index;

		// Token: 0x040009B2 RID: 2482
		public readonly string Name;

		// Token: 0x040009B3 RID: 2483
		public readonly int BlockIndex;
	}
}
