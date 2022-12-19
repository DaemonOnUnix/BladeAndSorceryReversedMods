using System;
using System.IO;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000F3 RID: 243
	internal class AudioData : IDisposable
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00017EA0 File Offset: 0x00016EA0
		internal AudioData(Uri uri, ResourceLoader resourceLoader)
		{
			this._uri = uri;
			this._resourceLoader = resourceLoader;
			Uri uri2;
			this._stream = this._resourceLoader.LoadFile(uri, out this._mimeType, out uri2, out this._localFile);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00017EE1 File Offset: 0x00016EE1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00017EF0 File Offset: 0x00016EF0
		~AudioData()
		{
			this.Dispose(false);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00017F20 File Offset: 0x00016F20
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

		// Token: 0x04000471 RID: 1137
		internal Uri _uri;

		// Token: 0x04000472 RID: 1138
		internal string _mimeType;

		// Token: 0x04000473 RID: 1139
		internal Stream _stream;

		// Token: 0x04000474 RID: 1140
		private string _localFile;

		// Token: 0x04000475 RID: 1141
		private ResourceLoader _resourceLoader;
	}
}
