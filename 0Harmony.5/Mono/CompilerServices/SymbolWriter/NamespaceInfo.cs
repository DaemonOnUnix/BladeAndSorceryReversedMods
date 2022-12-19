using System;
using System.Collections;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000316 RID: 790
	internal class NamespaceInfo
	{
		// Token: 0x04000A58 RID: 2648
		public string Name;

		// Token: 0x04000A59 RID: 2649
		public int NamespaceID;

		// Token: 0x04000A5A RID: 2650
		public ArrayList UsingClauses = new ArrayList();
	}
}
