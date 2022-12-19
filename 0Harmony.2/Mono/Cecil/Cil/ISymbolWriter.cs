using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002EF RID: 751
	public interface ISymbolWriter : IDisposable
	{
		// Token: 0x06001304 RID: 4868
		ISymbolReaderProvider GetReaderProvider();

		// Token: 0x06001305 RID: 4869
		ImageDebugHeader GetDebugHeader();

		// Token: 0x06001306 RID: 4870
		void Write(MethodDebugInformation info);
	}
}
