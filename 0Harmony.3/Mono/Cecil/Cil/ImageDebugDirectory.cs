using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D9 RID: 473
	public struct ImageDebugDirectory
	{
		// Token: 0x04000904 RID: 2308
		public const int Size = 28;

		// Token: 0x04000905 RID: 2309
		public int Characteristics;

		// Token: 0x04000906 RID: 2310
		public int TimeDateStamp;

		// Token: 0x04000907 RID: 2311
		public short MajorVersion;

		// Token: 0x04000908 RID: 2312
		public short MinorVersion;

		// Token: 0x04000909 RID: 2313
		public ImageDebugType Type;

		// Token: 0x0400090A RID: 2314
		public int SizeOfData;

		// Token: 0x0400090B RID: 2315
		public int AddressOfRawData;

		// Token: 0x0400090C RID: 2316
		public int PointerToRawData;
	}
}
