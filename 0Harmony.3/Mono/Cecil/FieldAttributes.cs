using System;

namespace Mono.Cecil
{
	// Token: 0x0200010E RID: 270
	[Flags]
	public enum FieldAttributes : ushort
	{
		// Token: 0x040002CE RID: 718
		FieldAccessMask = 7,
		// Token: 0x040002CF RID: 719
		CompilerControlled = 0,
		// Token: 0x040002D0 RID: 720
		Private = 1,
		// Token: 0x040002D1 RID: 721
		FamANDAssem = 2,
		// Token: 0x040002D2 RID: 722
		Assembly = 3,
		// Token: 0x040002D3 RID: 723
		Family = 4,
		// Token: 0x040002D4 RID: 724
		FamORAssem = 5,
		// Token: 0x040002D5 RID: 725
		Public = 6,
		// Token: 0x040002D6 RID: 726
		Static = 16,
		// Token: 0x040002D7 RID: 727
		InitOnly = 32,
		// Token: 0x040002D8 RID: 728
		Literal = 64,
		// Token: 0x040002D9 RID: 729
		NotSerialized = 128,
		// Token: 0x040002DA RID: 730
		SpecialName = 512,
		// Token: 0x040002DB RID: 731
		PInvokeImpl = 8192,
		// Token: 0x040002DC RID: 732
		RTSpecialName = 1024,
		// Token: 0x040002DD RID: 733
		HasFieldMarshal = 4096,
		// Token: 0x040002DE RID: 734
		HasDefault = 32768,
		// Token: 0x040002DF RID: 735
		HasFieldRVA = 256
	}
}
