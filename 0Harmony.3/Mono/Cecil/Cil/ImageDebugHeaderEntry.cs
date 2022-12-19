using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DC RID: 476
	public sealed class ImageDebugHeaderEntry
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000342B6 File Offset: 0x000324B6
		// (set) Token: 0x06000F05 RID: 3845 RVA: 0x000342BE File Offset: 0x000324BE
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

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x000342C7 File Offset: 0x000324C7
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000342CF File Offset: 0x000324CF
		public ImageDebugHeaderEntry(ImageDebugDirectory directory, byte[] data)
		{
			this.directory = directory;
			this.data = data ?? Empty<byte>.Array;
		}

		// Token: 0x04000912 RID: 2322
		private ImageDebugDirectory directory;

		// Token: 0x04000913 RID: 2323
		private readonly byte[] data;
	}
}
