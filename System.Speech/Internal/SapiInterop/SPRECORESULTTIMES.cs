using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	internal struct SPRECORESULTTIMES
	{
		// Token: 0x040001DC RID: 476
		internal FILETIME ftStreamTime;

		// Token: 0x040001DD RID: 477
		internal ulong ullLength;

		// Token: 0x040001DE RID: 478
		internal uint dwTickCount;

		// Token: 0x040001DF RID: 479
		internal ulong ullStart;
	}
}
