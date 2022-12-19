using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000307 RID: 775
	internal class PdbDebugException : IOException
	{
		// Token: 0x06001245 RID: 4677 RVA: 0x0003C545 File Offset: 0x0003A745
		internal PdbDebugException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
