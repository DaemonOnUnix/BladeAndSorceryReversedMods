using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003FE RID: 1022
	internal class PdbException : IOException
	{
		// Token: 0x060015B5 RID: 5557 RVA: 0x0004448D File Offset: 0x0004268D
		internal PdbException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
