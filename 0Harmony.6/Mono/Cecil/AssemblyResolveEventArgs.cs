using System;

namespace Mono.Cecil
{
	// Token: 0x020000FE RID: 254
	internal sealed class AssemblyResolveEventArgs : EventArgs
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001F6E6 File Offset: 0x0001D8E6
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001F6EE File Offset: 0x0001D8EE
		public AssemblyResolveEventArgs(AssemblyNameReference reference)
		{
			this.reference = reference;
		}

		// Token: 0x0400029E RID: 670
		private readonly AssemblyNameReference reference;
	}
}
