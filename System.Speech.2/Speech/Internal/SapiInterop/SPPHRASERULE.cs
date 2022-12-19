using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000147 RID: 327
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPPHRASERULE
	{
		// Token: 0x040007B9 RID: 1977
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszName;

		// Token: 0x040007BA RID: 1978
		internal uint ulId;

		// Token: 0x040007BB RID: 1979
		internal uint ulFirstElement;

		// Token: 0x040007BC RID: 1980
		internal uint ulCountOfElements;

		// Token: 0x040007BD RID: 1981
		internal IntPtr pNextSibling;

		// Token: 0x040007BE RID: 1982
		internal IntPtr pFirstChild;

		// Token: 0x040007BF RID: 1983
		internal float SREngineConfidence;

		// Token: 0x040007C0 RID: 1984
		internal byte Confidence;
	}
}
