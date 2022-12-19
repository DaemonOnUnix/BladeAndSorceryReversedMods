using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000303 RID: 771
	public class CodeBlockEntry
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x0003E575 File Offset: 0x0003C775
		public CodeBlockEntry(int index, int parent, CodeBlockEntry.Type type, int start_offset)
		{
			this.Index = index;
			this.Parent = parent;
			this.BlockType = type;
			this.StartOffset = start_offset;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0003E59C File Offset: 0x0003C79C
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

		// Token: 0x06001358 RID: 4952 RVA: 0x0003E60C File Offset: 0x0003C80C
		public void Close(int end_offset)
		{
			this.EndOffset = end_offset;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0003E615 File Offset: 0x0003C815
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128((int)this.BlockType);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.StartOffset);
			bw.WriteLeb128(this.EndOffset);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0003E648 File Offset: 0x0003C848
		public override string ToString()
		{
			return string.Format("[CodeBlock {0}:{1}:{2}:{3}:{4}]", new object[] { this.Index, this.Parent, this.BlockType, this.StartOffset, this.EndOffset });
		}

		// Token: 0x040009E6 RID: 2534
		public int Index;

		// Token: 0x040009E7 RID: 2535
		public int Parent;

		// Token: 0x040009E8 RID: 2536
		public CodeBlockEntry.Type BlockType;

		// Token: 0x040009E9 RID: 2537
		public int StartOffset;

		// Token: 0x040009EA RID: 2538
		public int EndOffset;

		// Token: 0x02000304 RID: 772
		public enum Type
		{
			// Token: 0x040009EC RID: 2540
			Lexical = 1,
			// Token: 0x040009ED RID: 2541
			CompilerGenerated,
			// Token: 0x040009EE RID: 2542
			IteratorBody,
			// Token: 0x040009EF RID: 2543
			IteratorDispatcher
		}
	}
}
