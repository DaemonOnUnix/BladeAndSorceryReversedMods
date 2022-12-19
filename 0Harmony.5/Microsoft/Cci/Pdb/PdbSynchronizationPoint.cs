using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000405 RID: 1029
	internal class PdbSynchronizationPoint
	{
		// Token: 0x060015D7 RID: 5591 RVA: 0x00045E4D File Offset: 0x0004404D
		internal PdbSynchronizationPoint(BitAccess bits)
		{
			bits.ReadUInt32(out this.synchronizeOffset);
			bits.ReadUInt32(out this.continuationMethodToken);
			bits.ReadUInt32(out this.continuationOffset);
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00045E79 File Offset: 0x00044079
		public uint SynchronizeOffset
		{
			get
			{
				return this.synchronizeOffset;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x00045E81 File Offset: 0x00044081
		public uint ContinuationOffset
		{
			get
			{
				return this.continuationOffset;
			}
		}

		// Token: 0x04000F58 RID: 3928
		internal uint synchronizeOffset;

		// Token: 0x04000F59 RID: 3929
		internal uint continuationMethodToken;

		// Token: 0x04000F5A RID: 3930
		internal uint continuationOffset;
	}
}
