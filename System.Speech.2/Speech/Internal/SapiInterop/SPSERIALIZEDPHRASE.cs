using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000149 RID: 329
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASE
	{
		// Token: 0x060009EA RID: 2538 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal SPSERIALIZEDPHRASE()
		{
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002B8AC File Offset: 0x00029AAC
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

		// Token: 0x040007CF RID: 1999
		internal uint ulSerializedSize;

		// Token: 0x040007D0 RID: 2000
		internal uint cbSize;

		// Token: 0x040007D1 RID: 2001
		internal ushort LangID;

		// Token: 0x040007D2 RID: 2002
		internal ushort wHomophoneGroupId;

		// Token: 0x040007D3 RID: 2003
		internal ulong ullGrammarID;

		// Token: 0x040007D4 RID: 2004
		internal ulong ftStartTime;

		// Token: 0x040007D5 RID: 2005
		internal ulong ullAudioStreamPosition;

		// Token: 0x040007D6 RID: 2006
		internal uint ulAudioSizeBytes;

		// Token: 0x040007D7 RID: 2007
		internal uint ulRetainedSizeBytes;

		// Token: 0x040007D8 RID: 2008
		internal uint ulAudioSizeTime;

		// Token: 0x040007D9 RID: 2009
		internal SPSERIALIZEDPHRASERULE Rule;

		// Token: 0x040007DA RID: 2010
		internal uint PropertiesOffset;

		// Token: 0x040007DB RID: 2011
		internal uint ElementsOffset;

		// Token: 0x040007DC RID: 2012
		internal uint cReplacements;

		// Token: 0x040007DD RID: 2013
		internal uint ReplacementsOffset;

		// Token: 0x040007DE RID: 2014
		internal Guid SREngineID;

		// Token: 0x040007DF RID: 2015
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x040007E0 RID: 2016
		internal uint SREnginePrivateDataOffset;

		// Token: 0x040007E1 RID: 2017
		internal uint SMLOffset;

		// Token: 0x040007E2 RID: 2018
		internal uint SemanticErrorInfoOffset;
	}
}
