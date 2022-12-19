using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000308 RID: 776
	internal class PdbException : IOException
	{
		// Token: 0x06001246 RID: 4678 RVA: 0x0003C545 File Offset: 0x0003A745
		internal PdbException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
