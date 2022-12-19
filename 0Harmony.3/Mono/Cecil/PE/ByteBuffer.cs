using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000193 RID: 403
	internal class ByteBuffer
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002B8A9 File Offset: 0x00029AA9
		public ByteBuffer()
		{
			this.buffer = Empty<byte>.Array;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002B8BC File Offset: 0x00029ABC
		public ByteBuffer(int length)
		{
			this.buffer = new byte[length];
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002B8D0 File Offset: 0x00029AD0
		public ByteBuffer(byte[] buffer)
		{
			this.buffer = buffer ?? Empty<byte>.Array;
			this.length = this.buffer.Length;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002B8F6 File Offset: 0x00029AF6
		public void Advance(int length)
		{
			this.position += length;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002B908 File Offset: 0x00029B08
		public byte ReadByte()
		{
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002B92D File Offset: 0x00029B2D
		public sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0002B938 File Offset: 0x00029B38
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002B96F File Offset: 0x00029B6F
		public ushort ReadUInt16()
		{
			ushort num = (ushort)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8));
			this.position += 2;
			return num;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002B99F File Offset: 0x00029B9F
		public short ReadInt16()
		{
			return (short)this.ReadUInt16();
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002B9A8 File Offset: 0x00029BA8
		public uint ReadUInt32()
		{
			uint num = (uint)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24));
			this.position += 4;
			return num;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002BA08 File Offset: 0x00029C08
		public int ReadInt32()
		{
			return (int)this.ReadUInt32();
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002BA10 File Offset: 0x00029C10
		public ulong ReadUInt64()
		{
			uint num = this.ReadUInt32();
			return ((ulong)this.ReadUInt32() << 32) | (ulong)num;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002BA31 File Offset: 0x00029C31
		public long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002BA3C File Offset: 0x00029C3C
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

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002BA98 File Offset: 0x00029C98
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

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002BAF1 File Offset: 0x00029CF1
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

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002BB2D File Offset: 0x00029D2D
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

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002BB6C File Offset: 0x00029D6C
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

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0001F575 File Offset: 0x0001D775
		public void WriteSByte(sbyte value)
		{
			this.WriteByte((byte)value);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002BBC4 File Offset: 0x00029DC4
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

		// Token: 0x06000CFA RID: 3322 RVA: 0x0002BC3A File Offset: 0x00029E3A
		public void WriteInt16(short value)
		{
			this.WriteUInt16((ushort)value);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002BC44 File Offset: 0x00029E44
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

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002BCF4 File Offset: 0x00029EF4
		public void WriteInt32(int value)
		{
			this.WriteUInt32((uint)value);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0002BD00 File Offset: 0x00029F00
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

		// Token: 0x06000CFE RID: 3326 RVA: 0x0002BE24 File Offset: 0x0002A024
		public void WriteInt64(long value)
		{
			this.WriteUInt64((ulong)value);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002BE30 File Offset: 0x0002A030
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

		// Token: 0x06000D00 RID: 3328 RVA: 0x0002BEB8 File Offset: 0x0002A0B8
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

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002BF10 File Offset: 0x0002A110
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

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002BF78 File Offset: 0x0002A178
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

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002BFC8 File Offset: 0x0002A1C8
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

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002C044 File Offset: 0x0002A244
		public void WriteSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002C06C File Offset: 0x0002A26C
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002C094 File Offset: 0x0002A294
		private void Grow(int desired)
		{
			byte[] array = this.buffer;
			int num = array.Length;
			byte[] array2 = new byte[Math.Max(num + desired, num * 2)];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			this.buffer = array2;
		}

		// Token: 0x040005AC RID: 1452
		internal byte[] buffer;

		// Token: 0x040005AD RID: 1453
		internal int length;

		// Token: 0x040005AE RID: 1454
		internal int position;
	}
}
