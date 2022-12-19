using System;
using System.IO;
using System.Text;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200023E RID: 574
	internal class BitAccess
	{
		// Token: 0x06001204 RID: 4612 RVA: 0x0003B16F File Offset: 0x0003936F
		internal BitAccess(int capacity)
		{
			this.buffer = new byte[capacity];
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0003B183 File Offset: 0x00039383
		internal BitAccess(byte[] buffer)
		{
			this.buffer = buffer;
			this.offset = 0;
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0003B199 File Offset: 0x00039399
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0003B1A1 File Offset: 0x000393A1
		internal void FillBuffer(Stream stream, int capacity)
		{
			this.MinCapacity(capacity);
			stream.Read(this.buffer, 0, capacity);
			this.offset = 0;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0003B1C0 File Offset: 0x000393C0
		internal void Append(Stream stream, int count)
		{
			int num = this.offset + count;
			if (this.buffer.Length < num)
			{
				byte[] array = new byte[num];
				Array.Copy(this.buffer, array, this.buffer.Length);
				this.buffer = array;
			}
			stream.Read(this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0003B225 File Offset: 0x00039425
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x0003B22D File Offset: 0x0003942D
		internal int Position
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0003B236 File Offset: 0x00039436
		internal void MinCapacity(int capacity)
		{
			if (this.buffer.Length < capacity)
			{
				this.buffer = new byte[capacity];
			}
			this.offset = 0;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0003B256 File Offset: 0x00039456
		internal void Align(int alignment)
		{
			while (this.offset % alignment != 0)
			{
				this.offset++;
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0003B272 File Offset: 0x00039472
		internal void ReadInt16(out short value)
		{
			value = (short)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0003B2AA File Offset: 0x000394AA
		internal void ReadInt8(out sbyte value)
		{
			value = (sbyte)this.buffer[this.offset];
			this.offset++;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0003B2CC File Offset: 0x000394CC
		internal void ReadInt32(out int value)
		{
			value = (int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24);
			this.offset += 4;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0003B334 File Offset: 0x00039534
		internal void ReadInt64(out long value)
		{
			value = (long)(((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56));
			this.offset += 8;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0003B3F1 File Offset: 0x000395F1
		internal void ReadUInt16(out ushort value)
		{
			value = (ushort)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0003B429 File Offset: 0x00039629
		internal void ReadUInt8(out byte value)
		{
			value = this.buffer[this.offset] & byte.MaxValue;
			this.offset++;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0003B450 File Offset: 0x00039650
		internal void ReadUInt32(out uint value)
		{
			value = (uint)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24));
			this.offset += 4;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0003B4B8 File Offset: 0x000396B8
		internal void ReadUInt64(out ulong value)
		{
			value = ((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56);
			this.offset += 8;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0003B578 File Offset: 0x00039778
		internal void ReadInt32(int[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadInt32(out values[i]);
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0003B5A0 File Offset: 0x000397A0
		internal void ReadUInt32(uint[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadUInt32(out values[i]);
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0003B5C8 File Offset: 0x000397C8
		internal void ReadBytes(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = i;
				byte[] array = this.buffer;
				int num2 = this.offset;
				this.offset = num2 + 1;
				bytes[num] = array[num2];
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0003B5FE File Offset: 0x000397FE
		internal float ReadFloat()
		{
			float num = BitConverter.ToSingle(this.buffer, this.offset);
			this.offset += 4;
			return num;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0003B61F File Offset: 0x0003981F
		internal double ReadDouble()
		{
			double num = BitConverter.ToDouble(this.buffer, this.offset);
			this.offset += 8;
			return num;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0003B640 File Offset: 0x00039840
		internal decimal ReadDecimal()
		{
			int[] array = new int[4];
			this.ReadInt32(array);
			return new decimal(array[2], array[3], array[1], array[0] < 0, (byte)((array[0] & 16711680) >> 16));
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0003B67C File Offset: 0x0003987C
		internal void ReadBString(out string value)
		{
			ushort num;
			this.ReadUInt16(out num);
			value = Encoding.UTF8.GetString(this.buffer, this.offset, (int)num);
			this.offset += (int)num;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0003B6B8 File Offset: 0x000398B8
		internal string ReadBString(int len)
		{
			string @string = Encoding.UTF8.GetString(this.buffer, this.offset, len);
			this.offset += len;
			return @string;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0003B6E0 File Offset: 0x000398E0
		internal void ReadCString(out string value)
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num++;
			}
			value = Encoding.UTF8.GetString(this.buffer, this.offset, num);
			this.offset += num + 1;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0003B744 File Offset: 0x00039944
		internal void SkipCString(out string value)
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num++;
			}
			this.offset += num + 1;
			value = null;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0003B790 File Offset: 0x00039990
		internal void ReadGuid(out Guid guid)
		{
			uint num;
			this.ReadUInt32(out num);
			ushort num2;
			this.ReadUInt16(out num2);
			ushort num3;
			this.ReadUInt16(out num3);
			byte b;
			this.ReadUInt8(out b);
			byte b2;
			this.ReadUInt8(out b2);
			byte b3;
			this.ReadUInt8(out b3);
			byte b4;
			this.ReadUInt8(out b4);
			byte b5;
			this.ReadUInt8(out b5);
			byte b6;
			this.ReadUInt8(out b6);
			byte b7;
			this.ReadUInt8(out b7);
			byte b8;
			this.ReadUInt8(out b8);
			guid = new Guid(num, num2, num3, b, b2, b3, b4, b5, b6, b7, b8);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0003B814 File Offset: 0x00039A14
		internal string ReadString()
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num += 2;
			}
			string @string = Encoding.Unicode.GetString(this.buffer, this.offset, num);
			this.offset += num + 2;
			return @string;
		}

		// Token: 0x04000A51 RID: 2641
		private byte[] buffer;

		// Token: 0x04000A52 RID: 2642
		private int offset;
	}
}
