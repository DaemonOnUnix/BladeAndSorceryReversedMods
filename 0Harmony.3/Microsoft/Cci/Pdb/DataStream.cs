using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FA RID: 762
	internal class DataStream
	{
		// Token: 0x06001225 RID: 4645 RVA: 0x00002AED File Offset: 0x00000CED
		internal DataStream()
		{
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0003B8E5 File Offset: 0x00039AE5
		internal DataStream(int contentSize, BitAccess bits, int count)
		{
			this.contentSize = contentSize;
			if (count > 0)
			{
				this.pages = new int[count];
				bits.ReadInt32(this.pages);
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0003B910 File Offset: 0x00039B10
		internal void Read(PdbReader reader, BitAccess bits)
		{
			bits.MinCapacity(this.contentSize);
			this.Read(reader, 0, bits.Buffer, 0, this.contentSize);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0003B934 File Offset: 0x00039B34
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

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x0003BA09 File Offset: 0x00039C09
		internal int Length
		{
			get
			{
				return this.contentSize;
			}
		}

		// Token: 0x04000EB3 RID: 3763
		internal int contentSize;

		// Token: 0x04000EB4 RID: 3764
		internal int[] pages;
	}
}
