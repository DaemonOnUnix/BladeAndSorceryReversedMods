using System;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x0200014F RID: 335
	public sealed class WriterParameters
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0002550D File Offset: 0x0002370D
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00025515 File Offset: 0x00023715
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

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0002551E File Offset: 0x0002371E
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00025526 File Offset: 0x00023726
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002552F File Offset: 0x0002372F
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00025537 File Offset: 0x00023737
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00025540 File Offset: 0x00023740
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00025548 File Offset: 0x00023748
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00025551 File Offset: 0x00023751
		public bool HasStrongNameKey
		{
			get
			{
				return this.key_pair != null || this.key_blob != null || this.key_container != null;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0002556E File Offset: 0x0002376E
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00025576 File Offset: 0x00023776
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002557F File Offset: 0x0002377F
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00025587 File Offset: 0x00023787
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00025590 File Offset: 0x00023790
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x00025598 File Offset: 0x00023798
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x000255A1 File Offset: 0x000237A1
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x000255A9 File Offset: 0x000237A9
		public bool DeterministicMvid { get; set; }

		// Token: 0x040003BE RID: 958
		private uint? timestamp;

		// Token: 0x040003BF RID: 959
		private Stream symbol_stream;

		// Token: 0x040003C0 RID: 960
		private ISymbolWriterProvider symbol_writer_provider;

		// Token: 0x040003C1 RID: 961
		private bool write_symbols;

		// Token: 0x040003C2 RID: 962
		private byte[] key_blob;

		// Token: 0x040003C3 RID: 963
		private string key_container;

		// Token: 0x040003C4 RID: 964
		private StrongNameKeyPair key_pair;
	}
}
