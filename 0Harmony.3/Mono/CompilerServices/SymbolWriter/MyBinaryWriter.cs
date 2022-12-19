using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000203 RID: 515
	internal sealed class MyBinaryWriter : BinaryWriter
	{
		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002B7EB File Offset: 0x000299EB
		public MyBinaryWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00035393 File Offset: 0x00033593
		public void WriteLeb128(int value)
		{
			base.Write7BitEncodedInt(value);
		}
	}
}
