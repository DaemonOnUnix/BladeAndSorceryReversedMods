using System;
using System.IO;
using System.Text;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000334 RID: 820
	internal class BitAccess
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x000430B7 File Offset: 0x000412B7
		internal BitAccess(int capacity)
		{
			this.buffer = new byte[capacity];
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000430CB File Offset: 0x000412CB
		internal BitAccess(byte[] buffer)
		{
			this.buffer = buffer;
			this.offset = 0;
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x000430E1 File Offset: 0x000412E1
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000430E9 File Offset: 0x000412E9
		internal void FillBuffer(Stream stream, int capacity)
		{
			this.MinCapacity(capacity);
			stream.Read(this.buffer, 0, capacity);
			this.offset = 0;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00043108 File Offset: 0x00041308
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

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0004316D File Offset: 0x0004136D
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x00043175 File Offset: 0x00041375
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

		// Token: 0x0600157A RID: 5498 RVA: 0x0004317E File Offset: 0x0004137E
		internal void MinCapacity(int capacity)
		{
			if (this.buffer.Length < capacity)
			{
				this.buffer = new byte[capacity];
			}
			this.offset = 0;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0004319E File Offset: 0x0004139E
		internal void Align(int alignment)
		{
			while (this.offset % alignment != 0)
			{
				this.offset++;
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000431BA File Offset: 0x000413BA
		internal void ReadInt16(out short value)
		{
			value = (short)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x000431F2 File Offset: 0x000413F2
		internal void ReadInt8(out sbyte value)
		{
			value = (sbyte)this.buffer[this.offset];
			this.offset++;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00043214 File Offset: 0x00041414
		internal void ReadInt32(out int value)
		{
			value = (int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24);
			this.offset += 4;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0004327C File Offset: 0x0004147C
		internal void ReadInt64(out long value)
		{
			value = (long)(((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56));
			this.offset += 8;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00043339 File Offset: 0x00041539
		internal void ReadUInt16(out ushort value)
		{
			value = (ushort)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00043371 File Offset: 0x00041571
		internal void ReadUInt8(out byte value)
		{
			value = this.buffer[this.offset] & byte.MaxValue;
			this.offset++;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00043398 File Offset: 0x00041598
		internal void ReadUInt32(out uint value)
		{
			value = (uint)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24));
			this.offset += 4;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00043400 File Offset: 0x00041600
		internal void ReadUInt64(out ulong value)
		{
			value = ((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56);
			this.offset += 8;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000434C0 File Offset: 0x000416C0
		internal void ReadInt32(int[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadInt32(out values[i]);
			}
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000434E8 File Offset: 0x000416E8
		internal void ReadUInt32(uint[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadUInt32(out values[i]);
			}
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00043510 File Offset: 0x00041710
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

		// Token: 0x06001587 RID: 5511 RVA: 0x00043546 File Offset: 0x00041746
		internal float ReadFloat()
		{
			float num = BitConverter.ToSingle(this.buffer, this.offset);
			this.offset += 4;
			return num;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00043567 File Offset: 0x00041767
		internal double ReadDouble()
		{
			double num = BitConverter.ToDouble(this.buffer, this.offset);
			this.offset += 8;
			return num;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00043588 File Offset: 0x00041788
		internal decimal ReadDecimal()
		{
			int[] array = new int[4];
			this.ReadInt32(array);
			return new decimal(array[2], array[3], array[1], array[0] < 0, (byte)((array[0] & 16711680) >> 16));
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x000435C4 File Offset: 0x000417C4
		internal void ReadBString(out string value)
		{
			ushort num;
			this.ReadUInt16(out num);
			value = Encoding.UTF8.GetString(this.buffer, this.offset, (int)num);
			this.offset += (int)num;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00043600 File Offset: 0x00041800
		internal string ReadBString(int len)
		{
			string @string = Encoding.UTF8.GetString(this.buffer, this.offset, len);
			this.offset += len;
			return @string;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00043628 File Offset: 0x00041828
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

		// Token: 0x0600158D RID: 5517 RVA: 0x0004368C File Offset: 0x0004188C
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

		// Token: 0x0600158E RID: 5518 RVA: 0x000436D8 File Offset: 0x000418D8
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

		// Token: 0x0600158F RID: 5519 RVA: 0x0004375C File Offset: 0x0004195C
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

		// Token: 0x04000A90 RID: 2704
		private byte[] buffer;

		// Token: 0x04000A91 RID: 2705
		private int offset;
	}
}
