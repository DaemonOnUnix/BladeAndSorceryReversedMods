using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x020002F9 RID: 761
	internal sealed class MyBinaryWriter : BinaryWriter
	{
		// Token: 0x06001319 RID: 4889 RVA: 0x000331CB File Offset: 0x000313CB
		public MyBinaryWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0003D2DF File Offset: 0x0003B4DF
		public void WriteLeb128(int value)
		{
			base.Write7BitEncodedInt(value);
		}
	}
}
