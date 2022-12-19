using System;

namespace Mono.Cecil
{
	// Token: 0x0200024E RID: 590
	[Flags]
	public enum ParameterAttributes : ushort
	{
		// Token: 0x0400047A RID: 1146
		None = 0,
		// Token: 0x0400047B RID: 1147
		In = 1,
		// Token: 0x0400047C RID: 1148
		Out = 2,
		// Token: 0x0400047D RID: 1149
		Lcid = 4,
		// Token: 0x0400047E RID: 1150
		Retval = 8,
		// Token: 0x0400047F RID: 1151
		Optional = 16,
		// Token: 0x04000480 RID: 1152
		HasDefault = 4096,
		// Token: 0x04000481 RID: 1153
		HasFieldMarshal = 8192,
		// Token: 0x04000482 RID: 1154
		Unused = 53216
	}
}
