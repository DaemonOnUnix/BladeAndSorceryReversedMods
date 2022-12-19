using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000409 RID: 1033
	internal class PdbReader
	{
		// Token: 0x060015DD RID: 5597 RVA: 0x00045ECB File Offset: 0x000440CB
		internal PdbReader(Stream reader, int pageSize)
		{
			this.pageSize = pageSize;
			this.reader = reader;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00045EE1 File Offset: 0x000440E1
		internal void Seek(int page, int offset)
		{
			this.reader.Seek((long)(page * this.pageSize + offset), SeekOrigin.Begin);
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00045EFB File Offset: 0x000440FB
		internal void Read(byte[] bytes, int offset, int count)
		{
			this.reader.Read(bytes, offset, count);
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00045F0C File Offset: 0x0004410C
		internal int PagesFromSize(int size)
		{
			return (size + this.pageSize - 1) / this.pageSize;
		}

		// Token: 0x04000F68 RID: 3944
		internal readonly int pageSize;

		// Token: 0x04000F69 RID: 3945
		internal readonly Stream reader;
	}
}
