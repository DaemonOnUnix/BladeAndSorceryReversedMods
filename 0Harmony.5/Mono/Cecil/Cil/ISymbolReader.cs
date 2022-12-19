using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E8 RID: 744
	public interface ISymbolReader : IDisposable
	{
		// Token: 0x060012F2 RID: 4850
		ISymbolWriterProvider GetWriterProvider();

		// Token: 0x060012F3 RID: 4851
		bool ProcessDebugHeader(ImageDebugHeader header);

		// Token: 0x060012F4 RID: 4852
		MethodDebugInformation Read(MethodDefinition method);
	}
}
