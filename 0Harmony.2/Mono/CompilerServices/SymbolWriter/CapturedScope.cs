using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000308 RID: 776
	public struct CapturedScope
	{
		// Token: 0x06001363 RID: 4963 RVA: 0x0003E7BE File Offset: 0x0003C9BE
		public CapturedScope(int scope, string captured_name)
		{
			this.Scope = scope;
			this.CapturedName = captured_name;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0003E7CE File Offset: 0x0003C9CE
		internal CapturedScope(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.CapturedName = reader.ReadString();
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0003E7E8 File Offset: 0x0003C9E8
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.Write(this.CapturedName);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0003E802 File Offset: 0x0003CA02
		public override string ToString()
		{
			return string.Format("[CapturedScope {0}:{1}]", this.Scope, this.CapturedName);
		}

		// Token: 0x040009FA RID: 2554
		public readonly int Scope;

		// Token: 0x040009FB RID: 2555
		public readonly string CapturedName;
	}
}
