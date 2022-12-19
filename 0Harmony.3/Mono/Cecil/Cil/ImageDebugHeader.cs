using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DB RID: 475
	public sealed class ImageDebugHeader
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00034267 File Offset: 0x00032467
		public bool HasEntries
		{
			get
			{
				return !this.entries.IsNullOrEmpty<ImageDebugHeaderEntry>();
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x00034277 File Offset: 0x00032477
		public ImageDebugHeaderEntry[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003427F File Offset: 0x0003247F
		public ImageDebugHeader(ImageDebugHeaderEntry[] entries)
		{
			this.entries = entries ?? Empty<ImageDebugHeaderEntry>.Array;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00034297 File Offset: 0x00032497
		public ImageDebugHeader()
			: this(Empty<ImageDebugHeaderEntry>.Array)
		{
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000342A4 File Offset: 0x000324A4
		public ImageDebugHeader(ImageDebugHeaderEntry entry)
			: this(new ImageDebugHeaderEntry[] { entry })
		{
		}

		// Token: 0x04000911 RID: 2321
		private readonly ImageDebugHeaderEntry[] entries;
	}
}
