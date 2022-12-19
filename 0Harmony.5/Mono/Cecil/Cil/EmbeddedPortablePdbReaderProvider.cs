using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C7 RID: 711
	internal sealed class EmbeddedPortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001230 RID: 4656 RVA: 0x0003B74C File Offset: 0x0003994C
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			ImageDebugHeaderEntry embeddedPortablePdbEntry = module.GetDebugHeader().GetEmbeddedPortablePdbEntry();
			if (embeddedPortablePdbEntry == null)
			{
				throw new InvalidOperationException();
			}
			return new EmbeddedPortablePdbReader((PortablePdbReader)new PortablePdbReaderProvider().GetSymbolReader(module, EmbeddedPortablePdbReaderProvider.GetPortablePdbStream(embeddedPortablePdbEntry)));
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0003B790 File Offset: 0x00039990
		private static Stream GetPortablePdbStream(ImageDebugHeaderEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream(entry.Data);
			BinaryStreamReader binaryStreamReader = new BinaryStreamReader(memoryStream);
			binaryStreamReader.ReadInt32();
			MemoryStream memoryStream2 = new MemoryStream(binaryStreamReader.ReadInt32());
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
			{
				deflateStream.CopyTo(memoryStream2);
			}
			return memoryStream2;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00003A32 File Offset: 0x00001C32
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
