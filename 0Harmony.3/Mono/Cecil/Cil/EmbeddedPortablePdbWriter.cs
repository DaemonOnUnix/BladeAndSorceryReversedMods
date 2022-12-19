using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D6 RID: 470
	internal sealed class EmbeddedPortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x00033E0C File Offset: 0x0003200C
		internal EmbeddedPortablePdbWriter(Stream stream, PortablePdbWriter writer)
		{
			this.stream = stream;
			this.writer = writer;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00033E22 File Offset: 0x00032022
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new EmbeddedPortablePdbReaderProvider();
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00033E2C File Offset: 0x0003202C
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

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00033F28 File Offset: 0x00032128
		public void Write(MethodDebugInformation info)
		{
			this.writer.Write(info);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00012279 File Offset: 0x00010479
		public void Dispose()
		{
		}

		// Token: 0x040008F5 RID: 2293
		private readonly Stream stream;

		// Token: 0x040008F6 RID: 2294
		private readonly PortablePdbWriter writer;
	}
}
