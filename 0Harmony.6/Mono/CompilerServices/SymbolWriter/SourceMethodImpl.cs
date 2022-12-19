using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021F RID: 543
	internal class SourceMethodImpl : IMethodDef
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x00038C2A File Offset: 0x00036E2A
		public SourceMethodImpl(string name, int token, int namespaceID)
		{
			this.name = name;
			this.token = token;
			this.namespaceID = namespaceID;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00038C47 File Offset: 0x00036E47
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00038C4F File Offset: 0x00036E4F
		public int NamespaceID
		{
			get
			{
				return this.namespaceID;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00038C57 File Offset: 0x00036E57
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x04000A16 RID: 2582
		private string name;

		// Token: 0x04000A17 RID: 2583
		private int token;

		// Token: 0x04000A18 RID: 2584
		private int namespaceID;
	}
}
