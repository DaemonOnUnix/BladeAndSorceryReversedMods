using System;

namespace Mono
{
	// Token: 0x020001A4 RID: 420
	internal class ArgumentNullOrEmptyException : ArgumentException
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x000176C0 File Offset: 0x000158C0
		public ArgumentNullOrEmptyException(string paramName)
			: base("Argument null or empty", paramName)
		{
		}
	}
}
