using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021A RID: 538
	public struct NamespaceEntry
	{
		// Token: 0x06001035 RID: 4149 RVA: 0x000380D4 File Offset: 0x000362D4
		public NamespaceEntry(string name, int index, string[] using_clauses, int parent)
		{
			this.Name = name;
			this.Index = index;
			this.Parent = parent;
			this.UsingClauses = ((using_clauses != null) ? using_clauses : new string[0]);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00038100 File Offset: 0x00036300
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

		// Token: 0x06001037 RID: 4151 RVA: 0x00038160 File Offset: 0x00036360
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

		// Token: 0x06001038 RID: 4152 RVA: 0x000381BF File Offset: 0x000363BF
		public override string ToString()
		{
			return string.Format("[Namespace {0}:{1}:{2}]", this.Name, this.Index, this.Parent);
		}

		// Token: 0x040009FA RID: 2554
		public readonly string Name;

		// Token: 0x040009FB RID: 2555
		public readonly int Index;

		// Token: 0x040009FC RID: 2556
		public readonly int Parent;

		// Token: 0x040009FD RID: 2557
		public readonly string[] UsingClauses;
	}
}
