using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000404 RID: 1028
	internal class PdbSynchronizationInformation
	{
		// Token: 0x060015D5 RID: 5589 RVA: 0x00045DEC File Offset: 0x00043FEC
		internal PdbSynchronizationInformation(BitAccess bits)
		{
			bits.ReadUInt32(out this.kickoffMethodToken);
			bits.ReadUInt32(out this.generatedCatchHandlerIlOffset);
			uint num;
			bits.ReadUInt32(out num);
			this.synchronizationPoints = new PdbSynchronizationPoint[num];
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				this.synchronizationPoints[(int)num2] = new PdbSynchronizationPoint(bits);
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00045E45 File Offset: 0x00044045
		public uint GeneratedCatchHandlerOffset
		{
			get
			{
				return this.generatedCatchHandlerIlOffset;
			}
		}

		// Token: 0x04000F55 RID: 3925
		internal uint kickoffMethodToken;

		// Token: 0x04000F56 RID: 3926
		internal uint generatedCatchHandlerIlOffset;

		// Token: 0x04000F57 RID: 3927
		internal PdbSynchronizationPoint[] synchronizationPoints;
	}
}
