using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x02000286 RID: 646
	internal class BinaryStreamReader : BinaryReader
	{
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0003311B File Offset: 0x0003131B
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x00033129 File Offset: 0x00031329
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

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00033138 File Offset: 0x00031338
		public int Length
		{
			get
			{
				return (int)this.BaseStream.Length;
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00033146 File Offset: 0x00031346
		public BinaryStreamReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003314F File Offset: 0x0003134F
		public void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00033160 File Offset: 0x00031360
		public void MoveTo(uint position)
		{
			this.BaseStream.Seek((long)((ulong)position), SeekOrigin.Begin);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00033174 File Offset: 0x00031374
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			this.Advance(((position + align) & ~align) - position);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003319B File Offset: 0x0003139B
		public DataDirectory ReadDataDirectory()
		{
			return new DataDirectory(this.ReadUInt32(), this.ReadUInt32());
		}
	}
}
