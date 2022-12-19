using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000310 RID: 784
	public struct NamespaceEntry
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x00040020 File Offset: 0x0003E220
		public NamespaceEntry(string name, int index, string[] using_clauses, int parent)
		{
			this.Name = name;
			this.Index = index;
			this.Parent = parent;
			this.UsingClauses = ((using_clauses != null) ? using_clauses : new string[0]);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004004C File Offset: 0x0003E24C
		internal NamespaceEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.Index = reader.ReadLeb128();
			this.Parent = reader.ReadLeb128();
			int num = reader.ReadLeb128();
			this.UsingClauses = new string[num];
			for (int i = 0; i < num; i++)
			{
				this.UsingClauses[i] = reader.ReadString();
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000400AC File Offset: 0x0003E2AC
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.WriteLeb128(this.Index);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.UsingClauses.Length);
			foreach (string text in this.UsingClauses)
			{
				bw.Write(text);
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0004010B File Offset: 0x0003E30B
		public override string ToString()
		{
			return string.Format("[Namespace {0}:{1}:{2}]", this.Name, this.Index, this.Parent);
		}

		// Token: 0x04000A39 RID: 2617
		public readonly string Name;

		// Token: 0x04000A3A RID: 2618
		public readonly int Index;

		// Token: 0x04000A3B RID: 2619
		public readonly int Parent;

		// Token: 0x04000A3C RID: 2620
		public readonly string[] UsingClauses;
	}
}
