using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014A RID: 330
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASERULE
	{
		// Token: 0x040007E3 RID: 2019
		internal uint pszNameOffset;

		// Token: 0x040007E4 RID: 2020
		internal uint ulId;

		// Token: 0x040007E5 RID: 2021
		internal uint ulFirstElement;

		// Token: 0x040007E6 RID: 2022
		internal uint ulCountOfElements;

		// Token: 0x040007E7 RID: 2023
		internal uint NextSiblingOffset;

		// Token: 0x040007E8 RID: 2024
		internal uint FirstChildOffset;

		// Token: 0x040007E9 RID: 2025
		internal float SREngineConfidence;

		// Token: 0x040007EA RID: 2026
		internal sbyte Confidence;
	}
}
