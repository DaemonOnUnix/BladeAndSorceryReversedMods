using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D2 RID: 722
	public sealed class ImageDebugHeaderEntry
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x0003C10A File Offset: 0x0003A30A
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x0003C112 File Offset: 0x0003A312
		public ImageDebugDirectory Directory
		{
			get
			{
				return this.directory;
			}
			internal set
			{
				this.directory = value;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0003C11B File Offset: 0x0003A31B
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003C123 File Offset: 0x0003A323
		public ImageDebugHeaderEntry(ImageDebugDirectory directory, byte[] data)
		{
			this.directory = directory;
			this.data = data ?? Empty<byte>.Array;
		}

		// Token: 0x0400094E RID: 2382
		private ImageDebugDirectory directory;

		// Token: 0x0400094F RID: 2383
		private readonly byte[] data;
	}
}
