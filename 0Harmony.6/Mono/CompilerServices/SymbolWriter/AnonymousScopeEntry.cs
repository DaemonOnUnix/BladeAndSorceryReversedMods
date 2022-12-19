using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000214 RID: 532
	public class AnonymousScopeEntry
	{
		// Token: 0x06000FFB RID: 4091 RVA: 0x00036939 File Offset: 0x00034B39
		public AnonymousScopeEntry(int id)
		{
			this.ID = id;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00036960 File Offset: 0x00034B60
		internal AnonymousScopeEntry(MyBinaryReader reader)
		{
			this.ID = reader.ReadLeb128();
			int num = reader.ReadLeb128();
			for (int i = 0; i < num; i++)
			{
				this.captured_vars.Add(new CapturedVariable(reader));
			}
			int num2 = reader.ReadLeb128();
			for (int j = 0; j < num2; j++)
			{
				this.captured_scopes.Add(new CapturedScope(reader));
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x000369DD File Offset: 0x00034BDD
		internal void AddCapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.captured_vars.Add(new CapturedVariable(name, captured_name, kind));
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x000369F4 File Offset: 0x00034BF4
		public CapturedVariable[] CapturedVariables
		{
			get
			{
				CapturedVariable[] array = new CapturedVariable[this.captured_vars.Count];
				this.captured_vars.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00036A20 File Offset: 0x00034C20
		internal void AddCapturedScope(int scope, string captured_name)
		{
			this.captured_scopes.Add(new CapturedScope(scope, captured_name));
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x00036A34 File Offset: 0x00034C34
		public CapturedScope[] CapturedScopes
		{
			get
			{
				CapturedScope[] array = new CapturedScope[this.captured_scopes.Count];
				this.captured_scopes.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00036A60 File Offset: 0x00034C60
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.ID);
			bw.WriteLeb128(this.captured_vars.Count);
			foreach (CapturedVariable capturedVariable in this.captured_vars)
			{
				capturedVariable.Write(bw);
			}
			bw.WriteLeb128(this.captured_scopes.Count);
			foreach (CapturedScope capturedScope in this.captured_scopes)
			{
				capturedScope.Write(bw);
			}
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00036B28 File Offset: 0x00034D28
		public override string ToString()
		{
			return string.Format("[AnonymousScope {0}]", this.ID);
		}

		// Token: 0x040009BF RID: 2495
		public readonly int ID;

		// Token: 0x040009C0 RID: 2496
		private List<CapturedVariable> captured_vars = new List<CapturedVariable>();

		// Token: 0x040009C1 RID: 2497
		private List<CapturedScope> captured_scopes = new List<CapturedScope>();
	}
}
