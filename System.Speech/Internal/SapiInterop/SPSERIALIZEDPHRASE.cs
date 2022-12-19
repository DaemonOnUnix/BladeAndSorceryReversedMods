using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASE
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x000093AC File Offset: 0x000083AC
		internal SPSERIALIZEDPHRASE()
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000093B4 File Offset: 0x000083B4
		internal SPSERIALIZEDPHRASE(SPSERIALIZEDPHRASE_Sapi51 source)
		{
			this.ulSerializedSize = source.ulSerializedSize;
			this.cbSize = source.cbSize;
			this.LangID = source.LangID;
			this.wHomophoneGroupId = source.wHomophoneGroupId;
			this.ullGrammarID = source.ullGrammarID;
			this.ftStartTime = source.ftStartTime;
			this.ullAudioStreamPosition = source.ullAudioStreamPosition;
			this.ulAudioSizeBytes = source.ulAudioSizeBytes;
			this.ulRetainedSizeBytes = source.ulRetainedSizeBytes;
			this.ulAudioSizeTime = source.ulAudioSizeTime;
			this.Rule = source.Rule;
			this.PropertiesOffset = source.PropertiesOffset;
			this.ElementsOffset = source.ElementsOffset;
			this.cReplacements = source.cReplacements;
			this.ReplacementsOffset = source.ReplacementsOffset;
			this.SREngineID = source.SREngineID;
			this.ulSREnginePrivateDataSize = source.ulSREnginePrivateDataSize;
			this.SREnginePrivateDataOffset = source.SREnginePrivateDataOffset;
		}

		// Token: 0x0400025D RID: 605
		internal uint ulSerializedSize;

		// Token: 0x0400025E RID: 606
		internal uint cbSize;

		// Token: 0x0400025F RID: 607
		internal ushort LangID;

		// Token: 0x04000260 RID: 608
		internal ushort wHomophoneGroupId;

		// Token: 0x04000261 RID: 609
		internal ulong ullGrammarID;

		// Token: 0x04000262 RID: 610
		internal ulong ftStartTime;

		// Token: 0x04000263 RID: 611
		internal ulong ullAudioStreamPosition;

		// Token: 0x04000264 RID: 612
		internal uint ulAudioSizeBytes;

		// Token: 0x04000265 RID: 613
		internal uint ulRetainedSizeBytes;

		// Token: 0x04000266 RID: 614
		internal uint ulAudioSizeTime;

		// Token: 0x04000267 RID: 615
		internal SPSERIALIZEDPHRASERULE Rule;

		// Token: 0x04000268 RID: 616
		internal uint PropertiesOffset;

		// Token: 0x04000269 RID: 617
		internal uint ElementsOffset;

		// Token: 0x0400026A RID: 618
		internal uint cReplacements;

		// Token: 0x0400026B RID: 619
		internal uint ReplacementsOffset;

		// Token: 0x0400026C RID: 620
		internal Guid SREngineID;

		// Token: 0x0400026D RID: 621
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x0400026E RID: 622
		internal uint SREnginePrivateDataOffset;

		// Token: 0x0400026F RID: 623
		internal uint SMLOffset;

		// Token: 0x04000270 RID: 624
		internal uint SemanticErrorInfoOffset;
	}
}
