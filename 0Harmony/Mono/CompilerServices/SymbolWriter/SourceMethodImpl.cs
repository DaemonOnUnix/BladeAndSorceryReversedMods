using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000315 RID: 789
	internal class SourceMethodImpl : IMethodDef
	{
		// Token: 0x060013EE RID: 5102 RVA: 0x00040B76 File Offset: 0x0003ED76
		public SourceMethodImpl(string name, int token, int namespaceID)
		{
			this.name = name;
			this.token = token;
			this.namespaceID = namespaceID;
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x00040B93 File Offset: 0x0003ED93
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00040B9B File Offset: 0x0003ED9B
		public int NamespaceID
		{
			get
			{
				return this.namespaceID;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00040BA3 File Offset: 0x0003EDA3
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x04000A55 RID: 2645
		private string name;

		// Token: 0x04000A56 RID: 2646
		private int token;

		// Token: 0x04000A57 RID: 2647
		private int namespaceID;
	}
}
