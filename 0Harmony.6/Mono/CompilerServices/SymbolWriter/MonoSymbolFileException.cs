using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000202 RID: 514
	internal class MonoSymbolFileException : Exception
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x0000A7EC File Offset: 0x000089EC
		public MonoSymbolFileException()
		{
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00035384 File Offset: 0x00033584
		public MonoSymbolFileException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0000A7FD File Offset: 0x000089FD
		public MonoSymbolFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
