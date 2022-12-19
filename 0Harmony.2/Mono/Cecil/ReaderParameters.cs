using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x02000241 RID: 577
	public sealed class ReaderParameters
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0002B98E File Offset: 0x00029B8E
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x0002B996 File Offset: 0x00029B96
		public ReadingMode ReadingMode
		{
			get
			{
				return this.reading_mode;
			}
			set
			{
				this.reading_mode = value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0002B99F File Offset: 0x00029B9F
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x0002B9A7 File Offset: 0x00029BA7
		public bool InMemory
		{
			get
			{
				return this.in_memory;
			}
			set
			{
				this.in_memory = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0002B9B0 File Offset: 0x00029BB0
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x0002B9B8 File Offset: 0x00029BB8
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				return this.assembly_resolver;
			}
			set
			{
				this.assembly_resolver = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0002B9C1 File Offset: 0x00029BC1
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x0002B9C9 File Offset: 0x00029BC9
		public IMetadataResolver MetadataResolver
		{
			get
			{
				return this.metadata_resolver;
			}
			set
			{
				this.metadata_resolver = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0002B9D2 File Offset: 0x00029BD2
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x0002B9DA File Offset: 0x00029BDA
		public IMetadataImporterProvider MetadataImporterProvider
		{
			get
			{
				return this.metadata_importer_provider;
			}
			set
			{
				this.metadata_importer_provider = value;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x0002B9E3 File Offset: 0x00029BE3
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x0002B9EB File Offset: 0x00029BEB
		public IReflectionImporterProvider ReflectionImporterProvider
		{
			get
			{
				return this.reflection_importer_provider;
			}
			set
			{
				this.reflection_importer_provider = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x0002B9F4 File Offset: 0x00029BF4
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0002B9FC File Offset: 0x00029BFC
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

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x0002BA05 File Offset: 0x00029C05
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x0002BA0D File Offset: 0x00029C0D
		public ISymbolReaderProvider SymbolReaderProvider
		{
			get
			{
				return this.symbol_reader_provider;
			}
			set
			{
				this.symbol_reader_provider = value;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0002BA16 File Offset: 0x00029C16
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x0002BA1E File Offset: 0x00029C1E
		public bool ReadSymbols
		{
			get
			{
				return this.read_symbols;
			}
			set
			{
				this.read_symbols = value;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0002BA27 File Offset: 0x00029C27
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x0002BA2F File Offset: 0x00029C2F
		public bool ThrowIfSymbolsAreNotMatching
		{
			get
			{
				return this.throw_symbols_mismatch;
			}
			set
			{
				this.throw_symbols_mismatch = value;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0002BA38 File Offset: 0x00029C38
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x0002BA40 File Offset: 0x00029C40
		public bool ReadWrite
		{
			get
			{
				return this.read_write;
			}
			set
			{
				this.read_write = value;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0002BA49 File Offset: 0x00029C49
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x0002BA51 File Offset: 0x00029C51
		public bool ApplyWindowsRuntimeProjections
		{
			get
			{
				return this.projections;
			}
			set
			{
				this.projections = value;
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0002BA5A File Offset: 0x00029C5A
		public ReaderParameters()
			: this(ReadingMode.Deferred)
		{
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002BA63 File Offset: 0x00029C63
		public ReaderParameters(ReadingMode readingMode)
		{
			this.reading_mode = readingMode;
			this.throw_symbols_mismatch = true;
		}

		// Token: 0x040003DE RID: 990
		private ReadingMode reading_mode;

		// Token: 0x040003DF RID: 991
		internal IAssemblyResolver assembly_resolver;

		// Token: 0x040003E0 RID: 992
		internal IMetadataResolver metadata_resolver;

		// Token: 0x040003E1 RID: 993
		internal IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x040003E2 RID: 994
		internal IReflectionImporterProvider reflection_importer_provider;

		// Token: 0x040003E3 RID: 995
		private Stream symbol_stream;

		// Token: 0x040003E4 RID: 996
		private ISymbolReaderProvider symbol_reader_provider;

		// Token: 0x040003E5 RID: 997
		private bool read_symbols;

		// Token: 0x040003E6 RID: 998
		private bool throw_symbols_mismatch;

		// Token: 0x040003E7 RID: 999
		private bool projections;

		// Token: 0x040003E8 RID: 1000
		private bool in_memory;

		// Token: 0x040003E9 RID: 1001
		private bool read_write;
	}
}
