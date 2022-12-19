using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000164 RID: 356
	[Guid("41B89B6B-9399-11D2-9623-00C04F8EE628")]
	[ComImport]
	internal class SpInprocRecognizer
	{
		// Token: 0x06000AC0 RID: 2752
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern SpInprocRecognizer();
	}
}
