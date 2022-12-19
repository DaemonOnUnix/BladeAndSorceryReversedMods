using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020D RID: 525
	public class CodeBlockEntry
	{
		// Token: 0x06000FE6 RID: 4070 RVA: 0x00036629 File Offset: 0x00034829
		public CodeBlockEntry(int index, int parent, CodeBlockEntry.Type type, int start_offset)
		{
			this.Index = index;
			this.Parent = parent;
			this.BlockType = type;
			this.StartOffset = start_offset;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00036650 File Offset: 0x00034850
		internal CodeBlockEntry(int index, MyBinaryReader reader)
		{
			this.Index = index;
			int num = reader.ReadLeb128();
			this.BlockType = (CodeBlockEntry.Type)(num & 63);
			this.Parent = reader.ReadLeb128();
			this.StartOffset = reader.ReadLeb128();
			this.EndOffset = reader.ReadLeb128();
			if ((num & 64) != 0)
			{
				int num2 = (int)reader.ReadInt16();
				reader.BaseStream.Position += (long)num2;
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x000366C0 File Offset: 0x000348C0
		public void Close(int end_offset)
		{
			this.EndOffset = end_offset;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000366C9 File Offset: 0x000348C9
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128((int)this.BlockType);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.StartOffset);
			bw.WriteLeb128(this.EndOffset);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x000366FC File Offset: 0x000348FC
		public override string ToString()
		{
			return string.Format("[CodeBlock {0}:{1}:{2}:{3}:{4}]", new object[] { this.Index, this.Parent, this.BlockType, this.StartOffset, this.EndOffset });
		}

		// Token: 0x040009A7 RID: 2471
		public int Index;

		// Token: 0x040009A8 RID: 2472
		public int Parent;

		// Token: 0x040009A9 RID: 2473
		public CodeBlockEntry.Type BlockType;

		// Token: 0x040009AA RID: 2474
		public int StartOffset;

		// Token: 0x040009AB RID: 2475
		public int EndOffset;

		// Token: 0x0200020E RID: 526
		public enum Type
		{
			// Token: 0x040009AD RID: 2477
			Lexical = 1,
			// Token: 0x040009AE RID: 2478
			CompilerGenerated,
			// Token: 0x040009AF RID: 2479
			IteratorBody,
			// Token: 0x040009B0 RID: 2480
			IteratorDispatcher
		}
	}
}
