using System;
using System.IO;
using System.Net;

namespace System.Speech.Internal
{
	// Token: 0x02000095 RID: 149
	internal class ResourceLoader
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x00014078 File Offset: 0x00012278
		internal Stream LoadFile(Uri uri, out string mimeType, out Uri baseUri, out string localPath)
		{
			localPath = null;
			Stream stream = null;
			if (!uri.IsAbsoluteUri || uri.IsFile)
			{
				string text = (uri.IsAbsoluteUri ? uri.LocalPath : uri.OriginalString);
				try
				{
					stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
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

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal void UnloadFile(string localPath)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00014120 File Offset: 0x00012320
		internal Stream LoadFile(Uri uri, out string localPath, out Uri redirectedUri)
		{
			string text;
			return this.LoadFile(uri, out text, out redirectedUri, out localPath);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00014138 File Offset: 0x00012338
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
