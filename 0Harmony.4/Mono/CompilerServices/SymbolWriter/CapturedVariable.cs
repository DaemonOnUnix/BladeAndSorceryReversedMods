using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000306 RID: 774
	public struct CapturedVariable
	{
		// Token: 0x0600135F RID: 4959 RVA: 0x0003E738 File Offset: 0x0003C938
		public CapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.Name = name;
			this.CapturedName = captured_name;
			this.Kind = kind;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0003E74F File Offset: 0x0003C94F
		internal CapturedVariable(MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.CapturedName = reader.ReadString();
			this.Kind = (CapturedVariable.CapturedKind)reader.ReadByte();
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0003E775 File Offset: 0x0003C975
		internal void Write(MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.Write(this.CapturedName);
			bw.Write((byte)this.Kind);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0003E79B File Offset: 0x0003C99B
		public override string ToString()
		{
			return string.Format("[CapturedVariable {0}:{1}:{2}]", this.Name, this.CapturedName, this.Kind);
		}

		// Token: 0x040009F3 RID: 2547
		public readonly string Name;

		// Token: 0x040009F4 RID: 2548
		public readonly string CapturedName;

		// Token: 0x040009F5 RID: 2549
		public readonly CapturedVariable.CapturedKind Kind;

		// Token: 0x02000307 RID: 775
		public enum CapturedKind : byte
		{
			// Token: 0x040009F7 RID: 2551
			Local,
			// Token: 0x040009F8 RID: 2552
			Parameter,
			// Token: 0x040009F9 RID: 2553
			This
		}
	}
}
