using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200013C RID: 316
	[Serializable]
	internal struct SPRECORESULTTIMES
	{
		// Token: 0x0400074E RID: 1870
		internal FILETIME ftStreamTime;

		// Token: 0x0400074F RID: 1871
		internal ulong ullLength;

		// Token: 0x04000750 RID: 1872
		internal uint dwTickCount;

		// Token: 0x04000751 RID: 1873
		internal ulong ullStart;
	}
}
