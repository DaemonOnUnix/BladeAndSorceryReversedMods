using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D1 RID: 465
	internal sealed class EmbeddedPortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000EC7 RID: 3783 RVA: 0x00033924 File Offset: 0x00031B24
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

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00033968 File Offset: 0x00031B68
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

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000039F6 File Offset: 0x00001BF6
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
