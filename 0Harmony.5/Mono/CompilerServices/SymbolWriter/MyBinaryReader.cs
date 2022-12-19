using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x020002FA RID: 762
	internal class MyBinaryReader : BinaryReader
	{
		// Token: 0x0600131B RID: 4891 RVA: 0x00033146 File Offset: 0x00031346
		public MyBinaryReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0003D2E8 File Offset: 0x0003B4E8
		public int ReadLeb128()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0003D2F0 File Offset: 0x0003B4F0
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
