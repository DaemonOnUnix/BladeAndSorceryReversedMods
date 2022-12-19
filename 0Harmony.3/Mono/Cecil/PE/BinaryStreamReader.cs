using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x02000191 RID: 401
	internal class BinaryStreamReader : BinaryReader
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0002B73B File Offset: 0x0002993B
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0002B749 File Offset: 0x00029949
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0002B758 File Offset: 0x00029958
		public int Length
		{
			get
			{
				return (int)this.BaseStream.Length;
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002B766 File Offset: 0x00029966
		public BinaryStreamReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002B76F File Offset: 0x0002996F
		public void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002B780 File Offset: 0x00029980
		public void MoveTo(uint position)
		{
			this.BaseStream.Seek((long)((ulong)position), SeekOrigin.Begin);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0002B794 File Offset: 0x00029994
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			this.Advance(((position + align) & ~align) - position);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0002B7BB File Offset: 0x000299BB
		public DataDirectory ReadDataDirectory()
		{
			return new DataDirectory(this.ReadUInt32(), this.ReadUInt32());
		}
	}
}
