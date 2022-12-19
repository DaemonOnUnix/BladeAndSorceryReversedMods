using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002CF RID: 719
	public struct ImageDebugDirectory
	{
		// Token: 0x04000940 RID: 2368
		public const int Size = 28;

		// Token: 0x04000941 RID: 2369
		public int Characteristics;

		// Token: 0x04000942 RID: 2370
		public int TimeDateStamp;

		// Token: 0x04000943 RID: 2371
		public short MajorVersion;

		// Token: 0x04000944 RID: 2372
		public short MinorVersion;

		// Token: 0x04000945 RID: 2373
		public ImageDebugType Type;

		// Token: 0x04000946 RID: 2374
		public int SizeOfData;

		// Token: 0x04000947 RID: 2375
		public int AddressOfRawData;

		// Token: 0x04000948 RID: 2376
		public int PointerToRawData;
	}
}
