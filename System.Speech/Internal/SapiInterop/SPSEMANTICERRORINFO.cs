using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200006A RID: 106
	[StructLayout(0)]
	internal class SPSEMANTICERRORINFO
	{
		// Token: 0x040001F7 RID: 503
		internal uint ulLineNumber;

		// Token: 0x040001F8 RID: 504
		internal uint pszScriptLineOffset;

		// Token: 0x040001F9 RID: 505
		internal uint pszSourceOffset;

		// Token: 0x040001FA RID: 506
		internal uint pszDescriptionOffset;

		// Token: 0x040001FB RID: 507
		internal int hrResultCode;
	}
}
