using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F9 RID: 505
	public interface ISymbolWriter : IDisposable
	{
		// Token: 0x06000F94 RID: 3988
		ISymbolReaderProvider GetReaderProvider();

		// Token: 0x06000F95 RID: 3989
		ImageDebugHeader GetDebugHeader();

		// Token: 0x06000F96 RID: 3990
		void Write(MethodDebugInformation info);
	}
}
