using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000145 RID: 325
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASE_Sapi51
	{
		// Token: 0x04000796 RID: 1942
		internal uint ulSerializedSize;

		// Token: 0x04000797 RID: 1943
		internal uint cbSize;

		// Token: 0x04000798 RID: 1944
		internal ushort LangID;

		// Token: 0x04000799 RID: 1945
		internal ushort wHomophoneGroupId;

		// Token: 0x0400079A RID: 1946
		internal ulong ullGrammarID;

		// Token: 0x0400079B RID: 1947
		internal ulong ftStartTime;

		// Token: 0x0400079C RID: 1948
		internal ulong ullAudioStreamPosition;

		// Token: 0x0400079D RID: 1949
		internal uint ulAudioSizeBytes;

		// Token: 0x0400079E RID: 1950
		internal uint ulRetainedSizeBytes;

		// Token: 0x0400079F RID: 1951
		internal uint ulAudioSizeTime;

		// Token: 0x040007A0 RID: 1952
		internal SPSERIALIZEDPHRASERULE Rule;

		// Token: 0x040007A1 RID: 1953
		internal uint PropertiesOffset;

		// Token: 0x040007A2 RID: 1954
		internal uint ElementsOffset;

		// Token: 0x040007A3 RID: 1955
		internal uint cReplacements;

		// Token: 0x040007A4 RID: 1956
		internal uint ReplacementsOffset;

		// Token: 0x040007A5 RID: 1957
		internal Guid SREngineID;

		// Token: 0x040007A6 RID: 1958
		internal uint ulSREnginePrivateDataSize;

		// Token: 0x040007A7 RID: 1959
		internal uint SREnginePrivateDataOffset;
	}
}
