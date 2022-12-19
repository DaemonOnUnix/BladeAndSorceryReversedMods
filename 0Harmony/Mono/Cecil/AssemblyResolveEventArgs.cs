using System;

namespace Mono.Cecil
{
	// Token: 0x020001F0 RID: 496
	internal sealed class AssemblyResolveEventArgs : EventArgs
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00025586 File Offset: 0x00023786
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002558E File Offset: 0x0002378E
		public AssemblyResolveEventArgs(AssemblyNameReference reference)
		{
			this.reference = reference;
		}

		// Token: 0x040002D0 RID: 720
		private readonly AssemblyNameReference reference;
	}
}
