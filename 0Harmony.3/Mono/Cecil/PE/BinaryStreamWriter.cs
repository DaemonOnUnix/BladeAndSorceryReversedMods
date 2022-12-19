using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x02000192 RID: 402
	internal class BinaryStreamWriter : BinaryWriter
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0002B7CE File Offset: 0x000299CE
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0002B7DC File Offset: 0x000299DC
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

		// Token: 0x06000CDA RID: 3290 RVA: 0x0002B7EB File Offset: 0x000299EB
		public BinaryStreamWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0002B7F4 File Offset: 0x000299F4
		public void WriteByte(byte value)
		{
			this.Write(value);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0002B7FD File Offset: 0x000299FD
		public void WriteUInt16(ushort value)
		{
			this.Write(value);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0002B806 File Offset: 0x00029A06
		public void WriteInt16(short value)
		{
			this.Write(value);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002B80F File Offset: 0x00029A0F
		public void WriteUInt32(uint value)
		{
			this.Write(value);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0002B818 File Offset: 0x00029A18
		public void WriteInt32(int value)
		{
			this.Write(value);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0002B821 File Offset: 0x00029A21
		public void WriteUInt64(ulong value)
		{
			this.Write(value);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0002B82A File Offset: 0x00029A2A
		public void WriteBytes(byte[] bytes)
		{
			this.Write(bytes);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0002B833 File Offset: 0x00029A33
		public void WriteDataDirectory(DataDirectory directory)
		{
			this.Write(directory.VirtualAddress);
			this.Write(directory.Size);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0002B84D File Offset: 0x00029A4D
		public void WriteBuffer(ByteBuffer buffer)
		{
			this.Write(buffer.buffer, 0, buffer.length);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0002B862 File Offset: 0x00029A62
		protected void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002B874 File Offset: 0x00029A74
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
