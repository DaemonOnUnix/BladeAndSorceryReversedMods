using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200031F RID: 799
	[Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISymUnmanagedDocumentWriter
	{
		// Token: 0x0600141B RID: 5147
		void SetSource(uint sourceSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] source);

		// Token: 0x0600141C RID: 5148
		void SetCheckSum(Guid algorithmId, uint checkSumSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] checkSum);
	}
}
