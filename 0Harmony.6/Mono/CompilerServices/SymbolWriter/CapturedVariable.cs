using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000210 RID: 528
	public struct CapturedVariable
	{
		// Token: 0x06000FEF RID: 4079 RVA: 0x000367EC File Offset: 0x000349EC
		public CapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.Name = name;
			this.CapturedName = captured_name;
			this.Kind = kind;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00036803 File Offset: 0x00034A03
		internal CapturedVariable(MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.CapturedName = reader.ReadString();
			this.Kind = (CapturedVariable.CapturedKind)reader.ReadByte();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00036829 File Offset: 0x00034A29
		internal void Write(MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.Write(this.CapturedName);
			bw.Write((byte)this.Kind);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003684F File Offset: 0x00034A4F
		public override string ToString()
		{
			return string.Format("[CapturedVariable {0}:{1}:{2}]", this.Name, this.CapturedName, this.Kind);
		}

		// Token: 0x040009B4 RID: 2484
		public readonly string Name;

		// Token: 0x040009B5 RID: 2485
		public readonly string CapturedName;

		// Token: 0x040009B6 RID: 2486
		public readonly CapturedVariable.CapturedKind Kind;

		// Token: 0x02000211 RID: 529
		public enum CapturedKind : byte
		{
			// Token: 0x040009B8 RID: 2488
			Local,
			// Token: 0x040009B9 RID: 2489
			Parameter,
			// Token: 0x040009BA RID: 2490
			This
		}
	}
}
