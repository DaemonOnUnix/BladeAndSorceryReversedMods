using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000313 RID: 787
	internal class PdbReader
	{
		// Token: 0x0600126E RID: 4718 RVA: 0x0003DF83 File Offset: 0x0003C183
		internal PdbReader(Stream reader, int pageSize)
		{
			this.pageSize = pageSize;
			this.reader = reader;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0003DF99 File Offset: 0x0003C199
		internal void Seek(int page, int offset)
		{
			this.reader.Seek((long)(page * this.pageSize + offset), SeekOrigin.Begin);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003DFB3 File Offset: 0x0003C1B3
		internal void Read(byte[] bytes, int offset, int count)
		{
			this.reader.Read(bytes, offset, count);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0003DFC4 File Offset: 0x0003C1C4
		internal int PagesFromSize(int size)
		{
			return (size + this.pageSize - 1) / this.pageSize;
		}

		// Token: 0x04000F2A RID: 3882
		internal readonly int pageSize;

		// Token: 0x04000F2B RID: 3883
		internal readonly Stream reader;
	}
}
