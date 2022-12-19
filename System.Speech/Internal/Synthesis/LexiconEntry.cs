using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DE RID: 222
	internal class LexiconEntry
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x00016AB3 File Offset: 0x00015AB3
		internal LexiconEntry(Uri uri, string mediaType)
		{
			this._uri = uri;
			this._mediaType = mediaType;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00016ACC File Offset: 0x00015ACC
		public override bool Equals(object obj)
		{
			LexiconEntry lexiconEntry = obj as LexiconEntry;
			return lexiconEntry != null && this._uri.Equals(lexiconEntry._uri);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00016AF6 File Offset: 0x00015AF6
		public override int GetHashCode()
		{
			return this._uri.GetHashCode();
		}

		// Token: 0x04000401 RID: 1025
		internal Uri _uri;

		// Token: 0x04000402 RID: 1026
		internal string _mediaType;
	}
}
