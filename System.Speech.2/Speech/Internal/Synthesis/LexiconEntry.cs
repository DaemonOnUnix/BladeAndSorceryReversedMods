using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B0 RID: 176
	internal class LexiconEntry
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00017E03 File Offset: 0x00016003
		internal LexiconEntry(Uri uri, string mediaType)
		{
			this._uri = uri;
			this._mediaType = mediaType;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00017E1C File Offset: 0x0001601C
		public override bool Equals(object obj)
		{
			LexiconEntry lexiconEntry = obj as LexiconEntry;
			return lexiconEntry != null && this._uri.Equals(lexiconEntry._uri);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00017E46 File Offset: 0x00016046
		public override int GetHashCode()
		{
			return this._uri.GetHashCode();
		}

		// Token: 0x04000494 RID: 1172
		internal Uri _uri;

		// Token: 0x04000495 RID: 1173
		internal string _mediaType;
	}
}
