using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F2 RID: 498
	public interface ISymbolReader : IDisposable
	{
		// Token: 0x06000F82 RID: 3970
		ISymbolWriterProvider GetWriterProvider();

		// Token: 0x06000F83 RID: 3971
		bool ProcessDebugHeader(ImageDebugHeader header);

		// Token: 0x06000F84 RID: 3972
		MethodDebugInformation Read(MethodDefinition method);
	}
}
