﻿using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000060 RID: 96
	internal enum SPINTERFERENCE
	{
		// Token: 0x040001C7 RID: 455
		SPINTERFERENCE_NONE,
		// Token: 0x040001C8 RID: 456
		SPINTERFERENCE_NOISE,
		// Token: 0x040001C9 RID: 457
		SPINTERFERENCE_NOSIGNAL,
		// Token: 0x040001CA RID: 458
		SPINTERFERENCE_TOOLOUD,
		// Token: 0x040001CB RID: 459
		SPINTERFERENCE_TOOQUIET,
		// Token: 0x040001CC RID: 460
		SPINTERFERENCE_TOOFAST,
		// Token: 0x040001CD RID: 461
		SPINTERFERENCE_TOOSLOW
	}
}