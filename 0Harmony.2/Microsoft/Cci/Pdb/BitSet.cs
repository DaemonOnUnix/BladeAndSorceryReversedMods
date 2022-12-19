using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000335 RID: 821
	internal struct BitSet
	{
		// Token: 0x06001590 RID: 5520 RVA: 0x000437BB File Offset: 0x000419BB
		internal BitSet(BitAccess bits)
		{
			bits.ReadInt32(out this.size);
			this.words = new uint[this.size];
			bits.ReadUInt32(this.words);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000437E8 File Offset: 0x000419E8
		internal bool IsSet(int index)
		{
			int num = index / 32;
			return num < this.size && (this.words[num] & BitSet.GetBit(index)) > 0U;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00043817 File Offset: 0x00041A17
		private static uint GetBit(int index)
		{
			return 1U << index % 32;
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001593 RID: 5523 RVA: 0x00043822 File Offset: 0x00041A22
		internal bool IsEmpty
		{
			get
			{
				return this.size == 0;
			}
		}

		// Token: 0x04000A92 RID: 2706
		private int size;

		// Token: 0x04000A93 RID: 2707
		private uint[] words;
	}
}
