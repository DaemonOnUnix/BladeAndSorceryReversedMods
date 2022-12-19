using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASERULE
	{
		// Token: 0x04000271 RID: 625
		internal uint pszNameOffset;

		// Token: 0x04000272 RID: 626
		internal uint ulId;

		// Token: 0x04000273 RID: 627
		internal uint ulFirstElement;

		// Token: 0x04000274 RID: 628
		internal uint ulCountOfElements;

		// Token: 0x04000275 RID: 629
		internal uint NextSiblingOffset;

		// Token: 0x04000276 RID: 630
		internal uint FirstChildOffset;

		// Token: 0x04000277 RID: 631
		internal float SREngineConfidence;

		// Token: 0x04000278 RID: 632
		internal sbyte Confidence;
	}
}
