using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000161 RID: 353
	[Guid("B9AC5783-FCD0-4b21-B119-B4F8DA8FD2C3")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComImport]
	internal interface ISpGrammarResourceLoader
	{
		// Token: 0x06000AB2 RID: 2738
		[PreserveSig]
		int LoadResource(string bstrResourceUri, bool fAlwaysReload, out IStream pStream, ref string pbstrMIMEType, ref short pfModified, ref string pbstrRedirectUrl);

		// Token: 0x06000AB3 RID: 2739
		string GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl);

		// Token: 0x06000AB4 RID: 2740
		void ReleaseLocalCopy(string path);
	}
}
