using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C8 RID: 712
	internal sealed class EmbeddedPortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06001234 RID: 4660 RVA: 0x0003B7EC File Offset: 0x000399EC
		internal EmbeddedPortablePdbReader(PortablePdbReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException();
			}
			this.reader = reader;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0003B804 File Offset: 0x00039A04
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new EmbeddedPortablePdbWriterProvider();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003B80B File Offset: 0x00039A0B
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.reader.ProcessDebugHeader(header);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003B819 File Offset: 0x00039A19
		public MethodDebugInformation Read(MethodDefinition method)
		{
			return this.reader.Read(method);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003B827 File Offset: 0x00039A27
		public void Dispose()
		{
			this.reader.Dispose();
		}

		// Token: 0x0400092C RID: 2348
		private readonly PortablePdbReader reader;
	}
}
