using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000468 RID: 1128
	internal struct NativeDetourData
	{
		// Token: 0x04001098 RID: 4248
		public IntPtr Method;

		// Token: 0x04001099 RID: 4249
		public IntPtr Target;

		// Token: 0x0400109A RID: 4250
		public byte Type;

		// Token: 0x0400109B RID: 4251
		public uint Size;

		// Token: 0x0400109C RID: 4252
		public IntPtr Extra;
	}
}
