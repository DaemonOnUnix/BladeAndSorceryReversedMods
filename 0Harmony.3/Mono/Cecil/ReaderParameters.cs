using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x0200014D RID: 333
	public sealed class ReaderParameters
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0002535A File Offset: 0x0002355A
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x00025362 File Offset: 0x00023562
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0002536B File Offset: 0x0002356B
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x00025373 File Offset: 0x00023573
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002537C File Offset: 0x0002357C
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x00025384 File Offset: 0x00023584
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

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0002538D File Offset: 0x0002358D
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x00025395 File Offset: 0x00023595
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

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002539E File Offset: 0x0002359E
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x000253A6 File Offset: 0x000235A6
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x000253AF File Offset: 0x000235AF
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x000253B7 File Offset: 0x000235B7
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x000253C0 File Offset: 0x000235C0
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x000253C8 File Offset: 0x000235C8
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

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x000253D1 File Offset: 0x000235D1
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x000253D9 File Offset: 0x000235D9
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x000253E2 File Offset: 0x000235E2
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x000253EA File Offset: 0x000235EA
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

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x000253F3 File Offset: 0x000235F3
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x000253FB File Offset: 0x000235FB
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

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00025404 File Offset: 0x00023604
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0002540C File Offset: 0x0002360C
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00025415 File Offset: 0x00023615
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x0002541D File Offset: 0x0002361D
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

		// Token: 0x06000A1F RID: 2591 RVA: 0x00025426 File Offset: 0x00023626
		public ReaderParameters()
			: this(ReadingMode.Deferred)
		{
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0002542F File Offset: 0x0002362F
		public ReaderParameters(ReadingMode readingMode)
		{
			this.reading_mode = readingMode;
			this.throw_symbols_mismatch = true;
		}

		// Token: 0x040003AA RID: 938
		private ReadingMode reading_mode;

		// Token: 0x040003AB RID: 939
		internal IAssemblyResolver assembly_resolver;

		// Token: 0x040003AC RID: 940
		internal IMetadataResolver metadata_resolver;

		// Token: 0x040003AD RID: 941
		internal IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x040003AE RID: 942
		internal IReflectionImporterProvider reflection_importer_provider;

		// Token: 0x040003AF RID: 943
		private Stream symbol_stream;

		// Token: 0x040003B0 RID: 944
		private ISymbolReaderProvider symbol_reader_provider;

		// Token: 0x040003B1 RID: 945
		private bool read_symbols;

		// Token: 0x040003B2 RID: 946
		private bool throw_symbols_mismatch;

		// Token: 0x040003B3 RID: 947
		private bool projections;

		// Token: 0x040003B4 RID: 948
		private bool in_memory;

		// Token: 0x040003B5 RID: 949
		private bool read_write;
	}
}
