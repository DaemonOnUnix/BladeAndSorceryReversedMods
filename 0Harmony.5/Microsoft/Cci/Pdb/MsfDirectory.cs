using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003FB RID: 1019
	internal class MsfDirectory
	{
		// Token: 0x060015B1 RID: 5553 RVA: 0x000441B4 File Offset: 0x000423B4
		internal MsfDirectory(PdbReader reader, PdbFileHeader head, BitAccess bits)
		{
			int num = reader.PagesFromSize(head.directorySize);
			bits.MinCapacity(head.directorySize);
			int num2 = head.directoryRoot.Length;
			int num3 = head.pageSize / 4;
			int num4 = num;
			for (int i = 0; i < num2; i++)
			{
				int num5 = ((num4 <= num3) ? num4 : num3);
				reader.Seek(head.directoryRoot[i], 0);
				bits.Append(reader.reader, num5 * 4);
				num4 -= num5;
			}
			bits.Position = 0;
			DataStream dataStream = new DataStream(head.directorySize, bits, num);
			bits.MinCapacity(head.directorySize);
			dataStream.Read(reader, bits);
			int num6;
			bits.ReadInt32(out num6);
			int[] array = new int[num6];
			bits.ReadInt32(array);
			this.streams = new DataStream[num6];
			for (int j = 0; j < num6; j++)
			{
				if (array[j] <= 0)
				{
					this.streams[j] = new DataStream();
				}
				else
				{
					this.streams[j] = new DataStream(array[j], bits, reader.PagesFromSize(array[j]));
				}
			}
		}

		// Token: 0x04000F33 RID: 3891
		internal DataStream[] streams;
	}
}
