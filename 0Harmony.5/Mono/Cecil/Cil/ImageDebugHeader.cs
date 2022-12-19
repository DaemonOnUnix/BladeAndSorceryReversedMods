using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D1 RID: 721
	public sealed class ImageDebugHeader
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0003C0BB File Offset: 0x0003A2BB
		public bool HasEntries
		{
			get
			{
				return !this.entries.IsNullOrEmpty<ImageDebugHeaderEntry>();
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0003C0CB File Offset: 0x0003A2CB
		public ImageDebugHeaderEntry[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0003C0D3 File Offset: 0x0003A2D3
		public ImageDebugHeader(ImageDebugHeaderEntry[] entries)
		{
			this.entries = entries ?? Empty<ImageDebugHeaderEntry>.Array;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0003C0EB File Offset: 0x0003A2EB
		public ImageDebugHeader()
			: this(Empty<ImageDebugHeaderEntry>.Array)
		{
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0003C0F8 File Offset: 0x0003A2F8
		public ImageDebugHeader(ImageDebugHeaderEntry entry)
			: this(new ImageDebugHeaderEntry[] { entry })
		{
		}

		// Token: 0x0400094D RID: 2381
		private readonly ImageDebugHeaderEntry[] entries;
	}
}
