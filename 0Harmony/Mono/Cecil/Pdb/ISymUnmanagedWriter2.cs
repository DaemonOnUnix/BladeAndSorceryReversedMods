using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000320 RID: 800
	[Guid("0B97726E-9E6D-4f05-9A26-424022093CAA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedWriter2
	{
		// Token: 0x0600141D RID: 5149
		void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] [In] string url, [In] ref Guid langauge, [In] ref Guid languageVendor, [In] ref Guid documentType, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter pRetVal);

		// Token: 0x0600141E RID: 5150
		void SetUserEntryPoint([In] int methodToken);

		// Token: 0x0600141F RID: 5151
		void OpenMethod([In] int methodToken);

		// Token: 0x06001420 RID: 5152
		void CloseMethod();

		// Token: 0x06001421 RID: 5153
		void OpenScope([In] int startOffset, out int pRetVal);

		// Token: 0x06001422 RID: 5154
		void CloseScope([In] int endOffset);

		// Token: 0x06001423 RID: 5155
		void SetScopeRange_Placeholder();

		// Token: 0x06001424 RID: 5156
		void DefineLocalVariable_Placeholder();

		// Token: 0x06001425 RID: 5157
		void DefineParameter_Placeholder();

		// Token: 0x06001426 RID: 5158
		void DefineField_Placeholder();

		// Token: 0x06001427 RID: 5159
		void DefineGlobalVariable_Placeholder();

		// Token: 0x06001428 RID: 5160
		void Close();

		// Token: 0x06001429 RID: 5161
		void SetSymAttribute(uint parent, string name, uint data, IntPtr signature);

		// Token: 0x0600142A RID: 5162
		void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x0600142B RID: 5163
		void CloseNamespace();

		// Token: 0x0600142C RID: 5164
		void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string fullName);

		// Token: 0x0600142D RID: 5165
		void SetMethodSourceRange_Placeholder();

		// Token: 0x0600142E RID: 5166
		void Initialize([MarshalAs(UnmanagedType.IUnknown)] [In] object emitter, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename, [In] IStream pIStream, [In] bool fFullBuild);

		// Token: 0x0600142F RID: 5167
		void GetDebugInfo(out ImageDebugDirectory pIDD, [In] int cData, out int pcData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] [Out] byte[] data);

		// Token: 0x06001430 RID: 5168
		void DefineSequencePoints([MarshalAs(UnmanagedType.Interface)] [In] ISymUnmanagedDocumentWriter document, [In] int spCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] offsets, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] lines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] columns, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endLines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endColumns);

		// Token: 0x06001431 RID: 5169
		void RemapToken_Placeholder();

		// Token: 0x06001432 RID: 5170
		void Initialize2_Placeholder();

		// Token: 0x06001433 RID: 5171
		void DefineConstant_Placeholder();

		// Token: 0x06001434 RID: 5172
		void Abort_Placeholder();

		// Token: 0x06001435 RID: 5173
		void DefineLocalVariable2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] int attributes, [In] int sigToken, [In] int addrKind, [In] int addr1, [In] int addr2, [In] int addr3, [In] int startOffset, [In] int endOffset);

		// Token: 0x06001436 RID: 5174
		void DefineGlobalVariable2_Placeholder();

		// Token: 0x06001437 RID: 5175
		void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.Struct)] [In] object variant, [In] int sigToken);
	}
}
