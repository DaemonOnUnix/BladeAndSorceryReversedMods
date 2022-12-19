using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030E RID: 782
	internal class PdbSynchronizationInformation
	{
		// Token: 0x06001266 RID: 4710 RVA: 0x0003DEA4 File Offset: 0x0003C0A4
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

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x0003DEFD File Offset: 0x0003C0FD
		public uint GeneratedCatchHandlerOffset
		{
			get
			{
				return this.generatedCatchHandlerIlOffset;
			}
		}

		// Token: 0x04000F17 RID: 3863
		internal uint kickoffMethodToken;

		// Token: 0x04000F18 RID: 3864
		internal uint generatedCatchHandlerIlOffset;

		// Token: 0x04000F19 RID: 3865
		internal PdbSynchronizationPoint[] synchronizationPoints;
	}
}
