using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000305 RID: 773
	public struct LocalVariableEntry
	{
		// Token: 0x0600135B RID: 4955 RVA: 0x0003E6AB File Offset: 0x0003C8AB
		public LocalVariableEntry(int index, string name, int block)
		{
			this.Index = index;
			this.Name = name;
			this.BlockIndex = block;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0003E6C2 File Offset: 0x0003C8C2
		internal LocalVariableEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Index = reader.ReadLeb128();
			this.Name = reader.ReadString();
			this.BlockIndex = reader.ReadLeb128();
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0003E6E8 File Offset: 0x0003C8E8
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Index);
			bw.Write(this.Name);
			bw.WriteLeb128(this.BlockIndex);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0003E70E File Offset: 0x0003C90E
		public override string ToString()
		{
			return string.Format("[LocalVariable {0}:{1}:{2}]", this.Name, this.Index, this.BlockIndex - 1);
		}

		// Token: 0x040009F0 RID: 2544
		public readonly int Index;

		// Token: 0x040009F1 RID: 2545
		public readonly string Name;

		// Token: 0x040009F2 RID: 2546
		public readonly int BlockIndex;
	}
}
