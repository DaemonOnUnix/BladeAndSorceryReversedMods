using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000035 RID: 53
	[InterfaceType(1)]
	[Guid("5B559F40-E952-11D2-BB91-00C04F8EE6C0")]
	[ComImport]
	internal interface ISpObjectWithToken
	{
		// Token: 0x0600015C RID: 348
		[PreserveSig]
		int SetObjectToken(ISpObjectToken pToken);

		// Token: 0x0600015D RID: 349
		[PreserveSig]
		int GetObjectToken(out ISpObjectToken ppToken);
	}
}
