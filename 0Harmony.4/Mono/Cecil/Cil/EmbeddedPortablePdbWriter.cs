using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002CC RID: 716
	internal sealed class EmbeddedPortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x0600124B RID: 4683 RVA: 0x0003BC60 File Offset: 0x00039E60
		internal EmbeddedPortablePdbWriter(Stream stream, PortablePdbWriter writer)
		{
			this.stream = stream;
			this.writer = writer;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0003BC76 File Offset: 0x00039E76
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new EmbeddedPortablePdbReaderProvider();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0003BC80 File Offset: 0x00039E80
		public ImageDebugHeader GetDebugHeader()
		{
			this.writer.Dispose();
			ImageDebugDirectory imageDebugDirectory = new ImageDebugDirectory
			{
				Type = ImageDebugType.EmbeddedPortablePdb,
				MajorVersion = 256,
				MinorVersion = 256
			};
			MemoryStream memoryStream = new MemoryStream();
			BinaryStreamWriter binaryStreamWriter = new BinaryStreamWriter(memoryStream);
			binaryStreamWriter.WriteByte(77);
			binaryStreamWriter.WriteByte(80);
			binaryStreamWriter.WriteByte(68);
			binaryStreamWriter.WriteByte(66);
			binaryStreamWriter.WriteInt32((int)this.stream.Length);
			this.stream.Position = 0L;
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
			{
				this.stream.CopyTo(deflateStream);
			}
			imageDebugDirectory.SizeOfData = (int)memoryStream.Length;
			return new ImageDebugHeader(new ImageDebugHeaderEntry[]
			{
				this.writer.GetDebugHeader().Entries[0],
				new ImageDebugHeaderEntry(imageDebugDirectory, memoryStream.ToArray())
			});
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0003BD7C File Offset: 0x00039F7C
		public void Write(MethodDebugInformation info)
		{
			this.writer.Write(info);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00018105 File Offset: 0x00016305
		public void Dispose()
		{
		}

		// Token: 0x04000931 RID: 2353
		private readonly Stream stream;

		// Token: 0x04000932 RID: 2354
		private readonly PortablePdbWriter writer;
	}
}
