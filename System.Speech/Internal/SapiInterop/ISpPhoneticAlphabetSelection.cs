using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000039 RID: 57
	[InterfaceType(1)]
	[Guid("B2745EFD-42CE-48CA-81F1-A96E02538A90")]
	[ComImport]
	internal interface ISpPhoneticAlphabetSelection
	{
		// Token: 0x06000183 RID: 387
		void IsAlphabetUPS([MarshalAs(2)] out bool pfIsUPS);

		// Token: 0x06000184 RID: 388
		void SetAlphabetToUPS([MarshalAs(2)] bool fForceUPS);
	}
}
