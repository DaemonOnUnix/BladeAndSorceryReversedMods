using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x02000287 RID: 647
	internal class BinaryStreamWriter : BinaryWriter
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x000331AE File Offset: 0x000313AE
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x000331BC File Offset: 0x000313BC
		public int Position
		{
			get
			{
				return (int)this.BaseStream.Position;
			}
			set
			{
				this.BaseStream.Position = (long)value;
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000331CB File Offset: 0x000313CB
		public BinaryStreamWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000331D4 File Offset: 0x000313D4
		public void WriteByte(byte value)
		{
			this.Write(value);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000331DD File Offset: 0x000313DD
		public void WriteUInt16(ushort value)
		{
			this.Write(value);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x000331E6 File Offset: 0x000313E6
		public void WriteInt16(short value)
		{
			this.Write(value);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000331EF File Offset: 0x000313EF
		public void WriteUInt32(uint value)
		{
			this.Write(value);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000331F8 File Offset: 0x000313F8
		public void WriteInt32(int value)
		{
			this.Write(value);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00033201 File Offset: 0x00031401
		public void WriteUInt64(ulong value)
		{
			this.Write(value);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003320A File Offset: 0x0003140A
		public void WriteBytes(byte[] bytes)
		{
			this.Write(bytes);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00033213 File Offset: 0x00031413
		public void WriteDataDirectory(DataDirectory directory)
		{
			this.Write(directory.VirtualAddress);
			this.Write(directory.Size);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003322D File Offset: 0x0003142D
		public void WriteBuffer(ByteBuffer buffer)
		{
			this.Write(buffer.buffer, 0, buffer.length);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00033242 File Offset: 0x00031442
		protected void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00033254 File Offset: 0x00031454
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			int num = ((position + align) & ~align) - position;
			for (int i = 0; i < num; i++)
			{
				this.WriteByte(0);
			}
		}
	}
}
