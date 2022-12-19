using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F0 RID: 1008
	internal class DataStream
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x00002AED File Offset: 0x00000CED
		internal DataStream()
		{
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0004382D File Offset: 0x00041A2D
		internal DataStream(int contentSize, BitAccess bits, int count)
		{
			this.contentSize = contentSize;
			if (count > 0)
			{
				this.pages = new int[count];
				bits.ReadInt32(this.pages);
			}
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00043858 File Offset: 0x00041A58
		internal void Read(PdbReader reader, BitAccess bits)
		{
			bits.MinCapacity(this.contentSize);
			this.Read(reader, 0, bits.Buffer, 0, this.contentSize);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0004387C File Offset: 0x00041A7C
		internal void Read(PdbReader reader, int position, byte[] bytes, int offset, int data)
		{
			if (position + data > this.contentSize)
			{
				throw new PdbException("DataStream can't read off end of stream. (pos={0},siz={1})", new object[] { position, data });
			}
			if (position == this.contentSize)
			{
				return;
			}
			int i = data;
			int num = position / reader.pageSize;
			int num2 = position % reader.pageSize;
			if (num2 != 0)
			{
				int num3 = reader.pageSize - num2;
				if (num3 > i)
				{
					num3 = i;
				}
				reader.Seek(this.pages[num], num2);
				reader.Read(bytes, offset, num3);
				offset += num3;
				i -= num3;
				num++;
			}
			while (i > 0)
			{
				int num4 = reader.pageSize;
				if (num4 > i)
				{
					num4 = i;
				}
				reader.Seek(this.pages[num], 0);
				reader.Read(bytes, offset, num4);
				offset += num4;
				i -= num4;
				num++;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00043951 File Offset: 0x00041B51
		internal int Length
		{
			get
			{
				return this.contentSize;
			}
		}

		// Token: 0x04000EF2 RID: 3826
		internal int contentSize;

		// Token: 0x04000EF3 RID: 3827
		internal int[] pages;
	}
}
