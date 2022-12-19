using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003FD RID: 1021
	internal class PdbDebugException : IOException
	{
		// Token: 0x060015B4 RID: 5556 RVA: 0x0004448D File Offset: 0x0004268D
		internal PdbDebugException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
