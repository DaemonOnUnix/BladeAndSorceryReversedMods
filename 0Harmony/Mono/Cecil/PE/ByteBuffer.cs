using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000288 RID: 648
	internal class ByteBuffer
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x00033289 File Offset: 0x00031489
		public ByteBuffer()
		{
			this.buffer = Empty<byte>.Array;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0003329C File Offset: 0x0003149C
		public ByteBuffer(int length)
		{
			this.buffer = new byte[length];
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000332B0 File Offset: 0x000314B0
		public ByteBuffer(byte[] buffer)
		{
			this.buffer = buffer ?? Empty<byte>.Array;
			this.length = this.buffer.Length;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000332D6 File Offset: 0x000314D6
		public void Advance(int length)
		{
			this.position += length;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000332E8 File Offset: 0x000314E8
		public byte ReadByte()
		{
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003330D File Offset: 0x0003150D
		public sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00033318 File Offset: 0x00031518
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0003334F File Offset: 0x0003154F
		public ushort ReadUInt16()
		{
			ushort num = (ushort)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8));
			this.position += 2;
			return num;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003337F File Offset: 0x0003157F
		public short ReadInt16()
		{
			return (short)this.ReadUInt16();
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00033388 File Offset: 0x00031588
		public uint ReadUInt32()
		{
			uint num = (uint)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24));
			this.position += 4;
			return num;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x000333E8 File Offset: 0x000315E8
		public int ReadInt32()
		{
			return (int)this.ReadUInt32();
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x000333F0 File Offset: 0x000315F0
		public ulong ReadUInt64()
		{
			uint num = this.ReadUInt32();
			return ((ulong)this.ReadUInt32() << 32) | (ulong)num;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00033411 File Offset: 0x00031611
		public long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0003341C File Offset: 0x0003161C
		public uint ReadCompressedUInt32()
		{
			byte b = this.ReadByte();
			if ((b & 128) == 0)
			{
				return (uint)b;
			}
			if ((b & 64) == 0)
			{
				return (((uint)b & 4294967167U) << 8) | (uint)this.ReadByte();
			}
			return (uint)((((int)b & -193) << 24) | ((int)this.ReadByte() << 16) | ((int)this.ReadByte() << 8) | (int)this.ReadByte());
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00033478 File Offset: 0x00031678
		public int ReadCompressedInt32()
		{
			byte b = this.buffer[this.position];
			uint num = this.ReadCompressedUInt32();
			int num2 = (int)num >> 1;
			if ((num & 1U) == 0U)
			{
				return num2;
			}
			int num3 = (int)(b & 192);
			if (num3 == 0 || num3 == 64)
			{
				return num2 - 64;
			}
			if (num3 != 128)
			{
				return num2 - 268435456;
			}
			return num2 - 8192;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000334D1 File Offset: 0x000316D1
		public float ReadSingle()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(4);
				Array.Reverse(array);
				return BitConverter.ToSingle(array, 0);
			}
			float num = BitConverter.ToSingle(this.buffer, this.position);
			this.position += 4;
			return num;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0003350D File Offset: 0x0003170D
		public double ReadDouble()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(8);
				Array.Reverse(array);
				return BitConverter.ToDouble(array, 0);
			}
			double num = BitConverter.ToDouble(this.buffer, this.position);
			this.position += 8;
			return num;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0003354C File Offset: 0x0003174C
		public void WriteByte(byte value)
		{
			if (this.position == this.buffer.Length)
			{
				this.Grow(1);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = value;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00025415 File Offset: 0x00023615
		public void WriteSByte(sbyte value)
		{
			this.WriteByte((byte)value);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000335A4 File Offset: 0x000317A4
		public void WriteUInt16(ushort value)
		{
			if (this.position + 2 > this.buffer.Length)
			{
				this.Grow(2);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0003361A File Offset: 0x0003181A
		public void WriteInt16(short value)
		{
			this.WriteUInt16((ushort)value);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00033624 File Offset: 0x00031824
		public void WriteUInt32(uint value)
		{
			if (this.position + 4 > this.buffer.Length)
			{
				this.Grow(4);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x000336D4 File Offset: 0x000318D4
		public void WriteInt32(int value)
		{
			this.WriteUInt32((uint)value);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000336E0 File Offset: 0x000318E0
		public void WriteUInt64(ulong value)
		{
			if (this.position + 8 > this.buffer.Length)
			{
				this.Grow(8);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
			byte[] array5 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array5[num] = (byte)(value >> 32);
			byte[] array6 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array6[num] = (byte)(value >> 40);
			byte[] array7 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array7[num] = (byte)(value >> 48);
			byte[] array8 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array8[num] = (byte)(value >> 56);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00033804 File Offset: 0x00031A04
		public void WriteInt64(long value)
		{
			this.WriteUInt64((ulong)value);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00033810 File Offset: 0x00031A10
		public void WriteCompressedUInt32(uint value)
		{
			if (value < 128U)
			{
				this.WriteByte((byte)value);
				return;
			}
			if (value < 16384U)
			{
				this.WriteByte((byte)(128U | (value >> 8)));
				this.WriteByte((byte)(value & 255U));
				return;
			}
			this.WriteByte((byte)((value >> 24) | 192U));
			this.WriteByte((byte)((value >> 16) & 255U));
			this.WriteByte((byte)((value >> 8) & 255U));
			this.WriteByte((byte)(value & 255U));
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00033898 File Offset: 0x00031A98
		public void WriteCompressedInt32(int value)
		{
			if (value >= 0)
			{
				this.WriteCompressedUInt32((uint)((uint)value << 1));
				return;
			}
			if (value > -64)
			{
				value = 64 + value;
			}
			else if (value >= -8192)
			{
				value = 8192 + value;
			}
			else if (value >= -536870912)
			{
				value = 536870912 + value;
			}
			this.WriteCompressedUInt32((uint)((value << 1) | 1));
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000338F0 File Offset: 0x00031AF0
		public void WriteBytes(byte[] bytes)
		{
			int num = bytes.Length;
			if (this.position + num > this.buffer.Length)
			{
				this.Grow(num);
			}
			Buffer.BlockCopy(bytes, 0, this.buffer, this.position, num);
			this.position += num;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00033958 File Offset: 0x00031B58
		public void WriteBytes(int length)
		{
			if (this.position + length > this.buffer.Length)
			{
				this.Grow(length);
			}
			this.position += length;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000339A8 File Offset: 0x00031BA8
		public void WriteBytes(ByteBuffer buffer)
		{
			if (this.position + buffer.length > this.buffer.Length)
			{
				this.Grow(buffer.length);
			}
			Buffer.BlockCopy(buffer.buffer, 0, this.buffer, this.position, buffer.length);
			this.position += buffer.length;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00033A24 File Offset: 0x00031C24
		public void WriteSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00033A4C File Offset: 0x00031C4C
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00033A74 File Offset: 0x00031C74
		private void Grow(int desired)
		{
			byte[] array = this.buffer;
			int num = array.Length;
			byte[] array2 = new byte[Math.Max(num + desired, num * 2)];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			this.buffer = array2;
		}

		// Token: 0x040005E3 RID: 1507
		internal byte[] buffer;

		// Token: 0x040005E4 RID: 1508
		internal int length;

		// Token: 0x040005E5 RID: 1509
		internal int position;
	}
}
