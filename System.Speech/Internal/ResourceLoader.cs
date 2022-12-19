using System;
using System.IO;
using System.Net;

namespace System.Speech.Internal
{
	// Token: 0x0200001B RID: 27
	internal class ResourceLoader
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00005F3C File Offset: 0x00004F3C
		internal Stream LoadFile(Uri uri, out string mimeType, out Uri baseUri, out string localPath)
		{
			localPath = null;
			Stream stream = null;
			if (!uri.IsAbsoluteUri || uri.IsFile)
			{
				string text = (uri.IsAbsoluteUri ? uri.LocalPath : uri.OriginalString);
				try
				{
					stream = new FileStream(text, 3, 1, 1);
				}
				catch
				{
					if (Directory.Exists(text))
					{
						throw new InvalidOperationException(SR.Get(SRID.CannotReadFromDirectory, new object[] { text }));
					}
					throw;
				}
				baseUri = null;
			}
			else
			{
				try
				{
					stream = ResourceLoader.DownloadData(uri, out baseUri);
				}
				catch (WebException ex)
				{
					throw new IOException(ex.Message, ex);
				}
			}
			mimeType = null;
			return stream;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005FE4 File Offset: 0x00004FE4
		internal void UnloadFile(string localPath)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005FE8 File Offset: 0x00004FE8
		internal Stream LoadFile(Uri uri, out string localPath, out Uri redirectedUri)
		{
			string text;
			return this.LoadFile(uri, out text, out redirectedUri, out localPath);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006000 File Offset: 0x00005000
		private static Stream DownloadData(Uri uri, out Uri redirectedUri)
		{
			WebRequest webRequest = WebRequest.Create(uri);
			webRequest.Credentials = CredentialCache.DefaultCredentials;
			Stream stream;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (httpWebResponse.GetResponseStream())
				{
					redirectedUri = httpWebResponse.ResponseUri;
					using (WebClient webClient = new WebClient())
					{
						webClient.UseDefaultCredentials = true;
						stream = new MemoryStream(webClient.DownloadData(redirectedUri));
					}
				}
			}
			return stream;
		}
	}
}
