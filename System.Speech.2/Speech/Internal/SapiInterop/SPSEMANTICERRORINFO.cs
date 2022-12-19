using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000141 RID: 321
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSEMANTICERRORINFO
	{
		// Token: 0x04000769 RID: 1897
		internal uint ulLineNumber;

		// Token: 0x0400076A RID: 1898
		internal uint pszScriptLineOffset;

		// Token: 0x0400076B RID: 1899
		internal uint pszSourceOffset;

		// Token: 0x0400076C RID: 1900
		internal uint pszDescriptionOffset;

		// Token: 0x0400076D RID: 1901
		internal int hrResultCode;
	}
}
