using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000311 RID: 785
	internal struct PdbLine
	{
		// Token: 0x0600126C RID: 4716 RVA: 0x0003DF41 File Offset: 0x0003C141
		internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
		{
			this.offset = offset;
			this.lineBegin = lineBegin;
			this.colBegin = colBegin;
			this.lineEnd = lineEnd;
			this.colEnd = colEnd;
		}

		// Token: 0x04000F23 RID: 3875
		internal uint offset;

		// Token: 0x04000F24 RID: 3876
		internal uint lineBegin;

		// Token: 0x04000F25 RID: 3877
		internal uint lineEnd;

		// Token: 0x04000F26 RID: 3878
		internal ushort colBegin;

		// Token: 0x04000F27 RID: 3879
		internal ushort colEnd;
	}
}
