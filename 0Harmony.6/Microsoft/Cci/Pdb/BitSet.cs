using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200023F RID: 575
	internal struct BitSet
	{
		// Token: 0x06001221 RID: 4641 RVA: 0x0003B873 File Offset: 0x00039A73
		internal BitSet(BitAccess bits)
		{
			bits.ReadInt32(out this.size);
			this.words = new uint[this.size];
			bits.ReadUInt32(this.words);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0003B8A0 File Offset: 0x00039AA0
		internal bool IsSet(int index)
		{
			int num = index / 32;
			return num < this.size && (this.words[num] & BitSet.GetBit(index)) > 0U;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0003B8CF File Offset: 0x00039ACF
		private static uint GetBit(int index)
		{
			return 1U << index % 32;
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x0003B8DA File Offset: 0x00039ADA
		internal bool IsEmpty
		{
			get
			{
				return this.size == 0;
			}
		}

		// Token: 0x04000A53 RID: 2643
		private int size;

		// Token: 0x04000A54 RID: 2644
		private uint[] words;
	}
}
