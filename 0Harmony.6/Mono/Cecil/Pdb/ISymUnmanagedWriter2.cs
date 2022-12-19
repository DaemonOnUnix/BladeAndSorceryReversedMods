using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022A RID: 554
	[Guid("0B97726E-9E6D-4f05-9A26-424022093CAA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedWriter2
	{
		// Token: 0x060010AD RID: 4269
		void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] [In] string url, [In] ref Guid langauge, [In] ref Guid languageVendor, [In] ref Guid documentType, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter pRetVal);

		// Token: 0x060010AE RID: 4270
		void SetUserEntryPoint([In] int methodToken);

		// Token: 0x060010AF RID: 4271
		void OpenMethod([In] int methodToken);

		// Token: 0x060010B0 RID: 4272
		void CloseMethod();

		// Token: 0x060010B1 RID: 4273
		void OpenScope([In] int startOffset, out int pRetVal);

		// Token: 0x060010B2 RID: 4274
		void CloseScope([In] int endOffset);

		// Token: 0x060010B3 RID: 4275
		void SetScopeRange_Placeholder();

		// Token: 0x060010B4 RID: 4276
		void DefineLocalVariable_Placeholder();

		// Token: 0x060010B5 RID: 4277
		void DefineParameter_Placeholder();

		// Token: 0x060010B6 RID: 4278
		void DefineField_Placeholder();

		// Token: 0x060010B7 RID: 4279
		void DefineGlobalVariable_Placeholder();

		// Token: 0x060010B8 RID: 4280
		void Close();

		// Token: 0x060010B9 RID: 4281
		void SetSymAttribute(uint parent, string name, uint data, IntPtr signature);

		// Token: 0x060010BA RID: 4282
		void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x060010BB RID: 4283
		void CloseNamespace();

		// Token: 0x060010BC RID: 4284
		void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] [In] string fullName);

		// Token: 0x060010BD RID: 4285
		void SetMethodSourceRange_Placeholder();

		// Token: 0x060010BE RID: 4286
		void Initialize([MarshalAs(UnmanagedType.IUnknown)] [In] object emitter, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename, [In] IStream pIStream, [In] bool fFullBuild);

		// Token: 0x060010BF RID: 4287
		void GetDebugInfo(out ImageDebugDirectory pIDD, [In] int cData, out int pcData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] [Out] byte[] data);

		// Token: 0x060010C0 RID: 4288
		void DefineSequencePoints([MarshalAs(UnmanagedType.Interface)] [In] ISymUnmanagedDocumentWriter document, [In] int spCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] offsets, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] lines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] columns, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endLines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] int[] endColumns);

		// Token: 0x060010C1 RID: 4289
		void RemapToken_Placeholder();

		// Token: 0x060010C2 RID: 4290
		void Initialize2_Placeholder();

		// Token: 0x060010C3 RID: 4291
		void DefineConstant_Placeholder();

		// Token: 0x060010C4 RID: 4292
		void Abort_Placeholder();

		// Token: 0x060010C5 RID: 4293
		void DefineLocalVariable2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] int attributes, [In] int sigToken, [In] int addrKind, [In] int addr1, [In] int addr2, [In] int addr3, [In] int startOffset, [In] int endOffset);

		// Token: 0x060010C6 RID: 4294
		void DefineGlobalVariable2_Placeholder();

		// Token: 0x060010C7 RID: 4295
		void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.Struct)] [In] object variant, [In] int sigToken);
	}
}
