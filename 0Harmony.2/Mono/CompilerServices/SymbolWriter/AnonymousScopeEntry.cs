using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200030A RID: 778
	public class AnonymousScopeEntry
	{
		// Token: 0x0600136B RID: 4971 RVA: 0x0003E885 File Offset: 0x0003CA85
		public AnonymousScopeEntry(int id)
		{
			this.ID = id;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0003E8AC File Offset: 0x0003CAAC
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

		// Token: 0x0600136D RID: 4973 RVA: 0x0003E929 File Offset: 0x0003CB29
		internal void AddCapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.captured_vars.Add(new CapturedVariable(name, captured_name, kind));
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0003E940 File Offset: 0x0003CB40
		public CapturedVariable[] CapturedVariables
		{
			get
			{
				CapturedVariable[] array = new CapturedVariable[this.captured_vars.Count];
				this.captured_vars.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0003E96C File Offset: 0x0003CB6C
		internal void AddCapturedScope(int scope, string captured_name)
		{
			this.captured_scopes.Add(new CapturedScope(scope, captured_name));
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0003E980 File Offset: 0x0003CB80
		public CapturedScope[] CapturedScopes
		{
			get
			{
				CapturedScope[] array = new CapturedScope[this.captured_scopes.Count];
				this.captured_scopes.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0003E9AC File Offset: 0x0003CBAC
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

		// Token: 0x06001372 RID: 4978 RVA: 0x0003EA74 File Offset: 0x0003CC74
		public override string ToString()
		{
			return string.Format("[AnonymousScope {0}]", this.ID);
		}

		// Token: 0x040009FE RID: 2558
		public readonly int ID;

		// Token: 0x040009FF RID: 2559
		private List<CapturedVariable> captured_vars = new List<CapturedVariable>();

		// Token: 0x04000A00 RID: 2560
		private List<CapturedScope> captured_scopes = new List<CapturedScope>();
	}
}
