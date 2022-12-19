using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x020002F8 RID: 760
	internal class MonoSymbolFileException : Exception
	{
		// Token: 0x06001316 RID: 4886 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		public MonoSymbolFileException()
		{
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0003D2D0 File Offset: 0x0003B4D0
		public MonoSymbolFileException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0000B9B1 File Offset: 0x00009BB1
		public MonoSymbolFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
