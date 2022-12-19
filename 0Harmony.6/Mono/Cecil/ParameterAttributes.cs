using System;

namespace Mono.Cecil
{
	// Token: 0x0200015A RID: 346
	[Flags]
	public enum ParameterAttributes : ushort
	{
		// Token: 0x04000445 RID: 1093
		None = 0,
		// Token: 0x04000446 RID: 1094
		In = 1,
		// Token: 0x04000447 RID: 1095
		Out = 2,
		// Token: 0x04000448 RID: 1096
		Lcid = 4,
		// Token: 0x04000449 RID: 1097
		Retval = 8,
		// Token: 0x0400044A RID: 1098
		Optional = 16,
		// Token: 0x0400044B RID: 1099
		HasDefault = 4096,
		// Token: 0x0400044C RID: 1100
		HasFieldMarshal = 8192,
		// Token: 0x0400044D RID: 1101
		Unused = 53216
	}
}
