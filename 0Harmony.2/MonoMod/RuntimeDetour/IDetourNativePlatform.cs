using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000465 RID: 1125
	internal interface IDetourNativePlatform
	{
		// Token: 0x06001846 RID: 6214
		NativeDetourData Create(IntPtr from, IntPtr to, byte? type = null);

		// Token: 0x06001847 RID: 6215
		void Free(NativeDetourData detour);

		// Token: 0x06001848 RID: 6216
		void Apply(NativeDetourData detour);

		// Token: 0x06001849 RID: 6217
		void Copy(IntPtr src, IntPtr dst, byte type);

		// Token: 0x0600184A RID: 6218
		void MakeWritable(IntPtr src, uint size);

		// Token: 0x0600184B RID: 6219
		void MakeExecutable(IntPtr src, uint size);

		// Token: 0x0600184C RID: 6220
		void MakeReadWriteExecutable(IntPtr src, uint size);

		// Token: 0x0600184D RID: 6221
		void FlushICache(IntPtr src, uint size);

		// Token: 0x0600184E RID: 6222
		IntPtr MemAlloc(uint size);

		// Token: 0x0600184F RID: 6223
		void MemFree(IntPtr ptr);
	}
}
