using System;
using System.Collections;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000220 RID: 544
	internal class NamespaceInfo
	{
		// Token: 0x04000A19 RID: 2585
		public string Name;

		// Token: 0x04000A1A RID: 2586
		public int NamespaceID;

		// Token: 0x04000A1B RID: 2587
		public ArrayList UsingClauses = new ArrayList();
	}
}
