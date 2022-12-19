using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030F RID: 783
	internal class PdbSynchronizationPoint
	{
		// Token: 0x06001268 RID: 4712 RVA: 0x0003DF05 File Offset: 0x0003C105
		internal PdbSynchronizationPoint(BitAccess bits)
		{
			bits.ReadUInt32(out this.synchronizeOffset);
			bits.ReadUInt32(out this.continuationMethodToken);
			bits.ReadUInt32(out this.continuationOffset);
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0003DF31 File Offset: 0x0003C131
		public uint SynchronizeOffset
		{
			get
			{
				return this.synchronizeOffset;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0003DF39 File Offset: 0x0003C139
		public uint ContinuationOffset
		{
			get
			{
				return this.continuationOffset;
			}
		}

		// Token: 0x04000F1A RID: 3866
		internal uint synchronizeOffset;

		// Token: 0x04000F1B RID: 3867
		internal uint continuationMethodToken;

		// Token: 0x04000F1C RID: 3868
		internal uint continuationOffset;
	}
}
