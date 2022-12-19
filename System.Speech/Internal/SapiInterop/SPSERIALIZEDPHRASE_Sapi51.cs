using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200006E RID: 110
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASE_Sapi51
	{
		// Token: 0x04000224 RID: 548
		internal uint ulSerializedSize;

		// Token: 0x04000225 RID: 549
		internal uint cbSize;

		// Token: 0x04000226 RID: 550
		internal ushort LangID;

		// Token: 0x04000227 RID: 551
		internal ushort wHomophoneGroupId;

		// Token: 0x04000228 RID: 552
		internal ulong ullGrammarID;

		// Token: 0x04000229 RID: 553
		internal ulong ftStartTime;

		// Token: 0x0400022A RID: 554
		internal ulong ullAudioStreamPosition;

		// Token: 0x0400022B RID: 555
		internal uint ulAudioSizeBytes;

		// Token: 0x0400022C RID: 556
		internal uint ulRetainedSizeBytes;

		// Token: 0x0400022D RID: 557
		internal uint ulAudioSizeTime;

		// Token: 0x0400022E RID: 558
		internal SPSERIALIZEDPHRASERULE Rule;

		// Token: 0x0400022F RID: 559
		internal uint PropertiesOffset;

		// Token: 0x04000230 RID: 560
		internal uint ElementsOffset;

		// Token: 0x04000231 RID: 561
		internal uint cReplacements;

		// Token: 0x04000232 RID: 562
		internal uint ReplacementsOffset;

		// Token: 0x04000233 RID: 563
		internal Guid SREngineID;

		// Token: 0x04000234 RID: 564
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x04000235 RID: 565
		internal uint SREnginePrivateDataOffset;
	}
}
