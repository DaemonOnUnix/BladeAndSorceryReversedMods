using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000212 RID: 530
	public struct CapturedScope
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x00036872 File Offset: 0x00034A72
		public CapturedScope(int scope, string captured_name)
		{
			this.Scope = scope;
			this.CapturedName = captured_name;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00036882 File Offset: 0x00034A82
		internal CapturedScope(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.CapturedName = reader.ReadString();
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003689C File Offset: 0x00034A9C
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.Write(this.CapturedName);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000368B6 File Offset: 0x00034AB6
		public override string ToString()
		{
			return string.Format("[CapturedScope {0}:{1}]", this.Scope, this.CapturedName);
		}

		// Token: 0x040009BB RID: 2491
		public readonly int Scope;

		// Token: 0x040009BC RID: 2492
		public readonly string CapturedName;
	}
}
