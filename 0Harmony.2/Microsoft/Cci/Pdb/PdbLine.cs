using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000407 RID: 1031
	internal struct PdbLine
	{
		// Token: 0x060015DB RID: 5595 RVA: 0x00045E89 File Offset: 0x00044089
		internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
		{
			this.offset = offset;
			this.lineBegin = lineBegin;
			this.colBegin = colBegin;
			this.lineEnd = lineEnd;
			this.colEnd = colEnd;
		}

		// Token: 0x04000F61 RID: 3937
		internal uint offset;

		// Token: 0x04000F62 RID: 3938
		internal uint lineBegin;

		// Token: 0x04000F63 RID: 3939
		internal uint lineEnd;

		// Token: 0x04000F64 RID: 3940
		internal ushort colBegin;

		// Token: 0x04000F65 RID: 3941
		internal ushort colEnd;
	}
}
