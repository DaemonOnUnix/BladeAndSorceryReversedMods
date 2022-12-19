using System;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x02000243 RID: 579
	public sealed class WriterParameters
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0002BB41 File Offset: 0x00029D41
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0002BB49 File Offset: 0x00029D49
		public uint? Timestamp
		{
			get
			{
				return this.timestamp;
			}
			set
			{
				this.timestamp = value;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0002BB52 File Offset: 0x00029D52
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x0002BB5A File Offset: 0x00029D5A
		public Stream SymbolStream
		{
			get
			{
				return this.symbol_stream;
			}
			set
			{
				this.symbol_stream = value;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0002BB63 File Offset: 0x00029D63
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x0002BB6B File Offset: 0x00029D6B
		public ISymbolWriterProvider SymbolWriterProvider
		{
			get
			{
				return this.symbol_writer_provider;
			}
			set
			{
				this.symbol_writer_provider = value;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0002BB74 File Offset: 0x00029D74
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x0002BB7C File Offset: 0x00029D7C
		public bool WriteSymbols
		{
			get
			{
				return this.write_symbols;
			}
			set
			{
				this.write_symbols = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0002BB85 File Offset: 0x00029D85
		public bool HasStrongNameKey
		{
			get
			{
				return this.key_pair != null || this.key_blob != null || this.key_container != null;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0002BBA2 File Offset: 0x00029DA2
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0002BBAA File Offset: 0x00029DAA
		public byte[] StrongNameKeyBlob
		{
			get
			{
				return this.key_blob;
			}
			set
			{
				this.key_blob = value;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0002BBB3 File Offset: 0x00029DB3
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x0002BBBB File Offset: 0x00029DBB
		public string StrongNameKeyContainer
		{
			get
			{
				return this.key_container;
			}
			set
			{
				this.key_container = value;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0002BBC4 File Offset: 0x00029DC4
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x0002BBCC File Offset: 0x00029DCC
		public StrongNameKeyPair StrongNameKeyPair
		{
			get
			{
				return this.key_pair;
			}
			set
			{
				this.key_pair = value;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0002BBD5 File Offset: 0x00029DD5
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x0002BBDD File Offset: 0x00029DDD
		public bool DeterministicMvid { get; set; }

		// Token: 0x040003F2 RID: 1010
		private uint? timestamp;

		// Token: 0x040003F3 RID: 1011
		private Stream symbol_stream;

		// Token: 0x040003F4 RID: 1012
		private ISymbolWriterProvider symbol_writer_provider;

		// Token: 0x040003F5 RID: 1013
		private bool write_symbols;

		// Token: 0x040003F6 RID: 1014
		private byte[] key_blob;

		// Token: 0x040003F7 RID: 1015
		private string key_container;

		// Token: 0x040003F8 RID: 1016
		private StrongNameKeyPair key_pair;
	}
}
