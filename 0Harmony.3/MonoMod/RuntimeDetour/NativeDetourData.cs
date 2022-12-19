using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000368 RID: 872
	internal struct NativeDetourData
	{
		// Token: 0x04001034 RID: 4148
		public IntPtr Method;

		// Token: 0x04001035 RID: 4149
		public IntPtr Target;

		// Token: 0x04001036 RID: 4150
		public byte Type;

		// Token: 0x04001037 RID: 4151
		public uint Size;

		// Token: 0x04001038 RID: 4152
		public IntPtr Extra;
	}
}
