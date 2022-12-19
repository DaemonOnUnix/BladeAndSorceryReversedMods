using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D2 RID: 466
	internal sealed class EmbeddedPortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x000339C4 File Offset: 0x00031BC4
		internal EmbeddedPortablePdbReader(PortablePdbReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException();
			}
			this.reader = reader;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000339DC File Offset: 0x00031BDC
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new EmbeddedPortablePdbWriterProvider();
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x000339E3 File Offset: 0x00031BE3
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.reader.ProcessDebugHeader(header);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000339F1 File Offset: 0x00031BF1
		public MethodDebugInformation Read(MethodDefinition method)
		{
			return this.reader.Read(method);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000339FF File Offset: 0x00031BFF
		public void Dispose()
		{
			this.reader.Dispose();
		}

		// Token: 0x040008F0 RID: 2288
		private readonly PortablePdbReader reader;
	}
}
