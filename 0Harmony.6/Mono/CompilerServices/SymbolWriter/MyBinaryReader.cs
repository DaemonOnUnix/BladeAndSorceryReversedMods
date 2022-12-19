using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000204 RID: 516
	internal class MyBinaryReader : BinaryReader
	{
		// Token: 0x06000FAB RID: 4011 RVA: 0x0002B766 File Offset: 0x00029966
		public MyBinaryReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0003539C File Offset: 0x0003359C
		public int ReadLeb128()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000353A4 File Offset: 0x000335A4
		public string ReadString(int offset)
		{
			long position = this.BaseStream.Position;
			this.BaseStream.Position = (long)offset;
			string text = this.ReadString();
			this.BaseStream.Position = position;
			return text;
		}
	}
}
