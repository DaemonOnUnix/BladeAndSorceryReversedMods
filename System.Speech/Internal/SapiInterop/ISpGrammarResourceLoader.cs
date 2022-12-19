using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200008A RID: 138
	[Guid("B9AC5783-FCD0-4b21-B119-B4F8DA8FD2C3")]
	[InterfaceType(0)]
	[ComImport]
	internal interface ISpGrammarResourceLoader
	{
		// Token: 0x060002B0 RID: 688
		[PreserveSig]
		int LoadResource(string bstrResourceUri, bool fAlwaysReload, out IStream pStream, ref string pbstrMIMEType, ref short pfModified, ref string pbstrRedirectUrl);

		// Token: 0x060002B1 RID: 689
		string GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl);

		// Token: 0x060002B2 RID: 690
		void ReleaseLocalCopy(string path);
	}
}
