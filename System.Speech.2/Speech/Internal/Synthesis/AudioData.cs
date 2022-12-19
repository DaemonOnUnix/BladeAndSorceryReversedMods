using System;
using System.IO;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000BF RID: 191
	internal class AudioData : IDisposable
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x0001919C File Offset: 0x0001739C
		internal AudioData(Uri uri, ResourceLoader resourceLoader)
		{
			this._uri = uri;
			this._resourceLoader = resourceLoader;
			Uri uri2;
			this._stream = this._resourceLoader.LoadFile(uri, out this._mimeType, out uri2, out this._localFile);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000191DD File Offset: 0x000173DD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000191EC File Offset: 0x000173EC
		~AudioData()
		{
			this.Dispose(false);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001921C File Offset: 0x0001741C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._localFile != null)
				{
					this._resourceLoader.UnloadFile(this._localFile);
				}
				if (this._stream != null)
				{
					this._stream.Dispose();
					this._stream = null;
					this._localFile = null;
					this._uri = null;
				}
			}
		}

		// Token: 0x040004F0 RID: 1264
		internal Uri _uri;

		// Token: 0x040004F1 RID: 1265
		internal string _mimeType;

		// Token: 0x040004F2 RID: 1266
		internal Stream _stream;

		// Token: 0x040004F3 RID: 1267
		private string _localFile;

		// Token: 0x040004F4 RID: 1268
		private ResourceLoader _resourceLoader;
	}
}
