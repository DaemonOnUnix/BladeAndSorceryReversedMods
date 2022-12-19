using System;

namespace HarmonyLib
{
	// Token: 0x0200000E RID: 14
	internal class ByteBuffer
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002C17 File Offset: 0x00000E17
		internal ByteBuffer(byte[] buffer)
		{
			this.buffer = buffer;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C28 File Offset: 0x00000E28
		internal byte ReadByte()
		{
			this.CheckCanRead(1);
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C54 File Offset: 0x00000E54
		internal byte[] ReadBytes(int length)
		{
			this.CheckCanRead(length);
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C92 File Offset: 0x00000E92
		internal short ReadInt16()
		{
			this.CheckCanRead(2);
			short num = (short)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8));
			this.position += 2;
			return num;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002CCC File Offset: 0x00000ECC
		internal int ReadInt32()
		{
			this.CheckCanRead(4);
			int num = (int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24);
			this.position += 4;
			return num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D34 File Offset: 0x00000F34
		internal long ReadInt64()
		{
			this.CheckCanRead(8);
			uint num = (uint)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24));
			long num2 = (long)(((ulong)((int)this.buffer[this.position + 4] | ((int)this.buffer[this.position + 5] << 8) | ((int)this.buffer[this.position + 6] << 16) | ((int)this.buffer[this.position + 7] << 24)) << 32) | (ulong)num);
			this.position += 8;
			return num2;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DEC File Offset: 0x00000FEC
		internal float ReadSingle()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(4);
				Array.Reverse(array);
				return BitConverter.ToSingle(array, 0);
			}
			this.CheckCanRead(4);
			float num = BitConverter.ToSingle(this.buffer, this.position);
			this.position += 4;
			return num;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002E3C File Offset: 0x0000103C
		internal double ReadDouble()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(8);
				Array.Reverse(array);
				return BitConverter.ToDouble(array, 0);
			}
			this.CheckCanRead(8);
			double num = BitConverter.ToDouble(this.buffer, this.position);
			this.position += 8;
			return num;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E8C File Offset: 0x0000108C
		private void CheckCanRead(int count)
		{
			if (this.position + count > this.buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count", string.Format("position({0}) + count({1}) > buffer.Length({2})", this.position, count, this.buffer.Length));
			}
		}

		// Token: 0x0400001B RID: 27
		internal byte[] buffer;

		// Token: 0x0400001C RID: 28
		internal int position;
	}
}
