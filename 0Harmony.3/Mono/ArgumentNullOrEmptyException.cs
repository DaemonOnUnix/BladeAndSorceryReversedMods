using System;

namespace Mono
{
	// Token: 0x020000B2 RID: 178
	internal class ArgumentNullOrEmptyException : ArgumentException
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x00011834 File Offset: 0x0000FA34
		public ArgumentNullOrEmptyException(string paramName)
			: base("Argument null or empty", paramName)
		{
		}
	}
}
